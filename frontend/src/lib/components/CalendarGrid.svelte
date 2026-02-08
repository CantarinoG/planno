<div class="flex-1 flex flex-col overflow-hidden bg-base-100">
    <div
        class="placeholder border-b border-base-200 h-20 flex items-center justify-center font-bold"
    >
        CalendarHeader Placeholder
    </div>

    <div class="flex-1 overflow-y-auto flex">
        <div
            class="w-20 border-r border-base-200 flex flex-col items-center py-4 text-xs opacity-50"
        >
            TimeGutter
        </div>
        <div class="flex-1 grid grid-cols-7 relative">
            <!-- Day columns will go here -->
            {#each Array(7) as _}
                <div
                    class="border-r border-base-200 last:border-r-0 min-h-[1000px]"
                ></div>
            {/each}

            <div
                class="absolute inset-0 pointer-events-none flex items-center justify-center text-xl opacity-10"
            >
                Weekly Grid Placeholder
            </div>
        </div>
    </div>
</div>
