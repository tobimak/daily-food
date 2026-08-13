// login.js — versión con toasts
document.getElementById("form-login").addEventListener("submit", async e => {
  e.preventDefault();
  try {
    const auth = await api.post("/api/auth/login", {
      email: document.getElementById("login-email").value,
      contrasena: document.getElementById("login-pass").value
    });
    api.guardarSesion(auth);
    toast.exito(`¡Hola de nuevo, ${auth.nombre}! 👋`);
    setTimeout(() => location.href = "menu.html", 700);
  } catch (err) {
    toast.error(err.message === "Failed to fetch"
      ? "No se pudo conectar con el servidor. Revisa tu conexión."
      : err.message);
  }
});

document.getElementById("form-register").addEventListener("submit", async e => {
  e.preventDefault();
  try {
    const auth = await api.post("/api/auth/registro", {
      nombre: document.getElementById("reg-nombre").value,
      email: document.getElementById("reg-email").value,
      contrasena: document.getElementById("reg-pass").value
    });
    api.guardarSesion(auth);
    toast.exito("Cuenta creada correctamente 🎉");
    setTimeout(() => location.href = "menu.html", 700);
  } catch (err) {
    toast.error(err.message);
  }
});

// 👁️ Mostrar / ocultar contraseña
document.querySelectorAll(".toggle-pass").forEach(btn => {
  btn.addEventListener("click", () => {
    const input = document.getElementById(btn.dataset.target);
    if (!input) return;
    const mostrar = input.type === "password";
    input.type = mostrar ? "text" : "password";
    btn.textContent = mostrar ? "🙈" : "👁️";
  });
});