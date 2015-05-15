using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using QuestBuild_MVC.Models;

namespace QuestBuild_MVC.Controllers.SupportingClasses
{
    public class DataForShowRaiting
    {
        public DataForShowRaiting(int groupId, DateTime startDate, DateTime stopDate)
        {
            this.groupId = groupId;
            this.startDate = startDate;
            this.stopDate = stopDate;
            GetData();
        }
        public int groupId { get; set; }
        public DateTime startDate { get; set; }
        public DateTime stopDate { get; set; }
        public HashSet<DateTime> dates { get; set; }
        public List<StudAndR> studentsAndRait { get; set; }

        private void GetData()
        {
            dates = new HashSet<DateTime>();
            studentsAndRait = new List<StudAndR>();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                List<int> idStudents = GetIdStudents();
                foreach (var idS in idStudents)
                {
                    var datesAndRait = from dAndR in db.Works
                                       where dAndR.studentId == idS && dAndR.dateOfWork >= startDate && dAndR.dateOfWork <= stopDate
                                       select new
                                       {
                                           date = dAndR.dateOfWork,
                                           raiting = dAndR.rating
                                       };
                    var nameOfStudent = (from name in db.Students
                                            where name.studentId == idS
                                            select name.secondNameOfStudent + " " + name.firstNameOfStudent + " " + name.patronymicOfStudent).Single();
                    StudAndR studAndR = new StudAndR(nameOfStudent);
                    foreach (var dateAndRaiting in datesAndRait)
                    {
                        studAndR.raitingWithDay.Add(dateAndRaiting.date, dateAndRaiting.raiting);
                        dates.Add(dateAndRaiting.date);
                    }
                    studentsAndRait.Add(studAndR);
                }
            }
        }

        private List<int> GetIdStudents()
        {
            List<int> idStudents = new List<int>();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                idStudents = db.Students.Where(m => m.groupId == groupId).Select(m => m.studentId).ToList();
            }
            return idStudents;
        }
    }
}