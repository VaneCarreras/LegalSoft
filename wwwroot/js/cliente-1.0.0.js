
window.onload = ListadoClientes();

function ListadoClientes(){
 
    $.ajax({
        // la URL para la petición
        url: '../../Clientes/ListadoClientes',
        // la información a enviar
        // (también es posible utilizar una cadena de datos)
        data: { },
        // especifica si será una petición POST o GET
        type: 'POST',
        // el tipo de información que se espera de respuesta
        dataType: 'json',
        // código a ejecutar si la petición es satisfactoria;
        // la respuesta es pasada como argumento a la función
        success: function (vistaCliente) {

            $("#ModalClientes").modal("hide");
            // LimpiarModal();
            let contenidoTabla = ``;

            $.each(vistaCliente, function (index, cliente) {  
                
                contenidoTabla += `
                <tr>
                        <td>${cliente.nombreCompleto}</td>
                        <td>${cliente.nroTipoDoc}</td>
                        <td>${cliente.direccion}</td>
                        <td>${cliente.telefono}</td>
                        <td>${cliente.fechaNac}</td>
                    <td class="text-center">
                    <button type="button"  onclick="AbrirModalEditar(${cliente.clienteID})">
                    <i class="fa-solid fa-pen-nib" style="color: #B300FC;"></i>
                    </button>
                    </td>
                    <td class="text-center">
                    <button type="button"   onclick="EliminarRegistro(${cliente.clienteID})">
                    <i class="fa-solid fa-poo" style="color: #820d19;"></i>
                    </button>
                    </td>
                </tr>
             `;

            });

            document.getElementById("tbody-clientes").innerHTML = contenidoTabla;

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
    // document.getElementById("ClienteID").value = 0;
    // document.getElementById("PersonaID").value = 0; 
}

function NuevoRegistro(){
    $("#ModalTitulo").text("Nuevo Cliente");
}

function AbrirModalEditar(ClienteID){
    
    $.ajax({
        // la URL para la petición
        url: '../../Clientes/ListadoClientes',
        // la información a enviar
        // (también es posible utilizar una cadena de datos)
        data: { id: ClienteID},
        // especifica si será una petición POST o GET
        type: 'POST',
        // el tipo de información que se espera de respuesta
        dataType: 'json',
        // código a ejecutar si la petición es satisfactoria;
        // la respuesta es pasada como argumento a la función
        success: function (vistaCliente) {
            let cliente = vistaCliente[0];

            document.getElementById("ClienteID").value = cliente.ClienteID;
            $("#ModalTitulo").text("Editar Cliente");
            document.getElementById("NombreCompleto").value = cliente.nombreCompleto;
            document.getElementById("NroTipoDoc").value = cliente.nroTipoDoc;
            document.getElementById("Direccion").value = cliente.direccion;
            document.getElementById("Telefono").value = cliente.telefono;
            document.getElementById("FechaNac").value = cliente.fechaNac;

            $("#ModalClientes").modal("show");

        },

        // código a ejecutar si la petición falla;
        // son pasados como argumentos a la función
        // el objeto de la petición en crudo y código de estatus de la petición
        error: function (xhr, status) {
            console.log('Disculpe, existió un problema al consultar el registro para ser modificado.');
        }
    });
}

function GuardarRegistro(){
    //GUARDAMOS EN UNA VARIABLE LO ESCRITO EN EL INPUT DESCRIPCION
    let clienteID = document.getElementById("ClienteID").value;
    let nombreCompleto = document.getElementById("NombreCompleto").value;
    let nroTipoDoc = document.getElementById("NroTipoDoc").value;
    let direccion = document.getElementById("Direccion").value;
    let telefono = document.getElementById("Telefono").value;
    let fechaNac = document.getElementById("FechaNac").value;
    //POR UN LADO PROGRAMAR VERIFICACIONES DE DATOS EN EL FRONT CUANDO SON DE INGRESO DE VALORES Y NO SE NECESITA VERIFICAR EN BASES DE DATOS
    //LUEGO POR OTRO LADO HACER VERIFICACIONES DE DATOS EN EL BACK, SI EXISTE EL ELEMENTO SI NECESITAMOS LA BASE DE DATOS.
    $.ajax({
        // la URL para la petición
        url: '../../Clientes/GuardarNuevoCliente',

        // la información a enviar
        // (también es posible utilizar una cadena de datos)
        // data: {nroTipoDoc: nroTipoDoc, nombreCompleto: nombreCompleto, direccion: direccion, telefono: telefono, fechaNac: fechaNac},
        data: {ClienteID: clienteID, NombreCompleto: nombreCompleto, NroTipoDoc: nroTipoDoc, Direccion: direccion, Telefono: telefono, FechaNac: fechaNac},

        // especifica si será una petición POST o GET
        type: 'POST',
        // el tipo de información que se espera de respuesta
        dataType: 'json',
        // código a ejecutar si la petición es satisfactoria;
        // la respuesta es pasada como argumento a la función

        success: function (resultado) {

            if(resultado != ""){
             alert(resultado);
            }
            ListadoClientes();

        },

        // código a ejecutar si la petición falla;
        // son pasados como argumentos a la función
        // el objeto de la petición en crudo y código de estatus de la petición
        error: function (xhr, status) {
            console.log('Disculpe, existió un problema al guardar el registro');
        }

    });    
}

// // function EliminarRegistro(ClienteID){
// //     $.ajax({
// //         // la URL para la petición
// //         url: '../../Clientes/EliminarCliente',
// //         // la información a enviar
// //         // (también es posible utilizar una cadena de datos)
// //         data: { clienteID: ClienteID},
// //         // especifica si será una petición POST o GET
// //         type: 'POST',
// //         // el tipo de información que se espera de respuesta
// //         dataType: 'json',
// //         // código a ejecutar si la petición es satisfactoria;
// //         // la respuesta es pasada como argumento a la función
// //         success: function (resultado) {        
// //             ListadoClientes();
// //         },

// //         // código a ejecutar si la petición falla;
// //         // son pasados como argumentos a la función
// //         // el objeto de la petición en crudo y código de estatus de la petición
// //         error: function (xhr, status) {
// //             console.log('Disculpe, existió un problema al eliminar el registro.');
// //         }
// //     });    

// }

function EliminarRegistro(ClienteID) {
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
                url: '../../Clientes/EliminarCliente',
                data: { clienteID: ClienteID },
                type: 'POST',
                dataType: 'json',
                success: function (resultado) {
                    Swal.fire({
                        title: "¡Eliminado!",
                        icon: "success"
                    });
                    ListadoClientes(); // Refresca la tabla después de eliminar
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
