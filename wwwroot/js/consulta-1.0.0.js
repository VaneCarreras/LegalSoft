
window.onload = ListadoConsultas();




$(document).ready(function () {
    $('#NombreCompletoClienteBuscar').on('input', function () {
        const nombre = $(this).val().trim();
        if (nombre.length > 0) {
            $('#NombreCompletoEquipoBuscar')
                .prop('disabled', true)
                .val('') // Limpiar el campo DNI
                .attr('placeholder', 'Borrar cliente para habilitar');
        } else {
            $('#NombreCompletoEquipoBuscar')
                .prop('disabled', false)
                .attr('placeholder', 'Equipo...');
        }
    });

    $('#NombreCompletoEquipoBuscar').on('input', function () {
        const dni = $(this).val().trim();
        if (dni.length > 0) {
            $('#NombreCompletoClienteBuscar')
                .prop('disabled', true)
                .val('') // Limpiar el campo nombre
                .attr('placeholder', 'Borrar equipo para habilitar');
        } else {
            $('#NombreCompletoClienteBuscar')
                .prop('disabled', false)
                .attr('placeholder', 'Cliente...');
        }
    });
});

function ListadoConsultas(pagina = 1){
     const pageSize = 7; // cantidad de clientes por página
        const soloHabilitados = document.getElementById("chkSoloHabilitados").checked;

    $.ajax({
        // la URL para la petición
        url: '../../Consultas/ListadoConsultas',
        // la información a enviar
        // (también es posible utilizar una cadena de datos)
        data: { 
            pagina: pagina,
            tamanioPagina: pageSize,
            soloHabilitados: soloHabilitados
        },
        // especifica si será una petición POST o GET
        type: 'POST',
        // el tipo de información que se espera de respuesta
        dataType: 'json',
        // código a ejecutar si la petición es satisfactoria;
        // la respuesta es pasada como argumento a la función
        success: function (response) {
const vistaConsulta = response.consultas;
            const totalPaginas = response.totalPaginas;
            $("#ModalConsultas").modal("hide");
            LimpiarModal();
            let contenidoTabla = ``;

            $.each(vistaConsulta, function (index, consulta) {  

                const icono = consulta.habilitado 
        ? '<i class="fa-solid fa-user-slash" style="color: #820d19;"></i>' 
        : '<i class="fa-solid fa-user-check" style="color: #0a7c2c;"></i>';
    const tituloBoton = consulta.habilitado ? 'Deshabilitar' : 'Habilitar';
const claseFila = consulta.habilitado ? '' : 'fila-deshabilitada';
                
                contenidoTabla += `
                <tr>
                        <td class="${claseFila}">${consulta.nombreCompletoCliente}</td>
                        <td class="${claseFila}">${consulta.nombreCompletoEquipo}</td>
                                                                        <td class="${claseFila}">${consulta.motivo}</td>


                        <td class="${claseFila}">${consulta.fecha}</td>
                        <td class="${claseFila}">${consulta.estadoConsultaString}</td>

                    <td class="text-center"><button type="button" onclick="AbrirModalEditar(${consulta.consultaID})" title="Editar"><i class="fa-duotone fa-solid fa-angles-right" style="--fa-primary-color: #718cbaff; --fa-secondary-color: #4969a2;"></i></button></td>
<td class="text-center"><button type="button" onclick="DeshabilitarHabilitarConsulta(${consulta.consultaID}, ${consulta.habilitado})" title="${tituloBoton}">${icono}</button></td>

                </tr>
             `;

            });

            document.getElementById("tbody-consultas").innerHTML = contenidoTabla;

            // Si es la primera carga, configurar paginación
            if (!$('#pagination-consultas').data("twbs-pagination")) {
                $('#pagination-consultas').twbsPagination({
                    totalPages: totalPaginas,
                    visiblePages: 5,
                    onPageClick: function (event, page) {
                        ListadoConsultas(page);
                    },
                    first: 'Primera',
                    prev: '<span aria-hidden="true">&laquo;</span>',
                    next: '<span aria-hidden="true">&raquo;</span>',
                    last: 'Última'
                });
            }

        },

        // código a ejecutar si la petición falla;
        // son pasados como argumentos a la función
        // el objeto de la petición en crudo y código de estatus de la petición
        error: function (xhr, status) {
            console.log('Disculpe, existió un problema al cargar el listado');
        }
    });
}

