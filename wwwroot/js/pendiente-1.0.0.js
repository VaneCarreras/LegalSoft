document.addEventListener('DOMContentLoaded', function () {
  const calendarEl = document.getElementById('calendar');

  // Validar que el div exista
  if (!calendarEl) {
    console.warn("No se encontró el div #calendar. Se omite carga de FullCalendar.");
    return;
  }

  const calendar = new FullCalendar.Calendar(calendarEl, {
    initialView: 'dayGridMonth',
    locale: 'es',
    selectable: true,
    events: '/Pendientes/GetPendientes',

    dateClick: function (info) {
      LimpiarModal();
      $('#PendienteID').val(0);
      $('#FechaHora').val(info.dateStr + 'T00:00');
      $('#ModalPendientes').modal('show');
    },

    eventClick: function (info) {
      const pendiente = info.event.extendedProps;
      $('#PendienteID').val(info.event.id);
      $('#Motivo').val(pendiente.motivo);
      $('#EquipoID').val(pendiente.equipoID);
      $('#FechaHora').val(moment(info.event.start).format('YYYY-MM-DDTHH:mm'));
      $('#RecordatorioAlert').prop('checked', pendiente.recordatorio);
      $('#Estado').val(pendiente.estado);

      $('#ModalPendientes').modal('show');
    },
  


  
    // Personalizar contenido de los eventos
    eventContent: function (arg) {
      const estado = arg.event.extendedProps.estado;
      let color = '#007bff'; // Azul por defecto

      if (estado === "Realizado") {
        color = 'green'; // Realizado
      } else if (estado === "No realizado") {
        color = 'red'; // NoRealizado
      }

      const dotEl = document.createElement('span');
      dotEl.style.backgroundColor = color;
      dotEl.style.borderRadius = '50%';
      dotEl.style.display = 'inline-block';
      dotEl.style.width = '10px';
      dotEl.style.height = '10px';
      dotEl.style.marginRight = '6px';
      dotEl.style.verticalAlign = 'middle';

      const textEl = document.createElement('span');
      textEl.innerText = arg.event.title;

      return { domNodes: [dotEl, textEl] };
    }
  });

  calendar.render();

  cargarEquipos();

  // Guardar pendiente (crear o editar)
  window.GuardarPendiente = function () {
  const pendienteID = parseInt($('#PendienteID').val());
  const motivo = $('#Motivo').val().trim();
  const equipoID = parseInt($('#EquipoID').val());
  const fechaHora = $('#FechaHora').val();
  const recordatorio = $('#RecordatorioAlert').is(':checked');
  const estado = $('#Estado').val();

  // Validaciones
  if (!motivo || motivo.length < 3 || motivo.length > 50) {
    Swal.fire('El motivo es obligatorio y debe tener entre 3 y 50 caracteres.');
    return;
  }

  if (!equipoID || isNaN(equipoID)) {
    Swal.fire('Debe seleccionar un equipo.');
    return;
  }

  if (!fechaHora) {
    Swal.fire('Debe ingresar una fecha y hora.');
    return;
  }

  const fechaIngresada = new Date(fechaHora);
  const ahora = new Date();
  if (fechaIngresada < ahora) {
    Swal.fire('La fecha y hora deben ser iguales o posteriores a la actual.');
    return;
  }

  if (!estado || estado.trim() === "") {
    Swal.fire('Debe seleccionar un estado.');
    return;
  }

  // Si todo está bien, armar el objeto
  const pendiente = {
    PendienteID: pendienteID,
    Motivo: motivo,
    EquipoID: equipoID,
    FechaHora: fechaHora,
    RecordatorioAlert: recordatorio,
    Estado: estado
  };

  // Enviar por AJAX
  $.ajax({
    type: 'POST',
    url: '/Pendientes/SavePendiente',
    data: JSON.stringify(pendiente),
    contentType: 'application/json',
    success: function (response) {
      if (response.success) {
        $('#ModalPendientes').modal('hide');
    calendar.refetchEvents();

        if (pendiente.PendienteID === 0) {
          // Nuevo
          calendar.addEvent({
            id: response.id,
            title: pendiente.Motivo,
            start: pendiente.FechaHora,
            extendedProps: {
              motivo: pendiente.Motivo,
              equipoID: pendiente.EquipoID,
              recordatorio: pendiente.RecordatorioAlert,
              estado: pendiente.Estado
            }
          });
        } else {
          // Edición
          const existingEvent = calendar.getEventById(pendiente.PendienteID.toString());
          if (existingEvent) {
            existingEvent.setProp('title', pendiente.Motivo);
            existingEvent.setStart(pendiente.FechaHora);
            existingEvent.setExtendedProp('motivo', pendiente.Motivo);
            existingEvent.setExtendedProp('equipoID', pendiente.EquipoID);
            existingEvent.setExtendedProp('recordatorio', pendiente.RecordatorioAlert);
            existingEvent.setExtendedProp('estado', pendiente.Estado);
          }
        }
      }
    }
  });
};

});

// Cargar equipos
function cargarEquipos() {
  $('#EquipoID').empty();
    $('#EquipoID').append('<option value="" disabled selected>SELECCIONAR</option>');

  $.get('/Pendientes/GetEquipos', function (equipos) {
    equipos.forEach(function (e) {
      $('#EquipoID').append(`<option value="${e.equipoID}">${e.nombreCompleto}</option>`);
    });
  });
}

// Limpiar modal
function LimpiarModal() {
  $('#PendienteID').val(0);
  $('#Motivo').val('');
  $('#EquipoID').val('');
  $('#FechaHora').val('');
  $('#RecordatorioAlert').prop('checked', false);
}

