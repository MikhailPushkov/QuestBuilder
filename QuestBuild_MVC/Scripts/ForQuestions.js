var nameOfType;
function ChooseMultiple()
{
    DeleteActiveClass();
    var type = document.getElementById('multiple');
    type.className = "active";
    nameOfType = 'multiple';
    
    var description = document.getElementById('description');
    description.innerHTML = "Позволяет выбирать один или несколько правильных ответов из заданного списка.";
}
function ChooseTrueOrLie()
{
    DeleteActiveClass();
    var type = document.getElementById('trueOrLie');
    type.className = "active";
    nameOfType = 'trueOrLie';

    var description = document.getElementById('description');
    description.innerHTML = "Простая форма вопроса «Множественный выбор», предполагающая только два варианта ответа: «Верно» или «Неверно».";
}
function ChooseSmalAnswer()
{
    DeleteActiveClass();
    var type = document.getElementById('smalAnswer');
    type.className = "active";
    nameOfType = 'smalAnswer';

    var description = document.getElementById('description');
    description.innerHTML = "Позволяет вводить в качестве ответа одно или несколько слов.";
}
function ChooseEssay()
{
    DeleteActiveClass();
    var type = document.getElementById('essay');
    type.className = "active";
    nameOfType = 'essay';

    var description = document.getElementById('description');
    description.innerHTML = "Допускает ответ из нескольких предложений или абзацев. Должен быть оценен преподавателем вручную.";
}
function DeleteActiveClass()
{
    $("#types").children().removeClass('active');
}
function GetHref(idTheme)
{
    $("#link").attr("href", "AddQuestion?idTheme=" + idTheme + "&typeOfQuestion=" + nameOfType);
}
function AddSubTheme()
{

    $("<hr>").appendTo($("#panel"));

    $("<input>", {
        id: "nameST",
        class: "form - control",
        type: "text",
        name: "nameOfSubTheme",
    }).appendTo($("#panel"));
    $("<input>", {
        class: "btn btn-primary",
        type: "submit",
        value: "Сохранить"
    }).appendTo($("#panel"));
}

function OpenQuestion(id) {
    var jsonData = JSON.stringify(id);
    $.ajax({
        type: 'POST',
        url: 'AddQuestion',
        data: jsonData,
        contentType: 'application/json; charset=utf-8',
    });
}