import DefaultStore from "./defaultStore.js";
import { update, remove } from "../utils/array.js";

export default class EntityStore extends DefaultStore {
    constructor(value, comparer) {
        super(value);

        this._comparer = comparer;
    }

    updateItems(items) {
        this.update(value => {
            return update(value, items, this._comparer);
        })
    }

    removeItems(items) {
        this.update(value => {
            return remove(value, items, this._comparer);
        })
    }

    addItems(items) {
        this.update(value => {
            var len = items.length;
            for(var i = 0; i < len; i++)
                value.push(items[i]);
            return value;
        })
    }

    addItem(item) {
        this.update(value => {
            value.push(item);
            return value;
        })
    }
}