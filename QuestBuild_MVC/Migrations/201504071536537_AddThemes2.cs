namespace QuestBuild_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddThemes2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Themes",
                c => new
                    {
                        themeId = c.Int(nullable: false, identity: true),
                        nameOfTheme = c.String(),
                        parentTheme_themeId = c.Int(),
                        teacherId = c.Int(nullable: false),
                        subjectId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.themeId)
                .ForeignKey("dbo.Themes", t => t.parentTheme_themeId)
                .ForeignKey("dbo.Subjects", t => t.subjectId, cascadeDelete: false)
                .ForeignKey("dbo.Teachers", t => t.teacherId, cascadeDelete: false)
                .Index(t => t.teacherId)
                .Index(t => t.subjectId)
                .Index(t => t.themeId);
            
            AddColumn("dbo.Questions", "themeId", c => c.Int(nullable: false));
            CreateIndex("dbo.Questions", "themeId");
            AddForeignKey("dbo.Questions", "themeId", "dbo.Themes", "themeId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Themes", "teacherId", "dbo.Teachers");
            DropForeignKey("dbo.Themes", "subjectId", "dbo.Subjects");
            DropForeignKey("dbo.Questions", "themeId", "dbo.Themes");
            DropForeignKey("dbo.Themes", "parentTheme_themeId1", "dbo.Themes");
            DropIndex("dbo.Themes", new[] { "parentTheme_themeId1" });
            DropIndex("dbo.Themes", new[] { "subjectId" });
            DropIndex("dbo.Themes", new[] { "teacherId" });
            DropIndex("dbo.Questions", new[] { "themeId" });
            DropColumn("dbo.Questions", "themeId");
            DropTable("dbo.Themes");
        }
    }
}
