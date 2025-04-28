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
        public DbSet<Image> Images => Set<Image>();
        public DbSet<Setting> Settings => Set<Setting>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.Projects)
                .WithOne(p => p.Owner)
                .HasForeignKey(p => p.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Images)
                .WithOne(i => i.User)
                .HasForeignKey(i => i.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Project>()
                .HasMany(p => p.Elements)
                .WithOne(e => e.Project)
                .HasForeignKey(e => e.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Element>()
                .HasMany(e => e.Settings)
                .WithOne(s => s.Element)
                .HasForeignKey(s => s.ElementId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Element>()
                .Property(e => e.Id)
                .ValueGeneratedNever();

            modelBuilder.Entity<Element>()
                .HasOne(e => e.Parent)
                .WithMany(e => e.Children)
                .HasForeignKey(e => e.ParentId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(false);

            modelBuilder.Entity<Element>()
                .HasDiscriminator(e => e.Type)
                .HasValue<TextElement>("Text")
                .HasValue<ImageElement>("Image")
                .HasValue<LinkElement>("Link")
                .HasValue<FrameElement>("Frame")
                .HasValue<ButtonElement>("Button")
                .HasValue<CarouselElement>("Carousel")
                .HasValue<InputElement>("Input")
                .HasValue<ListElement>("ListItem")
                .HasValue<SelectElement>("Select");

            modelBuilder.Entity<Project>().Property(e => e.Styles)
                .HasConversion(
                    v => v == null ? null : JsonSerializer.Serialize(v, new JsonSerializerOptions()),
                    v => string.IsNullOrEmpty(v) ? new Dictionary<string, object>() : JsonSerializer.Deserialize<Dictionary<string, object>>(v, new JsonSerializerOptions())
                )
                .Metadata.SetValueComparer(new ValueComparer<Dictionary<string, object>>(
                    (d1, d2) => d1 != null && d2 != null && d1.SequenceEqual(d2),
                    d => d.Aggregate(0, (a, v) => HashCode.Combine(a, v.Key.GetHashCode(), v.Value.GetHashCode())),
                    d => d.ToDictionary(kvp => kvp.Key, kvp => kvp.Value)
                ));
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

            // Configure JSON conversion for Setting.Settings
            modelBuilder.Entity<Setting>()
                .Property(s => s.Settings)
                .HasConversion(
                    v => v == null ? null : JsonSerializer.Serialize(v, new JsonSerializerOptions()),
                    v => string.IsNullOrEmpty(v) ? new Dictionary<string, object>() : JsonSerializer.Deserialize<Dictionary<string, object>>(v, new JsonSerializerOptions())
                )
                .Metadata.SetValueComparer(new ValueComparer<Dictionary<string, object>>(
                    (d1, d2) => d1 != null && d2 != null && d1.SequenceEqual(d2),
                    d => d.Aggregate(0, (a, v) => HashCode.Combine(a, v.Key.GetHashCode(), v.Value.GetHashCode())),
                    d => d.ToDictionary(kvp => kvp.Key, kvp => kvp.Value)
                ));

            // Configure Options conversion for SelectElement
            modelBuilder.Entity<SelectElement>()
                .Property(e => e.Options)
                .HasConversion(
                    v => v == null ? null : JsonSerializer.Serialize(v, new JsonSerializerOptions()),
                    v => string.IsNullOrEmpty(v) ? new List<Dictionary<string, object>>() : JsonSerializer.Deserialize<List<Dictionary<string, object>>>(v, new JsonSerializerOptions())
                );
        }
    }
}