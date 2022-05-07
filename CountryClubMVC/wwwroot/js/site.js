$(function () {
    $(document).on('click', '.confirm-delete', function (event) {
        const itemName = $(this).data('item-name') ?? 'the item';
        if (!confirm(`Delete ${itemName}?`)) {
            event.preventDefault();
        }
    });
});


//function changeRole(roleId, projectId, personId) {
//    // applicationBaseUrl is set in Layout.cshtml
//    const payload = { roleId: roleId, projectId: projectId, personId: personId };
//    $.post(window.applicationBaseUrl + "People/ChangeRole", payload, function (data) {
//        if ('successful' in data) {
//            if (data.successful) {
//                toastr.success(data.message);
//            }
//            else {
//                toastr.error(data.message);
//            }
//        }
//        else {
//            toastr.error(data);
//        }
//    })
//        .fail(function (response) {
//            toastr.error(`${response.status} ${response.responseText}`);
//        });
//}
