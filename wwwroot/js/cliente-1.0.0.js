
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
            LimpiarModal();
            let contenidoTabla = ``;

            $.each(vistaCliente, function (index, cliente) {  
                
                contenidoTabla += `
                <tr>
                        <td>${cliente.nombreCompleto}</td>
                        <td>${cliente.nroTipoDoc}</td>
                        <td>${cliente.direccion}</td>
                        <td>${cliente.telefono}</td>
                        <td>${cliente.fechaNac}</td>


                    <td class="text-center"><button type="button" onclick="BuscarImagenes(${cliente.clienteID})"  data-bs-toggle="modal" data-bs-target=".MostrarSubirImagenes" title="Mostrar Imagenes"><i class="fa-duotone fa-regular fa-images" style="color:rgb(54, 176, 89);"></i></button></td>


                    <td class="text-center">
                    <button type="button"  onclick="AbrirModalEditar(${cliente.clienteID})" title="Editar">
                    <i class="fa-solid fa-pen-nib" style="color: #B300FC;"></i>
                    </button>
                    </td>
                    <td class="text-center">
                    <button type="button"   onclick="EliminarRegistro(${cliente.clienteID})" title="Eliminar">
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
    document.getElementById("ClienteID").value = 0;
    document.getElementById("NombreCompleto").value = ""; 
    document.getElementById("NroTipoDoc").value = ""; 

    document.getElementById("Direccion").value = ""; 
    document.getElementById("Telefono").value = ""; 
    document.getElementById("FechaNac").value = ""; 


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

            document.getElementById("ClienteID").value = cliente.clienteID;
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

// function GuardarRegistro(){
//     //GUARDAMOS EN UNA VARIABLE LO ESCRITO EN EL INPUT DESCRIPCION
//     let clienteID = document.getElementById("ClienteID").value;
//     let nombreCompleto = document.getElementById("NombreCompleto").value;
//     let nroTipoDoc = document.getElementById("NroTipoDoc").value;
//     let direccion = document.getElementById("Direccion").value;
//     let telefono = document.getElementById("Telefono").value;
//     let fechaNac = document.getElementById("FechaNac").value;
//     //POR UN LADO PROGRAMAR VERIFICACIONES DE DATOS EN EL FRONT CUANDO SON DE INGRESO DE VALORES Y NO SE NECESITA VERIFICAR EN BASES DE DATOS
//     //LUEGO POR OTRO LADO HACER VERIFICACIONES DE DATOS EN EL BACK, SI EXISTE EL ELEMENTO SI NECESITAMOS LA BASE DE DATOS.
//     $.ajax({
//         // la URL para la petición
//         url: '../../Clientes/GuardarNuevoCliente',

//         // la información a enviar
//         // (también es posible utilizar una cadena de datos)
//         // data: {nroTipoDoc: nroTipoDoc, nombreCompleto: nombreCompleto, direccion: direccion, telefono: telefono, fechaNac: fechaNac},
//         data: {ClienteID: clienteID, NombreCompleto: nombreCompleto, NroTipoDoc: nroTipoDoc, Direccion: direccion, Telefono: telefono, FechaNac: fechaNac},

//         // especifica si será una petición POST o GET
//         type: 'POST',
//         // el tipo de información que se espera de respuesta
//         dataType: 'json',
//         // código a ejecutar si la petición es satisfactoria;
//         // la respuesta es pasada como argumento a la función

//         success: function (resultado) {

//             if(resultado != ""){
//              alert(resultado);
//             }
//             ListadoClientes();

//         },

//         // código a ejecutar si la petición falla;
//         // son pasados como argumentos a la función
//         // el objeto de la petición en crudo y código de estatus de la petición
//         error: function (xhr, status) {
//             console.log('Disculpe, existió un problema al guardar el registro');
//         }

//     });    
// }

function GuardarRegistro() {
    let clienteID = document.getElementById("ClienteID").value;
    let nombreCompleto = document.getElementById("NombreCompleto").value;
    let nroTipoDoc = document.getElementById("NroTipoDoc").value;
    let direccion = document.getElementById("Direccion").value;
    let telefono = document.getElementById("Telefono").value;
    let fechaNac = document.getElementById("FechaNac").value;

    if (clienteID == 0 || clienteID == "") {
        // Llamar al método de creación si ClienteID es 0 o está vacío
        $.ajax({
            url: '../../Clientes/GuardarNuevoCliente', // Método para crear nuevo cliente
            type: 'POST',
            data: {
                nombreCompleto: nombreCompleto,
                nroTipoDoc: nroTipoDoc,
                direccion: direccion,
                telefono: telefono,
                fechaNac: fechaNac
            },
            dataType: 'json',
            success: function (resultado) {
                if (resultado != "") {
                    alert(resultado);
                }
                ListadoClientes(); // Refresca la lista de clientes
                $("#ModalClientes").modal("hide"); // Cierra el modal
            },
            error: function (xhr, status) {
                console.log('Error al guardar el nuevo cliente.');
            }
        });
    } else {
        // Llamar al método de edición si ClienteID es distinto de 0
        $.ajax({
            url: '../../Clientes/EditarCliente', // Llamar al nuevo método EditarCliente
            type: 'POST',
            data: {
                ClienteID: clienteID,
                nombreCompleto: nombreCompleto,
                nroTipoDoc: nroTipoDoc,
                direccion: direccion,
                telefono: telefono,
                fechaNac: fechaNac
            },

            dataType: 'json',
            success: function (resultado) {
                if (resultado != "") {
                    alert(resultado);
                }
                ListadoClientes(); // Refresca la lista de clientes
                $("#ModalClientes").modal("hide"); // Cierra el modal
            },

            error: function (xhr, status) {
                console.log('Error al actualizar el cliente.');
            }
        });
    }
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

function BuscarCliente() {
    // Obtener los valores de búsqueda
    const nombreCompleto = $('#buscarNombre').val().trim();
    const nroTipoDoc = $('#buscarDNI').val().trim();

    $.ajax({
        // URL para la petición
        url: '../../Clientes/BuscarClientes', // Asegúrate que esta URL es correcta
        // Datos a enviar
        data: { nombreCompleto: nombreCompleto, nroTipoDoc: nroTipoDoc },
        // Especifica si será una petición POST o GET
        type: 'POST',
        // Tipo de información que se espera de respuesta
        dataType: 'json',
        // Código a ejecutar si la petición es satisfactoria
        success: function (vistaCliente) {
            $("#ModalClientes").modal("hide");
            LimpiarModal(); // Opcional: Si quieres limpiar el modal después de la búsqueda

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
                        <button type="button" onclick="AbrirModalEditar(${cliente.clienteID})">
                            <i class="fa-solid fa-pen-nib" style="color: #B300FC;"></i>
                        </button>
                    </td>
                    <td class="text-center">
                        <button type="button" onclick="EliminarRegistro(${cliente.clienteID})">
                            <i class="fa-solid fa-poo" style="color: #820d19;"></i>
                        </button>
                    </td>
                </tr>
             `;
            });
            // Actualizar el contenido de la tabla
            document.getElementById("tbody-clientes").innerHTML = contenidoTabla;
        },

        // Código a ejecutar si la petición falla
        error: function (xhr, status) {
            console.log(nroTipoDoc);

            console.log('Disculpe, existió un problema al buscar clientes');
        }
    });
}



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



