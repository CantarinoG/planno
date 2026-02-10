<script lang="ts">
    import { toasts, dismissToast, type Toast } from "../stores";
    import { flip } from "svelte/animate";
    import { fade, fly } from "svelte/transition";
</script>

<div class="toast toast-top toast-end z-[100]">
    {#each $toasts as toast (toast.id)}
        <div
            animate:flip={{ duration: 300 }}
            in:fly={{ y: -20, duration: 300 }}
            out:fade={{ duration: 200 }}
            class="alert alert-{toast.type} shadow-lg min-w-[300px] flex justify-between"
        >
            <div>
                {#if toast.type === "success"}
                    <svg
                        xmlns="http://www.w3.org/2000/svg"
                        class="stroke-current flex-shrink-0 h-6 w-6"
                        fill="none"
                        viewBox="0 0 24 24"
                        ><path
                            stroke-linecap="round"
                            stroke-linejoin="round"
                            stroke-width="2"
                            d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z"
                        /></svg
                    >
                {:else if toast.type === "error"}
                    <svg
                        xmlns="http://www.w3.org/2000/svg"
                        class="stroke-current flex-shrink-0 h-6 w-6"
                        fill="none"
                        viewBox="0 0 24 24"
                        ><path
                            stroke-linecap="round"
                            stroke-linejoin="round"
                            stroke-width="2"
                            d="M10 14l2-2m0 0l2-2m-2 2l-2-2m2 2l2 2m7-2a9 9 0 11-18 0 9 9 0 0118 0z"
                        /></svg
                    >
                {:else}
                    <svg
                        xmlns="http://www.w3.org/2000/svg"
                        fill="none"
                        viewBox="0 0 24 24"
                        class="stroke-current flex-shrink-0 w-6 h-6"
                        ><path
                            stroke-linecap="round"
                            stroke-linejoin="round"
                            stroke-width="2"
                            d="M13 16h-1v-4h-1m1-4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z"
                        ></path></svg
                    >
                {/if}
                <span>{toast.message}</span>
            </div>
            <button
                class="btn btn-ghost btn-xs btn-circle"
                on:click={() => dismissToast(toast.id)}
            >
                <svg
                    xmlns="http://www.w3.org/2000/svg"
                    class="h-4 w-4"
                    fill="none"
                    viewBox="0 0 24 24"
                    stroke="currentColor"
                    ><path
                        stroke-linecap="round"
                        stroke-linejoin="round"
                        stroke-width="2"
                        d="M6 18L18 6M6 6l12 12"
                    /></svg
                >
            </button>
        </div>
    {/each}
</div>
