namespace QuestBuild_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSubject : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Subjects",
                c => new
                    {
                        subjectId = c.Int(nullable: false, identity: true),
                        nameOfSubject = c.String(),
                    })
                .PrimaryKey(t => t.subjectId);
            
            CreateTable(
                "dbo.SubjectsTeachers",
                c => new
                    {
                        Subjects_subjectId = c.Int(nullable: false),
                        Teachers_teacherId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Subjects_subjectId, t.Teachers_teacherId })
                .ForeignKey("dbo.Subjects", t => t.Subjects_subjectId, cascadeDelete: true)
                .ForeignKey("dbo.Teachers", t => t.Teachers_teacherId, cascadeDelete: true)
                .Index(t => t.Subjects_subjectId)
                .Index(t => t.Teachers_teacherId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SubjectsTeachers", "Teachers_teacherId", "dbo.Teachers");
            DropForeignKey("dbo.SubjectsTeachers", "Subjects_subjectId", "dbo.Subjects");
            DropIndex("dbo.SubjectsTeachers", new[] { "Teachers_teacherId" });
            DropIndex("dbo.SubjectsTeachers", new[] { "Subjects_subjectId" });
            DropTable("dbo.SubjectsTeachers");
            DropTable("dbo.Subjects");
        }
    }
}
