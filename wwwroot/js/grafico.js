// Ejecuta la función de inicialización cuando la página carga
window.onload = initGrafico;

// Variables para los gráficos y el tipo seleccionado
let graficoCircular;
let tipoSeleccionado = "consultas";

// Inicializar gráficos y eventos
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

function actualizarGraficoCircular() {
    let mesBuscar = $("#MesBuscar").val();
    let anioBuscar = $("#AnioBuscar").val();

    $.ajax({
        url: '/Graficos/GetDatosEstado',
        method: 'GET',
        data: {
            tipo: tipoSeleccionado,
            mes: mesBuscar,
            anio: anioBuscar
        },
        success: function(respuestaDatos) {
            let labels = [];
            let data = [];
            
            respuestaDatos.forEach(function (estado) {
                labels.push(estado.nombreEstado);
                data.push(estado.cantidad);
            });

            if (graficoCircular) {
                graficoCircular.destroy();
            }

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
        },
        error: function() {
            console.error("Error al cargar datos del gráfico.");
        }
    });
}

function generarColores(numero) {
    const colores = [];
    for (let i = 0; i < numero; i++) {
        colores.push(`rgba(${Math.floor(Math.random() * 255)}, ${Math.floor(Math.random() * 255)}, ${Math.floor(Math.random() * 255)}, 0.5)`);
    }
    return colores;
}

