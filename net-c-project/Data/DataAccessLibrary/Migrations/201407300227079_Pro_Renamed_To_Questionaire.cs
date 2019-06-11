namespace PCHI.DataAccessLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pro_Renamed_To_Questionaire : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ProConcept", newName: "QuestionaireConcept");
            RenameTable(name: "dbo.ProInstrument", newName: "Questionaire");
            RenameTable(name: "dbo.ProSection", newName: "QuestionaireSection");
            RenameTable(name: "dbo.ProElement", newName: "QuestionaireElement");
            RenameTable(name: "dbo.ProItemInstruction", newName: "QuestionaireItemInstruction");
            RenameTable(name: "dbo.ProItemOptionGroup", newName: "QuestionaireItemOptionGroup");
            RenameTable(name: "dbo.ProItemOption", newName: "QuestionaireItemOption");
            RenameTable(name: "dbo.ProSectionInstruction", newName: "QuestionaireSectionInstruction");
            DropForeignKey("dbo.ProDomain", "Instrument_Id", "dbo.ProInstrument");
            AddColumn("dbo.ProDomain", "ProInstrument_Id", c => c.Int());
            AddColumn("dbo.Questionaire", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.ProDomain", "ProInstrument_Id");
            AddForeignKey("dbo.ProDomain", "ProInstrument_Id", "dbo.Questionaire", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProDomain", "ProInstrument_Id", "dbo.Questionaire");
            DropIndex("dbo.ProDomain", new[] { "ProInstrument_Id" });
            DropColumn("dbo.Questionaire", "Discriminator");
            DropColumn("dbo.ProDomain", "ProInstrument_Id");
            AddForeignKey("dbo.ProDomain", "Instrument_Id", "dbo.ProInstrument", "Id", cascadeDelete: true);
            RenameTable(name: "dbo.QuestionaireSectionInstruction", newName: "ProSectionInstruction");
            RenameTable(name: "dbo.QuestionaireItemOption", newName: "ProItemOption");
            RenameTable(name: "dbo.QuestionaireItemOptionGroup", newName: "ProItemOptionGroup");
            RenameTable(name: "dbo.QuestionaireItemInstruction", newName: "ProItemInstruction");
            RenameTable(name: "dbo.QuestionaireElement", newName: "ProElement");
            RenameTable(name: "dbo.QuestionaireSection", newName: "ProSection");
            RenameTable(name: "dbo.Questionaire", newName: "ProInstrument");
            RenameTable(name: "dbo.QuestionaireConcept", newName: "ProConcept");
        }
    }
}
