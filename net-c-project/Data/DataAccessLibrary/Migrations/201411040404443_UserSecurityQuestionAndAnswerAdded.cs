namespace PCHI.DataAccessLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserSecurityQuestionAndAnswerAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "SecurityQuestion", c => c.String(maxLength: 500));
            AddColumn("dbo.User", "SecurityAnswer", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "SecurityAnswer");
            DropColumn("dbo.User", "SecurityQuestion");
        }
    }
}
