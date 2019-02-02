using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace fns.Models.DB
{
    public partial class FinancialNewsContext : DbContext
    {
        public static string ConnectionString { get; set; }
        public FinancialNewsContext()
        {
        }

        public FinancialNewsContext(DbContextOptions<FinancialNewsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Admin> Admin { get; set; }
        public virtual DbSet<Banner> Banner { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<Splash> Splash { get; set; }
        public virtual DbSet<UpdateInfo> UpdateInfo { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConnectionString, b => b.UseRowNumberForPaging());//使用老版本SQL分页
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.0-rtm-35687");

            modelBuilder.Entity<Admin>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.InsDt)
                    .HasColumnName("insDT")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(50);

                entity.Property(e => e.Status).HasColumnName("status");
            });

            modelBuilder.Entity<Banner>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.LinkUrl).HasColumnName("linkUrl");

                entity.Property(e => e.PicUrl)
                    .IsRequired()
                    .HasColumnName("picUrl");

                entity.Property(e => e.Type).HasColumnName("type");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<News>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Auth)
                    .HasColumnName("auth")
                    .HasMaxLength(100);

                entity.Property(e => e.Cid).HasColumnName("cid");

                entity.Property(e => e.CommentCount).HasColumnName("commentCount");

                entity.Property(e => e.Content).HasColumnName("content");

                entity.Property(e => e.DoRef).HasColumnName("doRef");

                entity.Property(e => e.FocusCount).HasColumnName("focusCount");

                entity.Property(e => e.InsDt)
                    .HasColumnName("insDT")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.PicUrlList).HasColumnName("picUrlList");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Tag).HasColumnName("tag");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasMaxLength(500);

                entity.Property(e => e.UpCount).HasColumnName("upCount");

                entity.Property(e => e.ViewCount).HasColumnName("viewCount");

                entity.HasOne(d => d.C)
                    .WithMany(p => p.News)
                    .HasForeignKey(d => d.Cid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NewsToCategory");
            });

            modelBuilder.Entity<Splash>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Duration).HasColumnName("duration");

                entity.Property(e => e.PicUrl)
                    .IsRequired()
                    .HasColumnName("picUrl");

                entity.Property(e => e.RedirectUrl).HasColumnName("redirectUrl");
            });

            modelBuilder.Entity<UpdateInfo>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.InsDt)
                    .HasColumnName("insDT")
                    .HasColumnType("datetime");

                entity.Property(e => e.MinVer)
                    .IsRequired()
                    .HasColumnName("minVer")
                    .HasMaxLength(50);

                entity.Property(e => e.NewVer)
                    .IsRequired()
                    .HasColumnName("newVer")
                    .HasMaxLength(50);

                entity.Property(e => e.UpdateDesc)
                    .IsRequired()
                    .HasColumnName("updateDesc")
                    .HasMaxLength(1000);

                entity.Property(e => e.UpdateUrl)
                    .IsRequired()
                    .HasColumnName("updateUrl")
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Avatar).HasColumnName("avatar");

                entity.Property(e => e.Birthday)
                    .HasColumnName("birthday")
                    .HasColumnType("datetime");

                entity.Property(e => e.Gender).HasColumnName("gender");

                entity.Property(e => e.InsDt)
                    .HasColumnName("insDT")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(50);

                entity.Property(e => e.Status).HasColumnName("status");
            });
        }
    }
}
