import ApiEntityStore from "./apiEntityStore.js";

function compare(a, b) {
    return a.userId == b.userId;
}

let userStore = new ApiEntityStore([], compare, "api/users");

export default userStore;