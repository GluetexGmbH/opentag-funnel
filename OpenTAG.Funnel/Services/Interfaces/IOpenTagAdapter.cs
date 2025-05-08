using OpenTAG.Funnel.Services.Types;

namespace OpenTAG.Funnel.Services.Interfaces;

public interface IOpenTagAdapter
{

    Task<OpenTagTemplate?> LoadTemplate();
    
    Task<OpenTagId?> LoadId(string? tagId);
    
    Task<bool> SaveTag(OpenTagTemplate loadedTemplate, OpenTagId loadedId);
}