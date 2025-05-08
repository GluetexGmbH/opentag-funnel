namespace OpenTAG.Funnel.Services;

public interface IOpenTagValidator
{
    Guid? GetValidTagId(string tagUrl);
}