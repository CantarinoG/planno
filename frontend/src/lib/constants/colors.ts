export interface EventColor {
    id: string;
    label: string;
    hex: string;
    bgClass: string;
}

export const EVENT_COLORS: EventColor[] = [
    {
        id: "blue",
        label: "Blue",
        hex: "#3b82f6",
        bgClass: "bg-blue-500 hover:bg-blue-600",
    },
    {
        id: "green",
        label: "Green",
        hex: "#22c55e",
        bgClass: "bg-green-500 hover:bg-green-600",
    },
    {
        id: "orange",
        label: "Orange",
        hex: "#fb923c",
        bgClass: "bg-orange-400 hover:bg-orange-500",
    },
    {
        id: "purple",
        label: "Purple",
        hex: "#a855f7",
        bgClass: "bg-purple-500 hover:bg-purple-600",
    },
    {
        id: "red",
        label: "Red",
        hex: "#ef4444",
        bgClass: "bg-red-500 hover:bg-red-600",
    },
    {
        id: "teal",
        label: "Teal",
        hex: "#14b8a6",
        bgClass: "bg-teal-500 hover:bg-teal-600",
    },
];

export const DEFAULT_COLOR_ID = "blue";

export function getColorById(id: string): EventColor {
    return (
        EVENT_COLORS.find((c) => c.id === id) ||
        EVENT_COLORS.find((c) => c.id === DEFAULT_COLOR_ID)!
    );
}

export function getColorByHex(hex: string): EventColor {
    return (
        EVENT_COLORS.find((c) => c.hex === hex) ||
        EVENT_COLORS.find((c) => c.id === DEFAULT_COLOR_ID)!
    );
}
