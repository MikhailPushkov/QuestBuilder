namespace QuestBuild_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddComplexity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Questions", "complexity", c => c.Int(nullable: false));
            AddColumn("dbo.Works", "complexity", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Works", "complexity");
            DropColumn("dbo.Questions", "complexity");
        }
    }
}
