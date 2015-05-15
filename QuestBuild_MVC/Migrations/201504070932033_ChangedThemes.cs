namespace QuestBuild_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedThemes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Themes", "teacherId", c => c.Int(nullable: false));
            AddColumn("dbo.Themes", "subjectId", c => c.Int(nullable: false));
            CreateIndex("dbo.Themes", "teacherId");
            CreateIndex("dbo.Themes", "subjectId");
            AddForeignKey("dbo.Themes", "subjectId", "dbo.Subjects", "subjectId", cascadeDelete: false);
            AddForeignKey("dbo.Themes", "teacherId", "dbo.Teachers", "teacherId", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Themes", "teacherId", "dbo.Teachers");
            DropForeignKey("dbo.Themes", "subjectId", "dbo.Subjects");
            DropIndex("dbo.Themes", new[] { "subjectId" });
            DropIndex("dbo.Themes", new[] { "teacherId" });
            DropColumn("dbo.Themes", "subjectId");
            DropColumn("dbo.Themes", "teacherId");
        }
    }
}
