using System;
using System.Linq;
using webbuilder.api.dtos;
using webbuilder.api.models;

namespace webbuilder.api.mapping
{
    public static class ElementMapping
    {
        public static Element ToElement(this CreateElementDto element, int order)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            return new Element()
            {
                Type = element.Type ?? throw new ArgumentException("Element type is required"),
                Id = element.Id ?? Guid.NewGuid().ToString(),
                Name = element.Name,
                Content = element.Content,
                IsSelected = false,
                Styles = element.Styles ?? new(),
                X = element.X,
                Y = element.Y,
                TailwindStyles = element.TailwindStyles,
                Src = element.Src,
                Href = element.Href,
                ParentId = element.ParentId,
                ProjectId = element.ProjectId ?? throw new ArgumentException("Project ID is required"),
                Order = order
            };
        }

        public static ElementDto ToElementDto(this Element element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            return element.Type switch
            {
                "Frame" => element.ToFrameElementDto(),
                "Carousel" => element.ToCarouselElementDto(),
                _ => new ElementDto()
                {
                    Type = element.Type,
                    Id = element.Id,
                    Name = element.Name,
                    Content = element.Content,
                    IsSelected = element.IsSelected,
                    Styles = element.Styles,
                    TailwindStyles = element.TailwindStyles,
                    X = element.X,
                    Y = element.Y,
                    Src = element.Src,
                    Href = element.Href,
                    ParentId = element.ParentId,
                    ProjectId = element.ProjectId,
                }
            };
        }

        public static CarouselElementDto ToCarouselElementDto(this Element element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
            if (element.Type != "Carousel")
                throw new ArgumentException("Element must be of type Carousel");

            var carouselElement = element as CarouselElement;

            return new CarouselElementDto()
            {
                Type = element.Type,
                Id = element.Id,
                Name = element.Name,
                Content = element.Content,
                IsSelected = element.IsSelected,
                Styles = element.Styles,
                TailwindStyles = element.TailwindStyles,
                X = element.X,
                Y = element.Y,
                Src = element.Src,
                Href = element.Href,
                ParentId = element.ParentId,
                ProjectId = element.ProjectId,
                Elements = carouselElement?.Elements?
                    .OrderBy(e => e.Order)
                    .Select(e => e.ToElementDto())
                    .ToList() ?? new()
            };
        }

        public static FrameElementDto ToFrameElementDto(this Element element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
            if (element.Type != "Frame")
                throw new ArgumentException("Element must be of type Frame");

            var frameElement = element as FrameElement;

            return new FrameElementDto()
            {
                Type = element.Type,
                Id = element.Id,
                Name = element.Name,
                Content = element.Content,
                IsSelected = element.IsSelected,
                Styles = element.Styles,
                TailwindStyles = element.TailwindStyles,
                X = element.X,
                Y = element.Y,
                Src = element.Src,
                Href = element.Href,
                ParentId = element.ParentId,
                ProjectId = element.ProjectId,
                Elements = frameElement?.Elements?
                    .OrderBy(e => e.Order)
                    .Select(e => e.ToElementDto())
                    .ToList() ?? new()
            };
        }
    }
}