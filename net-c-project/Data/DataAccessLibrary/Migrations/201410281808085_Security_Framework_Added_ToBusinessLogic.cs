namespace PCHI.DataAccessLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Security_Framework_Added_ToBusinessLogic : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Episode", "User_Id", "dbo.User");
            DropForeignKey("dbo.QuestionnaireUserResponseGroup", "User_Id", "dbo.User");
            DropIndex("dbo.Episode", new[] { "User_Id" });
            DropIndex("dbo.QuestionnaireUserResponseGroup", new[] { "User_Id" });
            CreateTable(
                "dbo.Patient",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Title = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        ExternalId = c.String(maxLength: 128),
                        DateOfBirth = c.DateTime(storeType: "date"),
                        Email = c.String(),
                        PhoneNumber = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.ExternalId);
            
            CreateTable(
                "dbo.ProxyUserPatientMap",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Patient_Id = c.String(maxLength: 128),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Patient", t => t.Patient_Id)
                .ForeignKey("dbo.User", t => t.User_Id)
                .Index(t => t.Patient_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.SessionDetails",
                c => new
                    {
                        SessionId = c.String(nullable: false, maxLength: 128),
                        Role = c.String(),
                        LastAccess = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.SessionId);
            
            AddColumn("dbo.Episode", "Patient_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.QuestionnaireUserResponseGroup", "Patient_Id", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Episode", "Patient_Id");
            CreateIndex("dbo.QuestionnaireUserResponseGroup", "Patient_Id");
            AddForeignKey("dbo.Episode", "Patient_Id", "dbo.Patient", "Id");
            AddForeignKey("dbo.QuestionnaireUserResponseGroup", "Patient_Id", "dbo.Patient", "Id", cascadeDelete: true);
            DropColumn("dbo.Episode", "User_Id");
            DropColumn("dbo.User", "DateOfBirth");
            DropColumn("dbo.User", "IsPatient");
            DropColumn("dbo.QuestionnaireUserResponseGroup", "User_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.QuestionnaireUserResponseGroup", "User_Id", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.User", "IsPatient", c => c.Boolean(nullable: false));
            AddColumn("dbo.User", "DateOfBirth", c => c.DateTime(storeType: "date"));
            AddColumn("dbo.Episode", "User_Id", c => c.String(maxLength: 128));
            DropForeignKey("dbo.QuestionnaireUserResponseGroup", "Patient_Id", "dbo.Patient");
            DropForeignKey("dbo.Episode", "Patient_Id", "dbo.Patient");
            DropForeignKey("dbo.ProxyUserPatientMap", "User_Id", "dbo.User");
            DropForeignKey("dbo.ProxyUserPatientMap", "Patient_Id", "dbo.Patient");
            DropIndex("dbo.QuestionnaireUserResponseGroup", new[] { "Patient_Id" });
            DropIndex("dbo.ProxyUserPatientMap", new[] { "User_Id" });
            DropIndex("dbo.ProxyUserPatientMap", new[] { "Patient_Id" });
            DropIndex("dbo.Patient", new[] { "ExternalId" });
            DropIndex("dbo.Episode", new[] { "Patient_Id" });
            DropColumn("dbo.QuestionnaireUserResponseGroup", "Patient_Id");
            DropColumn("dbo.Episode", "Patient_Id");
            DropTable("dbo.SessionDetails");
            DropTable("dbo.ProxyUserPatientMap");
            DropTable("dbo.Patient");
            CreateIndex("dbo.QuestionnaireUserResponseGroup", "User_Id");
            CreateIndex("dbo.Episode", "User_Id");
            AddForeignKey("dbo.QuestionnaireUserResponseGroup", "User_Id", "dbo.User", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Episode", "User_Id", "dbo.User", "Id");
        }
    }
}
