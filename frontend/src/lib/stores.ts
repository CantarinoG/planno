import { writable } from "svelte/store";

export type ToastType = "success" | "error" | "info";

export interface Toast {
    id: number;
    message: string;
    type: ToastType;
}

export const toasts = writable<Toast[]>([]);

export const addToast = (message: string, type: ToastType = "info") => {
    const id = Date.now();
    toasts.update((all) => [...all, { id, message, type }]);
    setTimeout(() => {
        dismissToast(id);
    }, 3000);
};

export const dismissToast = (id: number) => {
    toasts.update((all) => all.filter((t) => t.id !== id));
};

export const isLoading = writable(false);

export type Theme = "light" | "dark";

const getInitialTheme = (): Theme => {
    if (typeof window === "undefined") return "light";
    const stored = localStorage.getItem("theme") as Theme | null;
    if (stored) return stored;
    return window.matchMedia("(prefers-color-scheme: dark)").matches
        ? "dark"
        : "light";
};

export const theme = writable<Theme>(getInitialTheme());

if (typeof window !== "undefined") {
    theme.subscribe((val) => {
        localStorage.setItem("theme", val);
    });
}

export const toggleTheme = () => {
    theme.update((current) => (current === "light" ? "dark" : "light"));
};

export interface UserProfile {
    username: string;
    email: string;
}

export const user = writable<UserProfile | null>(null);

export const logoutUser = async () => {
    try {
        const { logout } = await import("./api");
        await logout();
    } catch (e) {
        console.error("Logout failed:", e);
    } finally {
        user.set(null);
        if (typeof window !== "undefined") {
            window.location.href = "/auth";
        }
    }
};
