import { test, expect } from '@playwright/test';

test.use({
    launchOptions: {
        slowMo: 1000,
    },
});

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
