<script lang="ts">
    import CalendarHeader from "$lib/components/CalendarHeader.svelte";
    import EventCard from "$lib/components/EventCard.svelte";
    import { onMount, onDestroy, createEventDispatcher } from "svelte";
    import {
        format,
        getHours,
        getMinutes,
        getDay,
        setMinutes,
        setHours,
        addMinutes,
        addDays,
    } from "date-fns";

    const HOUR_HEIGHT = 80;

    export let weekStart: Date = new Date();
    export let events: any[] = [];

    const dispatch = createEventDispatcher<{
        cellclick: { date: Date };
        eventclick: { event: any };
        eventmove: { id: string; startAt: string; endAt: string };
    }>();

    function handleDragOver(e: DragEvent): void {
        e.preventDefault();
        if (e.dataTransfer) {
            e.dataTransfer.dropEffect = "move";
        }
    }

    function handleDrop(e: DragEvent, dayIndex: number): void {
        e.preventDefault();
        const data = e.dataTransfer?.getData("application/json");
        if (!data) return;

        try {
            const { id, durationMinutes } = JSON.parse(data);

            // Get coordinates relative to the column
            const rect = (
                e.currentTarget as HTMLElement
            ).getBoundingClientRect();
            const y = e.clientY - rect.top;

            // Calculate time
            const quarters = Math.round(y / (HOUR_HEIGHT / 4));
            const totalMinutes = quarters * 15;
            const hours = Math.floor(totalMinutes / 60);
            const minutes = totalMinutes % 60;

            const targetDay = addDays(weekStart, dayIndex);
            const newStart = setMinutes(setHours(targetDay, hours), minutes);
            const newEnd = addMinutes(newStart, durationMinutes);

            dispatch("eventmove", {
                id,
                startAt: newStart.toISOString(),
                endAt: newEnd.toISOString(),
            });
        } catch (err) {
            console.error("Failed to process drop:", err);
        }
    }

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

    type LayoutEvent = {
        id?: string;
        title: string;
        startTime: string; // HH:mm for display
        endTime: string; // HH:mm for display
        startAt: string; // Full ISO
        endAt: string; // Full ISO
        color: string;
        description?: string;
        isAllDay?: boolean;
        dayIndex: number;
        top: number;
        height: number;
        column?: number;
        totalColumns?: number;
    };

    function eventsOverlap(event1: LayoutEvent, event2: LayoutEvent): boolean {
        const end1: number = event1.top + event1.height;
        const end2: number = event2.top + event2.height;
        return event1.top < end2 && end1 > event2.top;
    }

    function calculateEventLayout(backendEvents: any[]): LayoutEvent[] {
        const processedEvents: LayoutEvent[] = backendEvents.map((event) => {
            const start = new Date(event.startAt);
            const end = new Date(event.endAt);

            return {
                ...event,
                startTime: format(start, "HH:mm"),
                endTime: format(end, "HH:mm"),
                dayIndex: getDay(start),
                top: timeToPixels(getHours(start), getMinutes(start)),
                height: durationToPixels(
                    getHours(start),
                    getMinutes(start),
                    getHours(end),
                    getMinutes(end),
                ),
            };
        });

        const eventsByDay: Record<number, LayoutEvent[]> = {};
        processedEvents.forEach((event) => {
            if (!eventsByDay[event.dayIndex]) {
                eventsByDay[event.dayIndex] = [];
            }
            eventsByDay[event.dayIndex].push(event);
        });

        const layoutEvents: LayoutEvent[] = [];
        Object.values(eventsByDay).forEach((dayEvents: LayoutEvent[]) => {
            const sorted: LayoutEvent[] = [...dayEvents].sort(
                (a: LayoutEvent, b: LayoutEvent): number => {
                    if (a.top !== b.top) return a.top - b.top;
                    return b.height - a.height;
                },
            );

            const columns: LayoutEvent[][] = [];
            sorted.forEach((event: LayoutEvent) => {
                let placed: boolean = false;
                for (let i: number = 0; i < columns.length; i++) {
                    const lastEventInColumn = columns[i][columns[i].length - 1];
                    if (!eventsOverlap(lastEventInColumn, event)) {
                        columns[i].push(event);
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

            sorted.forEach((event: LayoutEvent) => {
                event.totalColumns = columns.length;
                layoutEvents.push(event);
            });
        });

        return layoutEvents;
    }

    $: layoutedEvents = calculateEventLayout(events);

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
                            ondragover={handleDragOver}
                            ondrop={(e) => handleDrop(e, i)}
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
                                    id={event.id}
                                    title={event.title}
                                    startTime={event.startTime}
                                    endTime={event.endTime}
                                    startAt={event.startAt}
                                    endAt={event.endAt}
                                    color={event.color}
                                    description={event.description || ""}
                                    isAllDay={event.isAllDay || false}
                                    top={event.top}
                                    height={event.height}
                                    column={event.column ?? 0}
                                    totalColumns={event.totalColumns ?? 1}
                                    on:click={(e) =>
                                        dispatch("eventclick", {
                                            event: e.detail,
                                        })}
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
