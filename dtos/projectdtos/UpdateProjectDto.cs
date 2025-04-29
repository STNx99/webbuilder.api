namespace webbuilder.api.dtos.projectdtos;
public record UpdateProjectDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Subdomain { get; set; }
    public bool Published { get; set; } = false;
    public Dictionary<string, object> Styles { get; set; } = [];
}