authController = window.location + "api/Authentication/"

window.buildTokenHeader = function () {
    var token = window.localStorage.getItem("token")
    if (token !== undefined && token !== null) {
        return {
            'Content-Type': 'application',
            'Authorization': "Token " + token,
        }
    } else return false;
}

window.login = function login(username, password) {
    return new Promise((resolve, reject) => {
        axios.post(authController + "Authenticate", {}, {
            headers: {
                'Concent-Type': 'application',
                'Authorization': "Basic " + btoa(username + ":" + password),
            }
        }).then((result) => {
            let token = result.data.token;
            if (token !== undefined) {
                window.localStorage.setItem("token", token)
                console.log("Login Successful.")
                resolve(result)
            } else {
                reject("Token was invalid.")
            }
        }).catch((error) => {
            reject(error)
        });
    })
}

window.verify = function verify(token) {
    return new Promise((resolve, reject) => {
        axios.post(authController + "Validate",
            {},
            {
                headers: {
                    'Content-Type': 'application',
                    'Authorization': "Token " + token,
                }
            }).then((response) => resolve(response))
            .catch((error) => {
                window.localStorage.removeItem("token")
                reject(error)
            })
    });
}

window.logout = function logout(token) {
    return new Promise((resolve, reject) => {
        axios.post(authController + "Revoke",
            {},
            {
                headers: {
                    'Content-Type': 'application',
                    'Authorization': "Token " + token,
                }
            }).then((response) => {
                window.localStorage.removeItem("token");
                resolve(response);
            }
        )
            .catch((error) => {
                window.localStorage.removeItem("token");
                reject(error)
            })
    });
}