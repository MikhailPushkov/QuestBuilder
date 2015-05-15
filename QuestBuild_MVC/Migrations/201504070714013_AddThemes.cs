namespace QuestBuild_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddThemes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Themes",
                c => new
                    {
                        themeId = c.Int(nullable: false, identity: true),
                        nameOfTheme = c.String(),
                        parentThemeId = c.Int(nullable: true)
                    })
                .PrimaryKey(t => t.themeId)
                .ForeignKey("dbo.Themes", t => t.parentThemeId)
                .Index(t => t.themeId);
            
            AddColumn("dbo.Questions", "themeId", c => c.Int(nullable: false));
            CreateIndex("dbo.Questions", "themeId");
            AddForeignKey("dbo.Questions", "themeId", "dbo.Themes", "themeId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Questions", "themeId", "dbo.Themes");
            DropForeignKey("dbo.Themes", "parentTheme_themeId", "dbo.Themes");
            DropIndex("dbo.Themes", new[] { "parentTheme_themeId" });
            DropIndex("dbo.Questions", new[] { "themeId" });
            DropColumn("dbo.Questions", "themeId");
            DropTable("dbo.Themes");
        }
    }
}
