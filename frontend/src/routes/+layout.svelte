<script lang="ts">
	import "./layout.css";
	import favicon from "$lib/assets/favicon.svg";
	import Toast from "$lib/components/Toast.svelte";
	import { isLoading, theme, user } from "$lib/stores";
	import { onMount } from "svelte";
	import { getCurrentUser } from "$lib/api";
	import { page } from "$app/state";
	import { goto } from "$app/navigation";

	let { children } = $props();

	onMount(async () => {
		try {
			const userData = await getCurrentUser();
			user.set({
				username: userData.username,
				email: userData.email,
			});

			if (page.url.pathname === "/auth") {
				goto("/");
			}
		} catch (error) {
			if (page.url.pathname !== "/auth") {
				goto("/auth");
			}
		}
	});

	$effect(() => {
		if (typeof document !== "undefined") {
			document.documentElement.setAttribute("data-theme", $theme);
		}
	});
</script>

<svelte:head>
	<title>LetsPlan</title>
	<link rel="icon" href={favicon} />
</svelte:head>

{#if $isLoading}
	<div class="fixed top-0 left-0 w-full h-1 bg-orange-100 z-[1000]">
		<div class="h-full bg-orange-600 animate-progress origin-left"></div>
	</div>
{/if}

<Toast />

{@render children()}

<style>
	@keyframes progress {
		0% {
			transform: scaleX(0);
		}
		50% {
			transform: scaleX(0.5);
		}
		100% {
			transform: scaleX(1);
		}
	}
	.animate-progress {
		animation: progress 2s infinite linear;
	}
</style>
