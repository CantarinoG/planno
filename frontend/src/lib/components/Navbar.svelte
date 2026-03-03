<script lang="ts">
    let {
        toggleSidebar = () => {},
        dateRangeString = "",
        onNext = () => {},
        onPrev = () => {},
        onToday = () => {},
    } = $props<{
        toggleSidebar?: () => void;
        dateRangeString?: string;
        onNext?: () => void;
        onPrev?: () => void;
        onToday?: () => void;
    }>();

    import ThemeToggle from "./ThemeToggle.svelte";
    import ChangePasswordModal from "./ChangePasswordModal.svelte";
    import { user, logoutUser } from "$lib/stores";

    let isChangePasswordModalOpen = $state(false);
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
                <div class="bg-orange-600 p-1.5 rounded-lg shadow-sm">
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
                    >LetsPlan</span
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

    <div class="navbar-end gap-1">
        <ThemeToggle />
        {#if $user}
            <div class="dropdown dropdown-end ml-1">
                <button
                    tabindex="0"
                    class="btn btn-ghost btn-circle avatar placeholder bg-orange-600 transition-all hover:bg-orange-600/60"
                >
                    <div
                        class="w-8 rounded-full text-base-content/70 flex items-center justify-center"
                    >
                        <span class="text-xs font-bold text-whited"
                            >{$user.username
                                .substring(0, 2)
                                .toUpperCase()}</span
                        >
                    </div>
                </button>
                <ul
                    tabindex="0"
                    role="menu"
                    class="mt-3 z-[1] p-2 shadow-2xl menu menu-sm dropdown-content bg-base-100 rounded-2xl w-64 border border-base-300"
                >
                    <li
                        class="px-4 py-3 border-b border-base-200 mb-2"
                        role="none"
                    >
                        <div
                            class="flex flex-col gap-0.5 p-0 hover:bg-transparent"
                            role="menuitem"
                            tabindex="-1"
                        >
                            <span class="text-sm font-bold text-base-content"
                                >{$user.username}</span
                            >
                            <span class="text-xs text-base-content/50"
                                >{$user.email}</span
                            >
                        </div>
                    </li>
                    <li role="none">
                        <button
                            role="menuitem"
                            class="flex items-center gap-3 py-2.5 rounded-xl hover:bg-base-200"
                            onclick={() => (isChangePasswordModalOpen = true)}
                        >
                            <svg
                                xmlns="http://www.w3.org/2000/svg"
                                width="16"
                                height="16"
                                viewBox="0 0 24 24"
                                fill="none"
                                stroke="currentColor"
                                stroke-width="2"
                                stroke-linecap="round"
                                stroke-linejoin="round"
                                ><rect
                                    x="3"
                                    y="11"
                                    width="18"
                                    height="11"
                                    rx="2"
                                    ry="2"
                                /><path d="M7 11V7a5 5 0 0 1 10 0v4" /></svg
                            >
                            Change Password
                        </button>
                    </li>
                    <li role="none">
                        <button
                            role="menuitem"
                            class="flex items-center gap-3 py-2.5 rounded-xl text-error hover:bg-error/10"
                            onclick={logoutUser}
                        >
                            <svg
                                xmlns="http://www.w3.org/2000/svg"
                                width="16"
                                height="16"
                                viewBox="0 0 24 24"
                                fill="none"
                                stroke="currentColor"
                                stroke-width="2"
                                stroke-linecap="round"
                                stroke-linejoin="round"
                                ><path
                                    d="M9 21H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h4"
                                ></path><polyline points="16 17 21 12 16 7"
                                ></polyline><line x1="21" y1="12" x2="9" y2="12"
                                ></line></svg
                            >
                            Logout
                        </button>
                    </li>
                </ul>
            </div>
        {/if}
    </div>
</nav>

<ChangePasswordModal bind:isOpen={isChangePasswordModalOpen} />
