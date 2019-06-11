namespace PCHI.DataAccessLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QuestionnaireUserResponseGroup_StartTimeNullable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.QuestionnaireUserResponseGroup", "DatetimeCreated", c => c.DateTime(nullable: false));
            AlterColumn("dbo.QuestionnaireUserResponseGroup", "StartTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.QuestionnaireUserResponseGroup", "StartTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.QuestionnaireUserResponseGroup", "DatetimeCreated");
        }
    }
}
