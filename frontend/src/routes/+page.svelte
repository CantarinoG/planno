<script>
    import Navbar from "$lib/components/Navbar.svelte";
    import Sidebar from "$lib/components/Sidebar.svelte";
    import CalendarGrid from "$lib/components/CalendarGrid.svelte";
</script>

<div class="h-screen flex flex-col overflow-hidden">
    <Navbar />

    <main class="flex-1 flex overflow-hidden">
        <Sidebar />
        <CalendarGrid />
    </main>
</div>
