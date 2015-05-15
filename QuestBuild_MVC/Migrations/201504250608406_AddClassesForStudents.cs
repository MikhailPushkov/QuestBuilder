namespace QuestBuild_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddClassesForStudents : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        groupId = c.Int(nullable: false, identity: true),
                        numberOfCours = c.Int(nullable: false),
                        numberOfGroup = c.Int(nullable: false),
                        instituteId = c.Int(nullable: false),
                        teacherId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.groupId)
                .ForeignKey("dbo.Institutes", t => t.instituteId, cascadeDelete: true)
                .ForeignKey("dbo.Teachers", t => t.teacherId, cascadeDelete: true)
                .Index(t => t.instituteId)
                .Index(t => t.teacherId);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        studentId = c.Int(nullable: false, identity: true),
                        firstNameOfStudent = c.String(),
                        secondNameOfStudent = c.String(),
                        patronymicOfStudent = c.String(),
                        groupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.studentId)
                .ForeignKey("dbo.Groups", t => t.groupId, cascadeDelete: true)
                .Index(t => t.groupId);
            
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
                .PrimaryKey(t => t.workId)
                .ForeignKey("dbo.Students", t => t.studentId, cascadeDelete: true)
                .Index(t => t.studentId);
            
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
            DropForeignKey("dbo.Groups", "teacherId", "dbo.Teachers");
            DropForeignKey("dbo.Works", "studentId", "dbo.Students");
            DropForeignKey("dbo.WorksQuestions", "Questions_questionId", "dbo.Questions");
            DropForeignKey("dbo.WorksQuestions", "Works_workId", "dbo.Works");
            DropForeignKey("dbo.Students", "groupId", "dbo.Groups");
            DropForeignKey("dbo.Groups", "instituteId", "dbo.Institutes");
            DropIndex("dbo.WorksQuestions", new[] { "Questions_questionId" });
            DropIndex("dbo.WorksQuestions", new[] { "Works_workId" });
            DropIndex("dbo.Works", new[] { "studentId" });
            DropIndex("dbo.Students", new[] { "groupId" });
            DropIndex("dbo.Groups", new[] { "teacherId" });
            DropIndex("dbo.Groups", new[] { "instituteId" });
            DropTable("dbo.WorksQuestions");
            DropTable("dbo.Works");
            DropTable("dbo.Students");
            DropTable("dbo.Groups");
        }
    }
}
