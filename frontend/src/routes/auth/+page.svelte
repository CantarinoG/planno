<script lang="ts">
    import ThemeToggle from "$lib/components/ThemeToggle.svelte";
    import AuthForm from "$lib/components/AuthForm.svelte";
</script>

<svelte:head>
    <title>LetsPlan – Sign In</title>
</svelte:head>

<div class="min-h-screen flex">
    <!-- Left panel: hero image with overlay text -->
    <div class="hidden lg:flex lg:w-1/2 relative overflow-hidden">
        <img
            src="/hero.png"
            alt="Auth background"
            class="absolute inset-0 w-full h-full object-cover"
        />

        <!-- Dark shading overlay -->
        <div
            class="absolute inset-0 bg-gradient-to-br from-orange-900/70 via-black/50 to-orange-600/40"
        ></div>

        <div
            class="absolute inset-0 z-10 flex flex-col items-center justify-center text-center text-white px-12"
        >
            <div class="mb-4 flex items-center justify-center gap-3">
                <div class="bg-orange-600 p-2 rounded-lg shadow-md">
                    <svg
                        xmlns="http://www.w3.org/2000/svg"
                        class="h-7 w-7 text-white"
                        viewBox="0 0 24 24"
                        fill="none"
                        stroke="currentColor"
                        stroke-width="2.5"
                        stroke-linecap="round"
                        stroke-linejoin="round"
                    >
                        <rect x="3" y="4" width="18" height="18" rx="2" ry="2"
                        ></rect>
                        <line x1="16" y1="2" x2="16" y2="6"></line>
                        <line x1="8" y1="2" x2="8" y2="6"></line>
                        <line x1="3" y1="10" x2="21" y2="10"></line>
                    </svg>
                </div>
                <span class="text-3xl font-bold tracking-tight">LetsPlan</span>
            </div>
            <h2
                class="w-full text-center text-6xl font-bold leading-tight mb-5"
            >
                Plan your week,<br />own your time.
            </h2>
            <p
                class="w-full text-center text-white/70 text-lg max-w-sm mx-auto"
            >
                A simple, powerful calendar to keep you on top of what matters
                most.
            </p>
        </div>
    </div>

    <!-- Right panel: form area -->
    <div class="flex flex-col w-full lg:w-1/2 bg-base-100">
        <!-- Top bar with theme toggle -->
        <div class="flex justify-between items-center px-8 pt-6">
            <!-- Show logo on mobile (left panel is hidden) -->
            <a href="/" class="flex items-center gap-2 lg:invisible">
                <div class="bg-orange-600 p-1.5 rounded-lg shadow-sm">
                    <svg
                        xmlns="http://www.w3.org/2000/svg"
                        class="h-5 w-5 text-white"
                        viewBox="0 0 24 24"
                        fill="none"
                        stroke="currentColor"
                        stroke-width="2.5"
                        stroke-linecap="round"
                        stroke-linejoin="round"
                    >
                        <rect x="3" y="4" width="18" height="18" rx="2" ry="2"
                        ></rect>
                        <line x1="16" y1="2" x2="16" y2="6"></line>
                        <line x1="8" y1="2" x2="8" y2="6"></line>
                        <line x1="3" y1="10" x2="21" y2="10"></line>
                    </svg>
                </div>
                <span class="text-xl font-bold tracking-tight text-base-content"
                    >LetsPlan</span
                >
            </a>
            <div class="ml-auto">
                <ThemeToggle />
            </div>
        </div>

        <!-- Centered form content -->
        <div class="flex-1 flex items-center justify-center px-8 py-12">
            <div class="w-full max-w-sm">
                <AuthForm />
            </div>
        </div>
    </div>
</div>
