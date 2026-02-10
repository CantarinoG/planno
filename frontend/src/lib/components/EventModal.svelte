<script lang="ts">
    import { createEventDispatcher } from "svelte";
    import { format } from "date-fns";
    import {
        EVENT_COLORS,
        DEFAULT_COLOR_ID,
        type EventColor,
    } from "$lib/constants/colors";

    export let isOpen: boolean = false;
    export let date: string = "";

    const dispatch = createEventDispatcher<{
        close: null;
        save: {
            title: string;
            isAllDay: boolean;
            startDate: string;
            endDate: string;
            description: string;
            color: string;
        };
    }>();

    let title: string = "";
    let isAllDay: boolean = false;
    let startDate: string = "";
    let endDate: string = "";
    let description: string = "";
    let selectedColorId: string = DEFAULT_COLOR_ID;

    $: if (isOpen && date) {
        const start: Date = new Date(date);

        const hasTime = date.includes("T") && date.length > 10;

        if (!hasTime) {
            const now: Date = new Date();
            start.setHours(now.getHours(), 0, 0, 0);
        }

        const end: Date = new Date(start);
        end.setHours(start.getHours() + 1);

        startDate = format(start, "yyyy-MM-dd'T'HH:mm");
        endDate = format(end, "yyyy-MM-dd'T'HH:mm");
    }

    function close(): void {
        dispatch("close");
    }

    function save(): void {
        dispatch("save", {
            title,
            isAllDay,
            startDate,
            endDate,
            description,
            color: selectedColorId,
        });
        close();
    }

    function handleBackdropClick(e: MouseEvent): void {
        const target = e.target as HTMLElement;
        if (target.classList.contains("modal-backdrop")) {
            close();
        }
    }
</script>

