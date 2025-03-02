using System.ComponentModel.DataAnnotations;

namespace webbuilder.api.dtos{
    public record class UpdateElementDto{
        [Required] public string Id { get; init; }
        [Required] public string Type { get; init; }
        public string Content { get; init; }
        public bool IsSelected { get; init; }
        public Dictionary<string, string> Styles { get; init; }
        public int X { get; init; }
        public int Y { get; init; }
        public string Src { get; init; }
        public string Href { get; init; }
    }
}