function LimpiarModal(){
    document.getElementById("ClienteID").value = 0;
    document.getElementById("ConsultaID").value = 0;
    document.getElementById("EquipoID").value = 0;
    // document.getElementById("PersonaID").value = 0; 
    // document.getElementById("NombreCompletoCliente").value = ""; 
    // document.getElementById("NombreCompletoEquipo").value = ""; 
    document.getElementById("Descripcion").value = ""; 
        document.getElementById("Motivo").value = ""; 

    document.getElementById("Fecha").value = ""; 
        document.getElementById("EstadoConsulta").value = "";



}

function NuevoRegistro(){
    $("#ModalTitulo").text("Nueva Consulta");
    LimpiarModal();
}

function AbrirModalEditar(ConsultaID){
    
    $.ajax({
        // la URL para la petición
        url: '../../Consultas/ListadoConsultas',
        // la información a enviar
        // (también es posible utilizar una cadena de datos)
        data: { id: ConsultaID},
        // especifica si será una petición POST o GET
        type: 'POST',
        // el tipo de información que se espera de respuesta
        dataType: 'json',
        // código a ejecutar si la petición es satisfactoria;
        // la respuesta es pasada como argumento a la función
        

            success: function (response) {
    let consulta = response.consultas[0]; // ✅ Accedés al primer elemento del array "clientes"


            document.getElementById("ConsultaID").value = consulta.consultaID;
            $("#ModalTitulo").text("Editar Consulta");

            document.getElementById("ClienteID").value = consulta.clienteID;
            document.getElementById("EquipoID").value = consulta.equipoID;
            
            document.getElementById("Descripcion").value = consulta.descripcion;
                        document.getElementById("Motivo").value = consulta.motivo; 
 
            document.getElementById("Fecha").value = consulta.fecha; 
            document.getElementById("EstadoConsulta").value = consulta.estadoConsulta;

            $("#ModalConsultas").modal("show");

        },

        // código a ejecutar si la petición falla;
        // son pasados como argumentos a la función
        // el objeto de la petición en crudo y código de estatus de la petición
        error: function (xhr, status) {
            console.log('Disculpe, existió un problema al consultar el registro para ser modificado.');
        }
    });
}
function GuardarRegistro() {
    let consultaID = document.getElementById("ConsultaID").value;
    let clienteID = document.getElementById("ClienteID").value;
    let equipoID = document.getElementById("EquipoID").value;
    let descripcion = document.getElementById("Descripcion").value.trim();
    let motivo = document.getElementById("Motivo").value.trim();
    let fecha = document.getElementById("Fecha").value;
    let estadoConsulta = document.getElementById("EstadoConsulta").value;

    // Validaciones

    if (clienteID === "" || clienteID === "0") {
        Swal.fire("Debe seleccionar un cliente.");
        return;
    }

    if (equipoID === "" || equipoID === "0") {
        Swal.fire("Debe seleccionar un equipo.");
        return;
    }

    if (descripcion.length < 5) {
        Swal.fire("La descripción debe tener al menos 5 caracteres.");
        return;
    }

    if (motivo.length < 5) {
        Swal.fire("El motivo debe tener al menos 5 caracteres.");
        return;
    }

    if (fecha === "") {
        Swal.fire("Debe ingresar una fecha.");
        return;
    }

    if (descripcion.length > 500) {
    Swal.fire("La descripción no debe superar los 500 caracteres.");
    return;
}
if (motivo.length > 50) {
    Swal.fire("El motivo no debe superar los 50 caracteres.");
    return;
}


    let hoy = new Date();
    let fechaConsulta = new Date(fecha);

    if (fechaConsulta > hoy) {
        Swal.fire("La fecha no puede ser futura.");
        return;
    }

    if (estadoConsulta === "" || estadoConsulta === "0") {
        Swal.fire("Debe seleccionar un estado para la consulta.");
        return;
    }

    // Si pasa todas las validaciones, continuar con AJAX

    let url = consultaID == 0 || consultaID == "" ?
        '../../Consultas/GuardarNuevaConsulta' :
        '../../Consultas/EditarConsulta';

    let data = {
        consultaID: consultaID,
        clienteID: clienteID,
        equipoID: equipoID,
        descripcion: descripcion,
        motivo: motivo,
        fecha: fecha,
        estadoConsulta: estadoConsulta
    };

    $.ajax({
        url: url,
        type: 'POST',
        data: data,
        dataType: 'json',
        success: function (resultado) {
            if (resultado != "") {
                alert(resultado);
            }
            ListadoConsultas();
            $("#ModalConsultas").modal("hide");
        },
        error: function (xhr, status) {
            console.log('Error al guardar la consulta.');
        }
    });
}





