$(document).ready(function () {
    $('.btn-inschrijven').click(function () {
        var lesid = $(this).attr('id');
        $.post('/Reserveren/GetLesData', { id: lesid }, function (data) {
            var les = $.parseJSON(data);
            var modalId = "modal" + lesid;
            createModal(modalId);
            fillModalLesDetails(modalId, les);          
            $("#" + modalId).modal('show');
        });
    });
});

function createModal(id) {   
    var modalHtml = "<div class='modal fade' id='" + id + "' tabindex='-1' role='dialog' aria-labelledby='myModalLabel'>"
                        +"<div class='modal-dialog' role='document'"
                            +"<div class='modal-content'>"
                                +"<div class='modal-header'>"
                                    +"<button type='button' class='close' data-dismiss='modal' aria-label='Close'><span aria-hidden='true'>&times;</span></button>"
                                    +"<h4 class='modal-title' id='myModalLabel'></h4>"
                                +"</div>"
                                +"<div class='modal-body'>"
                                +"</div>"
                                +"<div class='modal-footer'>"
                                    +"<button type='button' class='btn btn-default' data-dismiss='modal'>Sluiten</button>"
                                +"</div>"
                            +"</div>"
                        +"</div>"
                    + "</div>";
    $('#site-wrapper').after(modalHtml);
}
function fillModalLesDetails(id, les) {
    var lesDetailsHeaderHtml = "<h3>Weet u zeker dat u wilt inschrijven?</h3>";
    var lesDetailsBodyHtml = "<div class='les-detail'>"
                            + "<h3>" + les.lesNaam + "</h3>"
                            + "<h4 class='ta-l mar-b-zero'>Docent</h4>"
                            + "<p class='sub-res ta-l mar-zero'>" + les.docent + "</p>"
                            + "<h4 class='ta-l mar-b-zero'>Datum:</h4>"
                            + "<p class='sub-res ta-l mar-zero'>" + les.datum + "</p>"
                            + "<h4 class='ta-l mar-b-zero'>Tijd:</h4>"
                            + "<p class='sub-res ta-l mar-zero'>" + les.tijd + "</p>"
                            + "<h4 class='ta-l mar-b-zero'>Aantal plaatsen:</h4>"
                            + "<p class='sub-res ta-l mar-zero'>" + les.aantalPlaatsen + "</p>"
                            + "<h4 class='ta-l mar-b-zero'>Aantal gereserveerd:</h4>"
                            + "<p class='sub-res ta-l mar-zero'>" + les.aantalGereserveerd + "</p>"
                        + "</div>";
    var lesDetailsFooterHtml = "<a href='/Reserveren/ReserveerLes/" + les.lesId + "' type='button' class='btn btn-lg btn-primary btn-inschrijven'>Inschrijven</a>";

    $("#" + id + "> .modal-dialog > .modal-header").html(lesDetailsHeaderHtml);
    $("#" + id + "> .modal-dialog > .modal-body").html(lesDetailsBodyHtml);
    $("#" + id + "> .modal-dialog > .modal-footer").html(lesDetailsFooterHtml);
}
