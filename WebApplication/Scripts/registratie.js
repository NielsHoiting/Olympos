$(document).ready(function () {
    updateData()
    $('.btn-zoeken').click(function () {
        var lesid = $(this).attr('id');
        var achternaam = document.getElementById('achternaam_input').value;
        var geboortedatum = document.getElementById('geboortedatum_input').value;
        $.post('/Account/ZoekGebruiker', { achternaam: achternaam , geboortedatum: geboortedatum }, function (data) {
            var gebruiker = $.parseJSON(data);
            var modalId = "modal" + lesid;
            createModal(modalId);
            fillModalLesDetails(modalId, gebruiker);
            $("#" + modalId).modal('show');
        });
    });
    $('.btn-zoeken').click(function () {
        var lesid = $(this).attr('id');
        var achternaam = document.getElementById('achternaam_input').value;
        var geboortedatum = document.getElementById('geboortedatum_input').value;
        $.post('/Account/ZoekGebruiker', { achternaam: achternaam, geboortedatum: geboortedatum }, function (data) {
            var gebruiker = $.parseJSON(data);
            var modalId = "modal" + lesid;
            createModal(modalId);
            fillModalLesDetails(modalId, gebruiker);
            $("#" + modalId).modal('show');
        });
    });
});


function updateData() {
    var lesid = window.location.pathname.split('/')[3];
    $.post('/Registratie/GetDeelnemers', { lesid: lesid }, function (data) {
        var deelnemers = $.parseJSON(data);
        var deelnemerdata = "";
        for (i = 0; i < deelnemers.length; i++) {
            deelnemerdata = deelnemerdata + "<tr> <td>" + deelnemers[i].voornaam + " " + deelnemers[i].achternaam + " </td> <td> <button id='deelnemers[i].sco_nummer' type='button' class='btn smallbtn-anw btn-lg btn-primary btn-aanwezigheid'>Aanwezig</button></td></tr>";
        }
        document.getElementById("deelnemers").innerHTML = deelnemerdata;
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
    var lesDetailsHeaderHtml = "<h3>Weet u zeker dat u wilt inschrijven?</h3>";
    var lesDetailsBodyHtml = "<div class='les-detail'>"
                            + "<h3>" + gebruiker.naam + "</h3>"
                            + "<h4 class='ta-l mar-b-zero'>Docent</h4>"
                            + "<p class='sub-res ta-l mar-zero'>" + "</p>"
                            + "<h4 class='ta-l mar-b-zero'>Datum:</h4>"
                            + "<p class='sub-res ta-l mar-zero'>" + "</p>"
                            + "<h4 class='ta-l mar-b-zero'>Tijd:</h4>"
                            + "<p class='sub-res ta-l mar-zero'>" + "</p>"
                            + "<h4 class='ta-l mar-b-zero'>Aantal plaatsen:</h4>"
                            + "<p class='sub-res ta-l mar-zero'>" + "</p>"
                            + "<h4 class='ta-l mar-b-zero'>Aantal gereserveerd:</h4>"
                            + "<p class='sub-res ta-l mar-zero'>" + "</p>"
                        + "</div>";
    var lesDetailsFooterHtml = "<a href='/Reserveren/ReserveerLes/" + "' type='button' class='btn btn-lg btn-primary btn-inschrijven'>Inschrijven</a>";

    $("#" + id + "> .modal-dialog > .modal-header").html(lesDetailsHeaderHtml);
    $("#" + id + "> .modal-dialog > .modal-body").html(lesDetailsBodyHtml);
    $("#" + id + "> .modal-dialog > .modal-footer").html(lesDetailsFooterHtml);
}
