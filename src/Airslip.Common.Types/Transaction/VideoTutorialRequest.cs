namespace Airslip.Common.Types.Transaction;

public record VideoTutorialRequest
{
    /// <summary>
    /// The descriptive title to the video.
    /// </summary>
    /// <example>How to Use and Latte Art Tutorial on a Home Espresso Machine</example>
    public string? Description { get; init; }

    /// <summary>
    /// The URL to the video tutorial or any other relevant resource.
    /// </summary>
    /// <example>https://www.youtube.com/watch?v=rck0qoo_FDI</example>
    public string? Url { get; init; }
}