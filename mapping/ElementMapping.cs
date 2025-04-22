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

            return element.Type switch
            {
                "Input" => new InputElement()
                {
                    Type = element.Type,
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
                    Order = order,
                    InputSettings = element.InputSettings ?? new Dictionary<string, object>()
                },
                "Select" => new SelectElement()
                {
                    Type = element.Type,
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
                    Order = order,
                    Options = element.Options ?? new List<Dictionary<string, object>>(),
                    SelectSettings = element.SelectSettings
                },
                "Button" => new ButtonElement()
                {
                    Type = element.Type,
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
                    Order = order,
                    ButtonType = "default"
                },
                "Carousel" => new CarouselElement()
                {
                    Type = element.Type,
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
                    Order = order,
                    CarouselSettings = element.CarouselSettings ?? new Dictionary<string, object>(),
                },
                _ => new Element()
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
                }
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
                "List" => element.ToListElementDto(),
                "Input" => element.ToInputElementDto(),
                "Select" => element.ToSelectElementDto(),
                "Button" => element.ToButtonElementDto(),
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
                Elements = carouselElement?.Children?
                    .OrderBy(e => e.Order)
                    .Select(e => e.ToElementDto())
                    .ToList() ?? new(),
                CarouselSettings = carouselElement?.CarouselSettings ?? new()
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
                Elements = frameElement?.Children?
                    .OrderBy(e => e.Order)
                    .Select(e => e.ToElementDto())
                    .ToList() ?? new()
            };
        }

        public static ListElementDto ToListElementDto(this Element element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
            if (element.Type != "List")
                throw new ArgumentException("Element must be of type List");

            var listElement = element as ListElement;

            return new ListElementDto()
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
                Elements = listElement?.Children?
                    .OrderBy(e => e.Order)
                    .Select(e => e.ToElementDto())
                    .ToList() ?? new()
            };
        }

        public static InputElementDto ToInputElementDto(this Element element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
            if (element.Type != "Input")
                throw new ArgumentException("Element must be of type Input");

            var inputElement = element as InputElement;

            return new InputElementDto()
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
                InputSettings = inputElement?.InputSettings ?? new()
            };
        }

        public static SelectElementDto ToSelectElementDto(this Element element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
            if (element.Type != "Select")
                throw new ArgumentException("Element must be of type Select");

            var selectElement = element as SelectElement;

            return new SelectElementDto()
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
                Options = selectElement?.Options ?? new(),
                SelectSettings = selectElement?.SelectSettings
            };
        }

        public static ButtonElementDto ToButtonElementDto(this Element element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
            if (element.Type != "Button")
                throw new ArgumentException("Element must be of type Button");

            var buttonElement = element as ButtonElement;

            return new ButtonElementDto()
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
                ProjectId = element.ProjectId
            };
        }

        public static PublicElementDto ToPublicElementDto(this ElementDto element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            return element.Type switch
            {
                "Frame" => new PublicFrameElementDto
                {
                    Type = element.Type,
                    Id = element.Id,
                    Name = element.Name,
                    Content = element.Content,
                    Styles = element.Styles,
                    TailwindStyles = element.TailwindStyles,
                    X = element.X,
                    Y = element.Y,
                    Src = element.Src,
                    Href = element.Href,
                    Elements = (element as FrameElementDto)?.Elements?.Select(e => e.ToPublicElementDto()).ToList() ?? new()
                },
                "Carousel" => new PublicCarouselElementDto
                {
                    Type = element.Type,
                    Id = element.Id,
                    Name = element.Name,
                    Content = element.Content,
                    Styles = element.Styles,
                    TailwindStyles = element.TailwindStyles,
                    X = element.X,
                    Y = element.Y,
                    Src = element.Src,
                    Href = element.Href,
                    Elements = (element as CarouselElementDto)?.Elements?.Select(e => e.ToPublicElementDto()).ToList() ?? new(),
                    CarouselSettings = (element as CarouselElementDto)?.CarouselSettings ?? new()
                },
                "List" => new PublicListElementDto
                {
                    Type = element.Type,
                    Id = element.Id,
                    Name = element.Name,
                    Content = element.Content,
                    Styles = element.Styles,
                    TailwindStyles = element.TailwindStyles,
                    X = element.X,
                    Y = element.Y,
                    Src = element.Src,
                    Href = element.Href,
                    Elements = (element as ListElementDto)?.Elements?.Select(e => e.ToPublicElementDto()).ToList() ?? new()
                },
                "Input" => new PublicInputElementDto
                {
                    Type = element.Type,
                    Id = element.Id,
                    Name = element.Name,
                    Content = element.Content,
                    Styles = element.Styles,
                    TailwindStyles = element.TailwindStyles,
                    X = element.X,
                    Y = element.Y,
                    Src = element.Src,
                    Href = element.Href,
                    InputSettings = (element as InputElementDto)?.InputSettings ?? new()
                },
                "Select" => new PublicSelectElementDto
                {
                    Type = element.Type,
                    Id = element.Id,
                    Name = element.Name,
                    Content = element.Content,
                    Styles = element.Styles,
                    TailwindStyles = element.TailwindStyles,
                    X = element.X,
                    Y = element.Y,
                    Src = element.Src,
                    Href = element.Href,
                    Options = (element as SelectElementDto)?.Options ?? new()
                },
                "Button" => new PublicButtonElementDto
                {
                    Type = element.Type,
                    Id = element.Id,
                    Name = element.Name,
                    Content = element.Content,
                    Styles = element.Styles,
                    TailwindStyles = element.TailwindStyles,
                    X = element.X,
                    Y = element.Y,
                    Src = element.Src,
                    Href = element.Href
                },
                "Link" => new PublicLinkElementDto
                {
                    Type = element.Type,
                    Id = element.Id,
                    Name = element.Name,
                    Content = element.Content,
                    Styles = element.Styles,
                    TailwindStyles = element.TailwindStyles,
                    X = element.X,
                    Y = element.Y,
                    Src = element.Src,
                    Href = element.Href
                },
                "Image" => new PublicImageElementDto
                {
                    Type = element.Type,
                    Id = element.Id,
                    Name = element.Name,
                    Content = element.Content,
                    Styles = element.Styles,
                    TailwindStyles = element.TailwindStyles,
                    X = element.X,
                    Y = element.Y,
                    Src = element.Src,
                    Href = element.Href
                },
                "Text" => new PublicTextElementDto
                {
                    Type = element.Type,
                    Id = element.Id,
                    Name = element.Name,
                    Content = element.Content,
                    Styles = element.Styles,
                    TailwindStyles = element.TailwindStyles,
                    X = element.X,
                    Y = element.Y,
                    Src = element.Src,
                    Href = element.Href
                },
                _ => new PublicElementDto
                {
                    Type = element.Type,
                    Id = element.Id,
                    Name = element.Name,
                    Content = element.Content,
                    Styles = element.Styles,
                    TailwindStyles = element.TailwindStyles,
                    X = element.X,
                    Y = element.Y,
                    Src = element.Src,
                    Href = element.Href
                }
            };
        }

        public static PublicElementDto ToPublicElementDto(this Element element)
        {
            return element.ToElementDto().ToPublicElementDto();
        }
    }
}