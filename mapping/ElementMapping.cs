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

            var result = element.Type switch
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
                    Order = order
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
                    Options = element.Options ?? new List<Dictionary<string, object>>()
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
                    ButtonType = element.ButtonType ?? "default"
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
                    Order = order
                },
                "Form" => new FormElement()
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
                    Order = order
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

            if (element.Type == "Input" && element.InputSettings != null)
            {
                result.Settings.Add(new Setting
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Input Settings",
                    SettingType = "input",
                    ElementId = result.Id,
                    Settings = element.InputSettings
                });
            }
            else if (element.Type == "Select" && element.SelectSettings != null)
            {
                result.Settings.Add(new Setting
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Select Settings",
                    SettingType = "select",
                    ElementId = result.Id,
                    Settings = element.SelectSettings
                });
            }
            else if (element.Type == "Carousel" && element.CarouselSettings != null)
            {
                result.Settings.Add(new Setting
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Carousel Settings",
                    SettingType = "carousel",
                    ElementId = result.Id,
                    Settings = element.CarouselSettings
                });
            }
            else if (element.Type == "Form" && element.FormSettings != null)
            {
                result.Settings.Add(new Setting
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Form Settings",
                    SettingType = "form",
                    ElementId = result.Id,
                    Settings = element.FormSettings
                });
            }

            return result;
        }
        public static ElementDto ToElementDto(this Element element, Dictionary<string, object>? settings = null)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element)); return element.Type switch
                {
                    "Frame" => element.ToFrameElementDto(settings),
                    "Carousel" => element.ToCarouselElementDto(settings),
                    "List" => element.ToListElementDto(),
                    "Input" => element.ToInputElementDto(settings),
                    "Select" => element.ToSelectElementDto(settings),
                    "Button" => element.ToButtonElementDto(),
                    "Form" => element.ToFormElementDto(settings),
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
        public static CarouselElementDto ToCarouselElementDto(this Element element, Dictionary<string, object>? settings = null)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            // Case-insensitive type comparison
            if (!string.Equals(element.Type, "Carousel", StringComparison.OrdinalIgnoreCase))
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
                CarouselSettings = settings ?? carouselElement?.CarouselSettings ?? new()
            };
        }
        public static FrameElementDto ToFrameElementDto(this Element element, Dictionary<string, object>? settings = null)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            // Case-insensitive type comparison
            if (!string.Equals(element.Type, "Frame", StringComparison.OrdinalIgnoreCase))
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
                    .Select(child =>
                    {
                        // For any child elements that are carousels, ensure we get settings
                        if (string.Equals(child.Type, "Carousel", StringComparison.OrdinalIgnoreCase))
                        {
                            return child.ToCarouselElementDto(null);
                        }
                        return child.ToElementDto();
                    })
                    .ToList() ?? new()
            };
        }

        public static ListElementDto ToListElementDto(this Element element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
            if (element.Type != "ListItem")
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

        public static InputElementDto ToInputElementDto(this Element element, Dictionary<string, object>? settings = null)
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
                InputSettings = settings ?? new(),
            };
        }

        public static SelectElementDto ToSelectElementDto(this Element element, Dictionary<string, object>? settings = null)
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
                SelectSettings = settings ?? new()
            };
        }
        public static FormElementDto ToFormElementDto(this Element element, Dictionary<string, object>? settings)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
            if (element.Type != "Form")
                throw new ArgumentException("Element must be of type Form");
            var formElement = element as FormElement;
            return new FormElementDto()
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
                Elements = formElement?.Children?
                    .OrderBy(e => e.Order)
                    .Select(e => e.ToElementDto())
                    .ToList() ?? new(),
                FormSettings = settings ?? new()
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
                ProjectId = element.ProjectId,
                ButtonType = buttonElement?.ButtonType ?? "default"
            };
        }


    }
}