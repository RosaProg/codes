namespace PCHI.DataAccessLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProConcept",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProDomainResultRange",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Start = c.Double(nullable: false),
                        End = c.Double(nullable: false),
                        Meaning = c.String(nullable: false),
                        Domain_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProDomain", t => t.Domain_Id, cascadeDelete: true)
                .Index(t => t.Domain_Id);
            
            CreateTable(
                "dbo.ProDomain",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Description = c.String(nullable: false),
                        ScoreFormula = c.String(),
                        Instrument_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProInstrument", t => t.Instrument_Id, cascadeDelete: true)
                .Index(t => t.Instrument_Id);
            
            CreateTable(
                "dbo.ProInstrument",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Status = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        Concept_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProConcept", t => t.Concept_Id, cascadeDelete: true)
                .Index(t => t.Concept_Id);
            
            CreateTable(
                "dbo.ProSection",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ActionId = c.String(),
                        OrderInInstrument = c.Int(nullable: false),
                        Instrument_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProInstrument", t => t.Instrument_Id, cascadeDelete: true)
                .Index(t => t.Instrument_Id);
            
            CreateTable(
                "dbo.ProElement",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        ActionId = c.String(),
                        OrderInSection = c.Int(nullable: false),
                        DisplayId = c.String(),
                        IsMandatory = c.Boolean(),
                        IsInProDefinition = c.Boolean(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        Section_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProSection", t => t.Section_Id, cascadeDelete: true)
                .Index(t => t.Section_Id);
            
            CreateTable(
                "dbo.ProItemInstruction",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        Item_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProElement", t => t.Item_Id)
                .Index(t => t.Item_Id);
            
            CreateTable(
                "dbo.ProItemOptionGroup",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        OrderInItem = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        RangeStep = c.Int(nullable: false),
                        Item_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProElement", t => t.Item_Id)
                .Index(t => t.Item_Id);
            
            CreateTable(
                "dbo.ProItemOption",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OptionIdText = c.String(),
                        Text = c.String(),
                        Value = c.Double(nullable: false),
                        Action = c.String(),
                        Group_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProItemOptionGroup", t => t.Group_Id)
                .Index(t => t.Group_Id);
            
            CreateTable(
                "dbo.ProSectionInstruction",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        Section_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProSection", t => t.Section_Id)
                .Index(t => t.Section_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProDomainResultRange", "Domain_Id", "dbo.ProDomain");
            DropForeignKey("dbo.ProDomain", "Instrument_Id", "dbo.ProInstrument");
            DropForeignKey("dbo.ProSection", "Instrument_Id", "dbo.ProInstrument");
            DropForeignKey("dbo.ProSectionInstruction", "Section_Id", "dbo.ProSection");
            DropForeignKey("dbo.ProElement", "Section_Id", "dbo.ProSection");
            DropForeignKey("dbo.ProItemOptionGroup", "Item_Id", "dbo.ProElement");
            DropForeignKey("dbo.ProItemOption", "Group_Id", "dbo.ProItemOptionGroup");
            DropForeignKey("dbo.ProItemInstruction", "Item_Id", "dbo.ProElement");
            DropForeignKey("dbo.ProInstrument", "Concept_Id", "dbo.ProConcept");
            DropIndex("dbo.ProSectionInstruction", new[] { "Section_Id" });
            DropIndex("dbo.ProItemOption", new[] { "Group_Id" });
            DropIndex("dbo.ProItemOptionGroup", new[] { "Item_Id" });
            DropIndex("dbo.ProItemInstruction", new[] { "Item_Id" });
            DropIndex("dbo.ProElement", new[] { "Section_Id" });
            DropIndex("dbo.ProSection", new[] { "Instrument_Id" });
            DropIndex("dbo.ProInstrument", new[] { "Concept_Id" });
            DropIndex("dbo.ProDomain", new[] { "Instrument_Id" });
            DropIndex("dbo.ProDomainResultRange", new[] { "Domain_Id" });
            DropTable("dbo.ProSectionInstruction");
            DropTable("dbo.ProItemOption");
            DropTable("dbo.ProItemOptionGroup");
            DropTable("dbo.ProItemInstruction");
            DropTable("dbo.ProElement");
            DropTable("dbo.ProSection");
            DropTable("dbo.ProInstrument");
            DropTable("dbo.ProDomain");
            DropTable("dbo.ProDomainResultRange");
            DropTable("dbo.ProConcept");
        }
    }
}
