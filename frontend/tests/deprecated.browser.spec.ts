import { test, expect } from '@playwright/test';

test.use({
    launchOptions: {
        slowMo: 1000,
    },
});

test.describe('UI Navigation', () => {
    test.beforeEach(async ({ page }) => {
        await page.goto('/');
    });

    test('should toggle sidebar', async ({ page }) => {
        const sidebar = page.locator('aside');
        await expect(sidebar).toBeVisible();
        await page.getByRole('button', { name: 'Toggle Sidebar' }).click();
        await expect(sidebar).not.toBeVisible();
        await page.getByRole('button', { name: 'Toggle Sidebar' }).click();
        await expect(sidebar).toBeVisible();
    });

    test('should navigate between weeks using navbar arrows', async ({ page }) => {
        const dateRange = page.locator('.navbar-center span');
        const initialRange = await dateRange.textContent();
        const nextButton = page.locator('.navbar-center').getByRole('button', { name: 'Next Week' });
        await nextButton.click();
        await expect(dateRange).not.toHaveText(initialRange!);
        const prevButton = page.locator('.navbar-center').getByRole('button', { name: 'Previous Week' });
        await prevButton.click();

        await expect(dateRange).toHaveText(initialRange!);
    });

    test('should navigate weeks using mini-calendar day click', async ({ page }) => {
        const dateRange = page.locator('.navbar-center span');
        const initialRange = await dateRange.textContent();
        const calendar = page.locator('aside calendar-month');
        await expect(calendar).toBeVisible();
        const today = new Date();
        const targetDay = today.getDate() > 15 ? 1 : 28;
        await calendar.locator('button').getByText(targetDay.toString(), { exact: true }).click({ force: true });
        await expect(dateRange).not.toHaveText(initialRange!);
    });

    test('should return to today', async ({ page }) => {
        const dateRange = page.locator('.navbar-center span');
        const initialRange = await dateRange.textContent();
        await page.locator('.navbar-center').getByRole('button', { name: 'Next Week' }).click();
        await expect(dateRange).not.toHaveText(initialRange!);
        await page.getByRole('button', { name: 'Today' }).click();
        await expect(dateRange).toHaveText(initialRange!);
    });

    test('should toggle dark/light mode', async ({ page }) => {
        const html = page.locator('html');
        const initialTheme = await html.getAttribute('data-theme');
        await page.getByRole('button', { name: 'Toggle Theme' }).click();
        const toggledTheme = await html.getAttribute('data-theme');
        expect(toggledTheme).not.toBe(initialTheme);
        await page.getByRole('button', { name: 'Toggle Theme' }).click();
        const restoredTheme = await html.getAttribute('data-theme');
        expect(restoredTheme).toBe(initialTheme);
    });
});

test.describe('Keyboard Shortcuts', () => {
    test.beforeEach(async ({ page }) => {
        await page.goto('/');
    });

    test('should open create modal with "c" key', async ({ page }) => {
        await expect(page.getByRole('dialog')).not.toBeVisible();
        await page.keyboard.press('c');
        await expect(page.getByText('Create Event', { exact: true }).first()).toBeVisible();
    });

    test('should navigate with arrow keys', async ({ page }) => {
        const dateRange = page.locator('.navbar-center span');
        const initialRange = await dateRange.textContent();
        await page.keyboard.press('ArrowRight');
        await expect(dateRange).not.toHaveText(initialRange!);
        const nextWeekRange = await dateRange.textContent();
        await page.keyboard.press('ArrowLeft');
        await expect(dateRange).toHaveText(initialRange!);
    });

    test('should return to today with "t" key', async ({ page }) => {
        const dateRange = page.locator('.navbar-center span');
        const initialRange = await dateRange.textContent();
        await page.keyboard.press('ArrowRight');
        await expect(dateRange).not.toHaveText(initialRange!);
        await page.keyboard.press('t');
        await expect(dateRange).toHaveText(initialRange!);
    });
});

