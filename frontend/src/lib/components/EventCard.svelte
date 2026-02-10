<script lang="ts">
    import { getColorById } from "$lib/constants/colors";
    import { createEventDispatcher } from "svelte";

    export let title: string = "Event";
    export let startTime: string = "09:00";
    export let endTime: string = "10:00";
    export let color: string = "blue";
    export let description: string = "";
    export let isAllDay: boolean = false;
    export let top: number = 0; // Position in pixels from top
    export let height: number = 80; // Height in pixels
    export let column: number = 0; // Which column this event is in (for overlapping events)
    export let totalColumns: number = 1; // Total number of columns in this overlap group

    export let startAt: string = "";
    export let endAt: string = "";

    export let id: string | undefined = undefined;

    const dispatch = createEventDispatcher<{
        click: {
            id?: string;
            title: string;
            startTime: string;
            endTime: string;
            startAt: string;
            endAt: string;
            color: string;
            description: string;
            isAllDay: boolean;
        };
    }>();

    function handleCardClick(e: MouseEvent): void {
        e.stopPropagation();
        dispatch("click", {
            id,
            title,
            startTime,
            endTime,
            startAt,
            endAt,
            color,
            description,
            isAllDay,
        });
    }

    function handleDragStart(e: DragEvent): void {
        if (id && e.dataTransfer) {
            e.dataTransfer.setData(
                "application/json",
                JSON.stringify({
                    id,
                    durationMinutes: calculateDurationMinutes(
                        startTime,
                        endTime,
                    ),
                }),
            );
            e.dataTransfer.effectAllowed = "move";
        }
    }

    function calculateDurationMinutes(start: string, end: string): number {
        const [startH, startM] = start.split(":").map(Number);
        const [endH, endM] = end.split(":").map(Number);
        return endH * 60 + endM - (startH * 60 + startM);
    }

    $: bgColor = getColorById(color).bgClass;

    $: widthPercent = 100 / totalColumns;
    $: leftPercent = (100 / totalColumns) * column;
</script>

<div
    role="button"
    tabindex="0"
    draggable="true"
    class="absolute rounded-md shadow-md cursor-pointer transition-all hover:shadow-lg hover:z-20 {bgColor} text-white px-2 py-1 overflow-hidden"
    style="top: {top}px; height: {height}px; left: {leftPercent}%; width: {widthPercent}%;"
    onclick={handleCardClick}
    ondragstart={handleDragStart}
    onkeydown={(e) => {
        if (e.key === "Enter" || e.key === " ") {
            handleCardClick(e as any);
        }
    }}
>
    <div class="text-xs font-semibold truncate">{title}</div>
    {#if height > 30}
        <div class="text-[10px] opacity-90 mt-0.5">
            {startTime} - {endTime}
        </div>
    {/if}
</div>
