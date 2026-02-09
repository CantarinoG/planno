<script>
    import CalendarHeader from "$lib/components/CalendarHeader.svelte";
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
            <CalendarHeader />
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

                <!-- Current Time Indicator (Mock) -->
                <div
                    class="absolute left-0 right-0 top-[310px] flex items-center pointer-events-none z-10"
                >
                    <div
                        class="w-2 h-2 rounded-full bg-red-500 -ml-1 absolute left-0"
                    ></div>
                    <div class="h-[2px] bg-red-500 flex-1 ml-1"></div>
                </div>
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
