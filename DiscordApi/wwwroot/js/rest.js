botController = window.location + "api/Bots/"

window.loadBots = function loadBots() {
    return new Promise((resolve, reject) => {
        var header = window.buildTokenHeader();
        if (!header) reject();

        axios.get(botController + "Bots", {
                headers: header
            }
        )
            .then(result => resolve(result))
            .catch(error => reject(error))
    })
}