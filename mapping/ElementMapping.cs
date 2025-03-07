using webbuilder.api.dtos;
using webbuilder.api.models;

namespace webbuilder.api.mapping
{
    public static class ElementMapping
    {
        public static Element ToElement(this CreateElementDto element)
        {
            return new Element()
            {
                Type = element.Type,
                Id = element.Id,
                Content = element.Content,
                IsSelected = false,
                Styles = element.Styles,
                X = element.X,
                Y = element.Y,
                Src = element.Src,
                Href = element.Href,
                ParentId = element.ParentId,
                ProjectId = element.ProjectId
            };
        }


        public static ElementDto ToElementDto(this Element element)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            if (element.Type == "Frame")
            {
                return element.ToFrameElementDto();
            }

            return new ElementDto()
            {
                Type = element.Type,
                Id = element.Id,
                Content = element.Content,
                IsSelected = false,
                Styles = element.Styles,
                X = element.X,
                Y = element.Y,
                Src = element.Src,
                Href = element.Href,
                ParentId = element.ParentId,
                ProjectId = element.ProjectId
            };
        }

        public static FrameElementDto ToFrameElementDto(this Element element)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            var frameElementDto = new FrameElementDto()
            {
                Type = element.Type,
                Id = element.Id,
                Content = element.Content ?? string.Empty,
                IsSelected = false,
                Styles = element.Styles ?? new Dictionary<string, string>(),
                X = element.X,
                Y = element.Y,
                Src = element.Src,
                Href = element.Href,
                ParentId = element.ParentId,
                ProjectId = element.ProjectId,
                Elements = new List<ElementDto>()
            };

            if (element is FrameElement frameElement && frameElement.Elements != null)
            {
                foreach (var child in frameElement.Elements)
                {
                    frameElementDto.Elements.Add(child.ToElementDto());
                }
            }

            return frameElementDto;
        }

    }

}