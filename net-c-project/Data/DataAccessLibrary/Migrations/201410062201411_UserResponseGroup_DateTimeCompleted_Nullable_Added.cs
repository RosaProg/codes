namespace PCHI.DataAccessLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserResponseGroup_DateTimeCompleted_Nullable_Added : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.QuestionnaireUserResponseGroup", "DateTimeCompleted", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.QuestionnaireUserResponseGroup", "DateTimeCompleted", c => c.DateTime(nullable: false));
        }
    }
}
