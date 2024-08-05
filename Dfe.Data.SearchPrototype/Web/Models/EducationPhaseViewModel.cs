using Dfe.Data.SearchPrototype.SearchForEstablishments;

namespace Dfe.Data.SearchPrototype.Web.Models;

/// <summary>
/// A view model representation of an education phase from a single search result.
/// </summary>
public class EducationPhaseViewModel
{
    /// <summary>
    /// true if the establishment includes the Primary phase of education
    /// false if the establishment does not include the Primary phase of education
    /// </summary>
    public bool IsPrimary { get; set; }
    /// <summary>
    /// true if the establishment includes the secondary phase of education
    /// false if the establishment does not include the secondary phase of education 
    /// </summary>
    public bool IsSecondary { get; set; }
    /// <summary>
    /// true if the establishment includes the post 16 phase of education
    /// false if the establishment does not include the post 16 phase of education 
    /// </summary>
    public bool IsPost16 { get; set; }

    /// <summary>
    /// Establishment education phase
    /// </summary>
    /// <returns>
    /// Education phase formatted as a display-friendly string
    /// </returns>
    public string EducationPhaseAsString()
    {
        var mapEducationPhaseCodeToString = new Dictionary<string, bool>
        {
            {"Primary", IsPrimary },
            {"Secondary", IsSecondary },
            {"16 plus", IsPost16 }
        };
        var educationPhaseComponents = mapEducationPhaseCodeToString
            .Where(educationPhaseCode => educationPhaseCode.Value == true)
            .Select(educationPhaseCode => educationPhaseCode.Key);

        return string.Join(", ", educationPhaseComponents);
    }
}