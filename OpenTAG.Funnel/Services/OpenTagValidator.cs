using System.Text.RegularExpressions;

namespace OpenTAG.Funnel.Services;

public partial class OpenTagValidator : IOpenTagValidator
{
    
    public Guid? GetValidTagId(string tagUrl)
    {
        if (string.IsNullOrEmpty(tagUrl))
            return null;
        
        if (Guid.TryParse(tagUrl, out var tagId))
            return tagId;
        
        var regex = TagRegex();
        var match = regex.Match(tagUrl);
    
        if (match.Success && Guid.TryParse(match.Groups[1].Value, out Guid result))
            return result;
        
        return null;
    }

    [GeneratedRegex(@"(?<=^(?:https?:\/\/)?(?:devs\.myshade\.io|s\.opentag\.de)\/)([a-f0-9]{8}-[a-f0-9]{4}-[a-f0-9]{4}-[a-f0-9]{4}-[a-f0-9]{12})(?:-)?$")]
    private static partial Regex TagRegex();
}