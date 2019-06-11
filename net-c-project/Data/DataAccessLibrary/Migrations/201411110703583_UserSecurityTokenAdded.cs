namespace PCHI.DataAccessLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserSecurityTokenAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserSecurityCode",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SecurityCodePurpose = c.String(),
                        EncryptedCode = c.String(),
                        ExpiresAt = c.DateTime(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.User_Id)
                .Index(t => t.User_Id);
            
            AddColumn("dbo.User", "RegistrationConfirmationToken", c => c.String());
            AddColumn("dbo.QuestionnaireElement", "ScoringNote", c => c.String());
            AddColumn("dbo.QuestionnaireElement", "HigherIsBetter", c => c.Boolean());
            AddColumn("dbo.ProDomain", "ScoringNote", c => c.String());
            AddColumn("dbo.ProDomain", "HigherIsBetter", c => c.Boolean(nullable: false));
            AddColumn("dbo.ProDomainResultRange", "Qualifier", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserSecurityCode", "User_Id", "dbo.User");
            DropIndex("dbo.UserSecurityCode", new[] { "User_Id" });
            DropColumn("dbo.ProDomainResultRange", "Qualifier");
            DropColumn("dbo.ProDomain", "HigherIsBetter");
            DropColumn("dbo.ProDomain", "ScoringNote");
            DropColumn("dbo.QuestionnaireElement", "HigherIsBetter");
            DropColumn("dbo.QuestionnaireElement", "ScoringNote");
            DropColumn("dbo.User", "RegistrationConfirmationToken");
            DropTable("dbo.UserSecurityCode");
        }
    }
}
