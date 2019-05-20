using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace fns.Models.DB
{
    public partial class fnsContext : DbContext
    {
        public static string ConnectionString { get; set; }
        public fnsContext()
        {
        }

        public fnsContext(DbContextOptions<fnsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Admin> Admin { get; set; }
        public virtual DbSet<Banner> Banner { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Comment> Comment { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<Post> Post { get; set; }
        public virtual DbSet<Postcomment> Postcomment { get; set; }
        public virtual DbSet<Postcommentreply> Postcommentreply { get; set; }
        public virtual DbSet<Splash> Splash { get; set; }
        public virtual DbSet<Updateinfo> Updateinfo { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySQL(ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.0-rtm-35687");

            modelBuilder.Entity<Admin>(entity =>
            {
                entity.ToTable("admin", "fns");

                entity.HasIndex(e => e.Id)
                    .HasName("PK_Admin")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(10)");

                entity.Property(e => e.InsDt).HasColumnName("insDT");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("int(10)");
            });

            modelBuilder.Entity<Banner>(entity =>
            {
                entity.ToTable("banner", "fns");

                entity.HasIndex(e => e.Cid)
                    .HasName("FK_BannerToCategory");

                entity.HasIndex(e => e.Id)
                    .HasName("PK_Banner")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(10)");

                entity.Property(e => e.Cid)
                    .HasColumnName("cid")
                    .HasColumnType("int(10)");

                entity.Property(e => e.LinkUrl)
                    .HasColumnName("linkUrl")
                    .IsUnicode(false);

                entity.Property(e => e.PicUrl)
                    .IsRequired()
                    .HasColumnName("picUrl")
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasColumnType("int(10)");

                entity.HasOne(d => d.C)
                    .WithMany(p => p.Banner)
                    .HasForeignKey(d => d.Cid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BannerToCategory");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("category", "fns");

                entity.HasIndex(e => e.Id)
                    .HasName("PK_Category")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(10)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("comment", "fns");

                entity.HasIndex(e => e.NId)
                    .HasName("FK_CommentToNews");

                entity.HasIndex(e => e.UId)
                    .HasName("FK_CommentToUser");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasColumnName("content")
                    .IsUnicode(false);

                entity.Property(e => e.InsDt).HasColumnName("insDT");

                entity.Property(e => e.NId)
                    .HasColumnName("nId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UId)
                    .HasColumnName("uId")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.N)
                    .WithMany(p => p.Comment)
                    .HasForeignKey(d => d.NId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CommentToNews");

                entity.HasOne(d => d.U)
                    .WithMany(p => p.Comment)
                    .HasForeignKey(d => d.UId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CommentToUser");
            });

            modelBuilder.Entity<News>(entity =>
            {
                entity.ToTable("news", "fns");

                entity.HasIndex(e => e.Cid)
                    .HasName("FK_NewsToCategory");

                entity.HasIndex(e => e.Id)
                    .HasName("PK_News")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(10)");

                entity.Property(e => e.Auth)
                    .HasColumnName("auth")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Cid)
                    .HasColumnName("cid")
                    .HasColumnType("int(10)");

                entity.Property(e => e.CommentCount)
                    .HasColumnName("commentCount")
                    .HasColumnType("int(10)");

                entity.Property(e => e.Content)
                    .HasColumnName("content")
                    .IsUnicode(false);

                entity.Property(e => e.DoRef)
                    .HasColumnName("doRef")
                    .IsUnicode(false);

                entity.Property(e => e.FocusCount)
                    .HasColumnName("focusCount")
                    .HasColumnType("int(10)");

                entity.Property(e => e.InsDt).HasColumnName("insDT");

                entity.Property(e => e.PicUrlList)
                    .HasColumnName("picUrlList")
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("int(10)");

                entity.Property(e => e.Tag)
                    .HasColumnName("tag")
                    .HasColumnType("int(10)");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasColumnType("int(10)");

                entity.Property(e => e.UpCount)
                    .HasColumnName("upCount")
                    .HasColumnType("int(10)");

                entity.Property(e => e.ViewCount)
                    .HasColumnName("viewCount")
                    .HasColumnType("int(10)");

                entity.HasOne(d => d.C)
                    .WithMany(p => p.News)
                    .HasForeignKey(d => d.Cid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NewsToCategory");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.ToTable("post", "fns");

                entity.HasIndex(e => e.Id)
                    .HasName("PK_Post")
                    .IsUnique();

                entity.HasIndex(e => e.Uid)
                    .HasName("FK_PostToUser");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CommentCount)
                    .HasColumnName("commentCount")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Content)
                    .HasColumnName("content")
                    .IsUnicode(false);

                entity.Property(e => e.InsDt).HasColumnName("insDT");

                entity.Property(e => e.PicUrlList)
                    .HasColumnName("picUrlList")
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Uid)
                    .HasColumnName("uid")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UpCount)
                    .HasColumnName("upCount")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ViewCount)
                    .HasColumnName("viewCount")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.U)
                    .WithMany(p => p.Post)
                    .HasForeignKey(d => d.Uid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PostToUser");
            });

            modelBuilder.Entity<Postcomment>(entity =>
            {
                entity.ToTable("postcomment", "fns");

                entity.HasIndex(e => e.Id)
                    .HasName("PK_PostComment")
                    .IsUnique();

                entity.HasIndex(e => e.Pid)
                    .HasName("FK_PostCommentToPost");

                entity.HasIndex(e => e.Uid)
                    .HasName("FK_PostCommentToUser");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(10)");

                entity.Property(e => e.Content)
                    .HasColumnName("content")
                    .IsUnicode(false);

                entity.Property(e => e.InsDt).HasColumnName("insDT");

                entity.Property(e => e.Pid)
                    .HasColumnName("pid")
                    .HasColumnType("int(10)");

                entity.Property(e => e.ReplyCount)
                    .HasColumnName("replyCount")
                    .HasColumnType("int(10)");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("int(10)");

                entity.Property(e => e.Uid)
                    .HasColumnName("uid")
                    .HasColumnType("int(10)");

                entity.HasOne(d => d.P)
                    .WithMany(p => p.Postcomment)
                    .HasForeignKey(d => d.Pid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PostCommentToPost");

                entity.HasOne(d => d.U)
                    .WithMany(p => p.Postcomment)
                    .HasForeignKey(d => d.Uid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PostCommentToUser");
            });

            modelBuilder.Entity<Postcommentreply>(entity =>
            {
                entity.ToTable("postcommentreply", "fns");

                entity.HasIndex(e => e.Id)
                    .HasName("PK_PostCommentReply")
                    .IsUnique();

                entity.HasIndex(e => e.Pcid)
                    .HasName("FK_PostCommentReplyToPostComment");

                entity.HasIndex(e => e.Uid)
                    .HasName("FK_PostCommentReplyToUser");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(10)");

                entity.Property(e => e.Content)
                    .HasColumnName("content")
                    .IsUnicode(false);

                entity.Property(e => e.InsDt).HasColumnName("insDT");

                entity.Property(e => e.Pcid)
                    .HasColumnName("pcid")
                    .HasColumnType("int(10)");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("int(10)");

                entity.Property(e => e.Uid)
                    .HasColumnName("uid")
                    .HasColumnType("int(10)");

                entity.Property(e => e.UpCount)
                    .HasColumnName("upCount")
                    .HasColumnType("int(10)");

                entity.HasOne(d => d.Pc)
                    .WithMany(p => p.Postcommentreply)
                    .HasForeignKey(d => d.Pcid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PostCommentReplyToPostComment");

                entity.HasOne(d => d.U)
                    .WithMany(p => p.Postcommentreply)
                    .HasForeignKey(d => d.Uid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PostCommentReplyToUser");
            });

            modelBuilder.Entity<Splash>(entity =>
            {
                entity.ToTable("splash", "fns");

                entity.HasIndex(e => e.Id)
                    .HasName("PK_Splash")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(10)");

                entity.Property(e => e.Duration)
                    .HasColumnName("duration")
                    .HasColumnType("int(10)");

                entity.Property(e => e.PicUrl)
                    .IsRequired()
                    .HasColumnName("picUrl")
                    .IsUnicode(false);

                entity.Property(e => e.RedirectUrl)
                    .HasColumnName("redirectUrl")
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Updateinfo>(entity =>
            {
                entity.ToTable("updateinfo", "fns");

                entity.HasIndex(e => e.Id)
                    .HasName("PK_UpdateInfo")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(10)");

                entity.Property(e => e.InsDt).HasColumnName("insDT");

                entity.Property(e => e.MinVer)
                    .HasColumnName("minVer")
                    .HasColumnType("int(10)");

                entity.Property(e => e.NewVer)
                    .HasColumnName("newVer")
                    .HasColumnType("int(10)");

                entity.Property(e => e.UpdateDesc)
                    .IsRequired()
                    .HasColumnName("updateDesc")
                    .IsUnicode(false);

                entity.Property(e => e.UpdateUrl)
                    .IsRequired()
                    .HasColumnName("updateUrl")
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user", "fns");

                entity.HasIndex(e => e.Id)
                    .HasName("PK_User")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(10)");

                entity.Property(e => e.Avatar)
                    .HasColumnName("avatar")
                    .IsUnicode(false);

                entity.Property(e => e.Birthday).HasColumnName("birthday");

                entity.Property(e => e.Categories)
                    .HasColumnName("categories")
                    .IsUnicode(false);

                entity.Property(e => e.Collections)
                    .HasColumnName("collections")
                    .IsUnicode(false);

                entity.Property(e => e.Gender)
                    .HasColumnName("gender")
                    .HasColumnType("int(10)");

                entity.Property(e => e.InsDt).HasColumnName("insDT");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("int(10)");
            });
        }
    }
}
