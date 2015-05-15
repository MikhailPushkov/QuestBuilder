namespace QuestBuild_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewTeacher : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Teachers", "userId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Teachers", "userId");
        }
    }
}
