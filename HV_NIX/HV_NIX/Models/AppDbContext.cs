using System.Data.Entity;

namespace HV_NIX.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("ShopPAEntities")
        {
            // TỰ ĐỘNG DROP + TẠO LẠI DB nếu Model thay đổi
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<AppDbContext>());
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<UserInfo> UserInfos { get; set; }

        // Nếu bạn KHÔNG có model Cart và CartItems thì XÓA 2 Dòng Này!
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItems> CartItems { get; set; }

        public DbSet<Orders> Orders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }

        public DbSet<Products> Products { get; set; }

        // Đúng model viết hoa: Category
        public DbSet<Category> Categories { get; set; }

        // Đúng model PasswordReset
        public DbSet<PasswordReset> PasswordResets { get; set; }

        // Lịch sử đơn hàng (nếu có)
        public DbSet<OrderHistory> OrderHistories { get; set; }
    }
}
