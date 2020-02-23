using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace lab1
{
    public partial class FoodDelivery_v2Context : DbContext
    {
        public FoodDelivery_v2Context()
        {
        }

        public FoodDelivery_v2Context(DbContextOptions<FoodDelivery_v2Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<ClientCard> ClientCard { get; set; }
        public virtual DbSet<Dish> Dish { get; set; }
        public virtual DbSet<DishIngredient> DishIngredient { get; set; }
        public virtual DbSet<Ingredient> Ingredient { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<OrderDish> OrderDish { get; set; }
        public virtual DbSet<Restaurant> Restaurant { get; set; }
        public virtual DbSet<RestaurantLocation> RestaurantLocation { get; set; }
        public virtual DbSet<Type> Type { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=DESKTOP-7HR422F\\SQLEXPRESS; Database=FoodDelivery_v2; Trusted_Connection=True; ");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>(entity =>
            {
                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(e => e.Post)
                    .IsRequired()
                    .HasMaxLength(15);
            });

            modelBuilder.Entity<ClientCard>(entity =>
            {
                entity.Property(e => e.ClientCard1).HasColumnName("ClientCard");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.ClientCard)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ClientCard_Client");
            });

            modelBuilder.Entity<Dish>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(e => e.Recipe).HasColumnType("text");

                entity.HasOne(d => d.Restaurant)
                    .WithMany(p => p.Dish)
                    .HasForeignKey(d => d.RestaurantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Dish_Restaurant");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Dish)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Dish_Type");
            });

            modelBuilder.Entity<DishIngredient>(entity =>
            {
                entity.HasOne(d => d.Dish)
                    .WithMany(p => p.DishIngredient)
                    .HasForeignKey(d => d.DishId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DishIngredient_Dish");

                entity.HasOne(d => d.Ingredient)
                    .WithMany(p => p.DishIngredient)
                    .HasForeignKey(d => d.IngredientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DishIngredient_Ingredient");
            });

            modelBuilder.Entity<Ingredient>(entity =>
            {
                entity.Property(e => e.Info).HasColumnType("text");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(15);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.Time).HasColumnType("datetime");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.Order)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_Client");

                entity.HasOne(d => d.RestaurantLocation)
                    .WithMany(p => p.Order)
                    .HasForeignKey(d => d.RestaurantLocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_RestaurantLocation");
            });

            modelBuilder.Entity<OrderDish>(entity =>
            {
                entity.HasOne(d => d.Dish)
                    .WithMany(p => p.OrderDish)
                    .HasForeignKey(d => d.DishId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderDish_Dish");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDish)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderDish_Order");
            });

            modelBuilder.Entity<Restaurant>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(15);
            });

            modelBuilder.Entity<RestaurantLocation>(entity =>
            {
                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsFixedLength();

                entity.HasOne(d => d.Restaurant)
                    .WithMany(p => p.RestaurantLocation)
                    .HasForeignKey(d => d.RestaurantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RestaurantLocation_Restaurant");
            });

            modelBuilder.Entity<Type>(entity =>
            {
                entity.Property(e => e.Info).HasColumnType("text");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(15);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
