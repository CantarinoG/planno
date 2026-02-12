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

export async function createCalendarEvent(event: Omit<CalendarEvent, 'id' | 'createdAt' | 'updatedAt'>): Promise<CalendarEvent> {
    const response = await fetch(`${API_BASE_URL}/Events`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(event),
    });

    if (!response.ok) {
        const errorText = await response.text();
        throw new Error(`Failed to create event: ${errorText || response.statusText}`);
    }

    return response.json();
}

export async function getCalendarEvents(startDate?: string, endDate?: string): Promise<CalendarEvent[]> {
    const params = new URLSearchParams();
    if (startDate) params.append('start_date', startDate);
    if (endDate) params.append('end_date', endDate);

    const url = `${API_BASE_URL}/Events${params.toString() ? `?${params.toString()}` : ''}`;
    const response = await fetch(url);

    if (!response.ok) {
        const errorText = await response.text();
        throw new Error(`Failed to fetch events: ${errorText || response.statusText}`);
    }

    return response.json();
}

export async function deleteCalendarEvent(id: string): Promise<void> {
    const response = await fetch(`${API_BASE_URL}/Events/${id}`, {
        method: 'DELETE',
    });

    if (!response.ok) {
        const errorText = await response.text();
        throw new Error(`Failed to delete event: ${errorText || response.statusText}`);
    }
}

export async function updateCalendarEvent(id: string, event: Partial<CalendarEvent>): Promise<void> {
    const response = await fetch(`${API_BASE_URL}/Events/${id}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(event),
    });

    if (!response.ok) {
        const errorText = await response.text();
        throw new Error(`Failed to update event: ${errorText || response.statusText}`);
    }
}
