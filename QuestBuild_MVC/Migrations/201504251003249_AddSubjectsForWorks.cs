namespace QuestBuild_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSubjectsForWorks : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WorksSubjects",
                c => new
                    {
                        Works_workId = c.Int(nullable: false),
                        Subjects_subjectId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Works_workId, t.Subjects_subjectId })
                .ForeignKey("dbo.Works", t => t.Works_workId, cascadeDelete: false)
                .ForeignKey("dbo.Subjects", t => t.Subjects_subjectId, cascadeDelete: true)
                .Index(t => t.Works_workId)
                .Index(t => t.Subjects_subjectId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorksSubjects", "Subjects_subjectId", "dbo.Subjects");
            DropForeignKey("dbo.WorksSubjects", "Works_workId", "dbo.Works");
            DropIndex("dbo.WorksSubjects", new[] { "Subjects_subjectId" });
            DropIndex("dbo.WorksSubjects", new[] { "Works_workId" });
            DropTable("dbo.WorksSubjects");
        }
    }
}
