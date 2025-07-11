document.addEventListener('DOMContentLoaded', function () {
  const alertados = new Set();

  setInterval(function () {
    $.get('/Pendientes/GetPendientes', function (pendientes) {
      pendientes.forEach(p => {
        if (p.recordatorio && !alertados.has(p.pendienteID)) {
          const pendienteTime = new Date(p.start);
          const diffMin = (pendienteTime - new Date()) / (1000 * 60);

          if (diffMin > 59 && diffMin <= 60) {
            alertados.add(p.pendienteID);
            reproducirSonido();

            Swal.fire({
              icon: 'warning',
              title: 'Recordatorio de pendiente',
              text: `¡Te recordamos: ${p.title}!`,
              confirmButtonText: 'Aceptar'
            });
          }
        }
      });
    });
  }, 60000); // cada 1 minuto
});

function reproducirSonido() {
  const audio = new Audio('/img/alerta.mp3');
  audio.play();
}
