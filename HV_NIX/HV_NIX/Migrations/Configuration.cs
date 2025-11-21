namespace HV_NIX.Migrations
{
    using System.Data.Entity.Migrations;
    using HV_NIX.Models;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<HV_NIX.Models.AppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(HV_NIX.Models.AppDbContext context)
        {
            // SEED CATEGORY DATA (CHẠY 1 LẦN DUY NHẤT)
            if (!context.Categories.Any())
            {
                context.Categories.AddOrUpdate(
                    c => c.CategoryName,
                    new Category { CategoryName = "Áo Thun" },
                    new Category { CategoryName = "Áo Khoác" },
                    new Category { CategoryName = "Quần Dài" }
                );
            }

            context.SaveChanges();
        }
    }
}