{#if isOpen}
    <div
        class="modal modal-open items-center justify-center bg-black/40 backdrop-blur-sm z-[100]"
        on:mousedown={handleBackdropClick}
        role="dialog"
        aria-modal="true"
        tabindex="-1"
    >
        <div
            class="modal-box w-full max-w-lg p-0 bg-base-100 rounded-3xl shadow-2xl border border-base-300 overflow-hidden"
        >
            <div class="flex items-center justify-between px-8 py-6">
                <h2
                    class="text-xs font-bold text-base-content/40 tracking-widest uppercase"
                >
                    Create New Event
                </h2>
                <button
                    class="btn btn-ghost btn-circle btn-sm text-base-content/40 hover:text-base-content"
                    on:click={close}
                    aria-label="Close modal"
                >
                    <svg
                        xmlns="http://www.w3.org/2000/svg"
                        class="h-6 w-6"
                        fill="none"
                        viewBox="0 0 24 24"
                        stroke="currentColor"
                    >
                        <path
                            stroke-linecap="round"
                            stroke-linejoin="round"
                            stroke-width="2"
                            d="M6 18L18 6M6 6l12 12"
                        />
                    </svg>
                </button>
            </div>

            <div class="px-8 pb-8 space-y-8">
                <div>
                    <input
                        type="text"
                        bind:value={title}
                        placeholder="Event Title"
                        class="w-full text-4xl font-bold bg-transparent border-none outline-none focus:ring-0 placeholder:text-base-content/20 text-base-content"
                    />
                </div>

                <div class="flex items-center justify-between">
                    <div class="flex items-center gap-3">
                        <div
                            class="p-2 bg-base-200/50 rounded-xl text-base-content/60"
                        >
                            <svg
                                xmlns="http://www.w3.org/2000/svg"
                                width="20"
                                height="20"
                                viewBox="0 0 24 24"
                                fill="none"
                                stroke="currentColor"
                                stroke-width="2"
                                stroke-linecap="round"
                                stroke-linejoin="round"
                                ><circle cx="12" cy="12" r="10" /><polyline
                                    points="12 6 12 12 16 14"
                                /></svg
                            >
                        </div>
                        <label
                            for="all-day-toggle"
                            class="font-bold text-base-content/70 cursor-pointer"
                            >All Day Event</label
                        >
                    </div>
                    <input
                        id="all-day-toggle"
                        type="checkbox"
                        bind:checked={isAllDay}
                        class="toggle toggle-primary"
                    />
                </div>

                <div class="grid grid-cols-2 gap-4">
                    <div class="space-y-2">
                        <label
                            for="start-date"
                            class="text-[10px] font-bold text-base-content/40 tracking-widest uppercase px-1"
                            >Start At</label
                        >
                        <div class="relative group">
                            <div
                                class="absolute left-4 top-1/2 -translate-y-1/2 text-base-content/30 group-focus-within:text-blue-500 transition-colors"
                            >
                                <svg
                                    xmlns="http://www.w3.org/2000/svg"
                                    width="18"
                                    height="18"
                                    viewBox="0 0 24 24"
                                    fill="none"
                                    stroke="currentColor"
                                    stroke-width="2"
                                    stroke-linecap="round"
                                    stroke-linejoin="round"
                                    ><rect
                                        x="3"
                                        y="4"
                                        width="18"
                                        height="18"
                                        rx="2"
                                        ry="2"
                                    /><line
                                        x1="16"
                                        y1="2"
                                        x2="16"
                                        y2="6"
                                    /><line x1="8" y1="2" x2="8" y2="6" /><line
                                        x1="3"
                                        y1="10"
                                        x2="21"
                                        y2="10"
                                    /></svg
                                >
                            </div>
                            <input
                                id="start-date"
                                type={isAllDay ? "date" : "datetime-local"}
                                bind:value={startDate}
                                on:click={(e) => e.currentTarget.showPicker()}
                                class="w-full pl-12 pr-4 py-3 bg-base-200/30 border border-base-300 rounded-xl focus:border-blue-500 focus:bg-base-100 transition-all outline-none font-medium text-base-content/80 text-sm appearance-none"
                            />
                        </div>
                    </div>
                    <div class="space-y-2">
                        <label
                            for="end-date"
                            class="text-[10px] font-bold text-base-content/40 tracking-widest uppercase px-1"
                            >End At</label
                        >
                        <div class="relative group">
                            <div
                                class="absolute left-4 top-1/2 -translate-y-1/2 text-base-content/30 group-focus-within:text-blue-500 transition-colors"
                            >
                                <svg
                                    xmlns="http://www.w3.org/2000/svg"
                                    width="18"
                                    height="18"
                                    viewBox="0 0 24 24"
                                    fill="none"
                                    stroke="currentColor"
                                    stroke-width="2"
                                    stroke-linecap="round"
                                    stroke-linejoin="round"
                                    ><rect
                                        x="3"
                                        y="4"
                                        width="18"
                                        height="18"
                                        rx="2"
                                        ry="2"
                                    /><line
                                        x1="16"
                                        y1="2"
                                        x2="16"
                                        y2="6"
                                    /><line x1="8" y1="2" x2="8" y2="6" /><line
                                        x1="3"
                                        y1="10"
                                        x2="21"
                                        y2="10"
                                    /></svg
                                >
                            </div>
                            <input
                                id="end-date"
                                type={isAllDay ? "date" : "datetime-local"}
                                bind:value={endDate}
                                on:click={(e) => e.currentTarget.showPicker()}
                                class="w-full pl-12 pr-4 py-3 bg-base-200/30 border border-base-300 rounded-xl focus:border-blue-500 focus:bg-base-100 transition-all outline-none font-medium text-base-content/80 text-sm appearance-none"
                            />
                        </div>
                    </div>
                </div>

                <div class="space-y-2">
                    <label
                        for="event-description"
                        class="text-[10px] font-bold text-base-content/40 tracking-widest uppercase px-1"
                        >Description</label
                    >
                    <textarea
                        id="event-description"
                        bind:value={description}
                        placeholder="Add notes, location, or video call links..."
                        class="w-full p-4 bg-base-200/30 border border-base-300 rounded-xl focus:border-blue-500 focus:bg-base-100 transition-all outline-none min-h-[120px] resize-none text-base-content/80 placeholder:text-base-content/30"
                    ></textarea>
                </div>

                <div class="space-y-3">
                    <span
                        class="text-[10px] font-bold text-base-content/40 tracking-widest uppercase px-1"
                        >Category Color</span
                    >
                    <div class="flex items-center gap-4 px-1">
                        {#each EVENT_COLORS as color}
                            <button
                                class="relative w-8 h-8 rounded-full border-2 transition-all hover:scale-110 active:scale-95"
                                style="background-color: {color.hex}; border-color: {selectedColorId ===
                                color.id
                                    ? '#3b82f6'
                                    : 'transparent'}"
                                on:click={() => (selectedColorId = color.id)}
                            >
                                {#if selectedColorId === color.id}
                                    <div
                                        class="absolute inset-0 flex items-center justify-center text-white drop-shadow-sm"
                                    >
                                        <svg
                                            xmlns="http://www.w3.org/2000/svg"
                                            width="16"
                                            height="16"
                                            viewBox="0 0 24 24"
                                            fill="none"
                                            stroke="currentColor"
                                            stroke-width="3"
                                            stroke-linecap="round"
                                            stroke-linejoin="round"
                                            ><polyline
                                                points="20 6 9 17 4 12"
                                            /></svg
                                        >
                                    </div>
                                {/if}
                            </button>
                        {/each}
                    </div>
                </div>
            </div>

            <div
                class="flex items-center justify-end gap-4 p-8 bg-base-200/20 border-t border-base-300"
            >
                <button
                    class="btn btn-ghost font-bold text-base-content/60 hover:bg-base-300 px-6"
                    on:click={close}>Cancel</button
                >
                <button
                    class="btn bg-blue-600 hover:bg-blue-700 text-white border-none font-bold px-8 shadow-lg shadow-blue-500/20"
                    on:click={save}
                >
                    Save Event
                </button>
            </div>
        </div>
        <div class="modal-backdrop fixed inset-0"></div>
    </div>
{/if}

<style>
    input[type="datetime-local"]::-webkit-calendar-picker-indicator,
    input[type="date"]::-webkit-calendar-picker-indicator {
        position: absolute;
        right: 1rem;
        cursor: pointer;
        opacity: 0;
    }
</style>
