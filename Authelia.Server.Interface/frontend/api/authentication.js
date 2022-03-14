import { throwResponseError } from "./response";

export function signIn(user, password) {
    return fetch('/Authentication/login', {
        body: JSON.stringify({userName: user, password: password}),
        method: "POST",
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
    }).then(async response => {
        if (response.ok) return response;
        var err = response.json();
        throwResponseError(err.message, err);
    });
}