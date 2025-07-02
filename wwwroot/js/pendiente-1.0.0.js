document.addEventListener('DOMContentLoaded', function() {
  var calendarEl = document.getElementById('calendar');
  var calendar = new FullCalendar.Calendar(calendarEl, {
    initialView: 'dayGridMonth',
    locale: 'es',
    selectable: true,
    events: '/Pendientes/GetPendientes',

    dateClick: function(info) {
      LimpiarModal();
      $('#PendienteID').val(0);
      $('#FechaHora').val(info.dateStr + 'T00:00');
      $('#ModalPendientes').modal('show');
    },

    eventClick: function(info) {
      var pendiente = info.event.extendedProps;
      $('#PendienteID').val(info.event.id);
      $('#Motivo').val(pendiente.motivo);
      $('#EquipoID').val(pendiente.equipoID);
      $('#FechaHora').val(moment(info.event.start).format('YYYY-MM-DDTHH:mm'));
      $('#RecordatorioAlert').prop('checked', pendiente.recordatorio);
          $('#Estado').val(pendiente.estado); 

      $('#ModalPendientes').modal('show');
    }
  });

  calendar.render();

  cargarEquipos();

  // Check periódicamente para mostrar alertas
  setInterval(function() {
    $.get('/Pendientes/GetPendientes', function(pendientes) {
      pendientes.forEach(p => {
        if (p.recordatorio) {
          let pendienteTime = new Date(p.start);
          let diffMs = pendienteTime - new Date();
          let diffMin = diffMs / (1000 * 60);

          if (diffMin > 59 && diffMin <= 60) {
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

  // Guardar pendiente (crear o editar)
  window.GuardarPendiente = function() {
    var pendiente = {
      PendienteID: parseInt($('#PendienteID').val()),
      Motivo: $('#Motivo').val(),
      EquipoID: parseInt($('#EquipoID').val()),
      FechaHora: $('#FechaHora').val(),
      RecordatorioAlert: $('#RecordatorioAlert').is(':checked'),
          Estado: $('#Estado').val()

    };

    $.ajax({
      type: 'POST',
      url: '/Pendientes/SavePendiente',
      data: JSON.stringify(pendiente),
      contentType: 'application/json',
      success: function(response) {
        if (response.success) {
          $('#ModalPendientes').modal('hide');

          // Si es nuevo, agregar al calendario
          if (pendiente.PendienteID === 0) {
            calendar.addEvent({
              id: response.id, // asumimos que el backend devuelve el ID nuevo
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
            // Si es edición, actualizar el evento en el calendario
            var existingEvent = calendar.getEventById(pendiente.PendienteID.toString());
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

// Cargar equipos en el combo
function cargarEquipos() {
  $('#EquipoID').empty().append('<option value="">Abogado</option>');
  $.get('/Pendientes/GetEquipos', function(equipos) {
    equipos.forEach(function(e) {
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

