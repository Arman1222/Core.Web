namespace MyWeb.Migrations.SqlCore
{
    using System.Data.Entity.Migrations;

    internal sealed class SqlTemenosConfiguration : DbMigrationsConfiguration<MyWeb.DataLayer.SqlTemenos.SqlTemenosDbContext>
    {
        public SqlTemenosConfiguration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\SqlTemenos";
        }

        protected override void Seed(MyWeb.DataLayer.SqlTemenos.SqlTemenosDbContext context)
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
