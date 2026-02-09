<script lang="ts">
    import { onMount } from "svelte";

    function toISO(date: Date): string {
        return date.toISOString().split("T")[0];
    }

    // We can interact with the calendar element if needed
    let calendar: HTMLElement;
    let today: string = toISO(new Date());

    onMount(async () => {
        await import("cally");
    });
</script>

<div class="calendar-wrapper">
    <calendar-date
        class="cally bg-base-100 border-none shadow-none"
        first-day-of-week="0"
        show-outside-days="true"
        value={today}
        bind:this={calendar}
    >
        <!-- Custom Icons matching our design -->
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
    /* 
       Override DaisyUI/Cally variables to match bg-blue-600 (#2563eb)
       DaisyUI uses Oklch, but we can set the hex/rgb variables that Cally might use or standard variables.
       For DaisyUI v5, it uses --p (primary) for active states.
    */
    .calendar-wrapper :global(.cally) {
        /* Approximate blue-600 in OKLCH for consistency with DaisyUI v5 if needed, 
           or standard CSS variable overrides if supported by the theme. 
           Let's try to set the primary color for this scope.
        */
        --p: 53.6% 0.201 267.4; /* OKLCH value for blue-600 approx */
        --r: 0.5rem; /* Rounded corners */
    }

    /* 
       Ensure the calendar fits safely in the container 
    */
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
