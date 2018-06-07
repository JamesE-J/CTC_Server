namespace CTCServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedZones : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Users");
            CreateTable(
                "dbo.Zones",
                c => new
                    {
                        ZoneKey = c.Guid(nullable: false),
                        ZoneNumber = c.String(),
                        ZoneDominance = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ZoneKey);
            
            AddColumn("dbo.Users", "UserKey", c => c.Guid(nullable: false));
            AddColumn("dbo.Users", "Team", c => c.String());
            AddColumn("dbo.Users", "Score", c => c.Int(nullable: false));
            AddColumn("dbo.Users", "Zone", c => c.String());
            AddPrimaryKey("dbo.Users", "UserKey");
            DropColumn("dbo.Users", "ApiKey");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "ApiKey", c => c.Guid(nullable: false));
            DropPrimaryKey("dbo.Users");
            DropColumn("dbo.Users", "Zone");
            DropColumn("dbo.Users", "Score");
            DropColumn("dbo.Users", "Team");
            DropColumn("dbo.Users", "UserKey");
            DropTable("dbo.Zones");
            AddPrimaryKey("dbo.Users", "ApiKey");
        }
    }
}
