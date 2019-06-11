namespace PCHI.DataAccessLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tmp : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.ItemGroupFormat", name: "ItemGroupOptionsFormatDefinition_GroupOptionDefinitionName", newName: "ItemGroupOptionsFormatDefinition_Name");
            RenameColumn(table: "dbo.FormatContainer", name: "ContainerFormatDefinition_ContainerDefinitionName", newName: "ContainerFormatDefinition_Name");
            RenameColumn(table: "dbo.FormatContainer", name: "ItemFormatDefinition_ElementFormatDefinitionName", newName: "ItemFormatDefinition_Name");
            RenameColumn(table: "dbo.FormatContainer", name: "TextFormatDefinition_ElementFormatDefinitionName", newName: "TextFormatDefinition_Name");
            RenameIndex(table: "dbo.ItemGroupFormat", name: "IX_ItemGroupOptionsFormatDefinition_GroupOptionDefinitionName", newName: "IX_ItemGroupOptionsFormatDefinition_Name");
            RenameIndex(table: "dbo.FormatContainer", name: "IX_ContainerFormatDefinition_ContainerDefinitionName", newName: "IX_ContainerFormatDefinition_Name");
            RenameIndex(table: "dbo.FormatContainer", name: "IX_ItemFormatDefinition_ElementFormatDefinitionName", newName: "IX_ItemFormatDefinition_Name");
            RenameIndex(table: "dbo.FormatContainer", name: "IX_TextFormatDefinition_ElementFormatDefinitionName", newName: "IX_TextFormatDefinition_Name");
            CreateTable(
                "dbo.QuestionnaireTag",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        QuestionnaireName = c.String(nullable: false),
                        Tag_TagName = c.String(nullable: false, maxLength: 50),
                        Tag_Value = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tag", t => new { t.Tag_TagName, t.Tag_Value }, cascadeDelete: true)
                .Index(t => new { t.Tag_TagName, t.Tag_Value });
            
            CreateTable(
                "dbo.Tag",
                c => new
                    {
                        TagName = c.String(nullable: false, maxLength: 50),
                        Value = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => new { t.TagName, t.Value });
            
            AddColumn("dbo.ItemGroupOptionsForEachOptionDefinition", "StartText", c => c.String());
            AddColumn("dbo.ItemGroupOptionsForEachOptionDefinition", "ItemOptionDisplayType", c => c.Int(nullable: false));
            AddColumn("dbo.ItemGroupOptionsForEachOptionDefinition", "EndText", c => c.String());
            AddColumn("dbo.ItemGroupFormat", "ResponseType", c => c.Int(nullable: false));
            AddColumn("dbo.QuestionnaireElement", "SummaryText", c => c.String(maxLength: 30));
            DropColumn("dbo.ItemGroupOptionsForEachOptionDefinition", "ForEachOptionTextDefinition");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ItemGroupOptionsForEachOptionDefinition", "ForEachOptionTextDefinition", c => c.String());
            DropForeignKey("dbo.QuestionnaireTag", new[] { "Tag_TagName", "Tag_Value" }, "dbo.Tag");
            DropIndex("dbo.QuestionnaireTag", new[] { "Tag_TagName", "Tag_Value" });
            DropColumn("dbo.QuestionnaireElement", "SummaryText");
            DropColumn("dbo.ItemGroupFormat", "ResponseType");
            DropColumn("dbo.ItemGroupOptionsForEachOptionDefinition", "EndText");
            DropColumn("dbo.ItemGroupOptionsForEachOptionDefinition", "ItemOptionDisplayType");
            DropColumn("dbo.ItemGroupOptionsForEachOptionDefinition", "StartText");
            DropTable("dbo.Tag");
            DropTable("dbo.QuestionnaireTag");
            RenameIndex(table: "dbo.FormatContainer", name: "IX_TextFormatDefinition_Name", newName: "IX_TextFormatDefinition_ElementFormatDefinitionName");
            RenameIndex(table: "dbo.FormatContainer", name: "IX_ItemFormatDefinition_Name", newName: "IX_ItemFormatDefinition_ElementFormatDefinitionName");
            RenameIndex(table: "dbo.FormatContainer", name: "IX_ContainerFormatDefinition_Name", newName: "IX_ContainerFormatDefinition_ContainerDefinitionName");
            RenameIndex(table: "dbo.ItemGroupFormat", name: "IX_ItemGroupOptionsFormatDefinition_Name", newName: "IX_ItemGroupOptionsFormatDefinition_GroupOptionDefinitionName");
            RenameColumn(table: "dbo.FormatContainer", name: "TextFormatDefinition_Name", newName: "TextFormatDefinition_ElementFormatDefinitionName");
            RenameColumn(table: "dbo.FormatContainer", name: "ItemFormatDefinition_Name", newName: "ItemFormatDefinition_ElementFormatDefinitionName");
            RenameColumn(table: "dbo.FormatContainer", name: "ContainerFormatDefinition_Name", newName: "ContainerFormatDefinition_ContainerDefinitionName");
            RenameColumn(table: "dbo.ItemGroupFormat", name: "ItemGroupOptionsFormatDefinition_Name", newName: "ItemGroupOptionsFormatDefinition_GroupOptionDefinitionName");
        }
    }
}
