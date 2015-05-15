namespace QuestBuild_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LittleChange : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Questions", "typeOfQuestion_typeOfQuestionsId", "dbo.TypeOfQuestions");
            DropIndex("dbo.Questions", new[] { "typeOfQuestion_typeOfQuestionsId" });
            RenameColumn(table: "dbo.Questions", name: "typeOfQuestion_typeOfQuestionsId", newName: "typeOfQuestionsId");
            AlterColumn("dbo.Questions", "typeOfQuestionsId", c => c.Int(nullable: false));
            CreateIndex("dbo.Questions", "typeOfQuestionsId");
            AddForeignKey("dbo.Questions", "typeOfQuestionsId", "dbo.TypeOfQuestions", "typeOfQuestionsId", cascadeDelete: true);
            DropColumn("dbo.Questions", "typeOfQuestionId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Questions", "typeOfQuestionId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Questions", "typeOfQuestionsId", "dbo.TypeOfQuestions");
            DropIndex("dbo.Questions", new[] { "typeOfQuestionsId" });
            AlterColumn("dbo.Questions", "typeOfQuestionsId", c => c.Int());
            RenameColumn(table: "dbo.Questions", name: "typeOfQuestionsId", newName: "typeOfQuestion_typeOfQuestionsId");
            CreateIndex("dbo.Questions", "typeOfQuestion_typeOfQuestionsId");
            AddForeignKey("dbo.Questions", "typeOfQuestion_typeOfQuestionsId", "dbo.TypeOfQuestions", "typeOfQuestionsId");
        }
    }
}
