namespace PCHI.DataAccessLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderingOfOptions_Added : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.QuestionnaireItemOption", "OrderInGroup", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.QuestionnaireItemOption", "OrderInGroup");
        }
    }
}
