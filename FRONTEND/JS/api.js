const API_URL = (location.hostname === "localhost" || location.hostname === "127.0.0.1")
  ? "http://localhost:5156"                            // desarrollo en tu PC
  : "https://daily-food-ouhl.onrender.com";            // producción (Render)

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