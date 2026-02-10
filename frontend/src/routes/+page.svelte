<script lang="ts">
    import Navbar from "$lib/components/Navbar.svelte";
    import Sidebar from "$lib/components/Sidebar.svelte";
    import CalendarGrid from "$lib/components/CalendarGrid.svelte";
    import EventModal from "$lib/components/EventModal.svelte";
    import { fly } from "svelte/transition";
    import {
        addWeeks,
        subWeeks,
        startOfWeek,
        endOfWeek,
        format,
        isSameMonth,
    } from "date-fns";
    import {
        createCalendarEvent,
        getCalendarEvents,
        deleteCalendarEvent,
        updateCalendarEvent,
        type CalendarEvent,
    } from "$lib/api";
    import { isLoading, addToast } from "$lib/stores";

    const DEFAULT_COLOR_ID: string = "blue";

    let isSidebarOpen: boolean = true;
    let isEventModalOpen: boolean = false;
    let currentDate: Date = new Date();
    let selectedDateStr: string = format(new Date(), "yyyy-MM-dd");
    let events: CalendarEvent[] = [];

    // Edit fields
    let editEventId: string | undefined = undefined;
    let editTitle: string = "";
    let editDescription: string = "";
    let editColorId: string = DEFAULT_COLOR_ID;
    let editEndDate: string = "";
    let editIsAllDay: boolean = false;

    async function loadEvents(): Promise<void> {
        isLoading.set(true);
        try {
            const start = weekStart.toISOString();
            const end = weekEnd.toISOString();
            console.log(`Fetching events from ${start} to ${end}`);
            events = await getCalendarEvents(start, end);
            console.log("Fetched events:", events);
        } catch (error) {
            console.error("Failed to load events:", error);
            addToast("Failed to load events", "error");
        } finally {
            isLoading.set(false);
        }
    }

    function openEventModal(): void {
        selectedDateStr = format(currentDate, "yyyy-MM-dd");
        editEventId = undefined;
        editTitle = "";
        editDescription = "";
        editColorId = DEFAULT_COLOR_ID;
        editEndDate = "";
        editIsAllDay = false;
        isEventModalOpen = true;
    }

    function handleCellClick(event: CustomEvent<{ date: Date }>): void {
        selectedDateStr = format(event.detail.date, "yyyy-MM-dd'T'HH:mm");
        editEventId = undefined;
        editTitle = "";
        editDescription = "";
        editColorId = DEFAULT_COLOR_ID;
        editEndDate = "";
        editIsAllDay = false;
        isEventModalOpen = true;
    }

    function handleEventClick(
        event: CustomEvent<{ event: CalendarEvent }>,
    ): void {
        const detail = event.detail.event;
        selectedDateStr = detail.startAt;

        editEventId = detail.id;
        editTitle = detail.title;
        editDescription = detail.description || "";
        editColorId = detail.color;
        editIsAllDay = detail.isAllDay || false;
        editEndDate = detail.endAt;

        isEventModalOpen = true;
    }

    function toggleSidebar(): void {
        isSidebarOpen = !isSidebarOpen;
    }

    function nextWeek(): void {
        currentDate = addWeeks(currentDate, 1);
    }

    function prevWeek(): void {
        currentDate = subWeeks(currentDate, 1);
    }

    function goToToday(): void {
        currentDate = new Date();
    }

    function closeEventModal(): void {
        isEventModalOpen = false;
    }

    async function saveEvent(
        e: CustomEvent<{
            title: string;
            isAllDay: boolean;
            startDate: string;
            endDate: string;
            description: string;
            color: string;
        }>,
    ): Promise<void> {
        const detail = e.detail;
        console.log("Saving event to backend:", detail);
        isLoading.set(true);

        try {
            const eventPayload: Omit<CalendarEvent, "id"> = {
                title: detail.title,
                description: detail.description,
                startAt: new Date(detail.startDate).toISOString(),
                endAt: new Date(detail.endDate).toISOString(),
                color: detail.color,
                isAllDay: detail.isAllDay,
            };

            if (editEventId) {
                await updateCalendarEvent(editEventId, eventPayload);
                console.log("Event updated successfully");
                addToast("Event updated successfully", "success");
            } else {
                const newEvent = await createCalendarEvent(eventPayload);
                console.log("Event created successfully:", newEvent);
                addToast("Event created successfully", "success");
            }
            await loadEvents(); // Refresh list
        } catch (error) {
            console.error("Failed to save event:", error);
            addToast("Failed to save event. Please try again.", "error");
        } finally {
            isLoading.set(false);
        }

        isEventModalOpen = false;
    }

    async function handleDelete(): Promise<void> {
        if (!editEventId) return;

        if (confirm("Are you sure you want to delete this event?")) {
            isLoading.set(true);
            try {
                await deleteCalendarEvent(editEventId);
                console.log("Event deleted successfully");
                addToast("Event deleted successfully", "success");
                await loadEvents(); // Refresh list
            } catch (error) {
                console.error("Failed to delete event:", error);
                addToast("Failed to delete event. Please try again.", "error");
            } finally {
                isLoading.set(false);
            }
        }
        isEventModalOpen = false;
    }

    async function handleEventMove(
        event: CustomEvent<{
            id: string;
            startAt: string;
            endAt: string;
            eventData: CalendarEvent;
        }>,
    ): Promise<void> {
        const { id, startAt, endAt, eventData } = event.detail;
        console.log(`Moving event ${id} to ${startAt} - ${endAt}`);
        isLoading.set(true);

        try {
            await updateCalendarEvent(id, {
                ...eventData,
                startAt,
                endAt,
            });
            console.log("Event moved successfully");
            addToast("Event moved successfully", "success");
            await loadEvents(); // Refresh list
        } catch (error) {
            console.error("Failed to move event:", error);
            addToast("Failed to move event. Please try again.", "error");
        } finally {
            isLoading.set(false);
        }
    }

    $: weekStart = startOfWeek(currentDate, { weekStartsOn: 0 });
    $: weekEnd = endOfWeek(currentDate, { weekStartsOn: 0 });

    $: if (weekStart && weekEnd) {
        loadEvents();
    }

    $: dateRangeString = ((): string => {
        const startFormat: string = "MMM d";
        const endFormat: string = isSameMonth(weekStart, weekEnd)
            ? "d, yyyy"
            : "MMM d, yyyy";
        return `${format(weekStart, startFormat)} – ${format(weekEnd, endFormat)}`;
    })();

    // Format date for MiniCalendar (YYYY-MM-DD)
    $: formattedDate = format(currentDate, "yyyy-MM-dd");

    // Date change in MiniCalendar
    function handleDateChange(event: CustomEvent<string>): void {
        const [year, month, day]: number[] = event.detail
            .split("-")
            .map(Number);
        const newDate: Date = new Date(year, month - 1, day);

        if (!isNaN(newDate.getTime())) {
            currentDate = newDate;
        } else {
            console.error("Invalid date received:", event.detail);
        }
    }
