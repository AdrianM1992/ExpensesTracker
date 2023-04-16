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
      entity.Property(e => e.Quantity).HasColumnType("money");
      entity.Property(e => e.Recurring).HasColumnName("Recurring?");
      entity.Property(e => e.RecurringId).HasColumnName("RecurringID");
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
      entity.Property(e => e.CategoryId).HasDefaultValueSql("((1))");
      entity.Property(e => e.Name).HasMaxLength(50);
    });

    OnModelCreatingPartial(modelBuilder);
  }

  partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
