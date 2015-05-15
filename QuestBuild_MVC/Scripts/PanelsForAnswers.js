window.onload = function () {
    AddAnswers(5);
    $('#btn_moreQuest').click(function () {
        location.reload();
    });
    AddDataToFields();
}
var n = 1;
function AddAnswers(count)
{
    for (i = 0; i < count; i++)
    {
        $('<div>', {
            class: "panel panel-default",
            append: $('<div>', {
                class: "panel-heading",
                text: 'Ответ №' + n
            }).add($('<div>', {
                id: "answer" + n,
                class: "panel-body", 
                append: $('<textarea>').add($('<div>', {
                    class: "row",
                    append: $('<div>', {
                        class: "col-md-8",
                        append: $('<label>', {
                            text: "Верный:  ",
                            append: $('<input>', {
                                id: "verity" + n,
                                type: "checkbox"
                            })
                        })
                    })
                }))
            }))

        }).appendTo('#answers');
        n += 1;
        tinymce.init({
            selector: 'textarea',
            plugins: 'image',
            height: 150,
            plugins: "table",
            tools: "inserttable"
        });
    }
}




function AddQuestion(theme) {
    var type = $('#type').val();
    var frame = document.querySelector('iframe').contentWindow;
    var text = frame.document.querySelector('body').innerText;
    var dataforQuestion = {
        nameOfSubject: $('#subject').val(),
        typeOfQuestion: type,
        nameOfQuestion: $('#nameOfQuestionField').val(),
        textofQuestion: text,
        themeOfQuestion: theme,
        complexity: $('#complexity').val()
    }
    if ($('#idQuesion').val())
    {
        dataforQuestion.idOfQuestion = $('#idQuesion').val();
    }
    if ($('#idAnswer').val())
    {
        dataforQuestion.idOfAnswer = $('#idAnswer').val();
    }
    switch (type) {
        case 'essay':
            var jsonData = JSON.stringify(dataforQuestion);
            $.ajax({
                type: 'POST',
                url: 'AddQuestionEssay',
                data: jsonData,
                contentType: 'application/json; charset=utf-8',
                success: function (data, textStatus) {
                    $('#modalForOk').modal('show');
                }
            }); break
        case 'trueOrLie':
            dataforQuestion.trueOrLie = $('#true').prop("checked");
            var jsonData = JSON.stringify(dataforQuestion);
            $.ajax({
                type: 'POST',
                url: 'AddQuestionTrueOrLie',
                data: jsonData, 
                contentType: 'application/json; charset=utf-8',
                success: function (data, textStatus) {
                    $('#modalForOk').modal('show');
                }
            }); break
        case 'smalAnswer':
            dataforQuestion.answer = $('#answerField').val();
            var jsonData = JSON.stringify(dataforQuestion);
            $.ajax({
                type: 'POST',
                url: 'AddQuestionSmalAnswer',
                data: jsonData,
                contentType: 'application/json; charset=utf-8',
                success: function (data, textStatus) {
                    $('#modalForOk').modal('show');
                }
            }); break
        case 'multiple':
     
            var jsonData = JSON.stringify(dataforQuestion);
            $.ajax({
                type: 'POST',
                url: 'AddQuestionEssay',
                data: jsonData,
                contentType: 'application/json; charset=utf-8',
                success: function (data, textStatus) {
                    AddAnswersForQuest(dataforQuestion);
                }
            }); break
    }
}

function AddAnswersForQuest(dataforQuestion) {
    for(var i = 1; i < n; i++)
    {
        var id1 = '#answer' + i;
        var answer = $(id1).find('iframe').first()[0].contentDocument.all[5].innerText;
        if ((answer != '<p><br data-mce-bogus="1"></p>') && (answer != "")) {
            var id2 = '#verity' + i;
            var verity = $(id2).prop("checked");

            var answerForAdd = {
                textOfAnswer: answer,
                verity: verity,
                nameOfQuestion: dataforQuestion.nameOfQuestion
            }
            if ($('#idAnswers'))
            {
                var idAns = '#idAnswer' + i;
                answerForAdd.idOfAnswer = $(idAns).val();
            }
              var jsonData = JSON.stringify(answerForAdd);
              $.ajax({
                  type: 'POST',
                  url: 'AddQuestionMultiple',
                  data: jsonData,
                  contentType: 'application/json; charset=utf-8',
                  success: function (data, textStatus) {
                      if(i == n)
                      {
                          $('#modalForOk').modal('show');
                      }
                  }
              });
        }
    }
}

function AddDataToFields() {
    var nameOfQuestion = $('#nameOfQuestion').val();
    if(nameOfQuestion && (nameOfQuestion != ""))
    {
        var text = $('#textOfQuestion').val();
        $('#nameOfQuestionField').attr('value', nameOfQuestion);
        var type = $('#type').val();
        var value = $('#complexityGet').val();
        $("select#complexity").val(value);

        setTimeout(function () {
            var frame = document.querySelector('iframe').contentWindow;
            frame.document.querySelector('body').innerHTML = text;
        }, 1500);

        switch (type) {
            case 'trueOrLie':
                if ($('#truth').val() == 'true') {
                    $('#true').attr('checked', true);
                }
                break;
            case 'smalAnswer':
                $('#answerField').attr('value', $('#answer').val());
            case 'multiple':

                setTimeout(function () {

                for(var i = 1; i <= $('#count').val(); i++)
                {
                        var id1 = '#answer' + i;
                        var idA = '#answerField' + i;
                        $(id1).find('iframe').first()[0].contentDocument.all[5].innerHTML = $(idA).val();
                        var id2 = '#verity' + i;
                        var idT = '#truth' + i;
                        if ($(idT).val() == "true")
                        {
                            $(id2).attr('checked', true)
                        }
                    }
                }, 2000);
                break;
        }

    }
}

