$(document).ready(function(){
    $('#zoeken').click(function () {
        var sportcode = $('#sportcode-filter').val();
        var beginTijd = $('#begintijd-filter').val();
        var plekkenVrij = $('#plaatsen-filter').val();
        var bewaren = $('#instellingen-filter').prop('checked');
        var url = "/Reserveren/WeekOverzicht/?sportcode=" + sportcode + "&plekkenvrij=" + plekkenVrij + "&begintijd=" + beginTijd + "&bewaren=" + bewaren + "";
        window.location = url;
    });
});