<script>
    export let title = "Event";
    export let time = "09:00 - 10:00";
    export let color = "bg-primary text-primary-content";
</script>

<div
    class="{color} p-2 rounded-lg text-xs shadow-sm cursor-pointer hover:brightness-95 transition-all"
>
    <div class="font-bold truncate">{title}</div>
    <div class="opacity-80">{time}</div>
</div>
