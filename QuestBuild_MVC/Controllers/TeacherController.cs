using Newtonsoft.Json;
using QuestBuild_MVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuestBuild_MVC.Controllers
{
    public class TeacherController : Controller
    {
        private RegisterOfTeacherViewModel GetTeacher()
        {
            RegisterOfTeacherViewModel Teacher = new RegisterOfTeacherViewModel();

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Teachers teacher = db.Teachers.First(m => m.userId == User.Identity.Name);
                Teacher.firstNameOfTeacher = teacher.firstNameOfTeacher;
                Teacher.secondNameOfTeacher = teacher.secondNameOfTeacher;
                Teacher.patronymicOfTeacher = teacher.patronymicOfTeacher;

                Chairs chair = db.Chairs.First(m => m.chairId == teacher.chairId);
                Teacher.Chair = chair.nameOfChair;

                Faculties faculti = db.Faculties.First(m => m.facultiId == chair.facultiId);
                Teacher.Faculties = faculti.nameOfFaculti;

                Institute institute = db.Institutes.First(m => m.instituteId == faculti.instituteId);
                Teacher.Institute = institute.nameOfInstitute;

            }
            return Teacher;
        }
        //
        // GET: /Teacher/HomePageForTeacher
        [Authorize(Roles="teacher")]
        public ActionResult AboutMe()
        {
            ViewBag.Teacher = GetTeacher();
            return View(ViewBag);
        }

        //
        // GET: /Teacher/Questions
        [Authorize(Roles="teacher")]
        public ActionResult Questions()
        {
            using(ApplicationDbContext db = new ApplicationDbContext())
            {
                Teachers teacher = db.Teachers.Include("subjects").Single(m => m.userId == User.Identity.Name);
                ViewBag.Teacher = teacher;
                List<int> countOfQuestions = new List<int>();
                foreach(var subject in teacher.subjects)
                {
                    countOfQuestions.Add(db.Questions.Where(m => m.teacherId == teacher.teacherId).Where(m => m.subjectId == subject.subjectId).Count());
                }
                ViewBag.CountOfQuestions = countOfQuestions;
            }

            return View(ViewBag);
        }

        //
        //POST: /Teacher/Questions
        [HttpPost]
        [Authorize(Roles = "teacher")]
        [ValidateAntiForgeryToken]
        public ActionResult Questions(string nameOfSubject)
        {
            if (nameOfSubject != string.Empty)
            {
                int subjectId = GetIdOrAddItemOf.Subject(nameOfSubject);

                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    Teachers teacher = db.Teachers.Single(m => m.userId == User.Identity.Name);
                    Subjects subject = db.Subjects.Find(subjectId);
                    try
                    {
                        teacher.subjects.Add(subject);
                        db.Entry(teacher).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    catch
                    {

                    }
                }
            }

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Teachers teacher = db.Teachers.Include("subjects").Single(m => m.userId == User.Identity.Name);
                ViewBag.Teacher = teacher;
                List<int> countOfQuestions = new List<int>();
                foreach (var subject in teacher.subjects)
                {
                    countOfQuestions.Add(db.Questions.Where(m => m.teacherId == teacher.teacherId).Where(m => m.subjectId == subject.subjectId).Count());
                }
                ViewBag.CountOfQuestions = countOfQuestions;
            }

            return View(ViewBag);
        }

        //
        //GET: /Teacher/Edit
        [Authorize(Roles="teacher")]
        public ActionResult Edit()
        {
            RegisterOfTeacherViewModel Teacher = GetTeacher();
            return View(Teacher);
        }

        //
        //POST: /Teacher/Edit
        [HttpPost]
        [Authorize(Roles="teacher")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RegisterOfTeacherViewModel model)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                string userId = User.Identity.Name;
                int teacherId = db.Teachers.First(m => m.userId == userId).teacherId;
                int instituteId = GetIdOrAddItemOf.Institute(model.Institute);
                int facultiId = GetIdOrAddItemOf.Faculti(model.Faculties, instituteId);
                int chairId = GetIdOrAddItemOf.Chair(model.Chair, facultiId);

                Teachers teacher = db.Teachers.Find(teacherId);
                teacher.firstNameOfTeacher = model.firstNameOfTeacher;
                teacher.secondNameOfTeacher = model.secondNameOfTeacher;
                teacher.patronymicOfTeacher = model.patronymicOfTeacher;
                teacher.chairId = chairId;
                teacher.userId = userId;

                db.Entry(teacher).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("AboutMe", "Teacher");
            }
        }

       [HttpPost]
        public List<String> GetData(object b)
        {
            string ggg = (string)b;
            List<string> dd = new List<string>();
            return dd;
        }

        private List<string> GetThemes(string nameOfSubject)
        {
            List<string> themes = new List<string>();
            using(ApplicationDbContext db = new ApplicationDbContext())
            {
                int teacherId = db.Teachers.Single(m => m.userId == User.Identity.Name).teacherId;
                int subjectId = db.Subjects.Single(m => m.nameOfSubject == nameOfSubject).subjectId;
                themes = db.Themes.Where(m => m.teacherId == teacherId).Where(m => m.subjectId == subjectId).Select(m => m.nameOfTheme).ToList();          
            }
            return themes;
        }

        private List<string> GetThemes(int teacherId, int subjectId)
        {
            List<string> themes = new List<string>();
            using(ApplicationDbContext db = new ApplicationDbContext())
            {
                themes = db.Themes.Where(m => m.teacherId == teacherId).Where(m => m.subjectId == subjectId).Select(m => m.nameOfTheme).ToList();
            }
            return themes;
        }

        //
        // GET: /Teacher/Subject
        [Authorize(Roles="teacher")]
        public ActionResult Subject(string nameOfSubject)
        {
            ViewBag.Title = nameOfSubject;
            ViewBag.Themes = GetThemes(nameOfSubject);
            return View(ViewBag);
        }

        //
        //POST: /Teacher/Subject
        [HttpPost]
        [Authorize(Roles="teacher")]
        public ActionResult Subject(string nameOfTheme, string nameOfSubject)
        {
            int teacherId;
            int subjectId;
            using(ApplicationDbContext db = new ApplicationDbContext())
            {
                teacherId = db.Teachers.Single(m => m.userId == User.Identity.Name).teacherId;
                subjectId = db.Subjects.Single(m => m.nameOfSubject == nameOfSubject).subjectId;
            }
            GetIdOrAddItemOf.Theme(nameOfTheme, subjectId, teacherId);
            ViewBag.Title = nameOfSubject;
            ViewBag.Themes = GetThemes(teacherId, subjectId);
            return View(ViewBag);
        }

        private int GetTeacherId()
        {
            int teacherId;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                teacherId = db.Teachers.Single(m => m.userId == User.Identity.Name).teacherId;
            }
            return teacherId;
        }

        //
        //GET: /Teacher/SubthemesAndQuestions
        public ActionResult SubthemesAndQuestions(string nameOfTheme)
        {
            ViewBag.Title = nameOfTheme;
            using(ApplicationDbContext db = new ApplicationDbContext())
            {
                int teacherId = GetTeacherId();
                int themeId = db.Themes.Where(m => m.nameOfTheme == nameOfTheme).Where(m => m.teacherId == teacherId).Select(m => m.themeId).Single();
                List<string> subThemes = db.Themes.Where(m => m.parentTheme_themeId == themeId).Select(m => m.nameOfTheme).ToList();
                ViewBag.SubThemes = subThemes;

                var questions = db.Questions.Where(m => m.themeId == themeId).ToList();
                List<string> nameOfQuestions = new List<string>();
                List<string> nameOfTypes = new List<string>();
                List<int> idQuestions = new List<int>();
                foreach(var question in questions)
                {
                    nameOfQuestions.Add(question.nameOfQuestion);
                    idQuestions.Add(question.questionId);
                    switch(question.typeOfQuestionsId)
                    {
                        case 2: nameOfTypes.Add("Эссе");
                            break;
                        case 3: nameOfTypes.Add("Правда/Ложь");
                            break;
                        case 4: nameOfTypes.Add("Краткий ответ");
                            break;
                        case 5: nameOfTypes.Add("Множественный");
                            break;
                    }
                }

                ViewBag.idQuestions = idQuestions;
                ViewBag.Questions = nameOfQuestions;
                ViewBag.Types = nameOfTypes;
                ViewBag.idTheme = themeId;
            }
            return View(ViewBag);
        }

        //
        //POST: /Teacher/SubthemesAndQuestions
        [HttpPost]
        [Authorize(Roles = "teacher")]
        public ActionResult SubthemesAndQuestions(string nameOfSubTheme, int idTheme, string nameOfTheme)
        {

            ViewBag.Title = nameOfTheme;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                GetIdOrAddItemOf.SubTheme(nameOfSubTheme, idTheme);

                int teacherId = GetTeacherId();
                int themeId = db.Themes.Where(m => m.nameOfTheme == nameOfTheme).Where(m => m.teacherId == teacherId).Select(m => m.themeId).Single();
                List<string> subThemes = db.Themes.Where(m => m.parentTheme_themeId == themeId).Select(m => m.nameOfTheme).ToList();
                ViewBag.SubThemes = subThemes;

                List<string> questions = db.Questions.Where(m => m.themeId == themeId).Select(m => m.nameOfQuestion).ToList();
                ViewBag.Questions = questions;
                ViewBag.idTheme = themeId;
            }
            return View(ViewBag);
        }


        //
        // GET: /Teacher/AddQuestion
        [Authorize(Roles="teacher")]
        public ActionResult AddQuestion(int idTheme, string typeOfQuestion)
        {
            string subject;
            using(ApplicationDbContext db = new ApplicationDbContext())
            {
                subject = db.Themes.Where(m => m.themeId == idTheme).Select(m => m.subject.nameOfSubject).Single();
            }

            ViewBag.Title = subject;
            ViewBag.Type = typeOfQuestion;
            ViewBag.Theme = idTheme;
            ViewBag.nameOfQuestion = "";
            return View(ViewBag);
        }

        //
        //POST: /Teacher/AddQuestion
        [HttpPost]
        [Authorize(Roles = "teacher")]
        public ActionResult AddQuestion(int idQuestion, int idTheme, string nameOfTheme, char action)
        {
            if(action == 'd')
            {
                using(ApplicationDbContext db = new ApplicationDbContext())
                {
                    QuestBuild_MVC.Models.Questions question = db.Questions.Single(m => m.questionId == idQuestion);
                    db.Questions.Remove(question);
                    db.SaveChanges();
                    
                    SubthemesAndQuestions(nameOfTheme);
                    return View("SubthemesAndQuestions");
                }
            }
            else
            {
                using(ApplicationDbContext db = new ApplicationDbContext())
                {
                    QuestBuild_MVC.Models.Questions question = db.Questions.Include("typeOfQuestion").Where(m => m.questionId == idQuestion).Single();
                    string subject = db.Themes.Where(m => m.themeId == idTheme).Select(m => m.subject.nameOfSubject).Single();
                    List<string> answers = new List<string>();
                    List<string> truth = new List<string>();
                    List<Answers> getAnswers = db.Answers.Where(m => m.questionId == idQuestion).ToList();
                    foreach(var answer in getAnswers)
                    {
                        answers.Add(System.Text.Encoding.Unicode.GetString(answer.textOfAnswer));
                        if(answer.truth == true)
                        {
                            truth.Add("true");
                        }
                        else
                        {
                            truth.Add("false");
                        }
                    }

                    if(getAnswers.Count() == 1)
                    {
                        ViewBag.idAnswer = getAnswers[0].answerId;
                    }
                    else
                    {
                        List<int> idAnswers = new List<int>();
                        foreach(var answer in getAnswers)
                        {
                            idAnswers.Add(answer.answerId);
                        }
                        ViewBag.idAnswer = idAnswers;
                    }
                    ViewBag.nameOfQuestion = question.nameOfQuestion;
                    ViewBag.complexity = question.complexity;
                    ViewBag.idQuestion = question.questionId;
                    ViewBag.textOfQuestion = System.Text.Encoding.Unicode.GetString(question.textOfQuestion);
                    ViewBag.answers = answers;
                    ViewBag.truth = truth;
                    ViewBag.Count = getAnswers.Count();
                    ViewBag.Title = subject;
                    ViewBag.Type = question.typeOfQuestion.nameOfType;
                    ViewBag.Theme = idTheme;

                    return View(ViewBag);
                }
            }
        }

        //
        //POST: /Teacher/AddQuestion
        [HttpPost]
        [Authorize(Roles="teacher")]
        public void AddQuestionEssay(DataForQuestion dataForQuestion)
        {
            byte[] textOfQuestion = System.Text.Encoding.Unicode.GetBytes(dataForQuestion.textOfQuestion);
            int typeOfQuestionId = GetIdOrAddItemOf.TypeOfQuestion(dataForQuestion.typeOfQuestion);
            int subjectId = GetIdOrAddItemOf.Subject(dataForQuestion.nameOfSubject);

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                string userId = User.Identity.Name;
                int teacherId = db.Teachers.First(m => m.userId == userId).teacherId;
                Questions question;
                if ((dataForQuestion.idOfQuestion != null) && (dataForQuestion.idOfQuestion != 0))
                {
                    question = db.Questions.Where(m => m.questionId == dataForQuestion.idOfQuestion).Single();
                }
                else
                {
                    question = new Questions();
                }
                question.nameOfQuestion = dataForQuestion.nameOfQuestion;
                question.typeOfQuestionsId = typeOfQuestionId;
                question.teacherId = teacherId;
                question.subjectId = subjectId;
                question.textOfQuestion = textOfQuestion;
                question.themeId = dataForQuestion.themeOfQuestion;
                question.complexity = dataForQuestion.complexity;

                if ((dataForQuestion.idOfQuestion != null) && (dataForQuestion.idOfQuestion != 0))
                {
                    db.Entry(question).State = EntityState.Modified;
                }
                else
                {
                    db.Questions.Add(question);
                }
                db.SaveChanges();
            }
        }

        //
        //POST: /Teacher/AddQuestion
        [HttpPost]
        [Authorize(Roles = "teacher")]
        public void AddQuestionTrueOrLie(DataForQuestion dataForQuestions)
        {
            AddQuestionEssay(dataForQuestions);

            using(ApplicationDbContext db = new ApplicationDbContext())
            {
                string userId = User.Identity.Name;
                int teacherId = db.Teachers.First(m => m.userId == userId).teacherId;
                int questionId = db.Questions.Where(m => m.teacherId == teacherId).Where(m => m.nameOfQuestion == dataForQuestions.nameOfQuestion).Select(m => m.questionId).Single();
                Answers answer;
                if ((dataForQuestions.idOfAnswer != null) && (dataForQuestions.idOfAnswer != 0))
                {
                    answer = db.Answers.Where(m => m.answerId == dataForQuestions.idOfAnswer).Single();
                }
                else
                {
                    answer = new Answers();
                }
                answer.questionId = questionId;
                answer.textOfAnswer = System.Text.Encoding.Unicode.GetBytes("trueOrLie");
                answer.truth = dataForQuestions.trueOrLie;

                if ((dataForQuestions.idOfAnswer != null) && (dataForQuestions.idOfAnswer != 0))
                {
                    db.Entry(answer).State = EntityState.Modified;
                }
                else
                {
                    db.Answers.Add(answer);
                }
                db.SaveChanges();
            }
        }

        //
        //POST: /Teacher/AddQuestion
        [HttpPost]
        [Authorize(Roles = "teacher")]
        public void AddQuestionSmalAnswer(DataForQuestion dataForQuestions)
        {
            AddQuestionEssay(dataForQuestions);

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                string userId = User.Identity.Name;
                int teacherId = db.Teachers.First(m => m.userId == userId).teacherId;
                int questionId = db.Questions.Where(m => m.teacherId == teacherId).Where(m => m.nameOfQuestion == dataForQuestions.nameOfQuestion).Select(m => m.questionId).Single();

                Answers answer;
                if ((dataForQuestions.idOfAnswer != null) && (dataForQuestions.idOfAnswer != 0))
                {
                    answer = db.Answers.Where(m => m.answerId == dataForQuestions.idOfAnswer).Single();
                }
                else
                {
                    answer = new Answers();
                }
                answer.questionId = questionId;
                answer.textOfAnswer = System.Text.Encoding.Unicode.GetBytes(dataForQuestions.answer);
                answer.truth = true;

                if ((dataForQuestions.idOfAnswer != null) && (dataForQuestions.idOfAnswer != 0))
                {
                    db.Entry(answer).State = EntityState.Modified;
                }
                else
                {
                    db.Answers.Add(answer);
                }
                db.SaveChanges();
            }
        }

        //
        //POST: /Teacher/AddQuestion
        [HttpPost]
        [Authorize(Roles = "teacher")]
        public void AddQuestionMultiple(AnswerForAddInBase answerForAdd)
        {
            byte[] textOfAnswer = System.Text.Encoding.Unicode.GetBytes(answerForAdd.textOfAnswer);

            using(ApplicationDbContext db = new ApplicationDbContext())
            {
                string userId = User.Identity.Name;
                int teacherId = db.Teachers.First(m => m.userId == userId).teacherId;
                int questionId = db.Questions.Where(m => m.teacherId == teacherId).Where(m => m.nameOfQuestion == answerForAdd.nameOfQuestion).Select(m => m.questionId).Single();
                Answers answer;
                if(answerForAdd.idOfAnswer != 0)
                {
                    answer = db.Answers.Where(m => m.answerId == answerForAdd.idOfAnswer).Single();
                }
                else
                {
                    answer = new Answers();
                }

                answer.questionId = questionId;
                answer.textOfAnswer = textOfAnswer;
                answer.truth = answerForAdd.verity;

                if (answerForAdd.idOfAnswer != 0)
                {
                    db.Entry(answer).State = EntityState.Modified;
                }
                else
                {
                    db.Answers.Add(answer);
                }
                db.SaveChanges();
            }
        }
	}
}