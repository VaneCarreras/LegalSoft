
window.onload = ListadoLocalidades();

function ListadoLocalidades(){
 
    $.ajax({
        // la URL para la petición
        url: '../../Localidades/ListadoLocalidades',
        // la información a enviar
        // (también es posible utilizar una cadena de datos)
        data: { },
        // especifica si será una petición POST o GET
        type: 'POST',
        // el tipo de información que se espera de respuesta
        dataType: 'json',
        // código a ejecutar si la petición es satisfactoria;
        // la respuesta es pasada como argumento a la función
        success: function (vistaLocalidad) {

            $("#ModalLocalidades").modal("hide");
            LimpiarModal();
            let contenidoTabla = ``;

            $.each(vistaLocalidad, function (index, localidad) {  
                
                contenidoTabla += `
                <tr>
                        
                        
                        <td>${localidad.localidadNombre}</td>
                        <td>${localidad.provinciaString}</td>

                    <td class="text-center">
                    <button type="button"  onclick="AbrirModalEditar(${localidad.localidadID})" title="Editar" >
<i class="fa-duotone fa-solid fa-angles-right" style="--fa-primary-color: #4969a2; --fa-secondary-color: #4969a2;"></i>                    </button>
                    </td>
                    <td class="text-center">
                    <button type="button"   onclick="EliminarRegistro(${localidad.localidadID})" title="Eliminar">
                    <i class="fa-solid fa-poo" style="color: #820d19;"></i>
                    </button>
                    </td>
                </tr>
             `;

            });

            document.getElementById("tbody-localidades").innerHTML = contenidoTabla;

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
    document.getElementById("LocalidadID").value = 0;
     
    document.getElementById("LocalidadNombre").value = ""; 
        document.getElementById("Provincia").value = "";



}

function NuevoRegistro(){
    $("#ModalTitulo").text("Nueva Localidad");
    LimpiarModal();
}

function AbrirModalEditar(LocalidadID){
    
    $.ajax({
        // la URL para la petición
        url: '../../Localidades/ListadoLocalidades',
        // la información a enviar
        // (también es posible utilizar una cadena de datos)
        data: { id: LocalidadID},
        // especifica si será una petición POST o GET
        type: 'POST',
        // el tipo de información que se espera de respuesta
        dataType: 'json',
        // código a ejecutar si la petición es satisfactoria;
        // la respuesta es pasada como argumento a la función
        success: function (vistaLocalidad) {
            let localidad = vistaLocalidad[0];

            document.getElementById("LocalidadID").value = localidad.localidadID;
            $("#ModalTitulo").text("Editar Localidad");

            
            
            document.getElementById("LocalidadNombre").value = localidad.localidadNombre; 
            document.getElementById("Provincia").value = localidad.provincia;

            $("#ModalLocalidades").modal("show");

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
    let localidadID = document.getElementById("LocalidadID").value;
    

 
    let localidadNombre =        document.getElementById("LocalidadNombre").value; 
    let provincia = document.getElementById("Provincia").value;

const letrasYNumeros = /^[a-zA-Z0-9áéíóúÁÉÍÓÚñÑ\s]+$/;
if (
    localidadNombre.length < 3 ||
    !letrasYNumeros.test(localidadNombre)
) {
    Swal.fire("El nombre de la localidad debe tener al menos 3 caracteres y solo contener letras y números.");
    return;
}

if (provincia === "" || provincia === "0") {
        Swal.fire("Debe seleccionar una provincia.");
        return;
    }


    if (localidadID == 0 || localidadID == "") {
        // Llamar al método de creación si ClienteID es 0 o está vacío
        $.ajax({
            url: '../../Localidades/GuardarNuevaLocalidad', // Método para crear nuevo cliente
            type: 'POST',
            data: {
                
                localidadNombre: localidadNombre,
                provincia: provincia,

            },
            dataType: 'json',
            success: function (resultado) {
                if (resultado != "") {
                    Swal.fire(resultado);
                }
                ListadoLocalidades(); // Refresca la lista de clientes
                $("#ModalLocalidades").modal("hide"); // Cierra el modal
            },
            error: function (xhr, status) {
                console.log('Error al guardar la nueva localidad.');
            }
        });
    } else {
        // Llamar al método de edición si ClienteID es distinto de 0
        $.ajax({
            url: '../../Localidades/EditarLocalidad', // Llamar al nuevo método EditarCliente
            type: 'POST',
            data: {
                localidadID: localidadID,
                
                
                localidadNombre: localidadNombre,
                provincia: provincia,
                
            },

            dataType: 'json',
            success: function (resultado) {
                if (resultado != "") {
                    Swal.fire(resultado);
                }
                ListadoLocalidades(); // Refresca la lista de clientes
                $("#ModalLocalidades").modal("hide"); // Cierra el modal
            },

            error: function (xhr, status) {
                console.log('Error al actualizar la localidad.');
            }
        });
    }
}





function EliminarRegistro(LocalidadID) {
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
                url: '../../Localidades/EliminarLocalidad',
                data: { localidadID: LocalidadID },
                type: 'POST',
                dataType: 'json',
                success: function (resultado) {
                    Swal.fire({
                        title: "¡Eliminado!",
                        icon: "success"
                    });
                    ListadoLocalidades(); // Refresca la tabla después de eliminar
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