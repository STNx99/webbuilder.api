using System.ComponentModel.DataAnnotations.Schema;

namespace webbuilder.api.models
{
    public class Project
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required string OwnerId { get; set; }
        [Column("subdomain")]
        public string? Subdomain { get; set; }

        [Column("published")]
        public bool Published { get; set; } = false;

        public List<Element> Elements { get; set; } = [];
        public User Owner { get; set; } = null!;
    }
}