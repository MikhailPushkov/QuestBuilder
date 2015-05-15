using QuestBuild_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuestBuild_MVC.Controllers
{
    public static class GetIdOrAddItemOf
    {
        public static int Institute(string nameOfInstitute)
        {
            int id = 0;
            using(ApplicationDbContext db = new ApplicationDbContext())
            {
                try
                {
                    id = db.Institutes.First(m => m.nameOfInstitute.ToLower() == nameOfInstitute.ToLower()).instituteId;
                }
                catch
                {
                    db.Institutes.Add(new Institute(nameOfInstitute));
                    db.SaveChanges();
                }
            }
            if(id != 0)
            {
                return id;
            }
            else
            {
                using(ApplicationDbContext db = new ApplicationDbContext())
                {
                    return id = db.Institutes.First(m => m.nameOfInstitute.ToLower() == nameOfInstitute.ToLower()).instituteId;
                }
            }
        }

        public static int Faculti(string nameOfFaculti, int instituteId)
        {
            int id = 0;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                try
                {
                    id = db.Faculties.Where(m => m.nameOfFaculti == nameOfFaculti).Where(n => n.instituteId == instituteId).First().facultiId;
                }
                catch
                {
                    db.Faculties.Add(new Faculties(nameOfFaculti, instituteId));
                    db.SaveChanges();
                }
            }
            if (id != 0)
            {
                return id;
            }
            else
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    return id = db.Faculties.Where(m => m.nameOfFaculti == nameOfFaculti).Where(n => n.instituteId == instituteId).First().facultiId;
                }
            }
        }

        public static int Chair(string nameOfChair, int facultiId)
        {
            int id = 0;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                try
                {
                    id = db.Chairs.Where(m => m.facultiId == facultiId).Where(n => n.nameOfChair == nameOfChair).First().chairId;
                }
                catch
                {
                    db.Chairs.Add(new Chairs(nameOfChair, facultiId));
                    db.SaveChanges();
                }
            }
            if (id != 0)
            {
                return id;
            }
            else
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    return id = db.Chairs.Where(m => m.facultiId == facultiId).Where(n => n.nameOfChair == nameOfChair).First().chairId;
                }
            }
        }

        public static int Subject(string nameOfSubject)
        {
            int id = 0;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                try
                {
                    id = db.Subjects.First(m => m.nameOfSubject == nameOfSubject).subjectId;
                }
                catch
                {
                    db.Subjects.Add(new Subjects(nameOfSubject));
                    db.SaveChanges();
                }
            }
            if (id != 0)
            {
                return id;
            }
            else
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    return id = db.Subjects.First(m => m.nameOfSubject == nameOfSubject).subjectId;
                }
            }
        }

        public static int TypeOfQuestion(string nameOfType)
        {
            int id = 0;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                try
                {
                    id = db.TypeOfQuestions.First(m => m.nameOfType == nameOfType).typeOfQuestionsId;
                }
                catch
                {
                    db.TypeOfQuestions.Add(new TypeOfQuestions(nameOfType));
                    db.SaveChanges();
                }
            }
            if (id != 0)
            {
                return id;
            }
            else
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    return id = db.TypeOfQuestions.First(m => m.nameOfType == nameOfType).typeOfQuestionsId;
                }
            }
        }

        public static int Theme(string nameOfTheme, int subjectId, int teacherId)
        {
            int id = 0;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                try
                {
                    id = db.Themes.Where(m => m.nameOfTheme == nameOfTheme).Where(m => m.subjectId == subjectId).Where(m => m.teacherId == teacherId).Select(m => m.themeId).Single();
                }
                catch
                {
                    string sqlCommand = "INSERT INTO Themes (nameOfTheme, teacherId, subjectId, parentTheme_themeId) VALUES ({0}, {1}, {2}, NULL)";
                    db.Database.ExecuteSqlCommand(sqlCommand, nameOfTheme, teacherId, subjectId);
                    db.SaveChanges();
                }
            }
            if (id != 0)
            {
                return id;
            }
            else
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    return id = db.Themes.Where(m => m.nameOfTheme == nameOfTheme).Where(m => m.subjectId == subjectId).Where(m => m.teacherId == teacherId).Select(m => m.themeId).Single();
                }
            }
        }

        public static int SubTheme(string nameOfSubTheme, int idTheme)
        {
            int id = 0;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                try
                {
                    id = db.Themes.Where(m => m.nameOfTheme == nameOfSubTheme).Where(m => m.parentTheme_themeId == idTheme).Select(m => m.themeId).Single();
                }
                catch
                {
                    var theme = (from th in db.Themes
                                 where th.themeId == idTheme
                                 select new
                                 {
                                     teacherId = th.teacherId,
                                     subjectId = th.subjectId
                                 }).Single();

                    string sqlCommand = "INSERT INTO Themes (nameOfTheme, teacherId, subjectId, parentTheme_themeId) VALUES ({0}, {1}, {2}, {3})";
                    db.Database.ExecuteSqlCommand(sqlCommand, nameOfSubTheme, theme.teacherId, theme.subjectId, idTheme);
                    db.SaveChanges();
                }
            }
            if (id != 0)
            {
                return id;
            }
            else
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    return id = db.Themes.Where(m => m.nameOfTheme == nameOfSubTheme).Where(m => m.parentTheme_themeId == idTheme).Select(m => m.themeId).Single();
                }
            }
        }
    }
}