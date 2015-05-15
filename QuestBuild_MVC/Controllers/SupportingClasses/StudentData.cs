using QuestBuild_MVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuestBuild_MVC.Controllers.SupportingClasses
{
    public class StudentData
    {
        public StudentData(int idStudent)
        {
            this.idStudent = idStudent;
            GetName();
            GetSubjects();
        }
        public int idStudent { get; set; }
        public string name { get; set; }
        public HashSet<SubjectData> subjects { get; set; }

        private void GetName()
        {
            using(ApplicationDbContext db = new ApplicationDbContext())
            {
                name = (from st in db.Students
                        where st.studentId == idStudent
                        select st.secondNameOfStudent + " " + st.firstNameOfStudent + " " + st.patronymicOfStudent).Single(); 
            }
        }

        private void GetSubjects()
        {
            subjects = new HashSet<SubjectData>();
            using(ApplicationDbContext db = new ApplicationDbContext())
            {
                List<int> idSubjects = db.Works.Where(m => m.studentId == idStudent).Select(m => m.subjectId).ToList();
                HashSet<int> subjectId = new HashSet<int>();
                foreach (var idSub in idSubjects)
                {
                    subjectId.Add(idSub);
                }
                foreach(var idSub in subjectId)
                {
                    SubjectData data = new SubjectData();
                    data.subjectName = db.Subjects.Where(m => m.subjectId == idSub).Select(m => m.nameOfSubject).Single();
                    data.themesOfWorks = new List<string>();
                    data.dayOfWorks = new List<DateTime>();
                    data.raitings = new List<string>();

                    var worksInfo = from work in db.Works
                                    where work.subjectId == idSub && work.studentId == idStudent
                                    select new
                                    {
                                        theme = work.themeOfWork,
                                        date = work.dateOfWork,
                                        raiting = work.rating
                                    };
                    foreach(var work in worksInfo)
                    {
                        data.themesOfWorks.Add(work.theme);
                        data.dayOfWorks.Add(work.date);
                        data.raitings.Add(work.raiting);

                        data.averRait += Convert.ToDouble(work.raiting);
                    }
                    data.averRait /= worksInfo.Count();
                    data.countOfWorks = worksInfo.Count();

                    subjects.Add(data);
                }
            }
        }
    }
}