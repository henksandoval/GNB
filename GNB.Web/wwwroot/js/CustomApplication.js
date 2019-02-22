let ApplyDatePickerFormatDateTime = () => {
    $('.custom-dateTime').bootstrapMaterialDatePicker({
        format: 'YYYY-MM-DD',
        lang: 'es',
        weekStart: 0,
        weekEnd:0,
        time: true
    });

    $('.custom-date').bootstrapMaterialDatePicker({
        format: 'YYYY-MM-DD',
        lang: 'es',
        weekStart: 0,
        weekEnd: 0,
        time: false
    });

    $('.only-date').bootstrapMaterialDatePicker({ time: false });

    $('.only-time').bootstrapMaterialDatePicker({ date: false, format: 'LT', shortTime: true });
};
function MakeRequest(UrlAction, Data) {
  
    return new Promise((resolve, reject) => {
        UrlAction = `${window.location.protocol}//${window.location.host}${GROUP_APPLICATION}/${UrlAction}`;

        const TOKEN = 'PENDIENTE POR IMPLEMENTAR'; // document.getElementsByName('__RequestVerificationToken')['0'].value;

        const headers = new Headers({
            'Authorization': `bearer ${TOKEN}`,
            'Content-Type': 'application/json; charset=utf-8'
        });

        fetch(UrlAction, {
            mode: 'cors',
            method: 'post',
            withCredentials: true,
            crossdomain: true,
            headers: headers,
            body: JSON.stringify(Data)
        })
            .then(response => response.text().then(data => ({ status: response.status, body: data })))
            .then((data) => resolve(data))
            .catch(err => reject(Error(err.message)));
    });
}

function MakeRequestGET(UrlAction, params) {
    return new Promise(function (resolve, reject) {
        UrlAction = new URL(`${window.location.protocol}//${window.location.host}${GROUP_APPLICATION}/${UrlAction}`);
        Object.keys(params).forEach(key => UrlAction.searchParams.append(key, params[key]));

        fetch(UrlAction, {
            mode: 'cors',
            method: 'get',
            withCredentials: true,
            crossdomain: true,
        })
            .then(response => response.text().then(data => ({ status: response.status, body: data })))
            .then((data) => resolve(data))
            .catch(err => reject(Error(err.message)));
    });
}

let redirectApplication = (URLAction) => {
    window.location.href = GROUP_APPLICATION + URLAction;
}

function DeleteData(UrlAction, items) {
    let id;

    for (key in items) {
        if (key.includes('id_')) {
            id = items[key];
            break;
        }
    }

    return new Promise(function (resolve, reject) {

        fetch(`${UrlAction}?id=${id}`, {
            method: 'delete'
        })
            .then((resolved) => resolved.text())
            .then((data) => {
                resolve(data);
            })
            .catch(err => {
                reject(Error(err.message));
            });
    });
}

function IsNullOrEmpty(object) {
    if (typeof object === 'undefined') return true;
    if (typeof object === 'object') return true;
    if (object === null) return true;
    if (typeof object === '') return true;
    if (typeof object === '') return true;
    if (object === '') return true;
    if (object === '') return true;

    return false;
}

let getFieldInForm = (idForm) => $(`#${idForm} input, #${idForm} select, #${idForm} textarea `).not(".select2-search__field, .select2-multiple, .select2");

let getTextElementSelected = (idTagHtml) => $(`#${idTagHtml}>option:selected`).html();

function GetFormData(idForm, getSelect2Multiple = true) {
    let jsonData = {};
    const elementsForm = $(`#${idForm} input, #${idForm} select, #${idForm} textarea `).not(".select2-search__field, .select2-multiple, .select2");
    elementsForm.each((index, item) => {
        let value;
        if (!IsNullOrEmpty(item.id)) {
            if (item.type === 'checkbox') value = `${item.checked}`;
            else {
                if (item.id.includes('Id_')) {
                    value = IsNullOrEmpty(item.value) ? 0 : item.value;
                } else if (item.hasAttribute('data-dateTimeFormat')) {
                    let [_date, _month, _year] = item.value.split('-');
                    value = new Date(Date.UTC(_year, _month - 1, _date));
                } else if (item.hasAttribute('data-timeFormat')) {
                    let objTiempo = moment(item.value, "h:mm").toDate();
                    value = `${objTiempo.getHours()}:${objTiempo.getMinutes()}`
                } else {
                    value = IsNullOrEmpty(item.value) ? null : item.value;
                }
            }

            if (item.hasAttribute('data-group')) {
                let json = {};
                let [key, name] = item.getAttribute('data-group').split('.');
                json[name] = value;
                if (jsonData.hasOwnProperty(key)) {
                    Object.assign(jsonData[key],json);
                } else {
                    jsonData[key] = json;
                }
            } else {
                jsonData[item['name']] = value;
            }
        }
    });

    if (getSelect2Multiple) {
        const elementsByContainer = GetElementsClassMultiple(idForm);
        Object.assign(jsonData, elementsByContainer);
    }

    return jsonData;
}

function GetElementsClassMultiple(idForm) {
    const dataMultiple = $(`#${idForm} .select2-multiple`);
    return GroupElementsMultiple(dataMultiple);
}

