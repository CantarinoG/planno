<div class="p-2 border border-base-200 rounded-xl bg-base-100 shadow-sm">
    <div class="grid grid-cols-7 gap-1 text-[10px] text-center opacity-50 mb-2">
        <div>S</div>
        <div>M</div>
        <div>T</div>
        <div>W</div>
        <div>T</div>
        <div>F</div>
        <div>S</div>
    </div>
    <div class="grid grid-cols-7 gap-1 text-xs text-center">
        {#each Array(31) as _, i}
            <div
                class="p-1 hover:bg-primary hover:text-primary-content rounded-full cursor-pointer {i +
                    1 ===
                23
                    ? 'bg-primary text-primary-content font-bold'
                    : ''}"
            >
                {i + 1}
            </div>
        {/each}
    </div>
</div>
