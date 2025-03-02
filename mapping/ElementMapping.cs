using webbuilder.api.dtos;
using webbuilder.api.models;

namespace webbuilder.api.mapping
{
    public static class ElementMapping
    {
        public static Element ToElement(this ElementDto element, string? parentId = null)
        {
            return new Element()
            {
                Type = element.Type,
                Id = element.Id,
                Content = element.Content,
                IsSelected = element.IsSelected,
                Styles = element.Styles,
                X = element.X,
                Y = element.Y,
                Src = element.Src,
                Href = element.Href,
                ParentId = parentId
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
                IsSelected = element.IsSelected,
                Styles = element.Styles,
                X = element.X,
                Y = element.Y,
                Src = element.Src,
                Href = element.Href
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
                IsSelected = element.IsSelected,
                Styles = element.Styles ?? new Dictionary<string, string>(),
                X = element.X,
                Y = element.Y,
                Src = element.Src,
                Href = element.Href,
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
        public static Element ToUpdate(this UpdateElementDto element)
        {
            return new Element()
            {
                Type = element.Type,
                Id = element.Id,
                Content = element.Content,
                IsSelected = element.IsSelected,
                Styles = element.Styles,
                X = element.X,
                Y = element.Y,
                Src = element.Src,
                Href = element.Href
            };

        }
    }

}