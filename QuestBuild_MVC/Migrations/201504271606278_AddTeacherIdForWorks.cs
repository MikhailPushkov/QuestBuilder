namespace QuestBuild_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTeacherIdForWorks : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Works", "teacherId", c => c.Int(nullable: false));
            CreateIndex("dbo.Works", "teacherId");
            AddForeignKey("dbo.Works", "teacherId", "dbo.Teachers", "teacherId", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Works", "teacherId", "dbo.Teachers");
            DropIndex("dbo.Works", new[] { "teacherId" });
            DropColumn("dbo.Works", "teacherId");
        }
    }
}
