function confirmDelete(uniqueId, isDeleteClicked) { //to inject the custom script  we need to add <script> in the
                                                  //_Layout.cshtml
    var deleteSpan = 'deleteSpan_' + uniqueId;
    var confirmDeleteSpan = 'confirmDeleteSpan_' + uniqueId;
   console.log("Helooooooo")
    if (isDeleteClicked) {
        $("#" + deleteSpan).hide();
        $("#" + confirmDeletespan).show();
    } else {
        $("#" + deleteSpan).show();
        $("#" + confirmDeleteSpan).hide();
    }


}