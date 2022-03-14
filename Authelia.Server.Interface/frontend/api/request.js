export function getApi(controller, params, init) {
    if (init == null || typeof init !== "object") init = {};
    return fetch(createUrl(controller, params), Object.assign(init, {
        method:"GET"
    }))
}

export function putApi(controller, body, params, init) {
    if (init == null || typeof init !== "object") init = {};
    return fetch(createUrl(controller, params), Object.assign(init, {
        method:"PUT",
        body: JSON.stringify(body)
    }))
}

export function postApi(controller, body, params, init) {
    if (init == null || typeof init !== "object") init = {};
    return fetch(createUrl(controller, params), Object.assign(init, {
        method:"POST",
        body: JSON.stringify(body)
    }))
}

export function deleteApi(controller, body, params, init) {
    if (init == null || typeof init !== "object") init = {};
    return fetch(createUrl(controller, params), Object.assign(init, {
        method:"DELETE",
        body: JSON.stringify(body)
    }))
}


export function getApiJson(controller, params, init) {
    return getApi(controller, params, init).then(response => response.json())
}

export function putApiJson(controller, body, params, init) {
    return putApi(controller, body, params, init).then(response => response.json())
}

export function postApiJson(controller, body, params, init) {
    return putApi(controller, body, params, init).then(response => response.json())
}

export function deleteApiJson(controller, body, params, init) {
    return putApi(controller, body, params, init).then(response => response.json())
}


export function createUrl(baseUrl, params) {
    if (typeof baseUrl === "string") baseUrl = new URL(baseUrl, window.location.href);
    if (!(baseUrl instanceof URL)) throw new Error("invalid url parameter");

    Object.keys(params).forEach(key => baseUrl.searchParams.append(key, params[key]))

    return baseUrl.href;
}