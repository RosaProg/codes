namespace PCHI.DataAccessLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequiredAttributesAddedWhereNeeded : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.QuestionnaireSectionInstruction", "Section_Id", "dbo.QuestionnaireSection");
            DropForeignKey("dbo.QuestionnaireItemInstruction", "Item_Id", "dbo.QuestionnaireElement");
            DropForeignKey("dbo.QuestionnaireItemOptionGroup", "Item_Id", "dbo.QuestionnaireElement");
            DropForeignKey("dbo.QuestionnaireItemOption", "Group_Id", "dbo.QuestionnaireItemOptionGroup");
            DropForeignKey("dbo.QuestionnaireResponse", "QuestionnaireUserResponseGroup_Id", "dbo.QuestionnaireUserResponseGroup");
            DropForeignKey("dbo.QuestionnaireUserResponseGroup", "Questionnaire_Id", "dbo.Questionnaire");
            DropIndex("dbo.QuestionnaireItemInstruction", new[] { "Item_Id" });
            DropIndex("dbo.QuestionnaireItemOptionGroup", new[] { "Item_Id" });
            DropIndex("dbo.QuestionnaireItemOption", new[] { "Group_Id" });
            DropIndex("dbo.QuestionnaireSectionInstruction", new[] { "Section_Id" });
            DropIndex("dbo.QuestionnaireResponse", new[] { "QuestionnaireUserResponseGroup_Id" });
            DropIndex("dbo.QuestionnaireUserResponseGroup", new[] { "Questionnaire_Id" });
            RenameColumn(table: "dbo.QuestionnaireSection", name: "Instrument_Id", newName: "Questionnaire_Id");
            RenameIndex(table: "dbo.QuestionnaireSection", name: "IX_Instrument_Id", newName: "IX_Questionnaire_Id");
            AlterColumn("dbo.QuestionnaireItemInstruction", "Item_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.QuestionnaireItemOptionGroup", "Item_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.QuestionnaireItemOption", "Group_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.QuestionnaireSectionInstruction", "Section_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.QuestionnaireResponse", "QuestionnaireUserResponseGroup_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.QuestionnaireUserResponseGroup", "Questionnaire_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.QuestionnaireItemInstruction", "Item_Id");
            CreateIndex("dbo.QuestionnaireItemOptionGroup", "Item_Id");
            CreateIndex("dbo.QuestionnaireItemOption", "Group_Id");
            CreateIndex("dbo.QuestionnaireSectionInstruction", "Section_Id");
            CreateIndex("dbo.QuestionnaireResponse", "QuestionnaireUserResponseGroup_Id");
            CreateIndex("dbo.QuestionnaireUserResponseGroup", "Questionnaire_Id");
            AddForeignKey("dbo.QuestionnaireSectionInstruction", "Section_Id", "dbo.QuestionnaireSection", "Id", cascadeDelete: true);
            AddForeignKey("dbo.QuestionnaireItemInstruction", "Item_Id", "dbo.QuestionnaireElement", "Id", cascadeDelete: true);
            AddForeignKey("dbo.QuestionnaireItemOptionGroup", "Item_Id", "dbo.QuestionnaireElement", "Id", cascadeDelete: true);
            AddForeignKey("dbo.QuestionnaireItemOption", "Group_Id", "dbo.QuestionnaireItemOptionGroup", "Id", cascadeDelete: true);
            AddForeignKey("dbo.QuestionnaireResponse", "QuestionnaireUserResponseGroup_Id", "dbo.QuestionnaireUserResponseGroup", "Id", cascadeDelete: true);
            AddForeignKey("dbo.QuestionnaireUserResponseGroup", "Questionnaire_Id", "dbo.Questionnaire", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.QuestionnaireUserResponseGroup", "Questionnaire_Id", "dbo.Questionnaire");
            DropForeignKey("dbo.QuestionnaireResponse", "QuestionnaireUserResponseGroup_Id", "dbo.QuestionnaireUserResponseGroup");
            DropForeignKey("dbo.QuestionnaireItemOption", "Group_Id", "dbo.QuestionnaireItemOptionGroup");
            DropForeignKey("dbo.QuestionnaireItemOptionGroup", "Item_Id", "dbo.QuestionnaireElement");
            DropForeignKey("dbo.QuestionnaireItemInstruction", "Item_Id", "dbo.QuestionnaireElement");
            DropForeignKey("dbo.QuestionnaireSectionInstruction", "Section_Id", "dbo.QuestionnaireSection");
            DropIndex("dbo.QuestionnaireUserResponseGroup", new[] { "Questionnaire_Id" });
            DropIndex("dbo.QuestionnaireResponse", new[] { "QuestionnaireUserResponseGroup_Id" });
            DropIndex("dbo.QuestionnaireSectionInstruction", new[] { "Section_Id" });
            DropIndex("dbo.QuestionnaireItemOption", new[] { "Group_Id" });
            DropIndex("dbo.QuestionnaireItemOptionGroup", new[] { "Item_Id" });
            DropIndex("dbo.QuestionnaireItemInstruction", new[] { "Item_Id" });
            AlterColumn("dbo.QuestionnaireUserResponseGroup", "Questionnaire_Id", c => c.Int());
            AlterColumn("dbo.QuestionnaireResponse", "QuestionnaireUserResponseGroup_Id", c => c.Int());
            AlterColumn("dbo.QuestionnaireSectionInstruction", "Section_Id", c => c.Int());
            AlterColumn("dbo.QuestionnaireItemOption", "Group_Id", c => c.Int());
            AlterColumn("dbo.QuestionnaireItemOptionGroup", "Item_Id", c => c.Int());
            AlterColumn("dbo.QuestionnaireItemInstruction", "Item_Id", c => c.Int());
            RenameIndex(table: "dbo.QuestionnaireSection", name: "IX_Questionnaire_Id", newName: "IX_Instrument_Id");
            RenameColumn(table: "dbo.QuestionnaireSection", name: "Questionnaire_Id", newName: "Instrument_Id");
            CreateIndex("dbo.QuestionnaireUserResponseGroup", "Questionnaire_Id");
            CreateIndex("dbo.QuestionnaireResponse", "QuestionnaireUserResponseGroup_Id");
            CreateIndex("dbo.QuestionnaireSectionInstruction", "Section_Id");
            CreateIndex("dbo.QuestionnaireItemOption", "Group_Id");
            CreateIndex("dbo.QuestionnaireItemOptionGroup", "Item_Id");
            CreateIndex("dbo.QuestionnaireItemInstruction", "Item_Id");
            AddForeignKey("dbo.QuestionnaireUserResponseGroup", "Questionnaire_Id", "dbo.Questionnaire", "Id");
            AddForeignKey("dbo.QuestionnaireResponse", "QuestionnaireUserResponseGroup_Id", "dbo.QuestionnaireUserResponseGroup", "Id");
            AddForeignKey("dbo.QuestionnaireItemOption", "Group_Id", "dbo.QuestionnaireItemOptionGroup", "Id");
            AddForeignKey("dbo.QuestionnaireItemOptionGroup", "Item_Id", "dbo.QuestionnaireElement", "Id");
            AddForeignKey("dbo.QuestionnaireItemInstruction", "Item_Id", "dbo.QuestionnaireElement", "Id");
            AddForeignKey("dbo.QuestionnaireSectionInstruction", "Section_Id", "dbo.QuestionnaireSection", "Id");
        }
    }
}
