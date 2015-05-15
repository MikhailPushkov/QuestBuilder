function SelectStudents(elem) {
    if ($(elem).prop("checked"))
    {
        $(elem).parent().parent().next().find('input').prop('checked', true);
    }
    else
    {
        $(elem).parent().parent().next().find('input').prop('checked', false);
    }
}

function SelectSomeStudents() {
    $('.in').parent().find('#selectGroup').prop('checked', false);

}

function GetSelectedTheme() {
    $('#selectedTheme').attr('value', $('#theme').val());
}

function CreateDataForWork() {
    var dataForWork = {
        subjectId: $('#subject').val(),
        themeId: $('#theme').val(),
        usingSubThemes: $('#useSubThemes').prop('checked'),
        complexity: $('#complexity').val(),
        typeOfWork: $('#typeOfWork').val(),
        dateOfWork: $('#dateOfWork').val()
    }

    var students = $("input[name='student']:checked");
    var idStudents = "";
    $("input[name='student']:checked").each(function (i, elem) {
        idStudents += elem.value + ",";
    })

    var typesAndCounts = "";
    if(($('#multipleCount').val() >= 1) && $('#multipleCheck').prop('checked'))
    {
        typesAndCounts += 'multiple-' + $('#multipleCount').val() + "-";
    }
    if (($('#trueOrLieCount').val() >= 1) && $('#trueOrLieCheck').prop('checked')) {
        typesAndCounts += 'trueOrLie-' + $('#trueOrLieCount').val() + "-";
    }
    if (($('#smalAnswerCount').val() >= 1) && $('#smalAnswerCheck').prop('checked')) {
        typesAndCounts += 'smalAnswer-' + $('#smalAnswerCount').val() + "-";
    }
    if (($('#essayCount').val() >= 1) && $('#essayCheck').prop('checked')) {
        typesAndCounts += 'essay-' + $('#essayCount').val() + "-";
    }

    dataForWork.students = idStudents;
    dataForWork.typesAndCounts = typesAndCounts;

    var jsonData = JSON.stringify(dataForWork);

    $('#btnCreate').detach();
    $('#final').append('<h3 id="status">Идет конструирование билетов. Это может занять несколько минут</h3>');

    $.ajax({
        type: 'POST',
        url: 'CreateWork',
        data: jsonData,
        contentType: 'application/json; charset=utf-8',
        success: function (data, textStatus) {
            $('#status').detach();
            $('#final').append('<h3 id="status">Создание билетов завершено</h3>');
            $('#final').append('<a class="btn btn-primary btn-lg" href="result">Скачать архив</a>');
        }
    })
}

function UseSubT() {
    var c = $('#useSubThemes').prop('checked');
    if ($('#useSubThemes').prop('checked') == true)
    {
        $('#usingQuestOfSubT').attr('value', true);
    }
    else
    {
        $('#usingQuestOfSubT').attr('value', false);
    }
}