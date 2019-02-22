const languageDataTable = {
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

let CreateDataTable = (idTagHtml) => {
    const tagHtml = $(`#${idTagHtml}`);

    let customTags = tagHtml.data('options');
    const urlRequest = `${customTags.Controller}/${customTags.MethodSelectAll.Method}`;

    let obj;
    if ('Parameter' in customTags.MethodSelectAll) {
        obj = customTags.MethodSelectAll.Parameter;
    } else {
        obj = {};
    }

    let HttpRequest = MakeRequest(urlRequest, obj);

    let modalId = customTags.ModalId;

    HttpRequest
        .then(res => {
            const dataSet = $.parseJSON(res.body);
            const columns = GetColumns(dataSet.listColumnsDataTable);
            const dataBody = GetDataBody(dataSet.listData);
            tagHtml.DataTable({
                language: {
                    'url': '//cdn.datatables.net/plug-ins/1.10.11/i18n/Spanish.json'
                },
                data: dataBody,
                columns: columns[0],
                columnDefs: columns[1],
                responsive: true,
                dom: '<lf<t>ip>',
                pageLength: 50,
                buttons: [{
                    extend: 'excel',
                    title: 'titleDataTable',
                    exportOptions: {
                        orthogonal: 'sort',
                        columns: 'columnsToPrinter'
                    }
                }, {
                    extend: 'pdf',
                    title: 'titleDataTable',
                    orientation: 'orientationSheetPDF',
                    exportOptions: {
                        orthogonal: 'sort',
                        columns: 'columnsToPrinter'
                    }
                }]
            });

            if (customTags.MethodTryEdit.Type === 'Redirect') {
                Redirect(idTagHtml, modalId);
            } else {
                FillModal(idTagHtml);
            }
            DeleteRecord(idTagHtml);
            ChangeStatusRecord(idTagHtml);

        }).catch((err) => {
            console.log(`Algo salió mal: ${err}`);
        });
};

let GetColumns = (listColumns) => {
    let columns = [];
    let columnsDefs = [];
    $.each(listColumns, function (index, item) {
        let obj = {};
        obj.data = item['tx_ColumnName'];
        obj.title = item['tx_ColumnTitle'];
        columns.push(obj);
        if (item['bo_Hidden'] == true) {
            let obj = {};
            obj.targets = index;
            obj.visible = false;
            obj.searchable = false;
            columnsDefs.push(obj);
        }
    });
    return [columns, columnsDefs];
};

let GetDataBody = (dataBody) => {
    $.each(dataBody, (index, item) => {
        item = fnGetDataRow(item);
    });
    return dataBody;
};

let fnGetDataRow = (item) => {
    let tagHtml = `
        <div class="btn-group">
            <button type="button" class="btn btn-info"><i Class='fa fa-cogs'></i> Acciones</button>
            <button type="button" class="btn btn-info dropdown-toggle dropdown-toggle-split" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                <span class="sr-only">Toggle Dropdown</span>
            </button>
            <div class="dropdown-menu dropdown-menu-background animated lightSpeedIn">
			    ${item.action.tx_Edit}
			    ${item.action.tx_Delete}
                ${item.action.tx_ChangeStatus}
            </div>
        </div>`;

    item.action = tagHtml;
    return item;
};

let Redirect = (idTable, modalId) => {
    let table = $(`#${idTable}`).DataTable();

    $(`#${idTable} tbody`).on('click', '#editRecord', function () {
        const tagHtml = $(`#${idTable}`);

        let customTags = tagHtml.data('options');
        let urlRequest = `${customTags.Controller}/${customTags.MethodTryEdit.Method}`;

        let row = table.row($(this).parents('tr')[0]).data();

        const params = {
            id: row[customTags.MethodTryEdit.Parameter]
        };

        urlRequest = new URL(`${window.location.protocol}//${window.location.host}/${urlRequest}`);
        Object.keys(params).forEach(key => urlRequest.searchParams.append(key, params[key]));

        window.location.href = urlRequest.href;
    });
};

let FindRecord = (table, row) => {
};

let ChangeStatusRecord = (idTagHtml) => {
    let table = $(`#${idTagHtml}`).DataTable();
    $(`#${idTagHtml} tbody`).on('click', '#changeStatusRecord', function () {
        let row = table.row($(this).parents('tr')[0]).data();
        ConfirmChangeStatusRecord(row, idTagHtml);
    });
};

async function ConfirmChangeStatusRecord(row, idTagHtml) {
    const tagHtml = $(`#${idTagHtml}`);

    let customTags = tagHtml.data('options');
    const urlRequest = `${customTags.Controller}/${customTags.MethodChangeStatus.Method}`;
    const id = GetIdInRow(row);
    const status = row.bo_Enabled;
    let response = await ConfirmChangeStatusReturnData(urlRequest, { id: id, status: status });

    EditRow(idTagHtml, response.body);
}

let DeleteRecord = (idTagHtml) => {
    let table = $(`#${idTagHtml}`).DataTable();
    $(`#${idTagHtml} tbody`).on('click', '#deleteRecord', function () {
        let rowDataTble = table.row($(this).parents('tr')[0]).data();
        ConfirmDeleteRecord(rowDataTble, idTagHtml);
    });
};

async function ConfirmDeleteRecord(row, idTagHtml) {
    const tagHtml = $(`#${idTagHtml}`);

    let customTags = tagHtml.data('options');
    const urlRequest = `${customTags.Controller}/${customTags.MethodTryDelete.Method}`;
    const id = GetIdInRow(row);
    let response = await ConfirmDeleteActionReturnData(urlRequest, id);

    EditRow(idTagHtml, response.body);
}

async function InsertNewRow(idTable, urlRequest, obj) {
    let response = await ConfirmInsertActionReturnData(urlRequest, obj);

    const tagHtml = $(`#${idTable}`);
    AddNewRow(idTable, response.body);
}

let OpenModalDataTable = (idForm, tittleModal, clear = false) => {
    if (clear) FormClear(idForm);
    $('#ModalTitle').html(tittleModal);
    $(`#${idForm}`).modal();
    $('.modal').insertBefore($('#contentApplication'));
};

let FillModal = (idTable) => {
    let table = $(`#${idTable}`).DataTable();

    $(`#${idTable} tbody`).on('click', '#editRecord', function () {
        const tagHtml = $(`#${idTable}`);

        let customTags = tagHtml.data('options');
        const urlRequest = `${customTags.Controller}/${customTags.MethodTryEdit.Method}`;

        let row = table.row($(this).parents('tr')[0]).data();

        const params = {
            id: row[customTags.MethodTryEdit.Parameter]
        };

        let HttpRequest = MakeRequestGET(urlRequest, params);

        HttpRequest
            .then(res => {
                OpenModalDataTable(customTags.MethodTryEdit.IdModal, 'Agregar nueva información', true);
                $('.modal-body').empty();
                $('.modal-body').append(res.body);
            }).catch((err) => {
                console.log(`Algo salió mal: ${err}`);
            });
    });
};

let AddNewRow = (idTable, obj) => {
    let table = $(`#${idTable}`).DataTable();

    const dataSet = $.parseJSON(obj);
    const columns = GetColumns(dataSet.listColumnsDataTable);
    const dataBody = fnGetDataRow(dataSet.dataOnly);

    table.row.add(dataBody).draw(false);
};

let GetIdInRow = (rowDataTable) => {
    for (key in rowDataTable) {
        if (key.includes('id_')) {
            id = rowDataTable[key];
            return id;
        }
    }
};

function EditRow(idTable, obj) {
    let table = $(`#${idTable}`).DataTable();

    const dataSet = $.parseJSON(obj);
}

let generateDataTable = (overload) => {
    let table = $(`#${overload.idElement}`);

    if ($.fn.DataTable.isDataTable(`#${overload.idElement}`)) {
        $(`#${overload.idElement}`).DataTable().destroy();
    }

    table.DataTable({
        searching: overload.hasOwnProperty('searching') ? overload.searching : false,
        processing: true,
        serverSide: overload.hasOwnProperty('serverSide') ? overload.serverSide : true,
        orderMulti: overload.hasOwnProperty('orderMulti') ? overload.orderMulti : false,
        order: overload.hasOwnProperty('order') ? overload.order : false,
        ajax: {
            url: overload.URL,
            type: "POST",
            dataType: "json",
            data: (infFilter, infDataTable) => {
                if (overload.hasOwnProperty('ajaxData')) {
                    return overload.ajaxData(infFilter, infDataTable);
                }
            },
        },
        language: languageDataTable,
        columns: overload.columns,
        pageLength: 10,
        initComplete: function (settings, json) {
            totalizarColumnas(this);
        }
    });

    cambiarEstilosProccessing(overload.idElement);
};

let totalizarColumnas = (tabla) => {
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

let obtenerRegistrosSeleccionadosDataTable = (idDataTable) => {
    let table = document.getElementById(idDataTable);
    return table.querySelectorAll("input:checked");
};

let agregarRowDataTable = (idTabla, row) => {
    $(`#${idTabla}`).DataTable().row.add(row).draw(false);
};

let cambiarEstilosProccessing = (idTabla) => {
    let element = $(`#${idTabla}_processing`);
    let styles = {
        position: "sticky",
        opacity: 0.8,
    };
    element.css(styles);
};