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

    let isSidebarOpen: boolean = true;
    let isEventModalOpen: boolean = false;
    let currentDate: Date = new Date();
    let selectedDateStr: string = format(new Date(), "yyyy-MM-dd");

    function openEventModal(): void {
        selectedDateStr = format(currentDate, "yyyy-MM-dd");
        isEventModalOpen = true;
    }

    function handleCellClick(event: CustomEvent<{ date: Date }>): void {
        selectedDateStr = format(event.detail.date, "yyyy-MM-dd'T'HH:mm");
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

    function saveEvent(e: CustomEvent): void {
        console.log("Event saved:", e.detail);
        isEventModalOpen = false;
    }

    $: weekStart = startOfWeek(currentDate, { weekStartsOn: 0 });
    $: weekEnd = endOfWeek(currentDate, { weekStartsOn: 0 });

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
        <CalendarGrid {weekStart} on:cellclick={handleCellClick} />
    </main>

    <EventModal
        bind:isOpen={isEventModalOpen}
        date={selectedDateStr}
        on:close={closeEventModal}
        on:save={saveEvent}
    />
</div>
