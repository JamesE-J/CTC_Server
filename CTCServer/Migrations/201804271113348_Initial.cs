namespace CTCServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        ApiKey = c.Guid(nullable: false),
                        UserName = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.ApiKey);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Users");
        }
    }
}
