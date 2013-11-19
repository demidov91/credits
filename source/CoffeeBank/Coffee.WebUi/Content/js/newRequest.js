$(function () {
    $('#force-save').click(function (e) {
        if (this.checked) {
            $('#additional-info').show()
        } else {
            $('#additional-info').hide()
        }
    })
});