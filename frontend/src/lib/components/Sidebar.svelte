<script lang="ts">
    import { createEventDispatcher } from "svelte";
    import MiniCalendar from "./MiniCalendar.svelte";

    const dispatch = createEventDispatcher();

    export let date: string = "";
</script>

<aside
    class="w-72 bg-base-100 border-r border-base-300 flex flex-col h-full overflow-hidden"
>
    <div class="p-4 pt-6">
        <button
            class="btn bg-orange-600 hover:bg-orange-700 text-white font-bold w-full rounded-2xl shadow-lg border-none flex items-center justify-center gap-2 py-4 h-auto transition-transform hover:scale-[1.02] active:scale-[0.98]"
            onclick={() => dispatch("create")}
        >
            <svg
                xmlns="http://www.w3.org/2000/svg"
                width="24"
                height="24"
                viewBox="0 0 24 24"
                fill="none"
                stroke="currentColor"
                stroke-width="3"
                stroke-linecap="round"
                stroke-linejoin="round"
                ><line x1="12" y1="5" x2="12" y2="19"></line><line
                    x1="5"
                    y1="12"
                    x2="19"
                    y2="12"
                ></line></svg
            >
            <span class="text-base">Create Event</span>
        </button>
    </div>

    <div
        class="flex-1 px-2 py-4 overflow-y-auto overflow-x-hidden scrollbar-hide"
    >
        <MiniCalendar value={date} on:datechange />
    </div>

    <div class="px-4 pb-4">
        <div
            class="bg-base-200/50 rounded-xl p-3 flex flex-col gap-2 border border-base-300"
        >
            <h3
                class="text-[10px] font-bold uppercase tracking-wider text-base-content/40 px-1"
            >
                Shortcuts
            </h3>
            <div class="flex flex-col gap-1.5">
                <div class="flex items-center justify-between group">
                    <span class="text-xs text-base-content/70"
                        >Navigate weeks</span
                    >
                    <div class="flex gap-1">
                        <kbd class="kbd kbd-xs bg-base-100 border-base-300"
                            >←</kbd
                        >
                        <kbd class="kbd kbd-xs bg-base-100 border-base-300"
                            >→</kbd
                        >
                    </div>
                </div>
                <div class="flex items-center justify-between">
                    <span class="text-xs text-base-content/70"
                        >Create event</span
                    >
                    <kbd class="kbd kbd-xs bg-base-100 border-base-300">c</kbd>
                </div>
                <div class="flex items-center justify-between">
                    <span class="text-xs text-base-content/70">Today</span>
                    <kbd class="kbd kbd-xs bg-base-100 border-base-300">t</kbd>
                </div>
            </div>
        </div>
    </div>

    <div class="p-4 border-t border-base-200">
        <p class="text-xs text-base-content/60 text-center">
            Made by <a
                href="https://github.com/CantarinoG"
                target="_blank"
                rel="noopener noreferrer"
                class="link link-hover text-orange-500 font-medium"
                >@CantarinoG</a
            >
        </p>
    </div>
</aside>

<style>
    .scrollbar-hide::-webkit-scrollbar {
        display: none;
    }
    .scrollbar-hide {
        -ms-overflow-style: none;
        scrollbar-width: none;
    }
</style>
