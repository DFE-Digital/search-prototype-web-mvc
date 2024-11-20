namespace Dfe.Testing.Pages.DocumentQueryClient.Pages.GDSComponents.Buttons;

public record Button : IComponent
{
    public required ButtonStyleType ButtonType { get; init; } = ButtonStyleType.Primary;
    public string TagName { get; init; } = "button";
    public required string? Type { get; init; }
    public required string Text { get; init; }
    public required bool Disabled { get; init; }
}

public enum ButtonStyleType
{
    Primary,
    Secondary,
    Warning
}