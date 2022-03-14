export class ResponseError extends Error {
    constructor(message, response) {
        super(message, response);
        this.response = response;
    }
}

export function throwResponseError(message, response) {
    throw new ResponseError(message, response);
}