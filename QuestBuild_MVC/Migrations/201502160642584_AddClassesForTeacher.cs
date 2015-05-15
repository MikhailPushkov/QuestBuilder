namespace QuestBuild_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddClassesForTeacher : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Chairs",
                c => new
                    {
                        chairId = c.Int(nullable: false, identity: true),
                        nameOfChair = c.String(),
                        facultiId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.chairId)
                .ForeignKey("dbo.Faculties", t => t.facultiId, cascadeDelete: false)
                .Index(t => t.facultiId);
            
            CreateTable(
                "dbo.Faculties",
                c => new
                    {
                        facultiId = c.Int(nullable: false, identity: true),
                        nameOfFaculti = c.String(),
                        instituteId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.facultiId)
                .ForeignKey("dbo.Institutes", t => t.instituteId, cascadeDelete: false)
                .Index(t => t.instituteId);
            
            CreateTable(
                "dbo.Institutes",
                c => new
                    {
                        instituteId = c.Int(nullable: false, identity: true),
                        nameOfInstitute = c.String(),
                    })
                .PrimaryKey(t => t.instituteId);
            
            CreateTable(
                "dbo.Teachers",
                c => new
                    {
                        teacherId = c.Int(nullable: false, identity: true),
                        firstNameOfTeacher = c.String(),
                        secondNameOfTeacher = c.String(),
                        patronymicOfTeacher = c.String(),
                        chairId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.teacherId)
                .ForeignKey("dbo.Chairs", t => t.chairId, cascadeDelete: false)
                .Index(t => t.chairId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Teachers", "chairId", "dbo.Chairs");
            DropForeignKey("dbo.Faculties", "instituteId", "dbo.Institutes");
            DropForeignKey("dbo.Chairs", "facultiId", "dbo.Faculties");
            DropIndex("dbo.Teachers", new[] { "chairId" });
            DropIndex("dbo.Faculties", new[] { "instituteId" });
            DropIndex("dbo.Chairs", new[] { "facultiId" });
            DropTable("dbo.Teachers");
            DropTable("dbo.Institutes");
            DropTable("dbo.Faculties");
            DropTable("dbo.Chairs");
        }
    }
}