function GroupElementsMultiple(dataMultiple) {
    let obj = [];
    $.each(dataMultiple, (index, item) => {
        let data = [];
        const tagHtml = $(`#${item.id}`);

        _.forEach(tagHtml.val(), (value) => {
            let dict = {};
            let key = tagHtml[0].dataset.idclass;
            dict[key] = value;
            data.push(dict);
        });
        obj[item.id] = data;
    });
    return obj;
}

let GetTypeData = (item, nameCustomData) => $(`#${item.id}`).data(nameCustomData);

function GetDictionaryByItem(item) {
    let dict = {};
    let key = item.name;
    let value = $(`#${item.id}`).val();

    dict[key] = value;
    return dict;
}

function AnalizeDataForm(idForm) {
    let dataInContainer = $(`#${idForm}`).find('[data-container-app]').serializeArray();
    let allDataInForm = $(`#${idForm}`).serializeArray();

    let keysDataContainer = dataInContainer.map(a => a.name);

    return allDataInForm.filter(item => !(keysDataContainer.includes(item.name)));
}

let formClear = (idForm) => $(`#${idForm}`)[0].reset();

let GetElementByIdES6 = (idElement) => $(`#${idElement}`);

let GetElementByNameES6 = (nameElement) => $(`[name=${nameElement}]`);

let IsTrue = (value) => {
    if (typeof value === 'string') {
        value = value.trim().toLowerCase();
    }
    switch (value) {
        case true:
        case 'true':
        case 1:
        case '1':
        case 'on':
        case 'yes':
            return true;
        default:
            return false;
    }
};

let InvisibleElement = async (idElement) => $(`#${idElement}`).addClass('invisible');

let VisibleElement = async (idElement) => $(`#${idElement}`).removeClass('invisible');

let clearElement = async (id) => GetElementByIdES6(id).val("");

const levantarToolTips = () => {
    $('[data-toggle="tooltip"]').tooltip();
};

let ToggleHideShowElement = (idElement) => {
    let element = document.getElementById(idElement);
    if (element.style.display === 'none') {
        element.style.display = 'block';
    } else {
        element.style.display = 'none';
    }
};

let GetIconsFontAwesome = () => {
    const UrlAction = 'https://fontawesome.com/cheatsheet';

    fetch(UrlAction, {
        mode: 'no-cors',
        method: 'get',
        withCredentials: true,
        crossdomain: true,
    })
        .then(response => {
            response.text().then(
                data => ({ status: response.status, body: data }))
        })
        .catch(err => reject(Error(err.message)));
};

let fnCancelTimeChangesUTC = (date) => new Date(date.setHours(date.getHours() - date.getTimezoneOffset() / 60));

function PopulateForm(form, data, prefixField = '') {
    $.each(data, function (key, value) {
        if (!IsNullOrEmpty(value)) {
            let idAttributeHtml = `${prefixField}${CapitalizeFirstLetter(key)}`;

            let ctrl = $(`#${idAttributeHtml}`, form);
            if (ctrl.length > 0) {
                switch (ctrl.prop("tagName").toLocaleLowerCase()) {
                    case "input":
                        switch (ctrl.prop("type")) {
                            case "checkbox":
                                ctrl.checked = value;
                                break;
                            default:
                                ctrl.val(value);
                        }
                        break;
                    case "checkbox":
                        break;
                    case "select":
                        switch (ctrl.prop("type")) {
                            case "select-one":
                                Select2_SetValue(idAttributeHtml, value);
                                break;
                        }
                        break;
                    default:
                        ctrl.val(value);
                }
            }
        }
    });
};

let CapitalizeFirstLetter = (string) => string.charAt(0).toUpperCase() + string.slice(1);

let diferenciaDias = (fechaInicio, fechaFin) => {
    const DIFERENCIA_MILISEGUNDOS = (new Date(fechaFin) - new Date(fechaInicio));
    const MILISEGUNDOS_A_SEGUNDOS = 1000;
    const SEGUNDOS_A_MINUTOS = 60;
    const MINUTOS_A_HORAS = 60;
    const HORAS_A_DIAS = 24;

    return Math.round(DIFERENCIA_MILISEGUNDOS / (MILISEGUNDOS_A_SEGUNDOS * SEGUNDOS_A_MINUTOS * MINUTOS_A_HORAS * HORAS_A_DIAS));
};

let addDays = (fecha, cantidadDias) => new Date(fecha.getFullYear(), fecha.getMonth(), fecha.getDate() + cantidadDias);

let isBetweenDate = (date, from, to) => (date >= from && date <= to);

function getDateFormat_YYYYMMDD(date = null) {
    const twoDigit = (n) => (n < 10 ? '0' : '') + n;
    let now;
    if (IsNullOrEmpty(date)) {
        now = new Date();
    } else {
        now = date;
    }
    return `${now.getFullYear()}/${twoDigit(now.getMonth() + 1)}/${twoDigit(now.getDate())}`;
};

let cleanFormInModalToHide = () => {
    $('.modal').on('hidden.bs.modal', (element) => {
        const ID_FORM = element.currentTarget.querySelector("form").id;
        formClear(ID_FORM);
    });
};

let desplegarSwitchery = () => {
    $('.js-switch').each(function () {
        new Switchery($(this)[0], $(this).data());
    });
};

let blockElementsForm = (idForm) => $(`#${idForm} input,textarea,select`).prop("disabled", true);

let unBlockElementsForm = (idForm) => $(`#${idForm} input,textarea,select`).prop("disabled", false);