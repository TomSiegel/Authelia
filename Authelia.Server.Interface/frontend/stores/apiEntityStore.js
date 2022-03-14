import { deleteApi, getApiJson, postApi, putApi } from "../api/request";
import EntityStore from "./entityStore.js";

export default class ApiEntityStore extends EntityStore {
    constructor(value, comparer, endpoint) {
        super(value, comparer);
        this.parameters = {
            get: {},
            put: {},
            post: {},
            delete: {},
            default: {}
        };
        this.endpoint = endpoint;
        this.initialized = false;
        this.last = null;
    }

    loadApi() {
        return getApiJson(this.endpoint, Object.assign(this.parameters.default, this.parameters.get)).then(data => {
            this.set(data);
            return data;
        })
    }

    deleteApi(items) {
        return deleteApi(this.endpoint, items, Object.assign(this.parameters.default, this.parameters.delete)).then(response => {
            if (response.ok) this.removeItems(items);
            return this.get();
        })
    }

    updateApi(items) {
        return putApi(this.endpoint, items, Object.assign(this.parameters.default, this.parameters.put)).then(response => {
            if (response.ok) this.updateItems(items);
            return this.get();
        })
    }

    addApi(items) {
        return postApi(this.endpoint, items, Object.assign(this.parameters.default, this.parameters.post)).then(response => {
            if (response.ok) this.addItems(items);
            return this.get();
        })
    }

    initialize(expireInMs) {
        if (this.initialized == false || (this.last instanceof Date && (new Date() - this.last) > expireInMs)) {
            return this.loadApi();
        }

        return Promise.resolve(this.get());
    }
}