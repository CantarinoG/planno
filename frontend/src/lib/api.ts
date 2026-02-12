import { addToast } from "./stores";

const API_BASE_URL = 'http://localhost:5000/api';

export interface CalendarEvent {
    id?: string;
    title: string;
    description: string;
    startAt: string; // ISO string
    endAt: string;   // ISO string
    color: string;
    createdAt?: string;
    updatedAt?: string;
}

/**
 * Standardizes API error messages and triggers global notifications.
 */
async function httpRequest<T>(
    endpoint: string,
    method: string = 'GET',
    body?: any,
    options: RequestInit = {}
): Promise<T> {
    const url = `${API_BASE_URL}/${endpoint}`;
    const headers = {
        'Content-Type': 'application/json',
        ...options.headers,
    };

    try {
        const response = await fetch(url, {
            method,
            headers,
            body: body ? JSON.stringify(body) : undefined,
            ...options
        });

        if (!response.ok) {
            let errorMessage = response.statusText;
            try {
                const errorData = await response.json();
                errorMessage = errorData.message || errorData.error || errorMessage;
            } catch {
                errorMessage = await response.text() || errorMessage;
            }

            // Centralized Error Handling
            if (response.status === 401) {
                addToast("Unauthorized: Please log in again.", "error");
            } else if (response.status === 403) {
                addToast("Forbidden: You don't have permission for this action.", "error");
            } else {
                addToast(`API Error: ${errorMessage}`, "error");
            }

            throw new Error(errorMessage);
        }

        if (response.status === 204) return {} as T;
        return response.json();
    } catch (error: any) {
        // Network errors (e.g., DNS, Refused connection)
        if (error instanceof TypeError && error.message.includes('fetch')) {
            addToast("Network Error: Could not connect to the server.", "error");
        }
        throw error;
    }
}

export async function createCalendarEvent(event: Omit<CalendarEvent, 'id' | 'createdAt' | 'updatedAt'>): Promise<CalendarEvent> {
    return httpRequest<CalendarEvent>('Events', 'POST', event);
}

export async function getCalendarEvents(startDate?: string, endDate?: string): Promise<CalendarEvent[]> {
    const params = new URLSearchParams();
    if (startDate) params.append('start_date', startDate);
    if (endDate) params.append('end_date', endDate);

    const queryString = params.toString();
    return httpRequest<CalendarEvent[]>(`Events${queryString ? `?${queryString}` : ''}`);
}

export async function deleteCalendarEvent(id: string): Promise<void> {
    return httpRequest<void>(`Events/${id}`, 'DELETE');
}

export async function updateCalendarEvent(id: string, event: Partial<CalendarEvent>): Promise<void> {
    return httpRequest<void>(`Events/${id}`, 'PUT', event);
}
