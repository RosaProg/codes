namespace PCHI.DataAccessLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ItemOptionActionIdAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.QuestionnaireItemOption", "ActionId", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.QuestionnaireItemOption", "ActionId");
        }
    }
}
