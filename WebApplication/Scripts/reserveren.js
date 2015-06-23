$(document).ready(function () {
    $('.btn-inschrijven').click(function () {
        var lesid = $(this).attr('id');
        $.post('/Reserveren/GetLesData', { id: lesid }, function (data) {
            var les = $.parseJSON(data);
            $('#myModal').modal('show');
        });
    });   
});