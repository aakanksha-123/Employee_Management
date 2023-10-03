
console.log("helo")
function confirmDelete(uniqueId, isDeleteClicked) { //to inject the custom script  we need to add <script> in the
                                                  //Listusers.cshtml
    var deleteSpan = 'deleteSpan_' + uniqueId;
    var confirmDeleteSpan = 'confirmDeleteSpan_' + uniqueId;
    console.log(deleteSpan)
    console.log(confirmDeleteSpan)

    if (isDeleteClicked) {
        $("#" + deleteSpan).hide();
        $("#" + confirmDeleteSpan).show();
    } else {
        $("#" + deleteSpan).show();
        $("#" + confirmDeleteSpan).hide();
    }


}