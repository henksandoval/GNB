let ConfirmInsertActionReturnData = (urlRequest, obj) => {
   
    return new Promise((resolve, rejected) => {
        swal({
            title: 'Ejecutar cambios',
            text: '¿Está seguro de editar este registro?',
            type: 'question',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Sí, ejecutar!'
        }).then((result) => {
            if (result.value) {

                let HttpRequest = MakeRequest(urlRequest, obj);

                HttpRequest
                    .then(res => {
                        if ((res.status === 201 || res.status === 200)) {
                            AlertSuccess();
                            return resolve(res);
                        } else if (res.status === 404) {
                            AlertNotFound();
                            return resolve(res)
                        } else {
                            AlertError();
                            return resolve(res)
                        }
                    }).catch(err => {
                        AlertDisconnected();
                        return rejected(err);
                    });

            } else {
                AlertCanceled();
                return rejected('Acción cancelada');
            }
        });
    });
};

let ConfirmChangeStatusReturnData = (urlRequest, obj) => {
    return new Promise((resolve, rejected) => {
        const action = IsTrue(obj.status) ? "desactivar" : "activar";
        swal({
            title: 'Cambiar estatus',
            text: `¿Está seguro de querer ${action} este registro?`,
            type: 'question',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: `!Sí, ${action}!`
        }).then((result) => {
            if (result.value) {
                const params = {
                    id: obj.id
                };

                let UrlAction = new URL(`${window.location.protocol}//${window.location.host}/${urlRequest}`);
                Object.keys(params).forEach(key => UrlAction.searchParams.append(key, params[key]));

                fetch(UrlAction, {
                    mode: 'cors',
                    method: 'put',
                    withCredentials: true,
                    crossdomain: true,
                })
                    .then(response => response.text().then(data => ({ status: response.status, body: data })))
                    .then((res) => {
                        if ((res.status === 200)) {
                            AlertSuccessChangeStatus(obj.status);
                            return resolve(res);
                        } else {
                            AlertError();
                            return resolve(res)
                        }
                    })
                    .catch(err => {
                        reject(Error(err.message));
                        AlertDisconnected();
                        return rejected(err);
                    });
            } else {
                AlertCanceled();
                return rejected('Acción cancelada');
            }
        });
    });
};

let ConfirmChangeStatusReturnDataPost = (urlRequest, obj) => {
    return new Promise((resolve, rejected) => {
        const action = IsTrue(obj.status) ? "desactivar" : "activar";
        swal({
            title: 'Cambiar estatus',
            text: `¿Está seguro de querer ${action} este registro?`,
            type: 'question',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: `!Sí, ${action}!`
        }).then(async (result) => {
            if (result.value) {
                const res = await MakeRequest(urlRequest, obj);

                if ((res.status === 200)) {
                    AlertSuccessChangeStatus(obj.status);
                    console.log(res);
                    return resolve(res);
                } else {
                    AlertError();
                    return resolve(res)
                }
            } else {
                AlertCanceled();
                return rejected('Acción cancelada');
            }
        });
    });
};

let ConfirmDeleteActionReturnDataObject = (urlRequest, obj) => {
    return new Promise((resolve, rejected) => {
        swal({
            title: '¿Está seguro de eliminar este registro?',
            text: '¡No podrás revertir esto!',
            type: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Sí, eliminarlo!'
        }).then((result) => {
            if (result.value) {

                let HttpRequest = MakeRequest(urlRequest, obj);

                HttpRequest
                    .then(res => {
                        if (res.status === 204 || res.status === 200) {
                            AlertSuccessDeleted();
                            return resolve(res);
                        } else {
                            AlertError();
                            return resolve(res);
                        }
                    }).catch(err => {
                        AlertDisconnected();
                        return rejected(err);
                    });

            } else {
                AlertCanceled();
                return rejected('Acción cancelada');
            }
        });
    });
};

let ConfirmDeleteActionReturnData = (urlRequest, id) => {
    return new Promise((resolve, rejected) => {
        swal({
            title: '¿Está seguro de eliminar este registro?',
            text: '¡No podrás revertir esto!',
            type: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Sí, eliminarlo!'
        }).then((result) => {
            if (result.value) {
                const params = {
                    id: id
                };

                let UrlAction = new URL(`${window.location.protocol}//${window.location.host}/${urlRequest}`);
                Object.keys(params).forEach(key => UrlAction.searchParams.append(key, params[key]));

                fetch(UrlAction, {
                    mode: 'cors',
                    method: 'delete',
                    withCredentials: true,
                    crossdomain: true,
                })
                    .then(response => response.text().then(data => ({ status: response.status, body: data })))
                    .then((res) => {
                        if ((res.status === 204 || res.status === 200)) {
                            AlertSuccessDeleted();
                            return resolve(res);
                        } else {
                            AlertError();
                            return resolve(res)
                        }
                    })
                    .catch(err => {
                        reject(Error(err.message));
                        AlertDisconnected();
                        return rejected(err);
                    });
            } else {
                AlertCanceled();
                return rejected('Acción cancelada');
            }
        });
    });
};

let ConfirmInsertAction = (urlRequest, obj) => {
    swal({
        title: 'Ejecutar cambios',
        text: '¿Está seguro de editar este registro?',
        type: 'question',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Sí, ejecutar!'
    }).then((result) => {
        if (result.value) {

            let HttpRequest = MakeRequest(urlRequest, obj);

            HttpRequest
                .then(res => {
                    if ((res.status === 201 || res.status === 200)) {
                        AlertSuccess();
                        return res;
                    } else {
                        AlertError(res);
                    }
                }).catch(err => {
                    AlertDisconnected();
                });

        } else {
            AlertCanceled();
        }
    });
};

let AlertConfirmacion = () => {
    return new Promise((resolve, rejected) => {
        swal({
            title: 'Ejecutar cambios',
            text: '¿Está seguro de guardar los cambios?',
            type: 'question',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Sí, ejecutar!'
        }).then((result) => {
            if (result.value) {
                return resolve(res)
            } else {
                AlertCanceled();
            }
        });
    });
}

let AlertSuccess = () => {
    swal(
        'Procesado!',
        'Los cambios se efectuaron exitosamente.',
        'success'
    );
}

let AlertError = (mensaje) => {
    swal(
        'Error!',
        IsNullOrEmpty(mensaje) ? 'La solicitud no pudo ser procesada.' : mensaje,
        'info'
    );
}

let AlertNotFound = () => {
    swal(
        'Procesado!',
        'No se encontró ningún registro.',
        'info'
    );
}

let AlertDisconnected = () => {
    swal(
        'Error!',
        'Ha ocurrido un error de conexión.',
        'error'
    );
}

let AlertCanceled = () => {
    swal(
        '¡Cancelado!',
        'Se cancelo la acción.',
        'info'
    );
}

let AlertSuccessDeleted = () => {
    swal(
        '¡Eliminado!',
        'El registro ha sido eliminado.',
        'success'
    );
};

let AlertSuccessChangeStatus = (currentStatus) => {
    const action = IsTrue(currentStatus) ? "deshabilitado" : "habilitado";
    swal(
        'Procesado!',
        `¡El registro fue ${action} en forma exitosa!`,
        'success'
    );
}