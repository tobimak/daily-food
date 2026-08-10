// perfil.js — conectado a la API (con foto de perfil)
api.requiereLogin();
const $ = id => document.getElementById(id);

let usuario = null;

async function cargar() {
  usuario = await api.get("/api/usuario");
  $("p-nombre").textContent = usuario.nombre;
  $("p-email").textContent = usuario.email;
  $("p-cocina").textContent = "🍳 Chef MenuChef";
  $("p-alta").textContent = "🗓️ Miembro desde " +
    new Date(usuario.fechaAlta).toLocaleDateString("es-ES", { month: "long", year: "numeric" });
  $("f-nombre").value = usuario.nombre;
  $("f-email").value = usuario.email;
  pintarAvatar();
}

function pintarAvatar() {
  const av = $("avatar");
  if (usuario.foto) {
    av.style.backgroundImage = `url('${usuario.foto}')`;
    av.textContent = "";
  } else {
    av.style.backgroundImage = "";
    const p = usuario.nombre.trim().split(/\s+/);
    av.textContent = ((p[0]?.[0] || "") + (p[1]?.[0] || "")).toUpperCase();
  }
}

// ===== Cambiar foto: reduce a 256px y sube como base64 =====
$("f-foto").addEventListener("change", async e => {
  const file = e.target.files[0];
  if (!file) return;
  if (!file.type.startsWith("image/")) return toast("⚠️ El archivo debe ser una imagen");

  try {
    const dataUrl = await reducirImagen(file, 256);
    usuario = await api.post("/api/usuario/foto", { foto: dataUrl });
    pintarAvatar();
    toast("📷 Foto actualizada");
  } catch (err) { toast(err.message); }
  e.target.value = ""; // permite re-elegir el mismo archivo
});

function reducirImagen(file, size) {
  return new Promise((resolve, reject) => {
    const reader = new FileReader();
    reader.onload = ev => {
      const img = new Image();
      img.onload = () => {
        const canvas = document.createElement("canvas");
        const scale = Math.min(1, size / Math.max(img.width, img.height));
        canvas.width  = Math.round(img.width * scale);
        canvas.height = Math.round(img.height * scale);
        canvas.getContext("2d").drawImage(img, 0, 0, canvas.width, canvas.height);
        resolve(canvas.toDataURL("image/jpeg", 0.85));
      };
      img.onerror = () => reject(new Error("No se pudo leer la imagen."));
      img.src = ev.target.result;
    };
    reader.onerror = () => reject(new Error("No se pudo leer el archivo."));
    reader.readAsDataURL(file);
  });
}

// ===== Guardar cambios de datos =====
$("form-datos").addEventListener("submit", async e => {
  e.preventDefault();
  const p1 = $("f-pass").value, p2 = $("f-pass2").value;
  if (p1 || p2) {
    if (p1 !== p2) return toast("⚠️ Las contraseñas no coinciden");
    if (p1.length < 8) return toast("⚠️ Mínimo 8 caracteres");
  }
  try {
    usuario = await api.put("/api/usuario", {
      nombre: $("f-nombre").value.trim(),
      email: $("f-email").value.trim(),
      contrasenaNueva: p1 || null
    });
    $("f-pass").value = $("f-pass2").value = "";
    await cargar();
    toast("✔ Cambios guardados");
  } catch (err) { toast(err.message); }
});

// ===== Modales =====
const abrir = id => $(id).classList.add("open");
const cerrar = m => m.classList.remove("open");
document.querySelectorAll(".modal").forEach(m =>
  m.addEventListener("click", e => {
    if (e.target === m || e.target.hasAttribute("data-cerrar")) cerrar(m);
  }));

// Cerrar sesión
$("btn-logout").addEventListener("click", () => abrir("modal-logout"));
$("confirm-logout").addEventListener("click", () => api.cerrarSesion());

// Eliminar cuenta
$("btn-delete").addEventListener("click", () => {
  $("f-confirm").value = "";
  $("confirm-delete").disabled = true;
  abrir("modal-delete");
});
$("f-confirm").addEventListener("input", e => {
  $("confirm-delete").disabled = e.target.value.trim().toUpperCase() !== "ELIMINAR";
});
$("confirm-delete").addEventListener("click", async () => {
  await api.del("/api/usuario");
  api.cerrarSesion();
});

// Toast
let tTimer;
function toast(msg) {
  const t = $("toast");
  t.textContent = msg;
  t.classList.add("show");
  clearTimeout(tTimer);
  tTimer = setTimeout(() => t.classList.remove("show"), 2600);
}

cargar();