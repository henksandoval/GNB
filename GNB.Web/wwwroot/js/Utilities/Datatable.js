function DataTable(urlRequest, idElement, columnsDataTable, filteredConditions) {
    const LanguageDataTable = {
        "sProcessing": `
        <div class="row">
            <div style="display:block" class="col-12 text-center">
                <div style="color: #64d6e2" class="la-ball-clip-rotate-multiple la-2x">
                    <div></div>
                    <div></div>
                </div>
                <p>Estamos procesando la información...</p>
            </div>
        </div>`,
        "sLengthMenu": "Mostrar _MENU_ registros",
        "sZeroRecords": "No se encontraron resultados",
        "sEmptyTable": "Ningún dato disponible en esta tabla",
        "sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
        "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
        "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
        "sInfoPostFix": "",
        "sSearch": "Buscar:",
        "sUrl": "",
        "sInfoThousands": ",",
        "sLoadingRecords": "Cargando...",
        "oPaginate": {
            "sFirst": "Primero",
            "sLast": "Último",
            "sNext": "Siguiente",
            "sPrevious": "Anterior"
        },
        "oAria": {
            "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
            "sSortDescending": ": Activar para ordenar la columna de manera descendente"
        }
    };
    this.UrlRequest = urlRequest;
    this.IdTagHtml = idElement;
    this.Columns = columnsDataTable;
    this.FilteredConditions = filteredConditions;
    this.ElementTable;
    this.Searching = true;
    this.Processing = true;
    this.ServerSide = true;
    this.OrderMulti = false;
    this.Ordering = false;
    this.Order = [];
    this.TypeRequest = "POST";
    this.DataType = "json";
    this.PageLength = 10;


    this.GenerateDatatable = async () => {
        this.ElementTable = $(`#${this.IdTagHtml}`);

        if ($.fn.DataTable.isDataTable(`#${overload.idElement}`)) {
            $(`#${overload.idElement}`).DataTable().destroy();
        }

        this.ElementTable.DataTable({
            searching: this.Searching,
            processing: true,
            serverSide: this.ServerSide,
            orderMulti: this.OrderMulti,
            ordering: this.Ordering,
            order: this.Order,
            ajax: {
                url: this.UrlRequest,
                type: "POST",
                dataType: "json",
                data: (infFilter, infDataTable) => {
                    if (overload.hasOwnProperty('ajaxData')) {
                        return overload.ajaxData(infFilter, infDataTable);
                    }
                },
            },
            language: LanguageDataTable,
            columns: this.Columns,
            pageLength: this.PageLength,
            initComplete: function (settings, json) {
                totalizarColumnas(this);
            }
        });

        this.AlterStylesProcessing(overload.idElement);
    };

    this.TotalizeColumns = (tabla) => {
        tabla.api().columns('.sum').every(function () {
            let column = this;

            var sum = column
                .data()
                .reduce(function (a, b) {
                    a = parseInt(a, 10);
                    if (isNaN(a)) { a = 0; }

                    b = parseInt(b, 10);
                    if (isNaN(b)) { b = 0; }

                    return a + b;
                });

            $(column.footer()).html('Total: ' + sum);
        });
    };

    this.AlterStylesProcessing = () => {
        let element = $(`#${this.IdTagHtml}_processing`);
        let styles = {
            position: "sticky",
            opacity: 0.8,
        };
        element.css(styles);
    };
}