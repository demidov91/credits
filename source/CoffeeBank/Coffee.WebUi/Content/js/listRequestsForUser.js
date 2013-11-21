/**
*jRow - jQuery object for the table row.
* Shows additional text information input if no credict product was selected.
* returns: jQuery object for the table row.
*/
function hideOrShowAdditionalTextInformation(jRow) {
    var additionalTextInfo = jRow.find('textarea');
    if (jRow.find('.select-credit-product').val() == "0") {
        additionalTextInfo.show();
    } else {
        additionalTextInfo.hide();
    }
    return jRow;
}


$(function () {
    $('.like-a-form').on('click', '.edit-row', function (e) {
        hideOrShowAdditionalTextInformation($(this).closest('tr').toggleClass('view edit'));
        return false;
    });
    $('.like-a-form').on('click', '.save-row', function (e) {
        var form = $(this).closest('.like-a-form');
        hideOrShowAdditionalTextInformation(form);
        form.css('opacity', '0.5');
        var data = $(form.find('input, select, textarea')).serialize();
        $.ajax({
            url: form.data('update-url'),
            method: 'POST',
            data: data,
            success: function (data) {
                form.html(data);
                form.toggleClass('view edit')
            },
            complete: function () {
                form.css('opacity', 1);
            }

        });
        return false;
    });
    $('table').on('change', '.select-credit-product', function (e) {
        hideOrShowAdditionalTextInformation($(this).closest('tr'));
    })

});