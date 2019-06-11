namespace PCHI.DataAccessLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SpecialDateFields_Removed : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.QuestionnaireUserResponseGroup", "StartDate");
            DropColumn("dbo.QuestionnaireUserResponseGroup", "DateCompleted");
            DropColumn("dbo.QuestionnaireUserResponseGroup", "DateCreated");
        }
        
        public override void Down()
        {
            AddColumn("dbo.QuestionnaireUserResponseGroup", "DateCreated", c => c.DateTime(nullable: false));
            AddColumn("dbo.QuestionnaireUserResponseGroup", "DateCompleted", c => c.DateTime());
            AddColumn("dbo.QuestionnaireUserResponseGroup", "StartDate", c => c.DateTime());
        }
    }
}
