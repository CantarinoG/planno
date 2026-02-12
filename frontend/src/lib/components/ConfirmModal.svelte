<script lang="ts">
    import { createEventDispatcher } from "svelte";
    import { fade, scale } from "svelte/transition";

    export let isOpen: boolean = false;
    export let title: string = "Confirm Action";
    export let message: string = "Are you sure you want to proceed?";
    export let confirmText: string = "Confirm";
    export let cancelText: string = "Cancel";
    export let type: "danger" | "warning" | "info" = "danger";

    const dispatch = createEventDispatcher<{
        confirm: void;
        cancel: void;
    }>();

    function handleConfirm() {
        dispatch("confirm");
    }

    function handleCancel() {
        dispatch("cancel");
    }

    function handleKeydown(e: KeyboardEvent) {
        if (isOpen && e.key === "Escape") {
            handleCancel();
        }
    }
</script>

<svelte:window on:keydown={handleKeydown} />

{#if isOpen}
    <div
        class="modal modal-open bg-black/40 backdrop-blur-sm z-[60]"
        role="dialog"
        aria-modal="true"
        aria-labelledby="modal-title"
    >
        <div
            class="modal-box"
            in:scale={{ duration: 200, start: 0.95 }}
            out:scale={{ duration: 150, start: 0.95 }}
        >
            <h3 class="font-bold text-lg" id="modal-title">{title}</h3>
            <p class="py-4 text-base-content/80">{message}</p>
            <div class="modal-action">
                <button
                    class="btn btn-ghost font-bold text-base-content/60 hover:bg-base-300 px-6"
                    on:click={handleCancel}>{cancelText}</button
                >
                <button
                    class="btn border-none font-bold px-8 shadow-lg text-white {type ===
                    'danger'
                        ? 'bg-red-600 hover:bg-red-700 shadow-red-500/20'
                        : type === 'warning'
                          ? 'bg-yellow-500 hover:bg-yellow-600 shadow-yellow-500/20'
                          : 'bg-orange-600 hover:bg-orange-700 shadow-orange-500/20'}"
                    on:click={handleConfirm}
                >
                    {confirmText}
                </button>
            </div>
        </div>
        <div
            class="modal-backdrop"
            role="button"
            tabindex="0"
            aria-label="Close modal"
            on:click={handleCancel}
            on:keydown={(e) => {
                if (e.key === "Enter" || e.key === " ") handleCancel();
            }}
        >
            <button class="cursor-default">close</button>
        </div>
    </div>
{/if}
