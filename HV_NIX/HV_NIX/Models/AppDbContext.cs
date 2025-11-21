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
        public DbSet<Reviews> Reviews { get; set; }
        public DbSet<ShippingAddress> ShippingAddresses { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }

        public DbSet<Notification> Notifications { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }

        // Lịch sử đơn hàng (nếu có)
        public DbSet<OrderHistory> OrderHistories { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reviews>()
                .HasRequired(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Reviews>()
                .HasRequired(r => r.Product)
                .WithMany()
                .HasForeignKey(r => r.ProductID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Reviews>()
                .HasRequired(r => r.Order)
                .WithMany()
                .HasForeignKey(r => r.OrderID)
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }

    }
}