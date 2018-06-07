namespace CTCServer.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CTCServer.Models.UserContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            //AutomaticMigrationDataLossAllowed = true;
            ContextKey = "CTCServer.Models.UserContext";
        }

        protected override void Seed(CTCServer.Models.UserContext context)
        {
        }
    }
}