test.describe('Event CRUD operations', () => {
    test.beforeEach(async ({ page }) => {
        await page.goto('/');
    });

    test('should create a new event', async ({ page }) => {
        await page.getByRole('button', { name: 'Create Event' }).click();
        const eventTitle = `Test Event ${Date.now()}`;
        await page.getByPlaceholder('Event Title').fill(eventTitle);
        await page.getByLabel('Start Time').fill('01:00');
        await page.getByLabel('End Time').fill('01:30');
        await page.getByPlaceholder('Add notes, location, or video call links...').fill('Test Description');
        const colorButtons = page.locator('.modal-box button.rounded-full');
        const colorCount = await colorButtons.count();
        const randomIndex = Math.floor(Math.random() * colorCount);
        await colorButtons.nth(randomIndex).click();
        await page.getByRole('button', { name: 'Save Event' }).click();
        const eventCard = page.locator('[role="button"][draggable="true"]', { hasText: eventTitle });
        await expect(eventCard).toBeVisible();
        await expect(eventCard).toContainText('01:00 - 01:30');
    });

    test('should update an existing event', async ({ page }) => {
        const initialTitle = `Update Me ${Date.now()}`;
        await page.getByRole('button', { name: 'Create Event' }).click();
        await page.getByPlaceholder('Event Title').fill(initialTitle);
        await page.getByLabel('Start Time').fill('01:30');
        await page.getByLabel('End Time').fill('02:00');
        const colorButtons = page.locator('.modal-box button.rounded-full');
        await expect(colorButtons.first()).toBeVisible();
        const colorCount = await colorButtons.count();
        const randomIndex = Math.floor(Math.random() * colorCount);
        await colorButtons.nth(randomIndex).click();
        await page.getByRole('button', { name: 'Save Event' }).click();
        await page.getByText(initialTitle).click();
        const updatedTitle = `Updated Title ${Date.now()}`;
        await page.getByPlaceholder('Event Title').clear();
        await page.getByPlaceholder('Event Title').fill(updatedTitle);
        await page.getByRole('button', { name: 'Update Event' }).click();
        await expect(page.getByText(updatedTitle)).toBeVisible();
        await expect(page.getByText(initialTitle)).not.toBeVisible();
    });

    test('should delete an event', async ({ page }) => {
        const deleteTitle = `Delete Me ${Date.now()}`;
        await page.getByRole('button', { name: 'Create Event' }).click();
        await page.getByPlaceholder('Event Title').fill(deleteTitle);
        await page.getByLabel('Start Time').fill('01:30');
        await page.getByLabel('End Time').fill('02:00');
        const colorButtons = page.locator('.modal-box button.rounded-full');
        await expect(colorButtons.first()).toBeVisible();
        const colorCount = await colorButtons.count();
        const randomIndex = Math.floor(Math.random() * colorCount);
        await colorButtons.nth(randomIndex).click();
        await page.getByRole('button', { name: 'Save Event' }).click();
        await page.getByText(deleteTitle).click();
        const deleteButton = page.getByRole('dialog').getByRole('button', { name: 'Delete', exact: true });
        await deleteButton.click();
        const confirmDeleteButton = page.getByRole('button', { name: 'Delete', exact: true });
        await confirmDeleteButton.click();
        await expect(page.getByText(deleteTitle)).not.toBeVisible();
    });
});

test.describe('Grid Interaction', () => {
    test.beforeEach(async ({ page }) => {
        await page.goto('/');
    });

    test('should open create modal with correct time when clicking on grid', async ({ page }) => {
        const dayColumn = page.locator('[role="button"][aria-label^="Day column for"]').first();
        await expect(dayColumn).toBeVisible();
        await dayColumn.click({ position: { x: 20, y: 800 } });
        await expect(page.getByText('Create Event', { exact: true }).first()).toBeVisible();
        await expect(page.getByLabel('Start Time')).toHaveValue('10:00');
    });
});

