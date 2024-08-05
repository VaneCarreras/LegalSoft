

document.addEventListener('DOMContentLoaded', function() {
    var calendarEl = document.getElementById('calendar');
  
    var calendar = new FullCalendar.Calendar(calendarEl, {
      initialView: 'dayGridMonth',
      selectable: true,
      dateClick: function(info) {
        // Mostrar el modal
        $('#ModalTurnos').modal('show');
        // Configurar la fecha seleccionada en el campo de fecha del modal
        $('#FechaHora').val(info.dateStr + 'T00:00');
      }
    });
  
    calendar.render();
  });
  
  function GuardarTurno() {
    var turnoID = $('#TurnoID').val();
    var clienteID = $('#ClienteID').val();
    var equipoID = $('#EquipoID').val();
    var fechaHora = $('#FechaHora').val();
  
    // Aquí puedes agregar la lógica para guardar los datos en tu backend
    // Ejemplo de cómo agregar un evento al calendario después de guardar los datos:
    if (clienteID && equipoID && fechaHora) {
      var eventData = {
        title: `Cliente: ${clienteID} - Equipo: ${equipoID}`,
        start: fechaHora,
        description: `Turno ID: ${turnoID}`
      };
      calendar.addEvent(eventData);
      $('#ModalTurnos').modal('hide');
      LimpiarModal(); // Limpiar el formulario después de guardar
    }
  }
  
  function LimpiarModal() {
    $('#TurnoID').val('0');
    $('#ClienteID').val('');
    $('#EquipoID').val('');
    $('#FechaHora').val('');
  }
  
