<script lang="ts">
    export let toggleSidebar: () => void = () => {};
    export let dateRangeString: string = "";
    export let onNext: () => void = () => {};
    export let onPrev: () => void = () => {};
    export let onToday: () => void = () => {};
</script>

<nav class="navbar bg-base-100 border-b border-base-300 px-4 py-2">
    <div class="navbar-start gap-2 sm:gap-4">
        <button
            class="btn btn-ghost btn-circle btn-sm"
            aria-label="Toggle Sidebar"
            onclick={toggleSidebar}
        >
            <svg
                xmlns="http://www.w3.org/2000/svg"
                fill="none"
                viewBox="0 0 24 24"
                class="inline-block w-6 h-6 stroke-current"
                ><path
                    stroke-linecap="round"
                    stroke-linejoin="round"
                    stroke-width="2"
                    d="M4 6h16M4 12h16M4 18h16"
                ></path></svg
            >
        </button>

        <div class="flex items-center gap-2 sm:gap-6">
            <a href="/" class="flex items-center gap-2 px-1">
                <div class="bg-blue-600 p-1.5 rounded-lg shadow-sm">
                    <svg
                        xmlns="http://www.w3.org/2000/svg"
                        class="h-5 w-5 text-primary-content"
                        viewBox="0 0 24 24"
                        fill="none"
                        stroke="currentColor"
                        stroke-width="2.5"
                        stroke-linecap="round"
                        stroke-linejoin="round"
                        ><rect x="3" y="4" width="18" height="18" rx="2" ry="2"
                        ></rect><line x1="16" y1="2" x2="16" y2="6"></line><line
                            x1="8"
                            y1="2"
                            x2="8"
                            y2="6"
                        ></line><line x1="3" y1="10" x2="21" y2="10"
                        ></line></svg
                    >
                </div>
                <span
                    class="text-xl font-bold tracking-tight text-base-content hidden sm:inline"
                    >Planno</span
                >
            </a>

            <button
                class="btn btn-sm btn-outline border-base-300 bg-base-100 hover:bg-base-300 px-5 font-bold text-base-content/70 hidden md:inline-flex"
                onclick={onToday}
            >
                Today
            </button>
        </div>
    </div>

    <div class="navbar-center">
        <div class="flex items-center gap-2 sm:gap-8">
            <button
                class="btn btn-ghost btn-circle btn-sm text-base-content/60 hover:text-base-content"
                aria-label="Previous Week"
                onclick={onPrev}
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
            <span
                class="text-sm sm:text-base font-bold text-base-content whitespace-nowrap"
                >{dateRangeString}</span
            >
            <button
                class="btn btn-ghost btn-circle btn-sm text-base-content/60 hover:text-base-content"
                aria-label="Next Week"
                onclick={onNext}
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
        </div>
    </div>

    <div class="navbar-end"></div>
</nav>
