let LLenarSelect = async (idSelect, urlRequest, parameter, isSelectMultiple = false) => {
    let HttpRequest = MakeRequestGET(urlRequest, parameter);

    let items = isSelectMultiple ? "" : "<option value=''>Seleccione un valor</option>";
    let tagHtml = $(`#${idSelect}`);
    tagHtml.empty();

    HttpRequest
        .then(res => {
            if (res.status == 200) {
                $.each(JSON.parse(res.body), (index, item) => {
                    items += `<option value = '${item.Value}'>${item.Text}</option>`;
                });
            }
            tagHtml.html(items);
            return;
        }).catch((err) => {
            console.log(`Algo salió mal: ${err}`);
        });
}

let LLenarSelectWithData = async (idSelect, data) => {
    let tagHtml = $(`#${idSelect}`);
    tagHtml.empty();
    let Items = "<option value=''>Seleccione un valor</option>";
    data.each((index, item) => {
        Items += `<option value = '${index}'>${item}</option>`;
    });
    tagHtml.html(Items);
    return;
}

let ObtenerValoresSeleccionadosMultiSelect = () => {
    let objReturn = [];

    $("li[id$='-selection'][class*='ms-selected']").each((index, item) => {
        objReturn.push(item.innerText);
    })

    return objReturn;
}

let SeleccionarTodosLosElementos = (idElement) => { $(`#${idElement}`) };