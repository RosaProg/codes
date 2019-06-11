namespace PCHI.DataAccessLibrary.Migrations
{
    using PCHI.Model.Security;
    using System;
    using System.Data.Entity.Migrations;
    using System.Text;

    public partial class AuditLog_ActionAndEventChangeToEnums : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AuditLog", "EventTypeName", c => c.String());
            AddColumn("dbo.AuditLog", "ActionName", c => c.String());
            Sql("Update AuditLog set EventTypeName = EventType, EventType = -1");
            Sql("Update AuditLog set ActionName = Action, Action = -1");
            AlterColumn("dbo.AuditLog", "EventType", c => c.Int(nullable: false));
            AlterColumn("dbo.AuditLog", "Action", c => c.Int(nullable: false));
            StringBuilder sql = new StringBuilder();

            foreach (var value in Enum.GetValues(typeof(AuditEventType)))
            {
                sql.Append("Update AuditLog set EventType = ").Append(value.GetHashCode()).Append(" where EventTypeName = '").Append(value.ToString()).AppendLine("'");
            }

            foreach (var value in Enum.GetValues(typeof(Actions)))
            {
                sql.Append("Update AuditLog set Action = ").Append(value.GetHashCode()).Append(" where ActionName = '").Append(value.ToString()).AppendLine("'");
            }

            Sql(sql.ToString());
        }

        public override void Down()
        {
            AlterColumn("dbo.AuditLog", "Action", c => c.String());
            AlterColumn("dbo.AuditLog", "EventType", c => c.String(maxLength: 250));

            Sql("Update AuditLog set EventType = EventTypeName");
            Sql("Update AuditLog set Action = ActionName");

            DropColumn("dbo.AuditLog", "ActionName");
            DropColumn("dbo.AuditLog", "EventTypeName");
        }
    }
}