test.describe('Drag and Drop', () => {
    test.beforeEach(async ({ page }) => {
        await page.goto('/');
    });

    test('should drag and drop event within the same week', async ({ page }) => {
        const eventTitle = `Drag Me Week ${Date.now()}`;
        await page.getByRole('button', { name: 'Create Event' }).click();
        await page.getByPlaceholder('Event Title').fill(eventTitle);
        await page.getByLabel('Start Time').fill('02:00');
        await page.getByLabel('End Time').fill('03:00');
        const colorButtons = page.locator('.modal-box button.rounded-full');
        await expect(colorButtons.first()).toBeVisible();
        const colorCount = await colorButtons.count();
        const randomIndex = Math.floor(Math.random() * colorCount);
        await colorButtons.nth(randomIndex).click();
        await page.getByRole('button', { name: 'Save Event' }).click();
        const eventCard = page.locator('[role="button"][draggable="true"]', { hasText: eventTitle });
        await expect(eventCard).toBeVisible();
        const box = await eventCard.boundingBox();
        if (!box) throw new Error('Event card not found');
        const sourceX = box.x + box.width / 2;
        const sourceY = box.y + box.height / 2;
        const targetY = sourceY + 160;
        await page.mouse.move(sourceX, sourceY);
        await page.mouse.down();
        await page.mouse.move(sourceX, targetY, { steps: 10 });
        await page.mouse.up();
        await expect(eventCard).toContainText('04:30 - 05:30');
    });
    
    test('should drag and drop event across weeks', async ({ page }) => {
        const eventTitle = `Drag Me Cross Week ${Date.now()}`;
        await page.getByRole('button', { name: 'Create Event' }).click();
        await page.getByPlaceholder('Event Title').fill(eventTitle);
        await page.getByLabel('Start Time').fill('02:00');
        await page.getByLabel('End Time').fill('03:00');
        const colorButtons = page.locator('.modal-box button.rounded-full');
        await expect(colorButtons.first()).toBeVisible();
        const colorCount = await colorButtons.count();
        const randomIndex = Math.floor(Math.random() * colorCount);
        await colorButtons.nth(randomIndex).click();
        await page.getByRole('button', { name: 'Save Event' }).click();
        const eventCard = page.locator('[role="button"][draggable="true"]', { hasText: eventTitle });
        await expect(eventCard).toBeVisible();
        const box = await eventCard.boundingBox();
        if (!box) throw new Error('Event card not found');
        const dateRange = page.locator('.navbar-center span');
        const initialRange = await dateRange.textContent();
        const dayColumns = page.locator('[role="button"][aria-label^="Day column for"]');
        const lastColumn = dayColumns.nth(6);
        const lastColumnBox = await lastColumn.boundingBox();
        if (!lastColumnBox) throw new Error('Last column not found');
        const sourceX = box.x + box.width / 2;
        const sourceY = box.y + box.height / 2;
        await page.mouse.move(sourceX, sourceY);
        await page.mouse.down();
        const targetX = lastColumnBox.x + lastColumnBox.width / 2;
        const targetY = lastColumnBox.y + 200;
        await page.mouse.move(targetX, targetY, { steps: 50 });
        await page.waitForTimeout(1200);
        await expect(dateRange).not.toHaveText(initialRange!, { timeout: 2000 });
        const newDayColumns = page.locator('[role="button"][aria-label^="Day column for"]');
        const newLastColumn = newDayColumns.nth(6);
        const newLastColumnBox = await newLastColumn.boundingBox();
        if (!newLastColumnBox) throw new Error('New last column not found');
        let dropX = newLastColumnBox.x + newLastColumnBox.width / 2;
        let dropY = newLastColumnBox.y + 100;
        await page.mouse.move(dropX, dropY, { steps: 20 });
        await page.waitForTimeout(100);
        await page.mouse.up();
        await expect(page.locator('[role="button"][draggable="true"]', { hasText: eventTitle })).toBeVisible();
    });
});

test.describe('UI Layout and Overlap', () => {
    test.beforeEach(async ({ page }) => {
        await page.goto('/');
    });

    test('should display overlapping events side-by-side', async ({ page }) => {
        const eventTitle1 = `Overlap 1 ${Date.now()}`;
        const eventTitle2 = `Overlap 2 ${Date.now()}`;
        await page.getByRole('button', { name: 'Create Event' }).click();
        await page.getByPlaceholder('Event Title').fill(eventTitle1);
        await page.getByLabel('Start Time').fill('03:00');
        await page.getByLabel('End Time').fill('04:00');
        let colorButtons = page.locator('.modal-box button.rounded-full');
        let colorCount = await colorButtons.count();
        let randomIndex = Math.floor(Math.random() * colorCount);
        await colorButtons.nth(randomIndex).click();
        await page.getByRole('button', { name: 'Save Event' }).click();
        await page.getByRole('button', { name: 'Create Event' }).click();
        await page.getByPlaceholder('Event Title').fill(eventTitle2);
        await page.getByLabel('Start Time').fill('03:00');
        await page.getByLabel('End Time').fill('04:00');
        colorButtons = page.locator('.modal-box button.rounded-full');
        colorCount = await colorButtons.count();
        randomIndex = Math.floor(Math.random() * colorCount);
        await colorButtons.nth(randomIndex).click();
        await page.getByRole('button', { name: 'Save Event' }).click();
        const card1 = page.locator('[role="button"][draggable="true"]', { hasText: eventTitle1 });
        const card2 = page.locator('[role="button"][draggable="true"]', { hasText: eventTitle2 });
        await expect(card1).toBeVisible();
        await expect(card2).toBeVisible();
        const style1 = await card1.getAttribute('style');
        const style2 = await card2.getAttribute('style');
        const width1 = parseFloat(style1?.match(/width: ([\d.]+)%/)?.[1] || '0');
        const width2 = parseFloat(style2?.match(/width: ([\d.]+)%/)?.[1] || '0');
        const left1 = parseFloat(style1?.match(/left: ([\d.]+)%/)?.[1] || '0');
        const left2 = parseFloat(style2?.match(/left: ([\d.]+)%/)?.[1] || '0');
        expect(Math.abs(width1 - width2)).toBeLessThan(1);
        expect(width1).toBeLessThan(100);
        expect(Math.abs(left1 - left2)).toBeGreaterThan(0);
    });
});
