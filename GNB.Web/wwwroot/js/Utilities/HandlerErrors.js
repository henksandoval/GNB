function HandlerErrors(error) {
    this.ShowErrors = false;
    this.Error = error;

    this.ShowError = () => {
        if (this.ShowErrors) {
            this.ShowErrorInConsole();
        }
    };

    this.ShowErrorInConsole = () => {
        console.log(this.Error.message);
    };
}