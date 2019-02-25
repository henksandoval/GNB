$(document).ready(() => {
    documentReadyAsync();
});

let documentReadyAsync = async () => {
    await Promise.all([
        loadDatatable(),
    ]);
};

let loadDatatable = async () => {
    let dataTable = new CustomDatatable('Transaction/GetTransactions', 'tableTransactions');
    dataTable.Columns = await columnsDataTable();
    await dataTable.GenerateDatatable();
};

let condicionesFiltrado = (infFilter, infDataTable) => {
    let parametrosTabla = getSearchParameters();

    return $.extend({}, infFilter, {
        "CustomSearches": [
            { "Name": "TX_NumeroIdentificacion", "Value": parametrosTabla.TX_NumeroIdentificacion },
            { "Name": "TX_NombreReal", "Value": parametrosTabla.TX_NombreReal },
            { "Name": "Email", "Value": parametrosTabla.TX_DireccionCorreo },
            { "Name": "UserName", "Value": parametrosTabla.TX_NombreUsuario },
        ]
    });
};

let columnsDataTable = async () => {
    return await [
        { data: "sku", name: "sku" },
        { data: "amount", name: "amount" },
        { data: "currency", name: "currency" },
        {
            data: "TX_Accion",
            render: (data, type, row) => `<a class='btn btn-info btn-sm text-center text-white' data-dataTable><i class='fa fa-eye'></i> Ver detalle</a>`,
            className: 'dt-body-center'
        },
    ];
};

//$('#btnClear').click(function () {
//    $('.input-filter').val("");
//});

//$('#btnSearch').click(function () {
//    generateDataTable(personalizarDataTable());
//});

//let personalizarDataTable = () => {
//    return {
//        idElement: 'tableTransactions',
//        URL: 'transaction',
//        columns: columnsDataTable(),
//        ajaxData: condicionesFiltrado,
//        orderMulti: true,
//        order: true,
//    };
//}

//let columnsDataTable = () => {
//    return [
//        { data: "Email", name: "Email" },
//        { data: "UserName", name: "UserName" },
//        { data: "TX_NombreCompleto", name: "TX_NombreCompleto" },
//        { data: "TX_NumeroDocumento", name: "TX_NumeroDocumento" },
//        { data: "Lockout", name: "Lockout" },
//        {
//            data: "TX_Accion",
//            render: (data, type, row) => `<a class='btn btn-info btn-sm text-center text-white' data-dataTable><i class='fa fa-eye'></i> Ver detalle</a>`,
//            className: 'dt-body-center'
//        },
//    ];
//}

//let parametrosBusqueda = () => {
//    let obj = {};
//    obj = {
//        ID_Tabla: "tableTransactions",
//        TX_NumeroIdentificacion: IsNullOrEmpty($('#TX_NumeroIdentificacion').val()) ? '' : $('#TX_NumeroIdentificacion').val(),
//        TX_NombreReal: IsNullOrEmpty($('#TX_NombreReal').val()) ? '' : $('#TX_NombreReal').val(),
//        TX_DireccionCorreo: IsNullOrEmpty($('#TX_DireccionCorreo').val()) ? '' : $('#TX_DireccionCorreo').val(),
//        TX_NombreUsuario: IsNullOrEmpty($('#TX_NombreUsuario').val()) ? '' : $('#TX_NombreUsuario').val(),
//    };

//    return obj;
//};

//let condicionesFiltrado = (infFilter, infDataTable) => {
//    let parametrosTabla = parametrosBusqueda();

//    return $.extend({}, infFilter, {
//        "CustomSearches": [
//            { "Name": "TX_NumeroIdentificacion", "Value": parametrosTabla.TX_NumeroIdentificacion },
//            { "Name": "TX_NombreReal", "Value": parametrosTabla.TX_NombreReal },
//            { "Name": "Email", "Value": parametrosTabla.TX_DireccionCorreo },
//            { "Name": "UserName", "Value": parametrosTabla.TX_NombreUsuario },
//        ]
//    });
//}

//$('#tableTransactions').on('click', 'a[data-dataTable]', (element) => {
//    const data_row = $(`#${element.delegateTarget.id}`).DataTable().row($(element.target).closest('tr')).data();
//    redirectApplication(`/Usuario/Editar/${data_row.Id}`);
//});