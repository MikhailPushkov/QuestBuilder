function SetPeriod() {
    var bb = $('#nameOfPeriod').val();
    if($('#nameOfPeriod').val() == "anyPeriod")
    {
        $('#modalBody').append('<label class="col-sm-4 control-label">С</label>');
        $('#modalBody').append('<input id="firstDate" name="firstDate" type="date" class="form-control" />')
        $('#modalBody').append('<label class="col-sm-4 control-label">До</label>');
        $('#modalBody').append('<input id="lastDate" name="lastDate" type="date" class="form-control" />')
    }
}

function GoToRaiting()
{
    if($('input:checked'))
    {
        var date = $('input:checked').val();
        var group = $('#idGroup').val();
        var nameOfGroup = $('#nameOfGroup').val();
        location.href = "SetRaiting?idGroup=" + group + "&nameOfGroup" + nameOfGroup + "&date=" + date;
    }
}