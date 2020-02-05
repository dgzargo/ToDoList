using Microsoft.EntityFrameworkCore;
using TaskList.DAL.Entities;

namespace TaskList.DAL
{
    public class TaskListDbContext : DbContext
    {
        public TaskListDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<ToDo> ToDos { get; set; }
        public DbSet<List> Lists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ToDo>(builder =>
            {
                builder.HasKey(toDo => toDo.Id);
                builder.Property(toDo => toDo.Id).ValueGeneratedOnAdd();

                builder.Property(toDo => toDo.Title)
                    .IsRequired()
                    .HasMaxLength(30);

                builder.Property(e => e.Created)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                builder.Property(e => e.Deadline).HasColumnType("datetime");

                builder.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(100);

                builder.HasOne(d => d.List)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.ListId)
                    .HasConstraintName("FK_Tasks_ListId");
            });
            modelBuilder.Entity<List>(builder =>
            {
                builder.HasKey(list => list.Id);
                builder.Property(list => list.Id).ValueGeneratedOnAdd();
                builder.HasIndex(e => e.Title)
                    .HasName("UQ_Lists_Title")
                    .IsUnique();
                builder.Property(list => list.Title)
                    .IsRequired()
                    .HasMaxLength(30);
            });
        }
    }
}