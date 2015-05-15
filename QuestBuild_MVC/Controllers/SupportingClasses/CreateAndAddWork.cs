using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using QuestBuild_MVC.Models;

namespace QuestBuild_MVC.Controllers.SupportingClasses
{
    public class CreateAndAddWork
    {
        public CreateAndAddWork(DataForWork dataForWork, int teacherId, string folderName)
        {
            this.dataForWork = dataForWork;
            this.teacherId = teacherId;
            this.typeOfWork = dataForWork.typeOfWork;
            this.folderName = folderName;
            GetThemeOfWork();
            CreateWorkForStudent();
            CreateWordDocuments word = new CreateWordDocuments(typeOfWork, folderName, works);
        }
        public DataForWork dataForWork { get; set; }
        public Works work { get; set; }
        public string themeOfWork { get; set; }
        public int teacherId { get; set; }
        public string typeOfWork { get; set; }
        public string folderName { get; set; }
        public List<Works> works { get; set; }

        private void CreateWorkAndAddBaseData()
        {
            work = new Works();
            work.teacherId = teacherId;
            work.complexity = dataForWork.complexity;
            work.dateOfWork = dataForWork.dateOfWork;
            work.subjectId = dataForWork.subjectId;
            work.themeOfWork = themeOfWork;
        }
        
        private void GetThemeOfWork()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                themeOfWork = db.Themes.Where(m => m.themeId == dataForWork.themeId).Select(m => m.nameOfTheme).Single();
            }
        }

        private void CreateWorkForStudent()
        {
            works = new List<Works>();
            string[] idStudents = dataForWork.students.Split(',');
            string[] typesAndCounts = dataForWork.typesAndCounts.Split('-');
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var allQuestions = from question in db.Questions
                                   where question.themeId == dataForWork.themeId && question.complexity == dataForWork.complexity
                                   select new
                                   {
                                       questionId = question.questionId,
                                       typeOfT = question.typeOfQuestion.nameOfType
                                   };
                List<int> oldQuestions = new List<int>();
                Random rnd = new Random();
                foreach (string idStudent in idStudents)
                {
                    if (idStudent != "")
                    {
                        CreateWorkAndAddBaseData();
                        work.studentId = Convert.ToInt32(idStudent);
                        work.questions = new List<Questions>();
                        List<int> idOfQuestions = new List<int>();
                        for (int i = 0; i < typesAndCounts.Count(); i++)
                        {
                            if (typesAndCounts[i] != "")
                            {
                                string nameOfT = typesAndCounts[i];
                                List<int> questionsWithType = allQuestions.Where(m => m.typeOfT == nameOfT).Select(m => m.questionId).ToList();
                                HashSet<int> questionsThatWere = new HashSet<int>();
                                for (int j = 0; j < Convert.ToInt32(typesAndCounts[i + 1]); j++)
                                {
                                n: int rndInt = rnd.Next(0, (questionsWithType.Count - 1));
                                    if (oldQuestions.Contains(rndInt))
                                    {
                                        if ((Convert.ToInt32(typesAndCounts[i + 1]) - questionsThatWere.Count) > 2)
                                        {
                                            goto n;
                                        }
                                        else
                                        {
                                            foreach (int quest in questionsThatWere)
                                            {
                                                oldQuestions.Remove(quest);
                                            }
                                            goto n;
                                        }

                                    }
                                    else
                                    {
                                        if (idOfQuestions.Contains(questionsWithType[rndInt]))
                                        {
                                            goto n;
                                        }
                                        else
                                        {
                                            idOfQuestions.Add(questionsWithType[rndInt]);
                                            oldQuestions.Add(questionsWithType[rndInt]);
                                            questionsThatWere.Add(questionsWithType[rndInt]);
                                        }
                                        
                                    }
                                }
                                i += 1;
                            }           
                        }
                        foreach (int idquestion in idOfQuestions)
                        {
                            work.questions.Add(db.Questions.Where(m => m.questionId == idquestion).Single());
                        }

                        db.Works.Add(work);
                        db.SaveChanges();
                        works.Add(work);
                    }
                }
            }
        }
    }
}