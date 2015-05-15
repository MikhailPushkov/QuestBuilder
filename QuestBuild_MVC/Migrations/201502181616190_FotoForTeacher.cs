namespace QuestBuild_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FotoForTeacher : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Teachers", "foto", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Teachers", "foto");
        }
    }
}