//*****************************//************************************************************************//*************************//
//****************************// INICIO de la sección para las IMÁGENES //*******************************//************************//

        //**************************************************************
        //FUNCIÓN PARA BUSCAR LAS IMAGENES CORRESPONDIENTES AL PRODUCTO.
        //**************************************************************
        function BuscarImagenes(clienteID) {
    console.log("ClienteID recibido:", clienteID); // <-- VERIFICA QUE SE RECIBE EL ID


            $("#ClienteID").val(clienteID);
            $("#ImagenesCliente").empty();

            $.ajax({
                url: '../../Clientes/BuscarImagenes',
                data: { ClienteID: clienteID },
                dataType: 'json',
                success: function (listaImagenCliente) {
                    var imagenes = "";
                    var botonesEliminar = "";
                    $.each(listaImagenCliente, function (index, imagen) {

                                        console.log("Imagen recibida:", imagen); // <-- VERIFICA CADA IMAGEN INDIVIDUAL

                        imagenes += "<img src='data:image/jpeg;base64," + imagen.base64 + "'>";
                    });

                    //CREAMOS EL DIV PRINCIPAL QUE CONTIENE TODAS LAS IMAGENES DEL PRODUCTO.
                    if (imagenes == null || imagenes == "") {
                        var mensaje = $("<p style='font-size:16px; color:red; text-align:center; font-weight:bold;'> No hay imágenes cargadas para este producto.<p>");
                        $("#ImagenesCliente").empty().append(mensaje);
                    } else {
                        $("#ImagenesCliente").append("<div style='margin-bottom:20px;' class='fotorama' data-auto='false' data-width='700' data-ratio='700/467' data-max-width='100%' data-nav='thumbs' data-autoplay='true'>" +

                            imagenes +

                            "</div>");
                    }

                    //ESTO ES PARA INICIAR EL MÉTODO DE PASAR IMAGENES.
                    $('.fotorama').fotorama();

                    // LLAMAMOS A LA FUNCION DE BUSCAR LAS IMAGENES PARA PODER ELIMINARLAS.
                    BuscarImagenesEliminar(clienteID);

                },
                error: function (resultado) {
                    swal({
                        title: "ATENCIÓN",
                        icon: "error",
                        text: "Ocurrio un error al cargar las imagenes.",

                        dangerMode: true,
                        closeOnClickOutside: false,
                        closeOnEsc: false,

                        buttons: {
                            confirm: {
                                text: "Aceptar",
                                value: true,
                                visible: true,
                                className: "botonVerdeSweetAlert",
                                closeModal: true
                            }
                        },
                    });
                }
            });
        }

        //****************************************************************************
        //FUNCIÓN PARA GUARDAR EN LA TABLA LAS IMAGENES SELECCIONADAS PARA EL PRODUCTO 
        //****************************************************************************
        // function GuardarImagen() {
        //     var clienteID = $("#ClienteID").val();
        //     var image = $('#inputImagen').val();

        //     $.ajax({
        //         type: "POST",
        //         url: '../../Clientes/GuardarImagen',
        //         data: { ImagenAGuardar: image, ClienteID: clienteID },
        //         success: function (resultado) {
        //             if (resultado == 1) {
        //                 VaciarCampoImagen();

        //                 BuscarImagenes(clienteID);

        //             }
        //         },
        //         error: function (result) {
        //             swal({
        //                 title: "ATENCIÓN",
        //                 icon: "error",
        //                 text: "Ocurrio un error al Guardar",

        //                 dangerMode: true,
        //                 closeOnClickOutside: false,
        //                 closeOnEsc: false,

        //                 buttons: {
        //                     confirm: {
        //                         text: "Aceptar",
        //                         value: true,
        //                         visible: true,
        //                         className: "botonVerdeSweetAlert",
        //                         closeModal: true
        //                     }
        //                 },
        //             });
        //             // alert("Ocurrio un error al Guardar o llego al limite de 3 Imagenes por Producto, pruebe nuevamente más tarde o Elimine alguna Imagen!");
        //         }
        //     });
        // }

        function GuardarImagen() {
    console.log("Función GuardarImagen iniciada");

    var input = document.getElementById("inputImagen");
    var file = input.files[0];

    if (!file) {
        alert("Seleccioná una imagen antes de guardar.");
        return;
    }

    var reader = new FileReader();

    reader.onload = function (e) {
        var base64Image = e.target.result;
        console.log("Base64 generado:", base64Image);

        var clienteID = $("#ClienteID").val();

        $.ajax({
            type: "POST",
            url: "/Clientes/GuardarImagen",
            data: {
                ImagenAGuardar: base64Image,
                ClienteID: clienteID
            },
            success: function (resultado) {
                console.log("Resultado del servidor:", resultado);
                if (resultado === true) {
                    alert("Imagen guardada correctamente.");
                    BuscarImagenes(clienteID);
                } else {
                    alert("No se pudo guardar la imagen. ¿Ya hay 3?");
                }
            },
            error: function (xhr, status, error) {
                console.error("Error en la petición AJAX:", error);
            }
        });
    };

    reader.readAsDataURL(file);
}


        //******************************************************************************************
        //FUNCIÓN PARA BUSCAR EN LA TABLA LAS IMAGENES CORRESPONDIENTES AL PRODUCTO PARA MOSTRARLAS.
        //******************************************************************************************
        function BuscarImagenesEliminar(clienteID) {
            $("#ImagenesClienteEliminar").empty();

            $.ajax({
                url: '../../Clientes/BuscarImagenes',
                data: { ClienteID: clienteID },
                dataType: 'json',
                success: function (listaImagenCliente) {
                    var imagenes = "";
                    var botonesEliminar = "";
                    $.each(listaImagenCliente, function (index, imagen) {
                        //CREAMOS EL DIV PRINCIPAL QUE CONTIENE TODAS LAS IMAGENES DEL PRODUCTO
                        $("#ImagenesClienteEliminar").append("<tr class='altoBotonesTabla'>" +

                            "<td><img class='imagenClienteEliminar' src='data:image/jpeg;base64," + imagen.base64 + "' style='width: 100 %;'> </td>" +

                            "<td style='width:50%'>" +
                            "<p class='text-center'><a onclick='EliminarImagen(" + imagen.imgClientesID + ");' class='btn-ovaloEliminar' title='Eliminar Imagen'>Eliminar</a></p>" +
                            "</td>" +

                            "</tr>");
                    });
                },
                error: function (resultado) {
                    swal({
                        title: "ATENCIÓN",
                        icon: "error",
                        text: "Ocurrio un error al cargar las imagenes.",

                        dangerMode: true,
                        closeOnClickOutside: false,
                        closeOnEsc: false,

                        buttons: {
                            confirm: {
                                text: "Aceptar",
                                value: true,
                                visible: true,
                                className: "",
                                closeModal: true
                            }
                        },
                    });
                }
            });
        }

        //****************************************************
        //FUNCIÓN PARA ELIMINAR LA IMAGEN ASOCIADA AL PRODUCTO
        //****************************************************
        function EliminarImagen(imgClientesID) {
            var clienteID = $("#ClienteID").val();

            $.ajax({
                url: "../../Clientes/EliminarImagenCliente",
                type: "POST",
                data: { imgClientesID: imgClientesID },
                success: function (resultado) {

                    VaciarCampoImagen();

                    BuscarImagenes(clienteID);

                },
                error: function (resultado) {
                }
            });
        }

        //*********************************************************************************
        //FUNCIÓN PARA VACIAR EL CAMPO DE LA IMAGEN Y PODER CARGAR OTRA SIN CERRAR EL MODAL
        //*********************************************************************************
        function VaciarCampoImagen() {
            $("#volver-cortar").click();
        }

        //********************************************
        //FUNCIÓN PARA CERRAR EL MODAL DE LAS IMAGENES
        //********************************************
        function CerrarCargaImagen() {
            $('#ModalMostrarSubirImagenes').modal('hide');
        }





