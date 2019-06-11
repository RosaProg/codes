namespace PCHI.DataAccessLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserResponseGroup_DateTimeCompleted_Added : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.QuestionnaireUserResponseGroup", "DateTimeCompleted", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.QuestionnaireUserResponseGroup", "DateTimeCompleted");
        }
    }
}
