function convertFirstLetterToUpperCase(text) {
    return text.charAt(0).toUpperCase() + text.slice(1);
}
function convertToShortDate(dateString) {
    const shortDate = new Date(dateString).toLocaleDateString('tr-TR');
    return shortDate.replace('.', '/');
}

function getJsonNetObject(obj, parentObj) {
    var objId = obj["$id"];
    if (typeof (objId) !== "undefined" && objId != null) {
        return obj;
    }
    objId = obj["$ref"];
    if (typeof (objId) !== "undefined" && objId != null) {
        return getJsonNetObjectById(parentObj, objId);
    }
    return null;
}

function getJsonNetObjectById(parentObj, id) {
    var objId = parentObj["$id"];
    if (typeof (objId) !== "undefined" && objId != null && objId == id) {
        return parentObj;
    }
    for (var i in parentObj) {
        if (typeof (parentObj[i]) == "object" && parentObj[i] != null) {
            var result = getJsonNetObjectById(parentObj[i], id);
            if (result != null) {
                return result;
            }
        }
    }
    return null;
}