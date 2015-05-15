namespace QuestBuild_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteThemes : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Themes", "parentTheme_themeId", "dbo.Themes");
            DropForeignKey("dbo.Questions", "themeId", "dbo.Themes");
            DropForeignKey("dbo.Themes", "subjectId", "dbo.Subjects");
            DropForeignKey("dbo.Themes", "teacherId", "dbo.Teachers");
            DropIndex("dbo.Questions", new[] { "themeId" });
            DropIndex("dbo.Themes", new[] { "teacherId" });
            DropIndex("dbo.Themes", new[] { "subjectId" });
            DropIndex("dbo.Themes", new[] { "parentTheme_themeId" });
            DropColumn("dbo.Questions", "themeId");
            DropTable("dbo.Themes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Themes",
                c => new
                    {
                        themeId = c.Int(nullable: false, identity: true),
                        nameOfTheme = c.String(),
                        parentThemeId = c.Int(nullable: false),
                        teacherId = c.Int(nullable: false),
                        subjectId = c.Int(nullable: false),
                        parentTheme_themeId = c.Int(),
                    })
                .PrimaryKey(t => t.themeId);
            
            AddColumn("dbo.Questions", "themeId", c => c.Int(nullable: false));
            CreateIndex("dbo.Themes", "parentTheme_themeId");
            CreateIndex("dbo.Themes", "subjectId");
            CreateIndex("dbo.Themes", "teacherId");
            CreateIndex("dbo.Questions", "themeId");
            AddForeignKey("dbo.Themes", "teacherId", "dbo.Teachers", "teacherId", cascadeDelete: true);
            AddForeignKey("dbo.Themes", "subjectId", "dbo.Subjects", "subjectId", cascadeDelete: true);
            AddForeignKey("dbo.Questions", "themeId", "dbo.Themes", "themeId", cascadeDelete: true);
            AddForeignKey("dbo.Themes", "parentTheme_themeId", "dbo.Themes", "themeId");
        }
    }
}
