import { get_store_value } from "svelte/internal";
import { writable } from "svelte/store"

export default class Store {
    constructor(value) {
        this._inner = writable(value);
    }

    subscribe(subscriber, invalidate) {
        return this._inner.subscribe(subscriber, invalidate);
    }

    update(updater) {
        this._inner.update(updater)
    }

    set(value) {
        this._inner.set(value)
    }

    get() {
        return get_store_value(this._inner);
    }
}