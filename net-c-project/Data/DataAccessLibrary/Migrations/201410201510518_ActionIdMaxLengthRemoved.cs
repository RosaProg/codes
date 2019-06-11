namespace PCHI.DataAccessLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ActionIdMaxLengthRemoved : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.QuestionnaireItemOption", "ActionId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.QuestionnaireItemOption", "ActionId", c => c.String(maxLength: 50));
        }
    }
}
