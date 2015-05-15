namespace QuestBuild_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRightWork : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Works",
                c => new
                    {
                        workId = c.Int(nullable: false, identity: true),
                        dateOfWork = c.DateTime(nullable: false),
                        themeOfWork = c.String(),
                        rating = c.String(),
                        studentId = c.Int(nullable: false),
                        subjectId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.workId)
                .ForeignKey("dbo.Students", t => t.studentId, cascadeDelete: true)
                .ForeignKey("dbo.Subjects", t => t.subjectId, cascadeDelete: true)
                .Index(t => t.studentId)
                .Index(t => t.subjectId);
            
            CreateTable(
                "dbo.WorksQuestions",
                c => new
                    {
                        Works_workId = c.Int(nullable: false),
                        Questions_questionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Works_workId, t.Questions_questionId })
                .ForeignKey("dbo.Works", t => t.Works_workId, cascadeDelete: false)
                .ForeignKey("dbo.Questions", t => t.Questions_questionId, cascadeDelete: false)
                .Index(t => t.Works_workId)
                .Index(t => t.Questions_questionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Works", "subjectId", "dbo.Subjects");
            DropForeignKey("dbo.Works", "studentId", "dbo.Students");
            DropForeignKey("dbo.WorksQuestions", "Questions_questionId", "dbo.Questions");
            DropForeignKey("dbo.WorksQuestions", "Works_workId", "dbo.Works");
            DropIndex("dbo.WorksQuestions", new[] { "Questions_questionId" });
            DropIndex("dbo.WorksQuestions", new[] { "Works_workId" });
            DropIndex("dbo.Works", new[] { "subjectId" });
            DropIndex("dbo.Works", new[] { "studentId" });
            DropTable("dbo.WorksQuestions");
            DropTable("dbo.Works");
        }
    }
}
