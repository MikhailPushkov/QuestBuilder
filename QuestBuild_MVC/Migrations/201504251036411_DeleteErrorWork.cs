namespace QuestBuild_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteErrorWork : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.WorksQuestions", "Works_workId", "dbo.Works");
            DropForeignKey("dbo.WorksQuestions", "Questions_questionId", "dbo.Questions");
            DropForeignKey("dbo.Works", "studentId", "dbo.Students");
            DropForeignKey("dbo.WorksSubjects", "Works_workId", "dbo.Works");
            DropForeignKey("dbo.WorksSubjects", "Subjects_subjectId", "dbo.Subjects");
            DropIndex("dbo.Works", new[] { "studentId" });
            DropIndex("dbo.WorksQuestions", new[] { "Works_workId" });
            DropIndex("dbo.WorksQuestions", new[] { "Questions_questionId" });
            DropIndex("dbo.WorksSubjects", new[] { "Works_workId" });
            DropIndex("dbo.WorksSubjects", new[] { "Subjects_subjectId" });
            DropTable("dbo.Works");
            DropTable("dbo.WorksQuestions");
            DropTable("dbo.WorksSubjects");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.WorksSubjects",
                c => new
                    {
                        Works_workId = c.Int(nullable: false),
                        Subjects_subjectId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Works_workId, t.Subjects_subjectId });
            
            CreateTable(
                "dbo.WorksQuestions",
                c => new
                    {
                        Works_workId = c.Int(nullable: false),
                        Questions_questionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Works_workId, t.Questions_questionId });
            
            CreateTable(
                "dbo.Works",
                c => new
                    {
                        workId = c.Int(nullable: false, identity: true),
                        dateOfWork = c.DateTime(nullable: false),
                        themeOfWork = c.String(),
                        rating = c.String(),
                        studentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.workId);
            
            CreateIndex("dbo.WorksSubjects", "Subjects_subjectId");
            CreateIndex("dbo.WorksSubjects", "Works_workId");
            CreateIndex("dbo.WorksQuestions", "Questions_questionId");
            CreateIndex("dbo.WorksQuestions", "Works_workId");
            CreateIndex("dbo.Works", "studentId");
            AddForeignKey("dbo.WorksSubjects", "Subjects_subjectId", "dbo.Subjects", "subjectId", cascadeDelete: true);
            AddForeignKey("dbo.WorksSubjects", "Works_workId", "dbo.Works", "workId", cascadeDelete: true);
            AddForeignKey("dbo.Works", "studentId", "dbo.Students", "studentId", cascadeDelete: true);
            AddForeignKey("dbo.WorksQuestions", "Questions_questionId", "dbo.Questions", "questionId", cascadeDelete: true);
            AddForeignKey("dbo.WorksQuestions", "Works_workId", "dbo.Works", "workId", cascadeDelete: true);
        }
    }
}
