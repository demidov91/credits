$(function () {
    $('.active-credit').click(function () {
        window.location = $(this).data('detailed-view-url');
    });
});