function BuscarConsulta() {
    // Obtener los valores de búsqueda
    const nombreCompletoClienteBuscar = $('#NombreCompletoClienteBuscar').val().trim();
    const nombreCompletoEquipoBuscar = $('#NombreCompletoEquipoBuscar').val().trim();

    $.ajax({
        // URL para la petición
        url: '../../Consultas/BuscarConsultas', // Asegúrate que esta URL es correcta
        // Datos a enviar
        data: { nombreCompletoClienteBuscar: nombreCompletoClienteBuscar, nombreCompletoEquipoBuscar: nombreCompletoEquipoBuscar },
        // Especifica si será una petición POST o GET
        type: 'POST',
        // Tipo de información que se espera de respuesta
        dataType: 'json',
        // Código a ejecutar si la petición es satisfactoria
        success: function (vistaConsulta) {
            $("#ModalConsultas").modal("hide");
            LimpiarModal(); // Opcional: Si quieres limpiar el modal después de la búsqueda

            let contenidoTabla = ``;

            $.each(vistaConsulta, function (index, consulta) {
                const icono = consulta.habilitado 
        ? '<i class="fa-solid fa-user-slash" style="color: #820d19;"></i>' 
        : '<i class="fa-solid fa-user-check" style="color: #0a7c2c;"></i>';
    const tituloBoton = consulta.habilitado ? 'Deshabilitar' : 'Habilitar';
const claseFila = consulta.habilitado ? '' : 'fila-deshabilitada';
                contenidoTabla += `
                <tr>
                    <td class="${claseFila}">${consulta.nombreCompletoCliente}</td>
                    <td class="${claseFila}">${consulta.nombreCompletoEquipo}</td>
                    <td class="${claseFila}">${consulta.descripcion}</td>
                                        <td class="${claseFila}">${consulta.motivo}</td>

                    <td class="${claseFila}">${consulta.fecha}</td>
                                        <td class="${claseFila}">${consulta.estadoConsultaString}</td>

                    <td class="text-center"><button type="button" onclick="AbrirModalEditar(${consulta.consultaID})" title="Editar"><i class="fa-duotone fa-solid fa-angles-right" style="--fa-primary-color: #4969a2; --fa-secondary-color: #4969a2;"></i></button></td>
<td class="text-center"><button type="button" onclick="DeshabilitarHabilitarConsulta(${consulta.consultaID}, ${consulta.habilitado})" title="${tituloBoton}">${icono}</button></td>

                    
                </tr>
             `;
            });
            // Actualizar el contenido de la tabla
            document.getElementById("tbody-consultas").innerHTML = contenidoTabla;
        },

        // Código a ejecutar si la petición falla
        error: function (xhr, status) {

            console.log('Disculpe, existió un problema al buscar consultas');
        }
    });
}


function EliminarRegistro(ConsultaID) {
    Swal.fire({
        title: "¿Seguro de eliminar?",
        icon: "question",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Sí, eliminar"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '../../Consultas/EliminarConsulta',
                data: { consultaID: ConsultaID },
                type: 'POST',
                dataType: 'json',
                success: function (resultado) {
                    Swal.fire({
                        title: "¡Eliminado!",
                        icon: "success"
                    });
                    ListadoConsultas(); // Refresca la tabla después de eliminar
                },
                error: function (xhr, status) {
                    console.log('Disculpe, existió un problema al eliminar el registro.');
                    Swal.fire({
                        title: "Error",
                        text: "Hubo un problema al eliminar el registro.",
                        icon: "error"
                    });
                }
            });
        }
    });
}

function DeshabilitarHabilitarConsulta(ConsultaID, estaHabilitado) {
    const accion = estaHabilitado ? 'deshabilitar' : 'habilitar';
    const mensaje = `¿Seguro de ${accion} este consulta?`;

    Swal.fire({
        title: mensaje,
        icon: 'question',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: `Sí, ${accion}`
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '../../Consultas/CambiarEstadoConsulta',
                type: 'POST',
                dataType: 'json',
                data: {
                    consultaID: ConsultaID,
                    habilitar: !estaHabilitado
                },
                success: function () {
                    Swal.fire({
                        title: `Consulta ${accion}da correctamente`,
                        icon: 'success'
                    });
                    ListadoConsultas(); // Recarga la tabla
                },
                error: function () {
                    Swal.fire({
                        title: 'Error',
                        text: `No se pudo ${accion} el consulta.`,
                        icon: 'error'
                    });
                }
            });
        }
    });
}