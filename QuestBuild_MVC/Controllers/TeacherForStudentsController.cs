using Newtonsoft.Json;
using QuestBuild_MVC.Controllers.SupportingClasses;
using QuestBuild_MVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuestBuild_MVC.Controllers
{
    public class TeacherForStudentsController : Controller
    {
        private int GetIdOfTeacher()
        {
            using(ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.Teachers.Where(m => m.userId == User.Identity.Name).Select(m => m.teacherId).Single();
            }
        }

        private List<string> GetGroups()
        {
            List<string> groupsForReturn = new List<string>();
            using(ApplicationDbContext db = new ApplicationDbContext())
            {
                int teacherId = GetIdOfTeacher();
                var groups = from gr in db.Groups
                             where gr.teacherId == teacherId
                             select new
                             {
                                 corurs = gr.numberOfCours,
                                 groupp = gr.numberOfGroup
                             };
                foreach(var group in groups)
                {
                    groupsForReturn.Add(group.corurs + "/" + group.groupp);
                }
            }
            return groupsForReturn;
        }

        private List<int> GetStudentsInGroups()
        {
            List<int> students = new List<int>();
            using(ApplicationDbContext db = new ApplicationDbContext())
            {
                int teacherId = GetIdOfTeacher();
                List<int> groups = db.Groups.Where(m => m.teacherId == teacherId).Select(m => m.groupId).ToList();
                foreach(var group in groups)
                {
                    students.Add(db.Students.Where(m => m.groupId == group).Count());
                }
            }
            return students;
        }

        //
        // GET: /TeacherForStudents/
        public ActionResult Students()
        {
            int teacherId = GetIdOfTeacher();
            using(ApplicationDbContext db = new ApplicationDbContext())
            {
                int chairId = db.Teachers.Where(m => m.teacherId == teacherId).Select(m => m.chairId).Single();
                int facultiId = db.Chairs.Where(m => m.chairId == chairId).Select(m => m.facultiId).Single();
                string institute = db.Faculties.Where(m => m.facultiId == facultiId).Select(m => m.institute.nameOfInstitute).Single();
                ViewBag.institute = institute;
            }
            ViewBag.groups = GetGroups();
            ViewBag.students = GetStudentsInGroups();
            return View(ViewBag);
        }

        //
        // POST: /TeacherForStudents/
        [HttpPost]
        [Authorize(Roles = "teacher")]
        public ActionResult Students(string institute, string nameOfCours, string nameOfGroup)
        {
            try
            {
                int cours = Convert.ToInt32(nameOfCours);
                int group = Convert.ToInt32(nameOfGroup);

                int idInstitute = GetIdOrAddItemOf.Institute(institute);
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    try
                    {
                        var test = db.Groups.Where(m => m.institute.nameOfInstitute == institute).Where(m => m.numberOfCours == cours).
                            Where(m => m.numberOfGroup == group).Select(m => m.institute.nameOfInstitute).Single();
                        ViewBag.institute = test;
                        ViewBag.groups = GetGroups();
                        ViewBag.students = GetStudentsInGroups();
                        return View(ViewBag);
                    }
                    catch
                    {
                        Groups groupForAdd = new Groups();
                        groupForAdd.instituteId = idInstitute;
                        groupForAdd.numberOfCours = cours;
                        groupForAdd.numberOfGroup = group;
                        groupForAdd.teacherId = GetIdOfTeacher();

                        db.Groups.Add(groupForAdd);
                        db.SaveChanges();


                        ViewBag.institute = institute;
                        ViewBag.groups = GetGroups();
                        ViewBag.students = GetStudentsInGroups();
                        return View(ViewBag);
                    }
                }
            }
            catch
            {
                ViewBag.institute = institute;
                ViewBag.groups = GetGroups();
                ViewBag.students = GetStudentsInGroups();
                return View(ViewBag);
            }
        }

        private int GetIdGroup(string group)
        {
            using(ApplicationDbContext db = new ApplicationDbContext())
            {
                string[] groupAfterParse = group.Split('/');
                int idTeacher = GetIdOfTeacher();
                int cours = Convert.ToInt32(groupAfterParse[0]);
                int groupForSerch = Convert.ToInt32(groupAfterParse[1]);
                int idGroup = db.Groups.Where(m => m.numberOfCours == cours).Where(m => m.numberOfGroup == groupForSerch).
                    Where(m => m.teacherId == idTeacher).Select(m => m.groupId).Single();
                return idGroup;
            }
        }

        //
        // GET: /TeacherForStudents/
        public ActionResult Group(string group)
        {
            int idGroup = GetIdGroup(group);
            using(ApplicationDbContext db = new ApplicationDbContext())
            {
                List<string> names = new List<string>();
                List<string> surnames = new List<string>();
                List<string> patrNames = new List<string>();
                List<int> idStudent = new List<int>();

                var students = from st in db.Students
                               where st.groupId == idGroup
                               select new
                               {
                                   studentId = st.studentId,
                                   name = st.firstNameOfStudent,
                                   surname = st.secondNameOfStudent,
                                   patrName = st.patronymicOfStudent
                               };
                foreach(var student in students)
                {
                    names.Add(student.name);
                    surnames.Add(student.surname);
                    patrNames.Add(student.patrName);
                    idStudent.Add(student.studentId);
                }
                ViewBag.names = names;
                ViewBag.surnames = surnames;
                ViewBag.patronumicNames = patrNames;
                ViewBag.idStudents = idStudent;
                ViewBag.Title = group;
            }

            return View(ViewBag);
        }

        //
        // POST: /TeacherForStudents/
        [HttpPost]
        [Authorize(Roles = "teacher")]
        public ActionResult Group(string surname, string name, string patrName, string group)
        {
                int idGroup = GetIdGroup(group);
                using(ApplicationDbContext db = new ApplicationDbContext())
                {
                    if(db.Students.Where(m => m.firstNameOfStudent == name).Where(m => m.secondNameOfStudent == surname).
                        Where(m => m.patronymicOfStudent == patrName).Select(m => m.studentId).Count() > 0)
                    {
                        Group(group);
                        return View();
                    }
                    else
                    {
                        Students student = new Students();
                        student.firstNameOfStudent = name;
                        student.secondNameOfStudent = surname;
                        student.patronymicOfStudent = patrName;
                        student.groupId = idGroup;

                        db.Students.Add(student);
                        db.SaveChanges();
                        Group(group);
                        return View();
                    }
                } 
        }

        //
        // POST: /TeacherForStudents/
        [HttpPost]
        [Authorize(Roles = "teacher")]
        public ActionResult SaveRaiting(List<int> idStudents, List<string> raiting, DateTime date, string group)
        {
            using(ApplicationDbContext db = new ApplicationDbContext())
            {
                int i = 0;
                foreach(int idS in idStudents)
                {
                    if(raiting[i] != "")
                    {
                        QuestBuild_MVC.Models.Works work = db.Works.Where(m => m.studentId == idS).Where(m => m.dateOfWork == date).Single();
                        work.rating = raiting[i];

                        db.Entry(work).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    i += 1;
                }
            }

            Group(group);
            return View("Group");
        }

        //
        // POST: /TeacherForStudents/
        [HttpPost]
        [Authorize(Roles = "teacher")]
        public ActionResult DeleteStudent(int idStudent, string group)
        {
            using(ApplicationDbContext db = new ApplicationDbContext())
            {
                Students student = db.Students.Single(m => m.studentId == idStudent);
                db.Students.Remove(student);
                db.SaveChanges();
                Group(group);
                return View("Group");
            }
        }

        //
        // POST: /TeacherForStudents/
        [HttpPost]
        [Authorize(Roles = "teacher")]
        public ActionResult ShowStudent(int idStudent)
        {
            StudentData student = new StudentData(idStudent);
            ViewBag.Student = student;
            ViewBag.works = student.subjects.Count();
            return View(ViewBag);
        }

        //
        // GET: /TeacherForStudents/
        public ActionResult Works()
        {
            int idTeacher = GetIdOfTeacher();
            using(ApplicationDbContext db = new ApplicationDbContext())
            {
                Teachers teacher = db.Teachers.Include("subjects").Single(m => m.teacherId == idTeacher);
                ViewBag.subjects = teacher.subjects;
                var idGroups = db.Groups.Where(m => m.teacherId == teacher.teacherId).Select(m => m.groupId);
                List<GroupsAndPeople> groupsAndPeople = new List<GroupsAndPeople>();
                foreach(var idGroup in idGroups)
                {
                    GroupsAndPeople groupAndPeople = new GroupsAndPeople(idGroup);
                    groupsAndPeople.Add(groupAndPeople);
                }
                ViewBag.groupsAndPeople = groupsAndPeople;
            }

            return View(ViewBag);
        }

        
        public ActionResult ThemesSearch(int idSubject)
        {
            using(ApplicationDbContext db = new ApplicationDbContext())
            {
                var themes = from them in db.Themes
                             where them.subjectId == idSubject
                             select new
                             {
                                 name = them.nameOfTheme,
                                 idTheme = them.themeId
                             };
                List<string> names = new List<string>();
                List<int> idThemes = new List<int>();
                foreach(var theme in themes)
                {
                    names.Add(theme.name);
                    idThemes.Add(theme.idTheme);
                }
                ViewBag.names = names;
                ViewBag.idThemes = idThemes;
            }
            return PartialView(ViewBag);
        }

        public ActionResult GetCountOFQ(int complexity, int idTheme, bool usingQuestOfSubT)
        {
            using(ApplicationDbContext db = new ApplicationDbContext())
            {
                var questions = db.Questions.Include("TypeOfQuestions").Where(m => m.themeId == idTheme).Where(m => m.complexity == complexity);
                int multiple = questions.Where(m => m.typeOfQuestion.nameOfType == "multiple").Count();
                int trueOrLie = questions.Where(m => m.typeOfQuestion.nameOfType == "trueOrLie").Count();
                int smalAnswer = questions.Where(m => m.typeOfQuestion.nameOfType == "smalAnswer").Count();
                int essay = questions.Where(m => m.typeOfQuestion.nameOfType == "essay").Count();

                ViewBag.multiple = multiple;
                ViewBag.trueOrLie = trueOrLie;
                ViewBag.smalAnswer = smalAnswer;
                ViewBag.essay = essay;
            }
            return PartialView(ViewBag);
        }

        public void CreateWork(DataForWork dataForWork)
        {
            int teacherId = GetIdOfTeacher();

            string folderName = Server.MapPath("~/Files");
            string pathString = System.IO.Path.Combine(folderName, Convert.ToString(teacherId));
            try
            {
                System.IO.Directory.Delete(pathString, true);
                System.IO.Directory.Delete(pathString + "zip", true);
            }
            catch
            { }
            System.IO.Directory.CreateDirectory(pathString);

            CreateAndAddWork createAndAddWork = new CreateAndAddWork(dataForWork, teacherId, pathString);
    
        }
        public FileResult result()
        {
            int teacherId = GetIdOfTeacher();

            string folderName = Server.MapPath("~/Files");
            string pathString = System.IO.Path.Combine(folderName, Convert.ToString(teacherId) + "zip/Готовые_работы.zip");

            byte[] mas = System.IO.File.ReadAllBytes(pathString);
            string file_type = "application/zip";
            string file_name = "Готовые_работы.zip";
            return File(mas, file_type, file_name);
        }

        public ActionResult ShowRaitings(string group, string nameOfPeriod, DateTime? firstDate, DateTime? lastDate)
        {
            int groupId = GetIdGroup(group);
            DateTime startDate = DateTime.Now.Date;
            DateTime stopDate = DateTime.Now.Date;
            switch (nameOfPeriod)
            {
                case "week":
                    while(startDate.DayOfWeek != DayOfWeek.Monday)
                    {
                       startDate = startDate.AddDays(-1);
                    }
                    while(stopDate.DayOfWeek != DayOfWeek.Sunday)
                    {
                        stopDate = stopDate.AddDays(1);
                    }
                    break;
                case "month":
                    int lastDay = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
                    while(startDate.Day != 1)
                    {
                       startDate = startDate.AddDays(-1);
                    }
                    while(stopDate.Day != lastDay)
                    {
                        stopDate = stopDate.AddDays(1);
                    }
                    break;
                case "anyPeriod":
                    if((firstDate != null) && (lastDate != null))
                    {
                        startDate = (DateTime)firstDate;
                        stopDate = (DateTime)lastDate;
                    }
                    break;
            }

            DataForShowRaiting data = new DataForShowRaiting(groupId, startDate, stopDate);
            ViewBag.Title = group;
            ViewBag.data = data;

            return View(ViewBag);
        }

        public ActionResult ChooseTheme(string group, string move, DateTime? date)
        {
            int idGroup = GetIdGroup(group);
            Dictionary<DateTime, string> themeOfWork = new Dictionary<DateTime, string>();
            HashSet<DateTime> dayOfWork = new HashSet<DateTime>();
            DateTime startDate = DateTime.Now.Date;
            DateTime stopDate = DateTime.Now.Date;
            switch(move)
            {
                case "now":
                    while (startDate.DayOfWeek != DayOfWeek.Monday)
                {
                    startDate = startDate.AddDays(-1);
                }
                while (stopDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    stopDate = stopDate.AddDays(1);
                }
                break;
                case "forward":
                startDate = (DateTime)date;
                stopDate = startDate.AddDays(7);
                break;
                case "back":
                stopDate = (DateTime)date;
                startDate = stopDate.AddDays(-7);
                break;
            }

            using(ApplicationDbContext db = new ApplicationDbContext())
            {
                List<int> idStudents = db.Students.Where(m => m.groupId == idGroup).Select(m => m.studentId).ToList();
                foreach(int idStudent in idStudents)
                {
                    var themeAndDays = from work in db.Works
                                      where work.studentId == idStudent && work.dateOfWork >= startDate && work.dateOfWork <= stopDate
                                      select new
                                      {
                                          theme = work.themeOfWork,
                                          day = work.dateOfWork
                                      };
                    foreach(var themeAndDay in themeAndDays)
                    {
                        dayOfWork.Add(themeAndDay.day);
                        try
                        {
                            themeOfWork.Add(themeAndDay.day, themeAndDay.theme);
                        }
                        catch
                        {}
                        
                    }
                }

            }

            ViewBag.group = idGroup;
            ViewBag.themes = themeOfWork;
            ViewBag.days = dayOfWork;
            ViewBag.start = startDate;
            ViewBag.stop = stopDate;
            ViewBag.nameGroup = group;

            return PartialView(ViewBag);
        }

        public ActionResult SetRaiting(int idGroup, string nameOfGroup, string date)
        {
            DateTime dateOfWork = Convert.ToDateTime(date);

            using(ApplicationDbContext db = new ApplicationDbContext())
            {
                string group = (from gr in db.Groups
                                where gr.groupId == idGroup
                                select gr.numberOfCours + "/" + gr.numberOfGroup).Single();
                var students = from stud in db.Students
                               where stud.groupId == idGroup
                               select new
                               {
                                   name = stud.secondNameOfStudent + " " + stud.secondNameOfStudent + " " + stud.patronymicOfStudent,
                                   idStudent = stud.studentId
                               };
                List<string> names = new List<string>();
                List<int> idS = new List<int>();
                foreach(var student in students)
                {
                    names.Add(student.name);
                    idS.Add(student.idStudent);
                }
                ViewBag.names = names;
                ViewBag.idS = idS;
                ViewBag.group = group;
            }

            ViewBag.date = date;
            ViewBag.idGroup = idGroup;

            return View(ViewBag);
        }
	}
}