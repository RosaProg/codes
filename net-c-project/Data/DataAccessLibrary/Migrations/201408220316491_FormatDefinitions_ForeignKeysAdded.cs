namespace PCHI.DataAccessLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FormatDefinitions_ForeignKeysAdded : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.ElementFormatDefinition", name: "ContainerFormatDefinition_ContainerDefinitionName", newName: "ContainerDefinitionName");
            RenameColumn(table: "dbo.ItemGroupOptionsFormatDefinition", name: "ItemFormatDefinition_ElementFormatDefinitionName", newName: "ItemFormatDefinitionName");
            RenameColumn(table: "dbo.ItemGroupOptionsForEachOptionDefinition", name: "ItemGroupOptionsFormatDefinition_GroupOptionDefinitionName", newName: "GroupOptionDefinitionName");
            RenameIndex(table: "dbo.ElementFormatDefinition", name: "IX_ContainerFormatDefinition_ContainerDefinitionName", newName: "IX_ContainerDefinitionName");
            RenameIndex(table: "dbo.ItemGroupOptionsFormatDefinition", name: "IX_ItemFormatDefinition_ElementFormatDefinitionName", newName: "IX_ItemFormatDefinitionName");
            RenameIndex(table: "dbo.ItemGroupOptionsForEachOptionDefinition", name: "IX_ItemGroupOptionsFormatDefinition_GroupOptionDefinitionName", newName: "IX_GroupOptionDefinitionName");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.ItemGroupOptionsForEachOptionDefinition", name: "IX_GroupOptionDefinitionName", newName: "IX_ItemGroupOptionsFormatDefinition_GroupOptionDefinitionName");
            RenameIndex(table: "dbo.ItemGroupOptionsFormatDefinition", name: "IX_ItemFormatDefinitionName", newName: "IX_ItemFormatDefinition_ElementFormatDefinitionName");
            RenameIndex(table: "dbo.ElementFormatDefinition", name: "IX_ContainerDefinitionName", newName: "IX_ContainerFormatDefinition_ContainerDefinitionName");
            RenameColumn(table: "dbo.ItemGroupOptionsForEachOptionDefinition", name: "GroupOptionDefinitionName", newName: "ItemGroupOptionsFormatDefinition_GroupOptionDefinitionName");
            RenameColumn(table: "dbo.ItemGroupOptionsFormatDefinition", name: "ItemFormatDefinitionName", newName: "ItemFormatDefinition_ElementFormatDefinitionName");
            RenameColumn(table: "dbo.ElementFormatDefinition", name: "ContainerDefinitionName", newName: "ContainerFormatDefinition_ContainerDefinitionName");
        }
    }
}
