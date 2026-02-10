<script lang="ts">
    import CalendarHeader from "$lib/components/CalendarHeader.svelte";
    import EventCard from "$lib/components/EventCard.svelte";
    import { onMount, onDestroy, createEventDispatcher } from "svelte";

    export let weekStart: Date = new Date();

    const dispatch = createEventDispatcher<{
        cellclick: { date: Date };
    }>();

    let currentTime: Date = new Date();
    let intervalId: number | undefined;

    function handleCellClick(dayIndex: number, event: MouseEvent): void {
        const rect = (
            event.currentTarget as HTMLElement
        ).getBoundingClientRect();
        const y = event.clientY - rect.top;
        const pixelsPerHour = 80;
        const totalMinutes = (y / pixelsPerHour) * 60;

        const roundedMinutes = Math.round(totalMinutes / 15) * 15;
        const hours = Math.floor(roundedMinutes / 60);
        const minutes = roundedMinutes % 60;

        const clickedDate = new Date(weekStart);
        clickedDate.setDate(weekStart.getDate() + dayIndex);
        clickedDate.setHours(hours, minutes, 0, 0);

        dispatch("cellclick", { date: clickedDate });
    }

    function timeToPixels(hours: number, minutes: number): number {
        const totalMinutes = hours * 60 + minutes;
        const pixelsPerMinute = 80 / 60; // 80px per hour
        return totalMinutes * pixelsPerMinute;
    }

    function durationToPixels(
        startHour: number,
        startMin: number,
        endHour: number,
        endMin: number,
    ): number {
        const startMinutes = startHour * 60 + startMin;
        const endMinutes = endHour * 60 + endMin;
        const durationMinutes = endMinutes - startMinutes;
        const pixelsPerMinute = 80 / 60;
        return durationMinutes * pixelsPerMinute;
    }

    // Mock events for demonstration
    type CalendarEvent = {
        title: string;
        startTime: string;
        endTime: string;
        color: string;
        dayIndex: number; // 0-6 for Sunday-Saturday
        top: number;
        height: number;
        // Layout properties for overlap handling
        column?: number;
        totalColumns?: number;
    };

    function eventsOverlap(
        event1: CalendarEvent,
        event2: CalendarEvent,
    ): boolean {
        const end1: number = event1.top + event1.height;
        const end2: number = event2.top + event2.height;
        return event1.top < end2 && end1 > event2.top;
    }

    function calculateEventLayout(events: CalendarEvent[]): CalendarEvent[] {
        const eventsByDay: Record<number, CalendarEvent[]> = {};
        events.forEach((event) => {
            if (!eventsByDay[event.dayIndex]) {
                eventsByDay[event.dayIndex] = [];
            }
            eventsByDay[event.dayIndex].push(event);
        });

        const layoutEvents: CalendarEvent[] = [];
        Object.values(eventsByDay).forEach((dayEvents: CalendarEvent[]) => {
            const sorted: CalendarEvent[] = [...dayEvents].sort(
                (a: CalendarEvent, b: CalendarEvent): number => {
                    if (a.top !== b.top) return a.top - b.top;
                    return b.height - a.height;
                },
            );

            const columns: CalendarEvent[][] = [];
            sorted.forEach((event: CalendarEvent) => {
                let placed: boolean = false;
                for (let i: number = 0; i < columns.length; i++) {
                    const columnEvents: CalendarEvent[] = columns[i];
                    const hasOverlap: boolean = columnEvents.some(
                        (e: CalendarEvent) => eventsOverlap(e, event),
                    );
                    if (!hasOverlap) {
                        columnEvents.push(event);
                        event.column = i;
                        placed = true;
                        break;
                    }
                }
                if (!placed) {
                    event.column = columns.length;
                    columns.push([event]);
                }
            });
            sorted.forEach((event: CalendarEvent) => {
                const overlapping: CalendarEvent[] = sorted.filter(
                    (e: CalendarEvent) =>
                        eventsOverlap(e, event) || eventsOverlap(event, e),
                );
                const maxColumn: number = Math.max(
                    ...overlapping.map((e: CalendarEvent) => e.column ?? 0),
                );
                event.totalColumns = maxColumn + 1;
            });

            layoutEvents.push(...sorted);
        });

        return layoutEvents;
    }

    const mockEvents: CalendarEvent[] = [
        {
            title: "Team Sync",
            startTime: "08:00",
            endTime: "10:00",
            color: "blue",
            dayIndex: 0,
            top: timeToPixels(8, 0),
            height: durationToPixels(8, 0, 10, 0),
        },
        {
            title: "Team Sync 2",
            startTime: "09:00",
            endTime: "10:30",
            color: "orange",
            dayIndex: 0,
            top: timeToPixels(9, 0),
            height: durationToPixels(9, 0, 10, 30),
        },
        {
            title: "Team Sync 3",
            startTime: "09:00",
            endTime: "11:00",
            color: "green",
            dayIndex: 0,
            top: timeToPixels(9, 0),
            height: durationToPixels(9, 0, 11, 0),
        },
        {
            title: "Project Workshop",
            startTime: "09:00",
            endTime: "12:00",
            color: "blue",
            dayIndex: 2,
            top: timeToPixels(9, 0),
            height: durationToPixels(9, 0, 12, 0),
        },
        {
            title: "Mom's Birthday",
            startTime: "All Day Event",
            endTime: "",
            color: "orange",
            dayIndex: 3,
            top: timeToPixels(0, 0),
            height: 40,
        },
        {
            title: "Client Lunch",
            startTime: "13:00",
            endTime: "14:00",
            color: "blue",
            dayIndex: 0,
            top: timeToPixels(13, 0),
            height: durationToPixels(13, 0, 14, 0),
        },
        {
            title: "Gym Session",
            startTime: "15:00",
            endTime: "17:00",
            color: "green",
            dayIndex: 1,
            top: timeToPixels(15, 0),
            height: durationToPixels(15, 0, 17, 0),
        },
        {
            title: "Weekly Review",
            startTime: "15:00",
            endTime: "16:30",
            color: "blue",
            dayIndex: 4,
            top: timeToPixels(15, 0),
            height: durationToPixels(15, 0, 16, 30),
        },
        {
            title: "Dinner Party",
            startTime: "18:00",
            endTime: "20:00",
            color: "green",
            dayIndex: 5,
            top: timeToPixels(18, 0),
            height: durationToPixels(18, 0, 20, 0),
        },
    ];

    $: layoutedEvents = calculateEventLayout(mockEvents) as CalendarEvent[];

    $: isCurrentWeekDisplayed = (() => {
        const now: Date = new Date();
        const weekEnd: Date = new Date(weekStart);
        weekEnd.setDate(weekEnd.getDate() + 7);
        return now >= weekStart && now < weekEnd;
    })() as boolean;

    $: currentTimeTop = (() => {
        const hours: number = currentTime.getHours();
        const minutes: number = currentTime.getMinutes();
        const totalMinutes: number = hours * 60 + minutes;
        const pixelsPerMinute: number = 80 / 60;
        return totalMinutes * pixelsPerMinute;
    })() as number;

    onMount(() => {
        intervalId = window.setInterval(() => {
            currentTime = new Date();
        }, 60000);

        return () => {
            if (intervalId !== undefined) {
                clearInterval(intervalId);
            }
        };
    });

    onDestroy(() => {
        if (intervalId !== undefined) {
            clearInterval(intervalId);
        }
    });
