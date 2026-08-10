// comida.js — catálogo de platos
api.requiereLogin();
const $ = id => document.getElementById(id);

async function cargarLista() {
  const platos = await api.get("/api/platos");
  $("contador").textContent = platos.length;

  $("lista").innerHTML = platos.length
    ? platos.map(p => `
        <div class="item">
          <div class="info">
            <div class="nombre">🍽️ ${p.nombre}</div>
            <div class="detalle">${p.ingredientes ? "🧺 " + p.ingredientes : "🧺 Sin ingredientes"}</div>
            ${p.receta ? `<div class="detalle">📖 ${p.receta}</div>` : ""}
          </div>
          <button class="btn-borrar" data-id="${p.id}">Eliminar</button>
        </div>`).join("")
    : `<p class="vacio">Aún no has añadido ninguna comida. ¡Estrena tu recetario! ✨</p>`;
}

// Crear plato: solo el nombre es obligatorio
$("form-comida").addEventListener("submit", async e => {
  e.preventDefault();
  try {
    await api.post("/api/platos", {
      nombre: $("f-nombre").value.trim(),
      ingredientes: $("f-ingredientes").value.trim(),   // opcional → ""
      receta: $("f-receta").value.trim()                // opcional → ""
    });
    $("f-nombre").value = $("f-ingredientes").value = $("f-receta").value = "";
    await cargarLista();
  } catch (err) { alert(err.message); }
});

// Eliminar plato
$("lista").addEventListener("click", async e => {
  const btn = e.target.closest(".btn-borrar");
  if (!btn) return;
  if (!confirm("¿Eliminar este plato? (si está en un día, quítalo del calendario antes)")) return;
  try {
    await api.del(`/api/platos/${btn.dataset.id}`);
    await cargarLista();
  } catch (err) { alert(err.message); }
});

cargarLista();