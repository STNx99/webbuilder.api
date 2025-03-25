using webbuilder.api.dtos;
using webbuilder.api.models;

namespace webbuilder.api.mapping
{
    public static class ElementMapping
    {
        public static Element ToElement(this CreateElementDto element, int order)
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
                TailwindStyles = element.TailwindStyles,
                Src = element.Src,
                Href = element.Href,
                ParentId = element.ParentId,
                ProjectId = element.ProjectId,
                Order = order
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
                TailwindStyles = element.TailwindStyles,
                X = element.X,
                Y = element.Y,
                Src = element.Src,
                Href = element.Href,
                ParentId = element.ParentId,
                ProjectId = element.ProjectId,
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
                Content = element.Content,
                IsSelected = false,
                Styles = element.Styles,
                TailwindStyles = element.TailwindStyles,
                X = element.X,
                Y = element.Y,
                Src = element.Src,
                Href = element.Href,
                ParentId = element.ParentId,
                ProjectId = element.ProjectId,
                Elements = []
            };

            if (element is FrameElement frameElement && frameElement.Elements != null)
            {
                foreach (var child in frameElement.Elements.OrderBy(e => e.Order))
                {
                    frameElementDto.Elements.Add(child.ToElementDto());
                }
            }

            return frameElementDto;
        }

    }

}