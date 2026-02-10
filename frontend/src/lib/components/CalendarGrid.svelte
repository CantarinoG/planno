<script lang="ts">
    import CalendarHeader from "$lib/components/CalendarHeader.svelte";
    import { onMount, onDestroy } from "svelte";
    import { isSameDay } from "date-fns";

    export let weekStart = new Date(); // Default if not provided

    let currentTime = new Date();
    let intervalId: number | undefined;

    // Calculate if current time is within the displayed week
    $: isCurrentWeekDisplayed = (() => {
        const now = new Date();
        const weekEnd = new Date(weekStart);
        weekEnd.setDate(weekEnd.getDate() + 7);
        return now >= weekStart && now < weekEnd;
    })();

    // Calculate the top position based on current time (in pixels)
    // Each hour is 80px (h-20 = 5rem = 80px)
    $: currentTimeTop = (() => {
        const hours = currentTime.getHours();
        const minutes = currentTime.getMinutes();
        const totalMinutes = hours * 60 + minutes;
        const pixelsPerMinute = 80 / 60; // 80px per hour / 60 minutes
        return totalMinutes * pixelsPerMinute;
    })();

    onMount(() => {
        // Update current time every minute
        intervalId = window.setInterval(() => {
            currentTime = new Date();
        }, 60000); // Update every 60 seconds

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

<div class="flex-1 flex flex-col overflow-hidden bg-base-100">
    <!-- Header Area -->
    <div class="flex border-b border-base-200">
        <!-- Spacer for TimeGutter alignment -->
        <div
            class="w-16 lg:w-20 border-r border-base-200 bg-base-100 flex-shrink-0"
        ></div>

        <!-- The Calendar Header -->
        <div class="flex-1">
            <CalendarHeader {weekStart} />
        </div>

        <!-- Scrollbar spacer to prevent misalignment -->
        <div
            class="w-[15px] border-l border-base-200 bg-base-100 flex-shrink-0"
        ></div>
    </div>

    <!-- Scrollable Grid Area -->
    <div
        class="flex-1 overflow-y-auto flex flex-col relative scrollbar-gutter-stable"
    >
        <div class="flex">
            <!-- Time Gutter -->
            <div
                class="w-16 lg:w-20 border-r border-base-200 flex flex-col items-center text-xs font-medium text-base-content/50 bg-base-100 flex-shrink-0"
            >
                {#each Array(24) as _, i}
                    <!-- Height matches grid row height (h-20) -->
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

            <!-- Main Grid -->
            <div class="flex-1 grid grid-cols-7 relative min-w-[600px]">
                <!-- Day columns -->
                {#each Array(7) as _, i}
                    <div
                        class="border-r border-base-200 last:border-r-0 relative {i ===
                        0
                            ? 'bg-blue-50/10'
                            : ''}"
                    >
                        <!-- Hour rows -->
                        {#each Array(24) as _}
                            <div
                                class="h-20 border-b border-base-200/50 box-border"
                            ></div>
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

<style>
    /* Ensure the grid doesn't collapse on small screens */
    .min-w-\[600px\] {
        min-width: 600px;
    }
</style>
