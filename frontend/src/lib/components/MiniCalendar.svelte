<script lang="ts">
    import { createEventDispatcher, onMount } from "svelte";

    export let value: string = new Date().toISOString().split("T")[0];
    const dispatch = createEventDispatcher<{
        datechange: string;
    }>();

    let calendar: HTMLElement;

    function handleChange(event: any): void {
        const newValue: string = (event.target as any).value;
        dispatch("datechange", newValue);
    }

    onMount((): (() => void) => {
        let cleanup: () => void = () => {};
        (async (): Promise<void> => {
            await import("cally");
            if (calendar) {
                calendar.addEventListener("change", handleChange);
                cleanup = () => {
                    if (calendar) {
                        calendar.removeEventListener("change", handleChange);
                    }
                };
            }
        })();
        return () => cleanup();
    });
</script>

<div class="calendar-wrapper">
    <!-- Cally Calendar Component -->
    <calendar-date
        class="cally w-full bg-base-100 border-none shadow-none"
        {value}
        first-day-of-week="0"
        show-outside-days="true"
        bind:this={calendar}
    >
        <button
            class="btn btn-ghost btn-circle btn-sm text-base-content/60 hover:text-base-content"
            aria-label="Previous Week"
            slot="previous"
        >
            <svg
                xmlns="http://www.w3.org/2000/svg"
                class="h-5 w-5"
                fill="none"
                viewBox="0 0 24 24"
                stroke="currentColor"
                ><path
                    stroke-linecap="round"
                    stroke-linejoin="round"
                    stroke-width="2.5"
                    d="M15 19l-7-7 7-7"
                /></svg
            >
        </button>
        <button
            slot="next"
            class="btn btn-ghost btn-circle btn-sm text-base-content/60 hover:text-base-content"
            aria-label="Next Week"
        >
            <svg
                xmlns="http://www.w3.org/2000/svg"
                class="h-5 w-5"
                fill="none"
                viewBox="0 0 24 24"
                stroke="currentColor"
                ><path
                    stroke-linecap="round"
                    stroke-linejoin="round"
                    stroke-width="2.5"
                    d="M9 5l7 7-7 7"
                /></svg
            >
        </button>

        <calendar-month></calendar-month>
    </calendar-date>
</div>

<style>
    .calendar-wrapper :global(.cally) {
        --p: 64.5% 0.205 48.3; /* OKLCH value for orange-600 approx */
        --r: 0.5rem;
        font-size: 0.8rem;
    }

    .calendar-wrapper {
        width: 100%;
        max-width: 100%;
        display: flex;
        justify-content: center;
    }

    calendar-month::part(head),
    calendar-month::part(weekday) {
        visibility: visible;
        opacity: 1;
        color: #a5a5a5;
    }

    calendar-month::part(today) {
        background-color: oklch(var(--p) / 0.6);
    }

    calendar-month::part(selected) {
        background-color: oklch(var(--p));
    }

    calendar-date::part(previous),
    calendar-date::part(next) {
        background-color: transparent;
    }
</style>
