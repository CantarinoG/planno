<script lang="ts">
    type Tab = "login" | "register";
    let activeTab: Tab = $state("login");

    let loginEmailUsername = $state("");
    let loginPassword = $state("");

    let registerUsername = $state("");
    let registerEmail = $state("");
    let registerPassword = $state("");
    let registerConfirmPassword = $state("");

    function handleLogin(e: Event) {
        e.preventDefault();
        // TODO: implement login API call
    }

    function handleRegister(e: Event) {
        e.preventDefault();
        // TODO: implement register API call
    }
</script>

<div class="w-full">
    <!-- Tab switcher -->
    <div
        role="tablist"
        class="tabs tabs-boxed bg-base-200 border border-base-300 rounded-2xl p-1 mb-8"
    >
        <button
            role="tab"
            class="tab flex-1 font-bold transition-all rounded-xl {activeTab ===
            'login'
                ? 'bg-orange-600 text-white shadow-sm'
                : 'text-base-content/50 hover:text-base-content'}"
            onclick={() => (activeTab = "login")}
        >
            Sign In
        </button>
        <button
            role="tab"
            class="tab flex-1 font-bold transition-all rounded-xl {activeTab ===
            'register'
                ? 'bg-orange-600 text-white shadow-sm'
                : 'text-base-content/50 hover:text-base-content'}"
            onclick={() => (activeTab = "register")}
        >
            Register
        </button>
    </div>

    {#if activeTab === "login"}
        <!-- Heading -->
        <div class="mb-8">
            <h1 class="text-2xl font-bold text-base-content">Welcome back</h1>
            <p class="text-base-content/50 text-sm mt-1">
                Sign in to your LetsPlan account
            </p>
        </div>

        <form onsubmit={handleLogin} class="space-y-6">
            <!-- Email Field-->
            <div class="space-y-2">
                <label
                    for="login-email"
                    class="text-[10px] font-bold text-base-content/40 tracking-widest uppercase px-1"
                >
                    Email or Username
                </label>
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
                        >
                            <path
                                d="M4 4h16c1.1 0 2 .9 2 2v12c0 1.1-.9 2-2 2H4c-1.1 0-2-.9-2-2V6c0-1.1.9-2 2-2z"
                            ></path>
                            <polyline points="22,6 12,13 2,6"></polyline>
                        </svg>
                    </div>
                    <input
                        id="login-email"
                        type="email"
                        placeholder="you@example.com"
                        bind:value={loginEmailUsername}
                        class="w-full pl-12 pr-4 py-3 bg-base-200/30 border border-base-300 rounded-xl focus:border-orange-500 focus:bg-base-100 transition-all outline-none font-medium text-base-content/80 text-sm"
                        required
                    />
                </div>
            </div>

            <!-- Password Field-->
            <div class="space-y-2">
                <div class="flex items-center justify-between px-1">
                    <label
                        for="login-password"
                        class="text-[10px] font-bold text-base-content/40 tracking-widest uppercase"
                    >
                        Password
                    </label>
                    <a
                        href="#"
                        class="text-[10px] font-bold text-orange-600 hover:text-orange-700 tracking-wide uppercase transition-colors"
                    >
                        Forgot password?
                    </a>
                </div>
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
                        >
                            <rect
                                x="3"
                                y="11"
                                width="18"
                                height="11"
                                rx="2"
                                ry="2"
                            ></rect>
                            <path d="M7 11V7a5 5 0 0 1 10 0v4"></path>
                        </svg>
                    </div>
                    <input
                        id="login-password"
                        type="password"
                        placeholder="••••••••"
                        bind:value={loginPassword}
                        class="w-full pl-12 pr-4 py-3 bg-base-200/30 border border-base-300 rounded-xl focus:border-orange-500 focus:bg-base-100 transition-all outline-none font-medium text-base-content/80 text-sm"
                        required
                    />
                </div>
            </div>

            <button
                type="submit"
                class="w-full btn bg-orange-600 hover:bg-orange-700 text-white border-none font-bold rounded-xl shadow-lg shadow-orange-500/20 mt-2"
            >
                Sign In
            </button>
        </form>

        <p class="text-center text-sm text-base-content/50 mt-6">
            Don't have an account?
            <button
                class="text-orange-600 hover:text-orange-700 font-bold transition-colors cursor-pointer"
                onclick={() => (activeTab = "register")}
            >
                Register
            </button>
        </p>
    {:else}
        <!-- Heading -->
        <div class="mb-8">
            <h1 class="text-2xl font-bold text-base-content">
                Create an account
            </h1>
            <p class="text-base-content/50 text-sm mt-1">
                Start planning with LetsPlan
            </p>
        </div>

        <form onsubmit={handleRegister} class="space-y-6">
            <!-- Username Field -->
            <div class="space-y-2">
                <label
                    for="register-name"
                    class="text-[10px] font-bold text-base-content/40 tracking-widest uppercase px-1"
                >
                    Username
                </label>
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
                        >
                            <path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2"
                            ></path>
                            <circle cx="12" cy="7" r="4"></circle>
                        </svg>
                    </div>
                    <input
                        id="register-name"
                        type="text"
                        placeholder="Your username"
                        bind:value={registerUsername}
                        class="w-full pl-12 pr-4 py-3 bg-base-200/30 border border-base-300 rounded-xl focus:border-orange-500 focus:bg-base-100 transition-all outline-none font-medium text-base-content/80 text-sm"
                        required
                    />
                </div>
            </div>

            <!-- Email Field -->
            <div class="space-y-2">
                <label
                    for="register-email"
                    class="text-[10px] font-bold text-base-content/40 tracking-widest uppercase px-1"
                >
                    Email
                </label>
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
                        >
                            <path
                                d="M4 4h16c1.1 0 2 .9 2 2v12c0 1.1-.9 2-2 2H4c-1.1 0-2-.9-2-2V6c0-1.1.9-2 2-2z"
                            ></path>
                            <polyline points="22,6 12,13 2,6"></polyline>
                        </svg>
                    </div>
                    <input
                        id="register-email"
                        type="email"
                        placeholder="you@example.com"
                        bind:value={registerEmail}
                        class="w-full pl-12 pr-4 py-3 bg-base-200/30 border border-base-300 rounded-xl focus:border-orange-500 focus:bg-base-100 transition-all outline-none font-medium text-base-content/80 text-sm"
                        required
                    />
                </div>
            </div>

            <!-- Password Field -->
            <div class="space-y-2">
                <label
                    for="register-password"
                    class="text-[10px] font-bold text-base-content/40 tracking-widest uppercase px-1"
                >
                    Password
                </label>
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
                        >
                            <rect
                                x="3"
                                y="11"
                                width="18"
                                height="11"
                                rx="2"
                                ry="2"
                            ></rect>
                            <path d="M7 11V7a5 5 0 0 1 10 0v4"></path>
                        </svg>
                    </div>
                    <input
                        id="register-password"
                        type="password"
                        placeholder="••••••••"
                        bind:value={registerPassword}
                        class="w-full pl-12 pr-4 py-3 bg-base-200/30 border border-base-300 rounded-xl focus:border-orange-500 focus:bg-base-100 transition-all outline-none font-medium text-base-content/80 text-sm"
                        required
                    />
                </div>
            </div>

            <!-- Confirm Password Field-->
            <div class="space-y-2">
                <label
                    for="register-confirm-password"
                    class="text-[10px] font-bold text-base-content/40 tracking-widest uppercase px-1"
                >
                    Confirm Password
                </label>
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
                        >
                            <rect
                                x="3"
                                y="11"
                                width="18"
                                height="11"
                                rx="2"
                                ry="2"
                            ></rect>
                            <path d="M7 11V7a5 5 0 0 1 10 0v4"></path>
                        </svg>
                    </div>
                    <input
                        id="register-confirm-password"
                        type="password"
                        placeholder="••••••••"
                        bind:value={registerConfirmPassword}
                        class="w-full pl-12 pr-4 py-3 bg-base-200/30 border border-base-300 rounded-xl focus:border-orange-500 focus:bg-base-100 transition-all outline-none font-medium text-base-content/80 text-sm"
                        required
                    />
                </div>
            </div>

            <button
                type="submit"
                class="w-full btn bg-orange-600 hover:bg-orange-700 text-white border-none font-bold rounded-xl shadow-lg shadow-orange-500/20 mt-2"
            >
                Create Account
            </button>
        </form>

        <p class="text-center text-sm text-base-content/50 mt-6">
            Already have an account?
            <button
                class="text-orange-600 hover:text-orange-700 font-bold transition-colors cursor-pointer"
                onclick={() => (activeTab = "login")}
            >
                Sign In
            </button>
        </p>
    {/if}
</div>
