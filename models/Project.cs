namespace webbuilder.api.models{
    public class Project{
        public required string Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public List<Element> Elements { get; set; } = new();
        public required string  OwnerId { get; set; }
    }
}