</script>

<div class="h-screen flex flex-col overflow-hidden">
    <Navbar
        {toggleSidebar}
        {dateRangeString}
        onNext={nextWeek}
        onPrev={prevWeek}
        onToday={goToToday}
    />

    <main class="flex-1 flex overflow-hidden relative">
        {#if isSidebarOpen}
            <!-- Mobile Backdrop -->
            <div
                class="absolute inset-0 bg-black/50 z-40 lg:hidden"
                onclick={toggleSidebar}
                aria-hidden="true"
                transition:fly={{ duration: 200 }}
            ></div>

            <div
                class="absolute inset-y-0 left-0 z-50 h-full lg:static lg:z-auto shadow-xl lg:shadow-none bg-base-100"
                transition:fly={{ x: -288, duration: 200 }}
            >
                <Sidebar
                    date={selectedDateStr.split("T")[0]}
                    on:datechange={handleDateChange}
                    on:create={openEventModal}
                />
            </div>
        {/if}
        <CalendarGrid
            {weekStart}
            {events}
            on:cellclick={handleCellClick}
            on:eventclick={handleEventClick}
            on:eventmove={handleEventMove}
            on:navprev={prevWeek}
            on:navnext={nextWeek}
        />
    </main>

    <EventModal
        bind:isOpen={isEventModalOpen}
        date={selectedDateStr}
        initialTitle={editTitle}
        initialDescription={editDescription}
        initialColorId={editColorId}
        initialEndDate={editEndDate}
        initialIsAllDay={editIsAllDay}
        on:close={closeEventModal}
        on:save={saveEvent}
        on:delete={handleDelete}
    />
</div>
