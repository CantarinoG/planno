import { test, expect } from '@playwright/test';

test.use({
    launchOptions: {
        slowMo: 1000,
    },
});
/*
test.describe('Event CRUD operations', () => {
    test.beforeEach(async ({ page }) => {
        await page.goto('/');
    });

    test('should create a new event', async ({ page }) => {
        await page.getByRole('button', { name: 'Create Event' }).click();
        const eventTitle = `Test Event ${Date.now()}`;
        await page.getByPlaceholder('Event Title').fill(eventTitle);
        await page.getByLabel('Start Time').fill('02:00');
        await page.getByLabel('End Time').fill('03:00');
        await page.getByPlaceholder('Add notes, location, or video call links...').fill('Test Description');
        await page.getByRole('button', { name: 'Save Event' }).click();
        await expect(page.getByText(eventTitle)).toBeVisible();
        await expect(page.getByText('02:00 - 03:00')).toBeVisible();
    });

    test('should update an existing event', async ({ page }) => {
        const initialTitle = `Update Me ${Date.now()}`;
        await page.getByRole('button', { name: 'Create Event' }).click();
        await page.getByPlaceholder('Event Title').fill(initialTitle);
        await page.getByLabel('Start Time').fill('02:00');
        await page.getByLabel('End Time').fill('03:00');
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
        await page.getByLabel('Start Time').fill('02:00');
        await page.getByLabel('End Time').fill('03:00');
        await page.getByRole('button', { name: 'Save Event' }).click();
        await page.getByText(deleteTitle).click();
        const deleteButton = page.getByRole('dialog').getByRole('button', { name: 'Delete', exact: true });
        await deleteButton.click();
        const confirmDeleteButton = page.getByRole('button', { name: 'Delete', exact: true });
        await confirmDeleteButton.click();
        await expect(page.getByText(deleteTitle)).not.toBeVisible();
    });
});
*/
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
