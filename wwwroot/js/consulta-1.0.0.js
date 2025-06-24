
window.onload = ListadoConsultas();

function ListadoConsultas(){
 
    $.ajax({
        // la URL para la petición
        url: '../../Consultas/ListadoConsultas',
        // la información a enviar
        // (también es posible utilizar una cadena de datos)
        data: { },
        // especifica si será una petición POST o GET
        type: 'POST',
        // el tipo de información que se espera de respuesta
        dataType: 'json',
        // código a ejecutar si la petición es satisfactoria;
        // la respuesta es pasada como argumento a la función
        success: function (vistaConsulta) {

            $("#ModalConsultas").modal("hide");
            LimpiarModal();
            let contenidoTabla = ``;

            $.each(vistaConsulta, function (index, consulta) {  
                
                contenidoTabla += `
                <tr>
                        <td>${consulta.nombreCompletoCliente}</td>
                        <td>${consulta.nombreCompletoEquipo}</td>
                        
                        <td>${consulta.descripcion}</td>
                        <td>${consulta.fecha}</td>
                    <td class="text-center">
                    <button type="button"  onclick="AbrirModalEditar(${consulta.consultaID})" title="Editar" >
                    <i class="fa-solid fa-pen-nib" style="color: #B300FC;"></i>
                    </button>
                    </td>
                    <td class="text-center">
                    <button type="button"   onclick="EliminarRegistro(${consulta.consultaID})" title="Eliminar">
                    <i class="fa-solid fa-poo" style="color: #820d19;"></i>
                    </button>
                    </td>
                </tr>
             `;

            });

            document.getElementById("tbody-consultas").innerHTML = contenidoTabla;

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
    document.getElementById("Fecha").value = ""; 


}

function NuevoRegistro(){
    $("#ModalTitulo").text("Nueva Consulta");
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
        success: function (vistaConsulta) {
            let consulta = vistaConsulta[0];

            document.getElementById("ConsultaID").value = consulta.consultaID;
            $("#ModalTitulo").text("Editar Consulta");

            document.getElementById("ClienteID").value = consulta.clienteID;
            document.getElementById("EquipoID").value = consulta.equipoID;
            
            document.getElementById("Descripcion").value = consulta.descripcion; 
            document.getElementById("Fecha").value = consulta.fecha; 

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
    


    let clienteID =        document.getElementById("ClienteID").value;
    let equipoID =        document.getElementById("EquipoID").value;
    // let nombreCompletoCliente =        document.getElementById("NombreCompletoCliente").value; 
    // let nombreCompletoEquipo =        document.getElementById("NombreCompletoEquipo").value; 
    let descripcion =        document.getElementById("Descripcion").value; 
    let fecha =        document.getElementById("Fecha").value;

    if (consultaID == 0 || consultaID == "") {
        // Llamar al método de creación si ClienteID es 0 o está vacío
        $.ajax({
            url: '../../Consultas/GuardarNuevaConsulta', // Método para crear nuevo cliente
            type: 'POST',
            data: {
                // nombreCompletoCliente: nombreCompletoCliente,
                // nombreCompletoEquipo: nombreCompletoEquipo,
                clienteID: clienteID,
                equipoID: equipoID,
                descripcion: descripcion,
                fecha: fecha,

            },
            dataType: 'json',
            success: function (resultado) {
                if (resultado != "") {
                    alert(resultado);
                }
                ListadoConsultas(); // Refresca la lista de clientes
                $("#ModalConsultas").modal("hide"); // Cierra el modal
            },
            error: function (xhr, status) {
                console.log('Error al guardar la nueva consulta.');
            }
        });
    } else {
        // Llamar al método de edición si ClienteID es distinto de 0
        $.ajax({
            url: '../../Consultas/EditarConsulta', // Llamar al nuevo método EditarCliente
            type: 'POST',
            data: {
                consultaID: consultaID,
                clienteID: clienteID,
                equipoID: equipoID,
                
                descripcion: descripcion,
                fecha: fecha,
                
            },

            dataType: 'json',
            success: function (resultado) {
                if (resultado != "") {
                    alert(resultado);
                }
                ListadoConsultas(); // Refresca la lista de clientes
                $("#ModalConsultas").modal("hide"); // Cierra el modal
            },

            error: function (xhr, status) {
                console.log('Error al actualizar la consulta.');
            }
        });
    }
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
                contenidoTabla += `
                <tr>
                    <td>${consulta.nombreCompletoCliente}</td>
                    <td>${consulta.nombreCompletoEquipo}</td>
                    <td>${consulta.descripcion}</td>
                    <td>${consulta.fecha}</td>
                    <td class="text-center">
                        <button type="button" onclick="AbrirModalEditar(${consulta.consultaID})">
                            <i class="fa-solid fa-pen-nib" style="color: #B300FC;"></i>
                        </button>
                    </td>
                    <td class="text-center">
                        <button type="button" onclick="EliminarRegistro(${consulta.consultaID})">
                            <i class="fa-solid fa-poo" style="color: #820d19;"></i>
                        </button>
                    </td>
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
