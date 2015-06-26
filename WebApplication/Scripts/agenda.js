$(document).ready(function(){
    $('#zoeken').click(function () {
        var sportcodes = $('#sportcode-filter').val();
        //var array = sportcodes.split(',');
        var url = "/Reserveren/WeekOverzicht?";
        
        for (s in sportcodes) {
            url += "sportcodes=" + s + "&";
        }
        alert(url);
        window.location = url;
    });
});