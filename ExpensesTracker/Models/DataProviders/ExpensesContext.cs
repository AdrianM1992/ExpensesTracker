using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ExpensesTracker.Models.DataProviders;

public partial class ExpensesContext : DbContext
{
    public ExpensesContext()
    {
    }

    public ExpensesContext(DbContextOptions<ExpensesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Expense> Expenses { get; set; }

    public virtual DbSet<Recurring> Recurrings { get; set; }

    public virtual DbSet<Subcategory> Subcategories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(LocalDb)\\MSSQLLocalDB;Database=Expenses;Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.Property(e => e.Fixed).HasColumnName("Fixed?");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Expense>(entity =>
        {
            entity.Property(e => e.CategoryId).HasDefaultValueSql("((1))");
            entity.Property(e => e.Date).HasColumnType("date");
            entity.Property(e => e.DateOfEntry).HasColumnType("date");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Files).HasMaxLength(500);
            entity.Property(e => e.Income).HasColumnName("Income?");
            entity.Property(e => e.LastUpdate).HasColumnType("date");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Price).HasColumnType("money");
            entity.Property(e => e.Recurring).HasColumnName("Recurring?");
            entity.Property(e => e.RecurringId).HasColumnName("RecurringID");
            entity.Property(e => e.Subcategory).HasMaxLength(50);
            entity.Property(e => e.Total)
                .HasComputedColumnSql("([Price]*[Quantity])", false)
                .HasColumnType("money");
        });

        modelBuilder.Entity<Recurring>(entity =>
        {
            entity.ToTable("Recurring");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Subcategory>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
