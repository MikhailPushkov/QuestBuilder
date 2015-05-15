namespace QuestBuild_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddClassesForQuestions : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.SubjectsTeachers", newName: "TeachersSubjects");
            DropPrimaryKey("dbo.TeachersSubjects");
            CreateTable(
                "dbo.Answers",
                c => new
                    {
                        answerId = c.Int(nullable: false, identity: true),
                        truth = c.Boolean(nullable: false),
                        textOfAnswer = c.Binary(),
                        questionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.answerId)
                .ForeignKey("dbo.Questions", t => t.questionId, cascadeDelete: true)
                .Index(t => t.questionId);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        questionId = c.Int(nullable: false, identity: true),
                        nameOfQuestion = c.String(),
                        textOfQuestion = c.Binary(),
                        teacherId = c.Int(nullable: false),
                        typeOfQuestionId = c.Int(),
                        subjectId = c.Int(nullable: false),
                        typeOfQuestion_typeOfQuestionsId = c.Int(),
                    })
                .PrimaryKey(t => t.questionId)
                .ForeignKey("dbo.Subjects", t => t.subjectId, cascadeDelete: true)
                .ForeignKey("dbo.Teachers", t => t.teacherId, cascadeDelete: true)
                .ForeignKey("dbo.TypeOfQuestions", t => t.typeOfQuestion_typeOfQuestionsId)
                .Index(t => t.teacherId)
                .Index(t => t.subjectId)
                .Index(t => t.typeOfQuestion_typeOfQuestionsId);
            
            CreateTable(
                "dbo.TypeOfQuestions",
                c => new
                    {
                        typeOfQuestionsId = c.Int(nullable: false, identity: true),
                        nameOfType = c.String(),
                    })
                .PrimaryKey(t => t.typeOfQuestionsId);
            
            AddPrimaryKey("dbo.TeachersSubjects", new[] { "Teachers_teacherId", "Subjects_subjectId" });
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Questions", "typeOfQuestion_typeOfQuestionsId", "dbo.TypeOfQuestions");
            DropForeignKey("dbo.Questions", "teacherId", "dbo.Teachers");
            DropForeignKey("dbo.Questions", "subjectId", "dbo.Subjects");
            DropForeignKey("dbo.Answers", "questionId", "dbo.Questions");
            DropIndex("dbo.Questions", new[] { "typeOfQuestion_typeOfQuestionsId" });
            DropIndex("dbo.Questions", new[] { "subjectId" });
            DropIndex("dbo.Questions", new[] { "teacherId" });
            DropIndex("dbo.Answers", new[] { "questionId" });
            DropPrimaryKey("dbo.TeachersSubjects");
            DropTable("dbo.TypeOfQuestions");
            DropTable("dbo.Questions");
            DropTable("dbo.Answers");
            AddPrimaryKey("dbo.TeachersSubjects", new[] { "Subjects_subjectId", "Teachers_teacherId" });
            RenameTable(name: "dbo.TeachersSubjects", newName: "SubjectsTeachers");
        }
    }
}
