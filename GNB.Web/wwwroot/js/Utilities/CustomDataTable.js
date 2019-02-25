function CustomDatatable(urlRequest, idElement, functionSearch) {
    const LanguageDataTable = {
        "sProcessing": `
        <div class="row">
            <div style="display:block" class="col-12 text-center">
                <div style="color: #64d6e2" class="la-ball-clip-rotate-multiple la-2x">
                    <div></div>
                    <div></div>
                </div>
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
    this.Columns;
    this.Headers;
    this.FunctionSearch = functionSearch;
    this.ElementTable;
    this.Searching = false;
    this.Processing = true;
    this.ServerSide = true;
    this.OrderMulti = false;
    this.Ordering = false;
    this.Order = [];
    this.TypeRequest = "POST";
    this.DataType = "json";
    this.PageLength = 10;

    this.GenerateDatatable = async () => {
        await this.BuildDatatable();
        await this.RemoveProcessing();
    };

    this.BuildDatatable = async () => {
        this.ElementTable = $(`#${this.IdTagHtml}`);

        if ($.fn.DataTable.isDataTable(`#${this.IdTagHtmlt}`)) {
            $(`#${this.IdTagHtml}`).DataTable().destroy();
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
                    if (this.FunctionSearch) {
                        data = this.FunctionSearch(infFilter, infDataTable);
                        return data;
                    }
                }
            },
            language: LanguageDataTable,
            columns: this.Columns,
            columnDefs: this.Headers,
            pageLength: this.PageLength,
        });

        await this.AlterStylesProcessing();
    };

    this.AlterStylesProcessing = () => {
        let element = $(`#${this.IdTagHtml}_processing`);
        let styles = {
            'position': "absolute",
            'background-color': 'transparent'
        };
        element.css(styles);
    };

    this.RemoveProcessing = async () => {
        $(`#${this.IdTagHtml}_processing`).remove;
    };
}