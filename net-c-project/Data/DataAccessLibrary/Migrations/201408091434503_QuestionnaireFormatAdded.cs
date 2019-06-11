namespace PCHI.DataAccessLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QuestionnaireFormatAdded : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.QuestionnaireResponse", "Questionnaire_Id", "dbo.Questionnaire");
            DropIndex("dbo.QuestionnaireResponse", new[] { "Questionnaire_Id" });
            CreateTable(
                "dbo.ContainerFormatDefinition",
                c => new
                    {
                        ContainerDefinitionName = c.String(nullable: false, maxLength: 250),
                        StartHtml = c.String(),
                        EndHtml = c.String(),
                        StartEndRepeat = c.Int(nullable: false),
                        ParentContainer_ContainerDefinitionName = c.String(maxLength: 250),
                    })
                .PrimaryKey(t => t.ContainerDefinitionName)
                .ForeignKey("dbo.ContainerFormatDefinition", t => t.ParentContainer_ContainerDefinitionName)
                .Index(t => t.ParentContainer_ContainerDefinitionName);
            
            CreateTable(
                "dbo.ElementFormatDefinition",
                c => new
                    {
                        ElementFormatDefinitionName = c.String(nullable: false, maxLength: 250),
                        StartHtml = c.String(),
                        Html = c.String(),
                        EndHtml = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        ContainerFormatDefinition_ContainerDefinitionName = c.String(maxLength: 250),
                    })
                .PrimaryKey(t => t.ElementFormatDefinitionName)
                .ForeignKey("dbo.ContainerFormatDefinition", t => t.ContainerFormatDefinition_ContainerDefinitionName)
                .Index(t => t.ContainerFormatDefinition_ContainerDefinitionName);
            
            CreateTable(
                "dbo.ItemGroupOptionsFormatDefinition",
                c => new
                    {
                        GroupOptionDefinitionName = c.String(nullable: false, maxLength: 250),
                        StartHtml = c.String(),
                        EndHtml = c.String(),
                        ForEachOptionStart = c.String(),
                        ForEachOptionEnd = c.String(),
                        AnchorDisplay = c.Int(nullable: false),
                        ItemFormatDefinition_ElementFormatDefinitionName = c.String(maxLength: 250),
                    })
                .PrimaryKey(t => t.GroupOptionDefinitionName)
                .ForeignKey("dbo.ElementFormatDefinition", t => t.ItemFormatDefinition_ElementFormatDefinitionName)
                .Index(t => t.ItemFormatDefinition_ElementFormatDefinitionName);
            
            CreateTable(
                "dbo.ItemGroupOptionsForEachOptionDefinition",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ForEachOptionTextDefinition = c.String(),
                        ItemGroupOptionsFormatDefinition_GroupOptionDefinitionName = c.String(nullable: false, maxLength: 250),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ItemGroupOptionsFormatDefinition", t => t.ItemGroupOptionsFormatDefinition_GroupOptionDefinitionName, cascadeDelete: true)
                .Index(t => t.ItemGroupOptionsFormatDefinition_GroupOptionDefinitionName);
            
            CreateTable(
                "dbo.ItemGroupFormat",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ItemFormatContainer_Id = c.Int(nullable: false),
                        ItemGroupOptionsFormatDefinition_GroupOptionDefinitionName = c.String(maxLength: 250),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FormatContainer", t => t.ItemFormatContainer_Id, cascadeDelete: true)
                .ForeignKey("dbo.ItemGroupOptionsFormatDefinition", t => t.ItemGroupOptionsFormatDefinition_GroupOptionDefinitionName)
                .Index(t => t.ItemFormatContainer_Id)
                .Index(t => t.ItemGroupOptionsFormatDefinition_GroupOptionDefinitionName);
            
            CreateTable(
                "dbo.FormatContainer",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        Parent_Id = c.Int(),
                        ContainerFormatDefinition_ContainerDefinitionName = c.String(maxLength: 250),
                        QuestionnaireFormat_Name = c.String(maxLength: 50),
                        TextFormatDefinition_ElementFormatDefinitionName = c.String(maxLength: 250),
                        ItemFormatDefinition_ElementFormatDefinitionName = c.String(maxLength: 250),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FormatContainer", t => t.Parent_Id)
                .ForeignKey("dbo.ContainerFormatDefinition", t => t.ContainerFormatDefinition_ContainerDefinitionName)
                .ForeignKey("dbo.Format", t => t.QuestionnaireFormat_Name)
                .ForeignKey("dbo.ElementFormatDefinition", t => t.TextFormatDefinition_ElementFormatDefinitionName)
                .ForeignKey("dbo.ElementFormatDefinition", t => t.ItemFormatDefinition_ElementFormatDefinitionName)
                .Index(t => t.Parent_Id)
                .Index(t => t.ContainerFormatDefinition_ContainerDefinitionName)
                .Index(t => t.QuestionnaireFormat_Name)
                .Index(t => t.TextFormatDefinition_ElementFormatDefinitionName)
                .Index(t => t.ItemFormatDefinition_ElementFormatDefinitionName);
            
            CreateTable(
                "dbo.FormatContainerElement",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        QuestionnaireElementActionId = c.String(),
                        OrderInSection = c.Int(nullable: false),
                        FormatContainer_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FormatContainer", t => t.FormatContainer_Id, cascadeDelete: true)
                .Index(t => t.FormatContainer_Id);
            
            CreateTable(
                "dbo.Format",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Name);
            
            CreateTable(
                "dbo.QuestionnaireUserResponseGroup",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        StartTime = c.DateTime(nullable: false),
                        Completed = c.Boolean(nullable: false),
                        Questionnaire_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Questionnaire", t => t.Questionnaire_Id)
                .Index(t => t.Questionnaire_Id);
            
            AddColumn("dbo.QuestionnaireResponse", "QuestionnaireUserResponseGroup_Id", c => c.Int());
            CreateIndex("dbo.QuestionnaireResponse", "QuestionnaireUserResponseGroup_Id");
            AddForeignKey("dbo.QuestionnaireResponse", "QuestionnaireUserResponseGroup_Id", "dbo.QuestionnaireUserResponseGroup", "Id");
            DropColumn("dbo.QuestionnaireResponse", "UserId");
            DropColumn("dbo.QuestionnaireResponse", "Questionnaire_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.QuestionnaireResponse", "Questionnaire_Id", c => c.Int());
            AddColumn("dbo.QuestionnaireResponse", "UserId", c => c.Int(nullable: false));
            DropForeignKey("dbo.QuestionnaireResponse", "QuestionnaireUserResponseGroup_Id", "dbo.QuestionnaireUserResponseGroup");
            DropForeignKey("dbo.QuestionnaireUserResponseGroup", "Questionnaire_Id", "dbo.Questionnaire");
            DropForeignKey("dbo.ItemGroupFormat", "ItemGroupOptionsFormatDefinition_GroupOptionDefinitionName", "dbo.ItemGroupOptionsFormatDefinition");
            DropForeignKey("dbo.ItemGroupFormat", "ItemFormatContainer_Id", "dbo.FormatContainer");
            DropForeignKey("dbo.FormatContainer", "ItemFormatDefinition_ElementFormatDefinitionName", "dbo.ElementFormatDefinition");
            DropForeignKey("dbo.FormatContainer", "TextFormatDefinition_ElementFormatDefinitionName", "dbo.ElementFormatDefinition");
            DropForeignKey("dbo.FormatContainer", "QuestionnaireFormat_Name", "dbo.Format");
            DropForeignKey("dbo.FormatContainerElement", "FormatContainer_Id", "dbo.FormatContainer");
            DropForeignKey("dbo.FormatContainer", "ContainerFormatDefinition_ContainerDefinitionName", "dbo.ContainerFormatDefinition");
            DropForeignKey("dbo.FormatContainer", "Parent_Id", "dbo.FormatContainer");
            DropForeignKey("dbo.ElementFormatDefinition", "ContainerFormatDefinition_ContainerDefinitionName", "dbo.ContainerFormatDefinition");
            DropForeignKey("dbo.ItemGroupOptionsFormatDefinition", "ItemFormatDefinition_ElementFormatDefinitionName", "dbo.ElementFormatDefinition");
            DropForeignKey("dbo.ItemGroupOptionsForEachOptionDefinition", "ItemGroupOptionsFormatDefinition_GroupOptionDefinitionName", "dbo.ItemGroupOptionsFormatDefinition");
            DropForeignKey("dbo.ContainerFormatDefinition", "ParentContainer_ContainerDefinitionName", "dbo.ContainerFormatDefinition");
            DropIndex("dbo.QuestionnaireUserResponseGroup", new[] { "Questionnaire_Id" });
            DropIndex("dbo.QuestionnaireResponse", new[] { "QuestionnaireUserResponseGroup_Id" });
            DropIndex("dbo.FormatContainerElement", new[] { "FormatContainer_Id" });
            DropIndex("dbo.FormatContainer", new[] { "ItemFormatDefinition_ElementFormatDefinitionName" });
            DropIndex("dbo.FormatContainer", new[] { "TextFormatDefinition_ElementFormatDefinitionName" });
            DropIndex("dbo.FormatContainer", new[] { "QuestionnaireFormat_Name" });
            DropIndex("dbo.FormatContainer", new[] { "ContainerFormatDefinition_ContainerDefinitionName" });
            DropIndex("dbo.FormatContainer", new[] { "Parent_Id" });
            DropIndex("dbo.ItemGroupFormat", new[] { "ItemGroupOptionsFormatDefinition_GroupOptionDefinitionName" });
            DropIndex("dbo.ItemGroupFormat", new[] { "ItemFormatContainer_Id" });
            DropIndex("dbo.ItemGroupOptionsForEachOptionDefinition", new[] { "ItemGroupOptionsFormatDefinition_GroupOptionDefinitionName" });
            DropIndex("dbo.ItemGroupOptionsFormatDefinition", new[] { "ItemFormatDefinition_ElementFormatDefinitionName" });
            DropIndex("dbo.ElementFormatDefinition", new[] { "ContainerFormatDefinition_ContainerDefinitionName" });
            DropIndex("dbo.ContainerFormatDefinition", new[] { "ParentContainer_ContainerDefinitionName" });
            DropColumn("dbo.QuestionnaireResponse", "QuestionnaireUserResponseGroup_Id");
            DropTable("dbo.QuestionnaireUserResponseGroup");
            DropTable("dbo.Format");
            DropTable("dbo.FormatContainerElement");
            DropTable("dbo.FormatContainer");
            DropTable("dbo.ItemGroupFormat");
            DropTable("dbo.ItemGroupOptionsForEachOptionDefinition");
            DropTable("dbo.ItemGroupOptionsFormatDefinition");
            DropTable("dbo.ElementFormatDefinition");
            DropTable("dbo.ContainerFormatDefinition");
            CreateIndex("dbo.QuestionnaireResponse", "Questionnaire_Id");
            AddForeignKey("dbo.QuestionnaireResponse", "Questionnaire_Id", "dbo.Questionnaire", "Id");
        }
    }
}
