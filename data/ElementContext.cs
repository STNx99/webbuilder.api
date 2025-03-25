using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using webbuilder.api.models;
namespace webbuilder.api.data
{
    public class ElementStoreContext : DbContext
    {
        public ElementStoreContext(DbContextOptions<ElementStoreContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Element> Elements => Set<Element>();
        public DbSet<Project> Projects => Set<Project>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.Projects)
                .WithOne()
                .HasForeignKey(p => p.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Project>()
                .HasMany(p => p.Elements)
                .WithOne()
                .HasForeignKey(e => e.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Element>()
                .Property(e => e.Id)
                .ValueGeneratedNever();

            modelBuilder.Entity<Element>()
                .HasDiscriminator(e => e.Type)
                .HasValue<TextElement>("Text")
                .HasValue<LinkElement>("Link")
                .HasValue<FrameElement>("Frame");

            modelBuilder.Entity<FrameElement>()
                .HasMany(e => e.Elements)
                .WithOne()
                .HasForeignKey(e => e.ParentId);

            modelBuilder.Entity<Element>()
                .Property(e => e.Styles)
                .HasConversion(
                    v => v == null ? null : JsonSerializer.Serialize(v, new JsonSerializerOptions()),
                    v => string.IsNullOrEmpty(v) ? new Dictionary<string, object>() : JsonSerializer.Deserialize<Dictionary<string, object>>(v, new JsonSerializerOptions())
                )
                .Metadata.SetValueComparer(new ValueComparer<Dictionary<string, object>>(
                    (d1, d2) => d1 != null && d2 != null && d1.SequenceEqual(d2),
                    d => d.Aggregate(0, (a, v) => HashCode.Combine(a, v.Key.GetHashCode(), v.Value.GetHashCode())),
                    d => d.ToDictionary(kvp => kvp.Key, kvp => kvp.Value)
                ));
        }
    }
}