</script>

<div
    class="flex-1 flex flex-col overflow-x-auto bg-base-100 scrollbar-gutter-stable"
>
    <div class="min-w-[700px] flex-1 flex flex-col">
        <div class="flex border-b border-base-300">
            <!-- Spacer for TimeGutter alignment -->
            <div
                class="w-16 lg:w-20 border-r border-base-300 bg-base-100 flex-shrink-0"
            ></div>

            <div class="flex-1">
                <CalendarHeader {weekStart} />
            </div>
        </div>

        <!-- Scrollable Grid Area -->
        <div class="flex-1 overflow-y-auto flex flex-col relative">
            <div class="flex">
                <!-- Time Gutter -->
                <div
                    class="w-16 lg:w-20 border-r border-base-300 flex flex-col items-center text-xs font-medium text-base-content/50 bg-base-100 flex-shrink-0"
                >
                    {#each Array(24) as _, i}
                        <div class="h-20 relative w-full text-center">
                            <span class="block -mt-2.5"
                                >{i === 0
                                    ? ""
                                    : i < 12
                                      ? `${i} AM`
                                      : i === 12
                                        ? "12 PM"
                                        : `${i - 12} PM`}</span
                            >
                        </div>
                    {/each}
                </div>

                <div class="flex-1 grid grid-cols-7 relative">
                    <!-- Day columns -->
                    {#each Array(7) as _, i}
                        <div
                            role="button"
                            aria-label="Create event in day column"
                            tabindex="0"
                            class="border-r border-base-300 last:border-r-0 relative cursor-pointer {i ===
                            0
                                ? 'bg-blue-50/10'
                                : ''}"
                            onclick={(e) => handleCellClick(i, e)}
                            onkeydown={(e) => {
                                if (e.key === "Enter" || e.key === " ") {
                                    handleCellClick(i, e as any);
                                }
                            }}
                        >
                            <!-- Hour rows -->
                            {#each Array(24) as _}
                                <div
                                    class="h-20 border-b border-base-300/50 box-border"
                                ></div>
                            {/each}

                            {#each layoutedEvents.filter((e) => e.dayIndex === i) as event}
                                <EventCard
                                    title={event.title}
                                    startTime={event.startTime}
                                    endTime={event.endTime}
                                    color={event.color}
                                    top={event.top}
                                    height={event.height}
                                    column={event.column ?? 0}
                                    totalColumns={event.totalColumns ?? 1}
                                />
                            {/each}
                        </div>
                    {/each}

                    <!-- Current Time Indicator (Dynamic) -->
                    {#if isCurrentWeekDisplayed}
                        <div
                            class="absolute left-0 right-0 flex items-center pointer-events-none z-10"
                            style="top: {currentTimeTop}px;"
                        >
                            <div
                                class="w-2 h-2 rounded-full bg-red-500 -ml-1 absolute left-0"
                            ></div>
                            <div class="h-[2px] bg-red-500 flex-1 ml-1"></div>
                        </div>
                    {/if}
                </div>
            </div>
        </div>
    </div>
</div>

<style>
</style>
