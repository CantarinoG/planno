<script lang="ts">
    import { changePassword } from "$lib/api";
    import { addToast, isLoading } from "$lib/stores";

    let { isOpen = $bindable(false) } = $props<{
        isOpen?: boolean;
    }>();

    let currentPassword = $state("");
    let newPassword = $state("");
    let confirmPassword = $state("");
    let error = $state("");

    function close(): void {
        isOpen = false;
        currentPassword = "";
        newPassword = "";
        confirmPassword = "";
        error = "";
    }

    async function handleSave() {
        if (newPassword !== confirmPassword) {
            error = "New passwords do not match";
            return;
        }

        if (newPassword.length < 6) {
            error = "Password must be at least 6 characters";
            return;
        }

        isLoading.set(true);
        try {
            await changePassword({
                currentPassword,
                newPassword,
            });
            addToast("Password changed successfully", "success");
            close();
        } catch (e: any) {
            error = e.message || "Failed to change password";
        } finally {
            isLoading.set(false);
        }
    }

    function handleBackdropClick(e: MouseEvent): void {
        const target = e.target as HTMLElement;
        if (target.classList.contains("modal-backdrop")) {
            close();
        }
    }

    function handleKeydown(e: KeyboardEvent): void {
        if (e.key === "Escape") {
            close();
        }
    }

    let modalElement = $state<HTMLElement>();

    $effect(() => {
        if (isOpen && modalElement) {
            setTimeout(() => {
                const el = modalElement;
                if (!el) return;
                const firstInput = el.querySelector("input") as HTMLElement;
                if (firstInput) firstInput.focus();
            }, 50);
        }
    });
</script>

<svelte:window onkeydown={handleKeydown} />

{#if isOpen}
    <div
        class="modal modal-open items-center justify-center bg-black/40 backdrop-blur-sm z-[101]"
        onmousedown={handleBackdropClick}
        role="dialog"
        aria-modal="true"
        aria-labelledby="change-password-title"
        tabindex="-1"
        bind:this={modalElement}
    >
        <div
            class="modal-box w-full max-w-md p-0 bg-base-100 rounded-3xl shadow-2xl border border-base-300 overflow-hidden"
        >
            <div class="flex items-center justify-between px-8 py-6">
                <h2
                    id="change-password-title"
                    class="text-xs font-bold text-base-content/40 tracking-widest uppercase"
                >
                    Account Security
                </h2>
                <button
                    class="btn btn-ghost btn-circle btn-sm text-base-content/40 hover:text-base-content"
                    onclick={close}
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

            <div class="px-8 pb-8 space-y-6">
                <div>
                    <h1 class="text-3xl font-bold text-base-content">
                        Change Password
                    </h1>
                    <p class="text-sm text-base-content/50 mt-1">
                        Update your account password
                    </p>
                </div>

                <div class="space-y-4">
                    <div class="space-y-1.5">
                        <label
                            for="current-password"
                            class="text-[10px] font-bold text-base-content/40 tracking-widest uppercase px-1"
                            >Current Password</label
                        >
                        <div class="relative group">
                            <div
                                class="absolute left-4 top-1/2 -translate-y-1/2 text-base-content/30 group-focus-within:text-orange-500 transition-colors"
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
                                        y="11"
                                        width="18"
                                        height="11"
                                        rx="2"
                                        ry="2"
                                    /><path d="M7 11V7a5 5 0 0 1 10 0v4" /></svg
                                >
                            </div>
                            <input
                                id="current-password"
                                type="password"
                                bind:value={currentPassword}
                                placeholder="••••••••"
                                class="w-full pl-12 pr-4 py-3 bg-base-200/30 border border-base-300 rounded-xl focus:border-orange-500 focus:bg-base-100 transition-all outline-none font-medium text-base-content text-sm"
                            />
                        </div>
                    </div>

                    <div class="space-y-1.5">
                        <label
                            for="new-password"
                            class="text-[10px] font-bold text-base-content/40 tracking-widest uppercase px-1"
                            >New Password</label
                        >
                        <div class="relative group">
                            <div
                                class="absolute left-4 top-1/2 -translate-y-1/2 text-base-content/30 group-focus-within:text-orange-500 transition-colors"
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
                                        y="11"
                                        width="18"
                                        height="11"
                                        rx="2"
                                        ry="2"
                                    /><path d="M7 11V7a5 5 0 0 1 10 0v4" /></svg
                                >
                            </div>
                            <input
                                id="new-password"
                                type="password"
                                bind:value={newPassword}
                                placeholder="••••••••"
                                class="w-full pl-12 pr-4 py-3 bg-base-200/30 border border-base-300 rounded-xl focus:border-orange-500 focus:bg-base-100 transition-all outline-none font-medium text-base-content text-sm"
                            />
                        </div>
                    </div>

                    <div class="space-y-1.5">
                        <label
                            for="confirm-password"
                            class="text-[10px] font-bold text-base-content/40 tracking-widest uppercase px-1"
                            >Confirm New Password</label
                        >
                        <div class="relative group">
                            <div
                                class="absolute left-4 top-1/2 -translate-y-1/2 text-base-content/30 group-focus-within:text-orange-500 transition-colors"
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
                                    ><path
                                        d="M22 11.08V12a10 10 0 1 1-5.93-9.14"
                                    /><polyline
                                        points="22 4 12 14.01 9 11.01"
                                    /></svg
                                >
                            </div>
                            <input
                                id="confirm-password"
                                type="password"
                                bind:value={confirmPassword}
                                placeholder="••••••••"
                                class="w-full pl-12 pr-4 py-3 bg-base-200/30 border border-base-300 rounded-xl focus:border-orange-500 focus:bg-base-100 transition-all outline-none font-medium text-base-content text-sm"
                            />
                        </div>
                    </div>

                    {#if error}
                        <div
                            class="px-1 text-xs font-bold text-error flex items-center gap-1.5 animate-in fade-in slide-in-from-top-1"
                        >
                            <svg
                                xmlns="http://www.w3.org/2000/svg"
                                width="14"
                                height="14"
                                viewBox="0 0 24 24"
                                fill="none"
                                stroke="currentColor"
                                stroke-width="3"
                                stroke-linecap="round"
                                stroke-linejoin="round"
                                ><circle cx="12" cy="12" r="10" /><line
                                    x1="12"
                                    y1="8"
                                    x2="12"
                                    y2="12"
                                /><line
                                    x1="12"
                                    y1="16"
                                    x2="12.01"
                                    y2="16"
                                /></svg
                            >
                            {error}
                        </div>
                    {/if}
                </div>
            </div>

            <div
                class="flex items-center justify-end gap-3 p-8 bg-base-200/20 border-t border-base-300"
            >
                <button
                    class="btn btn-ghost font-bold text-base-content/60 hover:bg-base-300 px-6"
                    onclick={close}>Cancel</button
                >
                <button
                    class="btn bg-orange-600 hover:bg-orange-700 disabled:bg-base-300 disabled:text-base-content/30 text-white border-none font-bold px-8 shadow-lg shadow-orange-500/20"
                    onclick={handleSave}
                    disabled={$isLoading ||
                        !currentPassword ||
                        !newPassword ||
                        !confirmPassword}
                >
                    {#if $isLoading}
                        <span class="loading loading-spinner loading-sm"></span>
                    {:else}
                        Change Password
                    {/if}
                </button>
            </div>
        </div>
        <div class="modal-backdrop fixed inset-0"></div>
    </div>
{/if}
