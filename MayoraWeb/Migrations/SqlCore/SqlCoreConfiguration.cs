namespace MyWeb.Migrations.SqlCore
{
    using System.Data.Entity.Migrations;

    internal sealed class SqlCoreConfiguration : DbMigrationsConfiguration<MyWeb.DataLayer.SqlCore.SqlCoreDbContext>
    {
        public SqlCoreConfiguration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\SqlCore";
        }

        protected override void Seed(MyWeb.DataLayer.SqlCore.SqlCoreDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
