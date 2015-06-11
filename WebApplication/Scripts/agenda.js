$(document).ready(function(){
    $('').change(function () {
        var week = 0
        $.post('/Home/SelectCalendarWeek', { week: week }, function (data) {


        });
    });
});