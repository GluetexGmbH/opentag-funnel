using OpenTAG.Funnel.Services.Types;

namespace OpenTAG.Funnel.Services.Interfaces;

public interface IOpenTagMapper
{
    OpenTagTemplate? MapJsonToTemplate(string json);
    
    OpenTagId? MapJsonToTagId(string json);
}