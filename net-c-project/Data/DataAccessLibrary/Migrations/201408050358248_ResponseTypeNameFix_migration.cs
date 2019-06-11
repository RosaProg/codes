namespace PCHI.DataAccessLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ResponseTypeNameFix : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.QuestionnaireItemOptionGroup", "ResponseType", c => c.Int(nullable: false));
            DropColumn("dbo.QuestionnaireItemOptionGroup", "Type");
        }
        
        public override void Down()
        {
            AddColumn("dbo.QuestionnaireItemOptionGroup", "Type", c => c.Int(nullable: false));
            DropColumn("dbo.QuestionnaireItemOptionGroup", "ResponseType");
        }
    }
}
