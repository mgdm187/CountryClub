$(function () {
    $(document).on('click', '.confirm-delete', function (event) {
        const itemName = $(this).data('item-name') ?? 'the item';
        if (!confirm(`Delete ${itemName}?`)) {
            event.preventDefault();
        }
    });
});


function changeRole() {
    //$.each(localStorage.getItem("lista"), function (key, item) {
    //    html += '<div class="col-3">';
    //    html += '<p>' + item.Usluga + '</p></div>';
    //    html += '<div class="col-3"><p>' + item.Pocetak + '</p></div>';
    //    html += '<div class="col-3"><p>' + item.Zavrsetak + '</p></div>';
    //    html += '<div class="col-3"><button type=button" id="obrisi">Obrisi</button></div>';
    //});
    html += '<br /><div class="row"><div class="col-3">';
    html += '<select class="form-select" name="ulogaId" asp-items="ViewBag.Uloge"></select></div>';
    html += '<div class="col-3"><input type="time" name="pocetak" /></div>';
    html += '<div class="col-3"><input type="time" name="pocetak" /></div>';
    html += '<div class="col-3"><button type="button" id="add" /></div>';
}
