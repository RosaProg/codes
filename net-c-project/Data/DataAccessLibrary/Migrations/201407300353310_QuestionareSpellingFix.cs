namespace PCHI.DataAccessLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QuestionareSpellingFix : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Questionaire", newName: "Questionnaire");
            RenameTable(name: "dbo.QuestionaireConcept", newName: "QuestionnaireConcept");
            RenameTable(name: "dbo.QuestionaireSection", newName: "QuestionnaireSection");
            RenameTable(name: "dbo.QuestionaireElement", newName: "QuestionnaireElement");
            RenameTable(name: "dbo.QuestionaireItemInstruction", newName: "QuestionnaireItemInstruction");
            RenameTable(name: "dbo.QuestionaireItemOptionGroup", newName: "QuestionnaireItemOptionGroup");
            RenameTable(name: "dbo.QuestionaireItemOption", newName: "QuestionnaireItemOption");
            RenameTable(name: "dbo.QuestionaireSectionInstruction", newName: "QuestionnaireSectionInstruction");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.QuestionnaireSectionInstruction", newName: "QuestionaireSectionInstruction");
            RenameTable(name: "dbo.QuestionnaireItemOption", newName: "QuestionaireItemOption");
            RenameTable(name: "dbo.QuestionnaireItemOptionGroup", newName: "QuestionaireItemOptionGroup");
            RenameTable(name: "dbo.QuestionnaireItemInstruction", newName: "QuestionaireItemInstruction");
            RenameTable(name: "dbo.QuestionnaireElement", newName: "QuestionaireElement");
            RenameTable(name: "dbo.QuestionnaireSection", newName: "QuestionaireSection");
            RenameTable(name: "dbo.QuestionnaireConcept", newName: "QuestionaireConcept");
            RenameTable(name: "dbo.Questionnaire", newName: "Questionaire");
        }
    }
}
