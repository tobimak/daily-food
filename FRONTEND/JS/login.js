// login.js
const msgError = document.getElementById("msg-error");

document.getElementById("form-login").addEventListener("submit", async e => {
  e.preventDefault();
  try {
    const auth = await api.post("/api/auth/login", {
      email: document.getElementById("login-email").value,
      contrasena: document.getElementById("login-pass").value
    });
    api.guardarSesion(auth);
    location.href = "menu.html";   // ✅ antes decía calendario.html
  } catch (err) { msgError.textContent = err.message; }
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
    location.href = "menu.html";   // ✅ antes decía calendario.html
  } catch (err) { msgError.textContent = err.message; }
});

// 👁️ Mostrar / ocultar contraseña
document.querySelectorAll(".toggle-pass").forEach(btn => {
  btn.addEventListener("click", () => {
    const input = document.getElementById(btn.dataset.target);
    if (!input) return;
    const mostrar = input.type === "password";
    input.type = mostrar ? "text" : "password";
    btn.textContent = mostrar ? "👁‍🗨" : "👁️";
  });
});