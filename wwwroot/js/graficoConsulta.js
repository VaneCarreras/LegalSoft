// Ejecuta la función de inicialización cuando la página carga
window.onload = initGrafico;

// Variables para los gráficos y el tipo seleccionado
let graficoCircular;
let tipoSeleccionado = "consultas";

// Función inicial de gráficos
function initGrafico() {
    // Escuchar cambios en el selector de tipo
    $("#TipoGraficoID").change(function () {
        tipoSeleccionado = $(this).val();
        actualizarGraficoCircular();
    });

    // Escuchar cambios en el mes y año
    $("#MesBuscar, #AnioBuscar").change(function () {
        actualizarGraficoCircular();
    });

    // Cargar gráfico inicial
    actualizarGraficoCircular();
}

// Función para actualizar el gráfico circular según selección
function actualizarGraficoCircular() {
    let mesBuscar = $("#MesBuscar").val();
    let anioBuscar = $("#AnioBuscar").val();

    // Generar datos ficticios aleatorios para simular respuesta del servidor
    const respuestaDatos = generarDatosAleatorios(tipoSeleccionado);

    let labels = [];
    let data = [];
    
    $.each(respuestaDatos, function (index, estado) {
        labels.push(estado.nombreEstado);
        data.push(estado.cantidad);
    });

    // Destruir gráfico existente si existe
    if (graficoCircular) {
        graficoCircular.destroy();
    }

    // Crear nuevo gráfico circular
    const ctxPie = document.getElementById("grafico-circular").getContext('2d');
    graficoCircular = new Chart(ctxPie, {
        type: 'pie',
        data: {
            labels: labels,
            datasets: [{
                data: data,
                backgroundColor: generarColores(data.length)
            }]
        },
        options: {
            responsive: true,
            plugins: {
                legend: {
                    position: 'top'
                },
                tooltip: {
                    callbacks: {
                        label: function (tooltipItem) {
                            return tooltipItem.label + ': ' + tooltipItem.raw + ' casos';
                        }
                    }
                }
            }
        }
    });
}

// Generador de colores aleatorios
function generarColores(numero) {
    const colores = [];
    for (let i = 0; i < numero; i++) {
        colores.push(`rgba(${Math.floor(Math.random() * 255)}, ${Math.floor(Math.random() * 255)}, ${Math.floor(Math.random() * 255)}, 0.5)`);
    }
    return colores;
}

// Genera datos aleatorios según el tipo de selección (consultas, expedientes, turnos, pendientes)
function generarDatosAleatorios(tipo) {
    let datos = [];
    
    if (tipo === "consultas") {
        datos = [
            { nombreEstado: "Asesorada", cantidad: Math.floor(Math.random() * 100) },
            { nombreEstado: "Judicializada", cantidad: Math.floor(Math.random() * 100) },
            { nombreEstado: "Desestimada", cantidad: Math.floor(Math.random() * 100) }
        ];
    } else if (tipo === "expedientes") {
        datos = [
            { nombreEstado: "En Curso", cantidad: Math.floor(Math.random() * 100) },
            { nombreEstado: "Con Sentencia", cantidad: Math.floor(Math.random() * 100) }
        ];
    } else if (tipo === "turnos") {
        datos = [
            { nombreEstado: "Asistido", cantidad: Math.floor(Math.random() * 100) },
            { nombreEstado: "Suspendido", cantidad: Math.floor(Math.random() * 100) },
            { nombreEstado: "Vacante", cantidad: Math.floor(Math.random() * 100) }
        ];
    } else if (tipo === "pendientes") {
        datos = [
            { nombreEstado: "Realizado", cantidad: Math.floor(Math.random() * 100) },
            { nombreEstado: "No Realizado", cantidad: Math.floor(Math.random() * 100) }
        ];
    }
    
    return datos;
}
