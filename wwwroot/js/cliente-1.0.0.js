
window.onload = ListadoClientes();

function ListadoClientes(){
 
    $.ajax({
        // la URL para la petición
        url: '../../Clientes/ListadoClientes',
        // la información a enviar
        // (también es posible utilizar una cadena de datos)
        data: {  },
        // especifica si será una petición POST o GET
        type: 'POST',
        // el tipo de información que se espera de respuesta
        dataType: 'json',
        // código a ejecutar si la petición es satisfactoria;
        // la respuesta es pasada como argumento a la función
        success: function (clientes) {

            $("#ModalClientes").modal("hide");
            LimpiarModal();
            let contenidoTabla = ``;

            $.each(clientes, function (index, cliente) {  
                
                contenidoTabla += `
                <tr>
                    <button type="button" class="btn btn-success btn-sm" onclick="AbrirModalEditar(${cliente.clienteID})">
                    <i class="fa-solid fa-marker"></i>
                    </button>
                    </td>
                    <td class="text-center">
                    <button type="button" class="btn btn-danger btn-sm" onclick="EliminarRegistro(${cliente.clienteID})">
                    <i class="fa-solid fa-trash"></i>
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
}

function NuevoRegistro(){
    $("#ModalTitulo").text("Nuevo Cliente");
}

function AbrirModalEditar(clienteID){
    
    $.ajax({
        // la URL para la petición
        url: '../../Clientes/ListadoClientes',
        // la información a enviar
        // (también es posible utilizar una cadena de datos)
        data: { id: clienteID},
        // especifica si será una petición POST o GET
        type: 'POST',
        // el tipo de información que se espera de respuesta
        dataType: 'json',
        // código a ejecutar si la petición es satisfactoria;
        // la respuesta es pasada como argumento a la función
        success: function (clientes) {
            let cliente = cliente[0];

            document.getElementById("ClienteID").value = clienteID;
            $("#ModalTitulo").text("Editar Cliente");
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
    //POR UN LADO PROGRAMAR VERIFICACIONES DE DATOS EN EL FRONT CUANDO SON DE INGRESO DE VALORES Y NO SE NECESITA VERIFICAR EN BASES DE DATOS
    //LUEGO POR OTRO LADO HACER VERIFICACIONES DE DATOS EN EL BACK, SI EXISTE EL ELEMENTO SI NECESITAMOS LA BASE DE DATOS.
    $.ajax({
        // la URL para la petición
        url: '../../Clientes/ListadoClientes',
        // la información a enviar
        // (también es posible utilizar una cadena de datos)
        data: {nroTipoDoc: nroTipoDoc, nombreCompleto: nombreCompleto, direccion: direccion, telefono: telefono, fechaNac: fechaNac},
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

function EliminarRegistro(clienteID){
    $.ajax({
        // la URL para la petición
        url: '../../Clientes/ListadoClientes',
        // la información a enviar
        // (también es posible utilizar una cadena de datos)
        data: { clienteID: clienteID},
        // especifica si será una petición POST o GET
        type: 'POST',
        // el tipo de información que se espera de respuesta
        dataType: 'json',
        // código a ejecutar si la petición es satisfactoria;
        // la respuesta es pasada como argumento a la función
        success: function (resultado) {        
            ListadoClientes();
        },

        // código a ejecutar si la petición falla;
        // son pasados como argumentos a la función
        // el objeto de la petición en crudo y código de estatus de la petición
        error: function (xhr, status) {
            console.log('Disculpe, existió un problema al eliminar el registro.');
        }
    });    

}