using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace edusync.Models;

public partial class EduSyncDbContext : DbContext
{
    public EduSyncDbContext()
    {
    }

    public EduSyncDbContext(DbContextOptions<EduSyncDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Assessment> Assessments { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Result> Results { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Assessment>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__Assessme__3213E83FA29318BB");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.created_at).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.updated_at).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__Courses__3213E83FA61986A5");

            entity.Property(e => e.id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.created_at).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.updated_at).HasDefaultValueSql("(getdate())");
        });

            

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4CC2586F00");

            entity.Property(e => e.UserId).HasDefaultValueSql("(newid())");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
