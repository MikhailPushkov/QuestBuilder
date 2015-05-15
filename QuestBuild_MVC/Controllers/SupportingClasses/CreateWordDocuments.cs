using QuestBuild_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Novacode;
using System.IO;
using System.IO.Compression;


namespace QuestBuild_MVC.Controllers.SupportingClasses
{
    public class CreateWordDocuments
    {
        public CreateWordDocuments(string typeOfWork, string folderName, List<Works> works)
        {
            this.typeOfWork = typeOfWork;
            this.folderName = folderName;
            this.works = works;
            CreateWord();
            CreateZip();
        }
        public string typeOfWork { get; set; }
        public string folderName { get; set; }
        public List<Works> works { get; set; }

        private void CreateWord()
        {
            string HeadLineText = typeOfWork;
            string subject = GetSubject();
            string teacher = GetTeacher();

            var headLineFormat = new Formatting();
            headLineFormat.FontFamily = new System.Drawing.FontFamily("Arial Black");
            headLineFormat.Size = 18D;
            headLineFormat.Position = 10;

            var baseFormat = new Formatting();
            baseFormat.FontFamily = new System.Drawing.FontFamily("Arial");
            baseFormat.Size = 14D;

            var studentFormat = new Formatting();
            studentFormat.FontFamily = new System.Drawing.FontFamily("Arial");
            studentFormat.Size = 16D;

            int variant = 1;
            foreach(Works work in works)
            {
                string student = GetStudent(work.studentId);
                string fileName = System.IO.Path.Combine(folderName, (student + ".docx"));

                var doc = DocX.Create(fileName);

                Paragraph title = doc.InsertParagraph(HeadLineText, false, headLineFormat);
                title.Alignment = Alignment.center;

                string typeTeach = "Предмет: " + subject + "    Преподаватель: " + teacher;
                Paragraph typeAndTeacher = doc.InsertParagraph(typeTeach, false, baseFormat);
                typeAndTeacher.Alignment = Alignment.center;
                doc.InsertParagraph();

                Paragraph theme = doc.InsertParagraph("Тема: " + work.themeOfWork, false, baseFormat);
                theme.Alignment = Alignment.center;
                doc.InsertParagraph();

                string varAndStudent = "Вариант №" + variant + " " + student;
                Paragraph studentPara = doc.InsertParagraph(varAndStudent, false, studentFormat);
                studentPara.Alignment = Alignment.center;

                int zadanie = 1;
                foreach(Questions question in work.questions)
                {
                    doc.InsertParagraph("Вопрос №" + zadanie, false, baseFormat);
                    string typeOfQuestion;
                    string textOfQuestion;
                    GetQuestion(question.questionId, out typeOfQuestion, out textOfQuestion);
                    doc.InsertParagraph(textOfQuestion, false, baseFormat);

                    switch (typeOfQuestion)
                    {
                        case "essay":
                            doc.InsertParagraph("Ответ:", false, baseFormat);
                            for(int i = 0; i < 6; i++)
                            {
                                doc.InsertParagraph();
                            }
                            break;
                        case "smalAnswer":
                            doc.InsertParagraph("Ответ:", false, baseFormat);
                            for(int i = 0; i < 3; i++)
                            {
                                doc.InsertParagraph();
                            }
                            break;
                        case "trueOrLie":
                            doc.InsertParagraph("Верно:", false, baseFormat);
                            doc.InsertParagraph("Не верно:", false, baseFormat);
                            doc.InsertParagraph();
                            break;
                        case "multiple":
                            doc.InsertParagraph("Выберите ответ:", false, baseFormat);
                            int noAnswer = 1;
                            List<string> answers = GetAnswers(question.questionId);
                            foreach(string answer in answers)
                            {
                                doc.InsertParagraph(noAnswer + ". " + answer, false, baseFormat);
                                noAnswer += 1;
                            }
                            doc.InsertParagraph();
                            break;
                    }
                    zadanie += 1;
                }
                variant += 1;
                doc.Save();
            }
        }

        private void CreateZip()
        {
            string pathString = folderName + "zip";
            System.IO.Directory.CreateDirectory(pathString);


            ZipFile.CreateFromDirectory(folderName, System.IO.Path.Combine(pathString, "Готовые_работы.zip"));
        }

        private string GetTeacher()
        {
            using(ApplicationDbContext db = new ApplicationDbContext())
            {
                int teacherId = works[0].teacherId;
                string teacher = (from t in db.Teachers
                                  where t.teacherId == teacherId
                                  select t.secondNameOfTeacher + " " + t.firstNameOfTeacher + " " + t.patronymicOfTeacher).Single();
                return teacher;
            }
        }

        private string GetSubject()
        {
            using(ApplicationDbContext db = new ApplicationDbContext())
            {
                int idsub = works[0].subjectId;
                string subject = db.Subjects.Where(m => m.subjectId == idsub).Select(m => m.nameOfSubject).Single();
                return subject;
            }
        }

        private string GetStudent(int idStudent)
        {
            using(ApplicationDbContext db = new ApplicationDbContext())
            {
                string student = (from s in db.Students
                                  where s.studentId == idStudent
                                  select s.secondNameOfStudent + " " + s.firstNameOfStudent + " " + s.patronymicOfStudent).Single();
                return student;
            }
        }

        private void GetQuestion(int idQuestion, out string typeOfQuestion, out string textOfQuestion)
        {
            using(ApplicationDbContext db = new ApplicationDbContext())
            {
                var question = (from q in db.Questions
                                where q.questionId == idQuestion
                                select new
                                {
                                    type = q.typeOfQuestion.nameOfType,
                                    text = q.textOfQuestion
                                }).Single();
                typeOfQuestion = question.type;
                textOfQuestion = System.Text.Encoding.Unicode.GetString(question.text);
            }
        }

        private List<string> GetAnswers(int idQuestion)
        {
            List<string> answers = new List<string>();
            using(ApplicationDbContext db = new ApplicationDbContext())
            {
                var answersByte = db.Answers.Where(m => m.questionId == idQuestion).Select(m => m.textOfAnswer);
                foreach(var answer in answersByte)
                {
                    answers.Add(System.Text.Encoding.Unicode.GetString(answer));
                }
            }
            return answers;
        }
    }
}