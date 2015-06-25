$(document).ready(function () {
    updateData()
    $('.btn-zoeken').click(function () {
        var lesid = $(this).attr('id');
        var achternaam = document.getElementById('achternaam_input').value;
        var geboortedatum = document.getElementById('geboortedatum_input').value;
        var timestamp=Date.parse(geboortedatum);
        if (achternaam != "" && isNaN(timestamp)==false){
        $.post('/Account/ZoekGebruiker', { achternaam: achternaam , geboortedatum: geboortedatum }, function (data) {
            var gebruiker = $.parseJSON(data);
            var modalId = "modal" + lesid;
            createModal(modalId);
            fillModalLesDetails(modalId, gebruiker);
            $("#" + modalId).modal('show');
        });
        }
    });
    
        });


function updateData() {
    var lesid = window.location.pathname.split('/')[3];
    $.post('/Registratie/GetDeelnemers', { lesid: lesid }, function (data) {
        var deelnemers = $.parseJSON(data);
        var deelnemerdata = "";
        
        
        for (i = 0; i < deelnemers.length; i++) {
            var aanwezig;
            var aanwezigtext;
            if (deelnemers[i].isAanwezig) {
                aanwezig = 'n';
                aanwezigtext = "Aanwezig";
            } else {
                aanwezig = 'f';
                aanwezigtext = "Afwezig"
            }
            deelnemerdata = deelnemerdata + "<tr> <td>" + deelnemers[i].naam + " </td> <td> <button id='" + deelnemers[i].sco_nummer + "' type='button' class='btn smallbtn-a" + aanwezig + "w btn-lg btn-primary btn-aanwezigheid'>" + aanwezigtext +"</button></td></tr>";
        }
        document.getElementById("deelnemers").innerHTML = deelnemerdata;
        $('.btn-aanwezigheid').click(function () {
            var sco_nummer = $(this).attr('id');
            var lesid = window.location.pathname.split('/')[3];
            $.post('/Registratie/ToggleAanwezigheid', { sco_nummer: sco_nummer, lesid: lesid}, function (data) {
                var nieuw = $.parseJSON(data);
                updateData();
            });
        });
    });
}
function createModal(id) {
    var modalHtml = "<div class='modal fade' id='" + id + "' tabindex='-1' role='dialog' aria-labelledby='myModalLabel'>"
                        + "<div class='modal-dialog' role='document'"
                            + "<div class='modal-content'>"
                                + "<div class='modal-header'>"
                                    + "<button type='button' class='close' data-dismiss='modal' aria-label='Close'><span aria-hidden='true'>&times;</span></button>"
                                    + "<h4 class='modal-title' id='myModalLabel'></h4>"
                                + "</div>"
                                + "<div class='modal-body'>"
                                + "</div>"
                                + "<div class='modal-footer'>"
                                    + "<button type='button' class='btn btn-default' data-dismiss='modal'>Sluiten</button>"
                                + "</div>"
                            + "</div>"
                        + "</div>"
                    + "</div>";
    $('#site-wrapper').after(modalHtml);
}
function fillModalLesDetails(id, gebruiker) {
    var lesDetailsHeaderHtml = "<h3>Weet u zeker dat u deze Sporter wilt toevoegen?</h3>";
    var lesDetailsBodyHtml = "<div class='les-detail'>"
                            + "<h3>" + gebruiker.naam + "</h3>"
                            + "<h4 class='ta-l mar-b-zero'>foto</h4>"
                            + "<p class='sub-res ta-l mar-zero'> plek om foto in te voegen</p>"

                        + "</div>";
    var lesDetailsFooterHtml = "<button id='" + gebruiker.sco_nummer + "' type='button' class='btn btn-lg btn-primary btn-inschrijven'>Toevoegen</button>";

    $("#" + id + "> .modal-dialog > .modal-header").html(lesDetailsHeaderHtml);
    $("#" + id + "> .modal-dialog > .modal-body").html(lesDetailsBodyHtml);
    $("#" + id + "> .modal-dialog > .modal-footer").html(lesDetailsFooterHtml);
    $('.btn-inschrijven').click(function () {

        $("#"+id).modal('hide');
        var sco_nummer = $(this).attr('id');
        if(sco_nummer != 0){
        var lesid = window.location.pathname.split('/')[3];
        $.post('/Registratie/Inschrijven', { sco_nummer: sco_nummer, lesid: lesid }, function (data) {
            updateData();
            $(id).modal('hide');

        });
        }
    });
}
