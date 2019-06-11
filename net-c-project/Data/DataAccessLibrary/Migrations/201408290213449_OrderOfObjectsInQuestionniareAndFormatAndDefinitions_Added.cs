namespace PCHI.DataAccessLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderOfObjectsInQuestionniareAndFormatAndDefinitions_Added : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ItemGroupOptionsForEachOptionDefinition", "OrderInItemGroupOptionsFormatDefinition", c => c.Int(nullable: false));
            AddColumn("dbo.FormatContainer", "OrderInParent", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.FormatContainer", "OrderInParent");
            DropColumn("dbo.ItemGroupOptionsForEachOptionDefinition", "OrderInItemGroupOptionsFormatDefinition");
        }
    }
}
