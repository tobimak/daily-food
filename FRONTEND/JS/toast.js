// toast.js — notificaciones profesionales compartidas
// Uso: toast.error("..."), toast.exito("..."), toast.info("...")
const toast = (() => {
  let cont = null;

  const style = document.createElement("style");
  style.textContent = `
    .df-toasts{ position:fixed; top:18px; right:18px; z-index:99999;
      display:flex; flex-direction:column; gap:10px; pointer-events:none; }
    .df-toast{
      pointer-events:auto; min-width:260px; max-width:340px;
      display:flex; align-items:flex-start; gap:10px;
      padding:12px 16px; border-radius:14px;
      background:#fff; color:#4B3F63;
      box-shadow:0 12px 32px rgba(75,63,99,.28);
      border-left:5px solid #999;
      font:500 .85rem/1.45 'Outfit', system-ui, sans-serif;
      animation:dfIn .35s cubic-bezier(.21,1.02,.73,1);
    }
    .df-toast .df-ico{ font-size:1.05rem; }
    .df-toast.error{ border-color:#E0564A; background:#FFF6F5; }
    .df-toast.exito{ border-color:#57B87A; background:#F3FBF6; }
    .df-toast.info { border-color:#6C9EC9; background:#F3F9FD; }
    .df-toast.out{ animation:dfOut .3s forwards; }
    @keyframes dfIn { from{ opacity:0; transform:translateX(40px);} }
    @keyframes dfOut{ to  { opacity:0; transform:translateX(40px);} }
  `;
  document.head.appendChild(style);

  function avisar(tipo, msg, seg) {
    if (!cont) {
      cont = document.createElement("div");
      cont.className = "df-toasts";
      document.body.appendChild(cont);
    }
    const el = document.createElement("div");
    el.className = `df-toast ${tipo}`;
    const ico = tipo === "error" ? "⚠️" : tipo === "exito" ? "✅" : "ℹ️";
    el.innerHTML = `<span class="df-ico">${ico}</span><span></span>`;
    el.lastElementChild.textContent = msg; // seguro contra inyección de HTML
    cont.appendChild(el);
    setTimeout(() => {
      el.classList.add("out");
      setTimeout(() => el.remove(), 320);
    }, (seg || (tipo === "error" ? 5 : 3)) * 1000);
  }

  return {
    error: (m, s) => avisar("error", m, s),
    exito: (m, s) => avisar("exito", m, s),
    info:  (m, s) => avisar("info",  m, s)
  };
})();