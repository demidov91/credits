$(function () {
    $('.problem-credit-request .first-to-show, .problem-credit-request .hide-details').click(function (e) {
        $(this).closest('.problem-credit-request').toggleClass('first');
    });
});
