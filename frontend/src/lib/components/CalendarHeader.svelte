<script lang="ts">
    // Mock data to match the design reference
    // In a real app, this would be reactive based on the selected week
    const weekDays = [
        { name: "MON", number: 23, active: true },
        { name: "TUE", number: 24 },
        { name: "WED", number: 25 },
        { name: "THU", number: 26 },
        { name: "FRI", number: 27 },
        { name: "SAT", number: 28 },
        { name: "SUN", number: 29 },
    ];
</script>

<div class="grid grid-cols-7 border-b border-base-200 bg-base-100">
    {#each weekDays as day}
        <div
            class="flex flex-col items-center justify-center py-3 border-r border-base-200 last:border-r-0 {day.active
                ? 'bg-blue-50/30'
                : ''}"
        >
            <span
                class="text-xs font-bold mb-1 {day.active
                    ? 'text-blue-600'
                    : 'text-base-content/50'}"
            >
                {day.name}
            </span>
            <div
                class="{day.active
                    ? 'bg-blue-600 text-white shadow-md'
                    : 'text-base-content'} w-10 h-10 rounded-full flex items-center justify-center text-xl font-medium transition-all"
            >
                {day.number}
            </div>
        </div>
    {/each}
</div>
