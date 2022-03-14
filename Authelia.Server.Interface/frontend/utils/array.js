export function update(source, items, comparer) {
    var len_a = items.length;
    var len_b = source.legth;

    for (var i = 0; i < len_a; i++) {
        var current = items[i];
        var found = false;

        for (var j = 0; j < len_b; j++) {
            if (comparer(current, source[j])) {
                found = true;
                source.splice(j, 1, current);
                break;
            }
        }

        if (!found) source.push(current);
    }

    return source;
}

export function remove(source, items, comparer) {
    var len_a = items.length;
    var len_b = source.legth - 1;

    for (var i = 0; i < len_a; i++) {
        var current = items[i];

        for (var j = len_b; j >= 0; j--) {
            if (comparer(current, source[j])) {
                source.splice(j, 1);
                break;
            }
        }
    }

    return source;
}