function HtmlRequest(urlAction, parameters = null) {
    this.UrlAction = urlAction;
    this.Parameters = parameters;

    this.MakeRequest = () => {
        waitingDialog.show('Processing...', { dialogSize: 'sm' });
        return new Promise((resolve, reject) => {
            urlAction = `${window.location.protocol}//${window.location.host}/${this.UrlAction}`;

            const headers = new Headers({
                'Content-Type': 'application/json; charset=utf-8'
            });
            fetch(urlAction, {
                mode: 'cors',
                method: 'post',
                withCredentials: true,
                crossdomain: true,
                headers: headers,
                body: JSON.stringify(this.Parameters)
            })
                .then(response => response.text().then(res => ({ status: response.status, body: res })))
                .then((res) => {
                    waitingDialog.hide();
                    resolve(res);
                })
                .catch(err => {
                    waitingDialog.hide();
                    reject(Error(err.message));
                });
        });
    };
}