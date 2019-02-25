$(document).ready(() => {
    documentReadyAsync();
});

let documentReadyAsync = async () => {
    await Promise.all([
        loadDatatable()
    ]);
};

let loadDatatable = async () => {
    let dataTable = new CustomDatatable('Rate/GetRates', 'tableRates');
    dataTable.Headers = await headersDataTable();
    dataTable.Columns = await columnsDataTable();
    await dataTable.GenerateDatatable();
};

let headersDataTable = async () => {
    return [
        { "title": "From", "targets": 0 },
        { "title": "To", "targets": 1 },
        { "title": "Rate", "targets": 2 }
    ];
};

let columnsDataTable = async () => {
    return await [
        { data: "from", name: "from" },
        { data: "to", name: "to" },
        { data: "rate", name: "rate" }
    ];
};