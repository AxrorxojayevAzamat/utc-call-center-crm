let region_labels = ["г. Ташкент", "Андижан", "Бухара", "Фергана", "Джизак", "Наманган", "Навои", "Каш-дарья", "Самарканд",
    "Сырдарья", "Сурхан. об.", "Таш. об.", "Хорезм", "Каракалпак."];
let status_labels = ["в процессе", "решено", "отклоненный", "с задержкой"]

let bar_all_data = JSON.parse($("#bar_all_data").val());
let bar_done_data = JSON.parse($("#bar_done_data").val());
let doughnut_data = JSON.parse($("#doughnut_data").val());

let mybar = $("#myBar").get(0).getContext("2d");
let myDoughnut = $("#myDoughnut").get(0).getContext("2d");

$(document).ready(() => {

    new Chart(mybar, {
        type: "bar",
        data: {
            labels: region_labels,
            datasets: [{
                backgroundColor: "#52CDFF",
                label: "общий",
                data: bar_all_data
            },
            {
                backgroundColor: "#34B1AA",
                label: "решено",
                data: bar_done_data
            }]
        },
        options: {
            legend: { display: true },
            title: {
                display: true,
                text: "Количество заявок по регионам в виде графика"
            },
            responsive: true,
            scales: {
                x: {
                    stacked: true,
                    beginAtZero: true
                },
                y: {
                    stacked: true,
                    beginAtZero: true
                }
            }
        }
    })

    new Chart(myDoughnut, {
        type: "doughnut",
        data: {
            labels: status_labels,
            datasets: [{
                backgroundColor: ["#52CDFF", "#34B1AA", "#F95F53","#ffaf00"],
                label: "all",
                data: doughnut_data,
            }]
        },
        options: {
            legend: { display: true },
            title: {
                display: true,
                text: "Общее количество заявок по регионам в виде круговой диаграммы"
            }
        }
    })

})
