namespace PCHI.DataAccessLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AttributesAddedTo_Item_Format : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Episode", "ExternalId", c => c.String());
            AddColumn("dbo.QuestionnaireElement", "Attributes", c => c.Int());
            AddColumn("dbo.Format", "Attributes", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Format", "Attributes");
            DropColumn("dbo.QuestionnaireElement", "Attributes");
            DropColumn("dbo.Episode", "ExternalId");
        }
    }
}
