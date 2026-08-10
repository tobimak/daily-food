// api.js
const API_URL = "http://localhost:5156"; // ⚠️ puerto http de tu API (míralo en la consola al arrancar)

const api = {
    token: localStorage.getItem("token"),

    guardarSesion(auth) {
        localStorage.setItem("token", auth.token);
        this.token = auth.token;
    },

    cerrarSesion() {
        localStorage.removeItem("token");
        this.token = null;
        location.href = "index.html";
    },

    requiereLogin() {
        if (!this.token) location.href = "index.html";
    },

    async peticion(ruta, metodo = "GET", cuerpo = null) {
        const res = await fetch(API_URL + ruta, {
            method: metodo,
            headers: {
                "Content-Type": "application/json",
                ...(this.token ? { Authorization: `Bearer ${this.token}` } : {})
            },
            body: cuerpo ? JSON.stringify(cuerpo) : null
        });

        if (res.status === 401 && this.token && !location.pathname.endsWith("index.html")) {
            this.cerrarSesion(); return null;
        }
        if (res.status === 204) return true;

        const data = await res.json().catch(() => null);
        if (!res.ok) throw new Error(data?.error || `Error ${res.status}`);
        return data;
    },

    get(r) { return this.peticion(r); },
    post(r, c) { return this.peticion(r, "POST", c); },
    put(r, c) { return this.peticion(r, "PUT", c); },
    del(r) { return this.peticion(r, "DELETE"); }
};