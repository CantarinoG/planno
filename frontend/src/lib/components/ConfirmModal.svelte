<script lang="ts">
    import { createEventDispatcher } from "svelte";
    import { scale } from "svelte/transition";

    let {
        isOpen = $bindable(false),
        title = "Confirm Action",
        message = "Are you sure you want to proceed?",
        confirmText = "Confirm",
        cancelText = "Cancel",
        type = "danger",
    } = $props<{
        isOpen?: boolean;
        title?: string;
        message?: string;
        confirmText?: string;
        cancelText?: string;
        type?: "danger" | "warning" | "info";
    }>();

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

<svelte:window onkeydown={handleKeydown} />

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
                    onclick={handleCancel}>{cancelText}</button
                >
                <button
                    class="btn border-none font-bold px-8 shadow-lg text-white {type ===
                    'danger'
                        ? 'bg-red-600 hover:bg-red-700 shadow-red-500/20'
                        : type === 'warning'
                          ? 'bg-yellow-500 hover:bg-yellow-600 shadow-yellow-500/20'
                          : 'bg-orange-600 hover:bg-orange-700 shadow-orange-500/20'}"
                    onclick={handleConfirm}
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
            onclick={handleCancel}
            onkeydown={(e) => {
                if (e.key === "Enter" || e.key === " ") handleCancel();
            }}
        >
            <button class="cursor-default">close</button>
        </div>
    </div>
{/if}
