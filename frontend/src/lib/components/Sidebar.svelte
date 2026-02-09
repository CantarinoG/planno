<script lang="ts">
    import MiniCalendar from "./MiniCalendar.svelte";
</script>

<aside
    class="w-64 bg-base-100 border-r border-base-200 flex flex-col h-full overflow-hidden hidden lg:flex"
>
    <!-- Create Event Button -->
    <div class="p-4 pt-6">
        <button
            class="btn bg-blue-600 hover:bg-blue-700 text-white font-bold w-full rounded-2xl shadow-lg border-none flex items-center justify-center gap-2 py-4 h-auto transition-transform hover:scale-[1.02] active:scale-[0.98]"
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

    <!-- Mini Calendar -->
    <div
        class="flex-1 px-4 py-4 overflow-y-auto overflow-x-hidden scrollbar-hide"
    >
        <MiniCalendar />
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
