namespace PCHI.DataAccessLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DefaultValuesAdded_ToOptionGroup_AndOptions : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.QuestionnaireItemOptionGroup", "DefaultValue", c => c.String());
            AddColumn("dbo.QuestionnaireItemOption", "DefaultValue", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.QuestionnaireItemOption", "DefaultValue");
            DropColumn("dbo.QuestionnaireItemOptionGroup", "DefaultValue");
        }
    }
}
