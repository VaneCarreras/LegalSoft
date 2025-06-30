
window.onload = ListadoExpedientes();

function ListadoExpedientes(){
 
    $.ajax({
        // la URL para la petición
        url: '../../Expedientes/ListadoExpedientes',
        // la información a enviar
        // (también es posible utilizar una cadena de datos)
        data: { },
        // especifica si será una petición POST o GET
        type: 'POST',
        // el tipo de información que se espera de respuesta
        dataType: 'json',
        // código a ejecutar si la petición es satisfactoria;
        // la respuesta es pasada como argumento a la función
        success: function (vistaExpediente) {

            $("#ModalExpedientes").modal("hide");
            LimpiarModal();
            let contenidoTabla = ``;

            $.each(vistaExpediente, function (index, expediente) {  
                
                contenidoTabla += `
                <tr>
                       
                        <td>${expediente.numero}</td>
                        <td>${expediente.caratula}</td> 
                
                
                <td>${expediente.nombreCompletoCliente}</td>
                        <td>${expediente.nombreCompletoEquipo}</td>
                        


                        <td>${expediente.fechaInicio}</td>
                            <td>${expediente.fechaFin}</td>

                            <td><a href='  ${expediente.linkContenido}  ' target='_blank' style='text-decoration: underline; color: orange;'> ${expediente.linkContenido} </a></td>

                        <td>${expediente.estadoExpedienteString}</td>

<td class="text-center">
    <button type="button"
            onclick="AbrirModalDocsExpediente(${expediente.expedienteID})"
            data-bs-toggle="modal"
            data-bs-target="#ModalDocsExpediente"
            title="Mostrar Documentos">
<i class="fa-regular fa-folder-open" style="color: #63E6BE;"></i>   </button>
</td>





                    <td class="text-center">
                    <button type="button"  onclick="AbrirModalEditar(${expediente.expedienteID})" title="Editar" >
                    <i class="fa-solid fa-pen-nib" style="color: #B300FC;"></i>
                    </button>
                    </td>
                    <td class="text-center">
                    <button type="button"   onclick="EliminarRegistro(${expediente.expedienteID})" title="Eliminar">
                    <i class="fa-solid fa-poo" style="color: #820d19;"></i>
                    </button>
                    </td>
                </tr>
             `;

            });

            document.getElementById("tbody-expedientes").innerHTML = contenidoTabla;

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
    document.getElementById("ExpedienteID").value = 0;
    document.getElementById("EquipoID").value = 0;
    // document.getElementById("PersonaID").value = 0; 
    // document.getElementById("NombreCompletoCliente").value = ""; 
    // document.getElementById("NombreCompletoEquipo").value = ""; 
    document.getElementById("Numero").value = ""; 
        document.getElementById("Caratula").value = ""; 
    document.getElementById("UltimoDecreto").value = ""; 

    document.getElementById("FechaInicio").value = ""; 
        document.getElementById("FechaFin").value = ""; 
    document.getElementById("LinkContenido").value = ""; 

        document.getElementById("EstadoExpediente").value = 0;



}

function NuevoRegistro(){
    $("#ModalTitulo").text("Nuevo Expediente");
}

function AbrirModalEditar(ExpedienteID){
    
    $.ajax({
        // la URL para la petición
        url: '../../Expedientes/ListadoExpedientes',
        // la información a enviar
        // (también es posible utilizar una cadena de datos)
        data: { id: ExpedienteID},
        // especifica si será una petición POST o GET
        type: 'POST',
        // el tipo de información que se espera de respuesta
        dataType: 'json',
        // código a ejecutar si la petición es satisfactoria;
        // la respuesta es pasada como argumento a la función
        success: function (vistaExpediente) {
            let expediente = vistaExpediente[0];

            document.getElementById("ExpedienteID").value = expediente.expedienteID;
            $("#ModalTitulo").text("Editar Expediente");

            document.getElementById("ClienteID").value = expediente.clienteID;
            document.getElementById("EquipoID").value = expediente.equipoID;
            document.getElementById("Numero").value = expediente.numero;
            document.getElementById("Caratula").value = expediente.caratula;
            document.getElementById("UltimoDecreto").value = expediente.ultimoDecreto;
            document.getElementById("LinkContenido").value = expediente.linkContenido;

            document.getElementById("EstadoExpediente").value = expediente.estadoExpediente;

            $("#ModalExpedientes").modal("show");

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
    let expedienteID = document.getElementById("ExpedienteID").value;
    


    let clienteID =        document.getElementById("ClienteID").value;
    let equipoID =        document.getElementById("EquipoID").value;
    let numero =        document.getElementById("Numero").value;
        let caratula =        document.getElementById("Caratula").value;
        let ultimoDecreto =        document.getElementById("UltimoDecreto").value;
    let fechaInicio =        document.getElementById("FechaInicio").value;
        let fechaFin =        document.getElementById("FechaFin").value;
        let linkContenido =        document.getElementById("LinkContenido").value;

    let estadoExpediente = document.getElementById("EstadoExpediente").value;

    if (expedienteID == 0 || expedienteID == "") {
        // Llamar al método de creación si ClienteID es 0 o está vacío
        $.ajax({
            url: '../../Expedientes/GuardarNuevoExpediente', // Método para crear nuevo cliente
            type: 'POST',
            data: {
                // nombreCompletoCliente: nombreCompletoCliente,
                // nombreCompletoEquipo: nombreCompletoEquipo,
                clienteID: clienteID,
                equipoID: equipoID,
                numero: numero,
                caratula: caratula,
                ultimoDecreto: ultimoDecreto,
                fechaInicio: fechaInicio,
                fechaFin: fechaFin,
                linkContenido: linkContenido,
                estadoExpediente: estadoExpediente,

            },
            dataType: 'json',
            success: function (resultado) {
                if (resultado != "") {
                    alert(resultado);
                }
                ListadoExpedientes(); // Refresca la lista de clientes
                $("#ModalExpedientes").modal("hide"); // Cierra el modal
            },
            error: function (xhr, status) {
                console.log('Error al guardar el nuevo expediente.');
            }
        });
    } else {
        // Llamar al método de edición si ClienteID es distinto de 0
        $.ajax({
            url: '../../Expedientes/EditarExpediente', // Llamar al nuevo método EditarCliente
            type: 'POST',
            data: {
                expedienteID: expedienteID,
                clienteID: clienteID,
                equipoID: equipoID,
                numero: numero,
                caratula: caratula,
                ultimoDecreto: ultimoDecreto,
                fechaInicio: fechaInicio,
                fechaFin: fechaFin,
                linkContenido: linkContenido,
                estadoExpediente: estadoExpediente,
                
            },

            dataType: 'json',
            success: function (resultado) {
                if (resultado != "") {
                    alert(resultado);
                }
                ListadoExpedientes(); // Refresca la lista de clientes
                $("#ModalExpedientes").modal("hide"); // Cierra el modal
            },

            error: function (xhr, status) {
                console.log('Error al actualizar el expediente.');
            }
        });
    }
}


function BuscarExpediente() {
    // Obtener los valores de búsqueda
    const dniClienteBuscar = $('#DniClienteBuscar').val().trim();
    const nroExpBuscar = $('#NroExpBuscar').val().trim();

    $.ajax({
        // URL para la petición
        url: '../../Expedientes/BuscarExpedientes', // Asegúrate que esta URL es correcta
        // Datos a enviar
        data: { dniClienteBuscar: dniClienteBuscar, nroExpBuscar: nroExpBuscar },
        // Especifica si será una petición POST o GET
        type: 'POST',
        // Tipo de información que se espera de respuesta
        dataType: 'json',
        // Código a ejecutar si la petición es satisfactoria
        success: function (vistaExpediente) {
            $("#ModalExpedientes").modal("hide");
            LimpiarModal(); // Opcional: Si quieres limpiar el modal después de la búsqueda

            let contenidoTabla = ``;

            $.each(vistaExpediente, function (index, expediente) {
                contenidoTabla += `
                <tr>
                
                        <td>${expediente.numero}</td>
                        <td>${expediente.caratula}</td>
                        <td>${expediente.nombreCompletoCliente}</td>
                        <td>${expediente.nombreCompletoEquipo}</td>
                        
                        <td>${expediente.fechaInicio}</td>
                            <td>${expediente.fechaFin}</td>

                            <td><a href=' ${expediente.linkContenido} ' target='_blank' style='text-decoration: underline; color: orange;'>  ${expediente.linkContenido} </a></td>

                        <td>${expediente.estadoExpedienteString}</td>

<td class="text-center">
    <button type="button"
            onclick="AbrirModalDocsExpediente(${expediente.expedienteID})"
            data-bs-toggle="modal"
            data-bs-target="#ModalDocsExpediente"
            title="Mostrar Documentos">
<i class="fa-regular fa-folder-open" style="color: #63E6BE;"></i>  </button>
</td>


                    <td class="text-center">
                    <button type="button"  onclick="AbrirModalEditar(${expediente.expedienteID})" title="Editar" >
                    <i class="fa-solid fa-pen-nib" style="color: #B300FC;"></i>
                    </button>
                    </td>
                    <td class="text-center">
                    <button type="button"   onclick="EliminarRegistro(${expediente.expedienteID})" title="Eliminar">
                    <i class="fa-solid fa-poo" style="color: #820d19;"></i>
                    </button>
                    </td>
                </tr>
             `;
            });
            // Actualizar el contenido de la tabla
            document.getElementById("tbody-expedientes").innerHTML = contenidoTabla;
        },

        // Código a ejecutar si la petición falla
        error: function (xhr, status) {

            console.log('Disculpe, existió un problema al buscar expedientes');
        }
    });
}


function EliminarRegistro(ExpedienteID) {
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
                url: '../../Expedientes/EliminarExpediente',
                data: { expedienteID: ExpedienteID },
                type: 'POST',
                dataType: 'json',
                success: function (resultado) {
                    Swal.fire({
                        title: "¡Eliminado!",
                        icon: "success"
                    });
                    ListadoExpedientes(); // Refresca la tabla después de eliminar
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

function GuardarDocumento() {
    console.log("Función GuardarDocumento iniciada");

    var input = document.getElementById("inputDoc");
    var file = input.files[0];

    if (!file) {
        alert("Seleccioná un documento antes de guardar.");
        return;
    }

    var reader = new FileReader();

    reader.onload = function (e) {
        var base64Doc = e.target.result;
        console.log("Base64 generado:", base64Doc);

        var expedienteID = $("#ExpedienteID").val();

        $.ajax({
            type: "POST",
            url: "/Expedientes/GuardarDocumento",
            data: {
                DocumentoAGuardar: base64Doc,
                NombreArchivo: file.name,
                ExpedienteID: expedienteID
            },
            success: function(resultado) {
    if (resultado.resultado === true || resultado === true) {
        alert("Documento guardado correctamente.");
        BuscarDocumentos(expedienteID);
    } else {
        alert("No se pudo guardar el documento. " + (resultado.error ? resultado.error : ""));
    }


            
            },
            error: function (xhr, status, error) {
                console.error("Error en la petición AJAX:", error);
            }
        });
    };

    reader.readAsDataURL(file);
}

function BuscarDocumentos(expedienteID) {
    $("#DocsExpediente").empty();

    $.ajax({
        url: '/Expedientes/BuscarDocumentos',
        data: { ExpedienteID: expedienteID },
        dataType: 'json',
        success: function (listaDocs) {
                console.log("Datos recibidos:", listaDocs);

            if (listaDocs.length === 0) {
                $("#DocsExpediente").append("<p class='text-center'>No hay documentos cargados.</p>");
                return;
            }

            

            $.each(listaDocs, function (index, doc) {
    $("#DocsExpediente").append(
        "<div class='mb-3'>" +
            "<p><strong>" + doc.nombreArchivo + "</strong></p>" +
            "<a href='data:application/octet-stream;base64," + doc.base64 + "' download='" + doc.nombreArchivo + "' class='btn btn-ovalo'>Descargar</a>" +
        "</div>"
    );
});

        },
        error: function () {
            alert("Ocurrió un error al buscar los documentos.");
        }
    });
}

function BuscarDocumentosEliminar(expedienteID) {
    $("#DocsExpedienteEliminar").empty();

    $.ajax({
        url: '/Expedientes/BuscarDocumentos',
        data: { ExpedienteID: expedienteID },
        dataType: 'json',
        success: function (listaDocs) {
            if (listaDocs.length === 0) {
                $("#DocsExpedienteEliminar").append("<tr><td colspan='2' class='text-center'>No hay documentos cargados.</td></tr>");
                return;
            }

            $.each(listaDocs, function (index, doc) {
    $("#DocsExpedienteEliminar").append(
        "<tr>" +
            "<td>" + doc.nombreArchivo + "</td>" +
            "<td class='text-center'>" +
                "<a onclick='EliminarDocumento(" + doc.docID + ");' class='btn-ovaloEliminar' title='Eliminar Documento'>Eliminar</a>" +
            "</td>" +
        "</tr>"
    );
});

        },
        error: function () {
            alert("Ocurrió un error al cargar los documentos.");
        }
    });
}

function EliminarDocumento(docID) {
    var expedienteID = $("#ExpedienteID").val();

    $.ajax({
        url: "/Expedientes/EliminarDocumento",
        type: "POST",
        data: { docID: docID },
        success: function (resultado) {
            console.log("Documento eliminado:", resultado);
            BuscarDocumentos(expedienteID);
            BuscarDocumentosEliminar(expedienteID);
        },
        error: function () {
            alert("Ocurrió un error al eliminar el documento.");
        }
    });
}

function CerrarModalDocsExpediente() {
    $('#ModalDocsExpediente').modal('hide');
}


function AbrirModalDocsExpediente(expedienteID) {
    console.log("Abriendo modal para expediente:", expedienteID);

    // Asignar el ID al input oculto para que GuardarDocumento() lo use correctamente
    $("#ExpedienteID").val(expedienteID);

    // Cargar las listas de documentos en las pestañas Ver y Eliminar
    BuscarDocumentos(expedienteID);
    BuscarDocumentosEliminar(expedienteID);

    // Mostrar el modal
    $('#ModalDocsExpediente').modal('show');
}
