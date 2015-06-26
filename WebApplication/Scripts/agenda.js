$(document).ready(function(){
    $('#zoeken').click(function () {
        var sportcodes = [];
        $('#sportcode-filter :selected').each(function (i, selected) {
            sportcodes[i] = $(selected).text();
        });
        var url = "/Reserveren/WeekOverzicht?"; 
        for (var i = 0; i < sportcodes.length ; i++) {
            url += "sportcodes=" + sportcodes[i] + "&";
        }
        alert(url);
        window.location = url;
    });
});