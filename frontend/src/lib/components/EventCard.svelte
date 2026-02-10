<script lang="ts">
    export let title: string = "Event";
    export let startTime: string = "09:00";
    export let endTime: string = "10:00";
    export let color: string = "blue";
    export let top: number = 0; // Position in pixels from top
    export let height: number = 80; // Height in pixels
    export let column: number = 0; // Which column this event is in (for overlapping events)
    export let totalColumns: number = 1; // Total number of columns in this overlap group

    const colorClasses: Record<string, string> = {
        blue: "bg-blue-500 hover:bg-blue-600",
        green: "bg-green-500 hover:bg-green-600",
        orange: "bg-orange-400 hover:bg-orange-500",
        purple: "bg-purple-500 hover:bg-purple-600",
        red: "bg-red-500 hover:bg-red-600",
        teal: "bg-teal-500 hover:bg-teal-600",
    };

    $: bgColor = colorClasses[color] || colorClasses.blue;

    $: widthPercent = 100 / totalColumns;
    $: leftPercent = (100 / totalColumns) * column;
</script>

<div
    class="absolute rounded-md shadow-md cursor-pointer transition-all hover:shadow-lg hover:z-20 {bgColor} text-white px-2 py-1 overflow-hidden"
    style="top: {top}px; height: {height}px; left: {leftPercent}%; width: {widthPercent}%;"
>
    <div class="text-xs font-semibold truncate">{title}</div>
    {#if height > 30}
        <div class="text-[10px] opacity-90 mt-0.5">
            {startTime} - {endTime}
        </div>
    {/if}
</div>
