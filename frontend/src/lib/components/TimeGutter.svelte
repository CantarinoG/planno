<div
    class="h-full w-full flex flex-col items-center py-4 bg-base-50 border-r border-base-200"
>
    {#each Array(12) as _, i}
        <div class="h-20 text-[10px] uppercase opacity-40 font-bold">
            {8 + i < 10 ? "0" : ""}{8 + i} AM
        </div>
    {/each}
</div>
