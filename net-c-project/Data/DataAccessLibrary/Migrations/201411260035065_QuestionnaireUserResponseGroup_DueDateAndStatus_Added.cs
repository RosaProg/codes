namespace PCHI.DataAccessLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QuestionnaireUserResponseGroup_DueDateAndStatus_Added : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.QuestionnaireUserResponseGroup", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.QuestionnaireUserResponseGroup", "DueDate", c => c.DateTime());
            // Data migrations go here:
            this.Sql("update QuestionnaireUserResponseGroup set Status = 2 where Completed = 1 and (Status = 0 or status = 1)");
            
        }
        
        public override void Down()
        {
            DropColumn("dbo.QuestionnaireUserResponseGroup", "DueDate");
            DropColumn("dbo.QuestionnaireUserResponseGroup", "Status");
        }
    }
}
