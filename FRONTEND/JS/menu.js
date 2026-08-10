// menu.js — conectado a la API (solo almuerzo y cena)
api.requiereLogin();

const MESES = ['enero','febrero','marzo','abril','mayo','junio','julio','agosto','septiembre','octubre','noviembre','diciembre'];
const EMOJI = ['🍽️', '🌙'];
const CLASE = ['comida', 'cena'];
const $ = id => document.getElementById(id);

let vista = new Date(); vista.setDate(1);

const keyDe = f => `${f.getFullYear()}-${String(f.getMonth() + 1).padStart(2, "0")}-${String(f.getDate()).padStart(2, "0")}`;
const claveHoy = () => keyDe(new Date());

async function cargarPlatos() {
  const platos = await api.get("/api/platos");
  $("f-plato").innerHTML = platos.length
    ? platos.map(p => `<option value="${p.id}">${p.nombre}</option>`).join("")
    : `<option value="">(no hay platos: créalos en "Comidas")</option>`;
}

async function render() {
  const anio = vista.getFullYear(), mes = vista.getMonth();
  $("txt-mes").textContent = MESES[mes];
  $("txt-anio").textContent = anio;

  const dias = await api.get(`/api/dias/mes?anio=${anio}&mes=${mes + 1}`);
  const porFecha = Object.fromEntries(dias.map(d => [d.fecha.slice(0, 10), d]));

  const hoy = porFecha[claveHoy()];
  $("nota").value = hoy?.nota ?? "";

  const inicio = (new Date(anio, mes, 1).getDay() + 6) % 7;
  const diasMes = new Date(anio, mes + 1, 0).getDate();
  const diasPrev = new Date(anio, mes, 0).getDate();
  const total = Math.ceil((inicio + diasMes) / 7) * 7;

  let html = "";
  for (let i = 0; i < total; i++) {
    const num = i - inicio + 1;
    const otro = num < 1 || num > diasMes;
    const f = otro
      ? (num < 1 ? new Date(anio, mes - 1, diasPrev + num) : new Date(anio, mes + 1, num - diasMes))
      : new Date(anio, mes, num);
    const k = keyDe(f);
    const dia = porFecha[k];

    const chips = (otro || !dia) ? "" : dia.platos.map(pl =>
      `<span class="chip t-${CLASE[pl.tipoComida]}" data-fecha="${k}" data-plato="${pl.idPlato}" data-tipo="${pl.tipoComida}"
              title="${pl.nombre} (clic para quitar)">${EMOJI[pl.tipoComida]} ${pl.nombre}</span>`).join("");

    html += `<div class="dia${otro ? " otro" : ""}${!otro && k === claveHoy() ? " hoy" : ""}" data-fecha="${k}">
               <span class="num">${f.getDate()}</span>${chips}</div>`;
  }
  $("grid").innerHTML = html;
}

// Clic en el calendario: quitar chip o abrir modal con sugerencias
$("grid").addEventListener("click", async e => {
  const chip = e.target.closest(".chip");
  if (chip) {
    if (!confirm("¿Quitar este plato del día?")) return;
    await api.del(`/api/dias/plato?fecha=${chip.dataset.fecha}&idPlato=${chip.dataset.plato}&tipo=${chip.dataset.tipo}`);
    return render();
  }
  const dia = e.target.closest(".dia");
  if (dia && !dia.classList.contains("otro")) abrirModal(dia.dataset.fecha);
});

// Botón flotante → página de comidas
$("btn-add").addEventListener("click", () => location.href = "comida.html");

// ===== Modal =====
async function abrirModal(fechaK) {
  $("f-fecha").value = fechaK;
  $("modal").classList.add("open");
  await cargarSugerencias(fechaK);
}

async function cargarSugerencias(fechaK) {
  const cont = $("sugerencias");
  cont.innerHTML = `<p class="mini">Calculando…</p>`;
  try {
    const sugs = await api.get(`/api/dias/sugerencia?fecha=${fechaK}T00:00:00`);
    cont.innerHTML = sugs.length
      ? sugs.map(s => `<button type="button" class="sugerencia" data-id="${s.idPlato}" data-tipo="${s.tipoComida}"
                     title="${s.motivo}">⭐ ${s.nombrePlato}</button>`).join("")
      : `<p class="mini">Sin sugerencias.</p>`;
  } catch { cont.innerHTML = ""; }
}

// Clic en una ⭐ → asigna directo al día
$("sugerencias").addEventListener("click", async e => {
  const btn = e.target.closest(".sugerencia");
  if (!btn) return;
  await api.post("/api/dias/plato", {
    fecha: $("f-fecha").value + "T00:00:00",
    idPlato: +btn.dataset.id,
    tipoComida: +btn.dataset.tipo
  });
  $("modal").classList.remove("open");
  await render();
});

$("btn-cancel").addEventListener("click", () => $("modal").classList.remove("open"));
$("modal").addEventListener("click", e => { if (e.target === $("modal")) $("modal").classList.remove("open"); });

// Asignar desde el catálogo completo
$("form-plato").addEventListener("submit", async e => {
  e.preventDefault();
  try {
    const idPlato = $("f-plato").value;
    if (!idPlato) throw new Error('Aún no hay platos. Créalos en la pestaña "Comidas".');

    await api.post("/api/dias/plato", {
      fecha: $("f-fecha").value + "T00:00:00",
      idPlato: +idPlato,
      tipoComida: +$("f-tipo").value
    });

    $("modal").classList.remove("open");
    await render();
  } catch (err) { alert(err.message); }
});

// Nota del día
$("btn-nota").addEventListener("click", async () => {
  await api.put("/api/dias/nota", { fecha: claveHoy() + "T00:00:00", nota: $("nota").value });
  alert("Nota guardada ✔");
});

// 🧠 Sugerencia rápida para hoy (la nº1 de las 3)
$("btn-sugerencia").addEventListener("click", async () => {
  try {
    const sugs = await api.get(`/api/dias/sugerencia?fecha=${claveHoy()}T00:00:00`);
    if (!sugs.length) return alert("No hay platos suficientes.");
    const s = sugs[0];
    if (confirm(`🧠 Sugerencia: ${s.nombrePlato} (${CLASE[s.tipoComida]})\n${s.motivo}\n\n¿Lo añado a hoy?`)) {
      await api.post("/api/dias/plato", { fecha: claveHoy() + "T00:00:00", idPlato: s.idPlato, tipoComida: s.tipoComida });
      await render();
    }
  } catch (err) { alert(err.message); }
});

// Navegación de meses
$("btn-prev").addEventListener("click", () => { vista.setMonth(vista.getMonth() - 1); render(); });
$("btn-next").addEventListener("click", () => { vista.setMonth(vista.getMonth() + 1); render(); });

// Arranque
(async () => { await cargarPlatos(); await render(); })();