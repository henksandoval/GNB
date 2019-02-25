$(document).ready(() => {
    documentReadyAsync();
});

let documentReadyAsync = async () => {
    await Promise.all([
        loadDatatable()
    ]);
};

let loadDatatable = async () => {
    let dataTable = new CustomDatatable('Transaction/GetTransactions', 'tableTransactions');
    dataTable.Headers = await headersDataTable();
    dataTable.Columns = await columnsDataTable();
    dataTable.FunctionSearch = functionSearch;
    await dataTable.GenerateDatatable();
};

let headersDataTable = async () => {
    return [
        { "title": "Sku", "targets": 0 },
        { "title": "Amount", "targets": 1 },
        { "title": "Currency", "targets": 2 }
    ];
};

let columnsDataTable = async () => {
    return await [
        { data: "sku", name: "sku" },
        { data: "amount", name: "amount" },
        { data: "currency", name: "currency" },
    ];
};

let functionSearch = (infFilter, infDataTable) => {
    return $.extend({}, infFilter, {
        "CustomSearches": [
            { "Name": "sku", "Value": "" }
        ]
    });
};