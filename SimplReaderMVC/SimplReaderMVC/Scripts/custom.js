function ajaxFormSuccess(context, status, data) {
    if (typeof data == "undefined") {
        return 1;
    }
    switch (data.getResponseHeader("nano-action")) {
        case "reload":
            window.location.reload();
            break;
        case "reloadself":
            window.location.reload();
            break;
        case "reloadparent":
            reloadParent();
            break;
        case "redirect":
            window.location.href = data.getResponseHeader("nano-location");
            break;
        default:
            break;
    }
    var modelStateErrors = data.getResponseHeader("nano-modelStateErrors");
    if (typeof modelStateErrors != "undefined" && modelStateErrors != null && modelStateErrors.length > 0) {
        $.noty.closeAll();
        jQuery.each(jQuery.parseJSON(modelStateErrors), function (index, value) {
            top.noty({ text: value.Value, type: "error" });
        });
    }
    return true;
}