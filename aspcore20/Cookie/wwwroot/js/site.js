// Write your Javascript code.

$(function() {
    function onLogut(a, b) {
        console.log(a, b);
    }
    $('.nav').on('click', 'a[href="/Logout"]', function () {
        var form = document.getElementById('LogoutForm');
        form.submit();
        return false;
    });
});
