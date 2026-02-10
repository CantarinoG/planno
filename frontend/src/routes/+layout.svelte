<script lang="ts">
	import "./layout.css";
	import favicon from "$lib/assets/favicon.svg";
	import Toast from "$lib/components/Toast.svelte";
	import { isLoading } from "$lib/stores";

	let { children } = $props();
</script>

<svelte:head>
	<title>Planno</title>
	<link rel="icon" href={favicon} />
</svelte:head>

{#if $isLoading}
	<div class="fixed top-0 left-0 w-full h-1 bg-blue-100 z-[1000]">
		<div class="h-full bg-blue-600 animate-progress origin-left"></div>
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
