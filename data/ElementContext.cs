using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using webbuilder.api.models;
namespace webbuilder.api.data
{
    public class ElementStoreContext : DbContext
    {
        public ElementStoreContext(DbContextOptions<ElementStoreContext> options) : base(options) { }

        public DbSet<Element> Elements => Set<Element>();
        public DbSet<Project> Projects => Set<Project>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

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
                .HasValue<Element>("Text")
                .HasValue<FrameElement>("Frame");

            modelBuilder.Entity<FrameElement>()
                .HasMany(e => e.Elements)
                .WithOne()
                .HasForeignKey(e => e.ParentId);

            modelBuilder.Entity<Element>()
                .Property(e => e.Styles)
                .HasConversion(
                    v => v == null ? null : JsonSerializer.Serialize(v, new JsonSerializerOptions()),
                    v => string.IsNullOrEmpty(v) ? new Dictionary<string, string>() : JsonSerializer.Deserialize<Dictionary<string, string>>(v, new JsonSerializerOptions())
                )
                .Metadata.SetValueComparer(new ValueComparer<Dictionary<string, string>>(
                    (d1, d2) => d1 != null && d2 != null && d1.SequenceEqual(d2),
                    d => d.Aggregate(0, (a, v) => HashCode.Combine(a, v.Key.GetHashCode(), v.Value.GetHashCode())),
                    d => d.ToDictionary(kvp => kvp.Key, kvp => kvp.Value)
                ));

            modelBuilder.Entity<Project>().HasData(
                new Project
                {
                    Id = "1",
                    Name = "Sample Project",
                    Description = "A sample project with elements",
                    OwnerId = "1"
                }
            );

            modelBuilder.Entity<Element>().HasData(
                new Element
                {
                    Id = "1",
                    Type = "Text",
                    Content = "Hello, World!",
                    IsSelected = false,
                    ProjectId = "1",
                    Styles = new Dictionary<string, string> { { "color", "red" }, { "font-size", "12px" } },
                    X = 10,
                    Y = 20
                },
                new Element
                {
                    Id = "3",
                    Type = "Text",
                    Content = "Nested Text",
                    IsSelected = false,
                    ProjectId = "1",
                    Styles = new Dictionary<string, string> { { "color", "blue" } },
                    X = 10,
                    Y = 10,
                    ParentId = "2"
                },
                new Element
                {
                    Id = "5",
                    Type = "Text",
                    Content = "Deeply Nested Text",
                    IsSelected = false,
                    ProjectId = "1",
                    Styles = new Dictionary<string, string> { { "font-weight", "bold" } },
                    X = 5,
                    Y = 5,
                    ParentId = "4"
                }
            );

            modelBuilder.Entity<FrameElement>().HasData(
                new FrameElement
                {
                    Id = "2",
                    Type = "Frame",
                    IsSelected = false,
                    ProjectId = "1",
                    Styles = new Dictionary<string, string> { { "border", "1px solid black" } },
                    X = 50,
                    Y = 100,
                    ParentId = null
                },
                new FrameElement
                {
                    Id = "4",
                    Type = "Frame",
                    IsSelected = false,
                    ProjectId = "1",
                    Styles = new Dictionary<string, string> { { "background", "yellow" } },
                    X = 20,
                    Y = 20,
                    ParentId = "2"
                }
            );
        }

    }

}