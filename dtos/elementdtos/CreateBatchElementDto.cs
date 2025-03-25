namespace webbuilder.api.dtos
{
    public class BatchCreateElementsDto
    {
        public required List<CreateElementDto> Elements { get; set; }
    }
}