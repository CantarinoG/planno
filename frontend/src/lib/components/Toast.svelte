<script lang="ts">
    import { toasts, dismissToast, type Toast } from "../stores";
    import { flip } from "svelte/animate";
    import { fade, fly } from "svelte/transition";
</script>

<div class="toast toast-top toast-end z-[100]">
    {#each $toasts as toast (toast.id)}
        <div
            animate:flip={{ duration: 300 }}
            in:fly={{ y: 20, duration: 300 }}
            out:fade={{ duration: 200 }}
            class="flex items-center justify-between p-4 rounded-xl shadow-2xl min-w-[320px] max-w-md text-white {toast.type ===
            'success'
                ? 'bg-gradient-to-r from-green-600 to-green-500'
                : toast.type === 'error'
                  ? 'bg-gradient-to-r from-red-600 to-red-500'
                  : 'bg-gradient-to-r from-blue-600 to-blue-500'}"
        >
            <div class="flex items-center gap-3">
                {#if toast.type === "success"}
                    <div class="p-1 bg-white/20 rounded-full">
                        <svg
                            xmlns="http://www.w3.org/2000/svg"
                            class="h-5 w-5"
                            fill="none"
                            viewBox="0 0 24 24"
                            stroke="currentColor"
                        >
                            <path
                                stroke-linecap="round"
                                stroke-linejoin="round"
                                stroke-width="3"
                                d="M5 13l4 4L19 7"
                            />
                        </svg>
                    </div>
                {:else if toast.type === "error"}
                    <div class="p-1 bg-white/20 rounded-full">
                        <svg
                            xmlns="http://www.w3.org/2000/svg"
                            class="h-5 w-5"
                            fill="none"
                            viewBox="0 0 24 24"
                            stroke="currentColor"
                        >
                            <path
                                stroke-linecap="round"
                                stroke-linejoin="round"
                                stroke-width="3"
                                d="M6 18L18 6M6 6l12 12"
                            />
                        </svg>
                    </div>
                {:else}
                    <div class="p-1 bg-white/20 rounded-full">
                        <svg
                            xmlns="http://www.w3.org/2000/svg"
                            class="h-5 w-5"
                            fill="none"
                            viewBox="0 0 24 24"
                            stroke="currentColor"
                        >
                            <path
                                stroke-linecap="round"
                                stroke-linejoin="round"
                                stroke-width="3"
                                d="M13 16h-1v-4h-1m1-4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z"
                            />
                        </svg>
                    </div>
                {/if}
                <span class="font-medium text-sm tracking-wide"
                    >{toast.message}</span
                >
            </div>
            <button
                class="btn btn-ghost btn-xs btn-circle text-white/80 hover:bg-white/20 hover:text-white ml-4"
                aria-label="Close"
                on:click={() => dismissToast(toast.id)}
            >
                <svg
                    xmlns="http://www.w3.org/2000/svg"
                    class="h-4 w-4"
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
    {/each}
</div>
