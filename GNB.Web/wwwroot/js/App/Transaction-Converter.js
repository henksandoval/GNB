$('#btnSearch').click(function () {
    loadDatatable();
});

let loadDatatable = async () => {
    let dataTable = new CustomDatatable('GetTransactionsNewCurrency', 'tableTransactions');
    dataTable.Headers = await headersDataTable();
    dataTable.Columns = await columnsDataTable();
    dataTable.FunctionSearch = functionSearch;
    await dataTable.GenerateDatatable();
};

let headersDataTable = async () => {
    return [
        { "title": "Sku", "targets": 0 },
        { "title": "Amount", "targets": 1 },
        { "title": "Currency", "targets": 2 },
        { "title": "CurrencyConverter", "targets": 3 },
        { "title": "AmountConverter", "targets": 4 }
    ];
};

let columnsDataTable = async () => {
    return await [
        { data: "sku", name: "sku" },
        { data: "amount", name: "amount" },
        { data: "currency", name: "currency" },
        { data: "currencyConverter", name: "currencyConverter" },
        { data: "amountConverter", name: "amountConverter" },
    ];
};

let functionSearch = (infFilter, infDataTable) => {
    return $.extend({}, infFilter, {
        "CustomSearches": [
            { "Name": "sku", "Value": $('#skuCode').val() }
        ]
    });
};