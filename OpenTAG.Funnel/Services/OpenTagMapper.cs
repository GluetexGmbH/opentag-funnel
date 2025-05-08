using System.Text.Json;
using OpenTAG.Funnel.Services.Interfaces;
using OpenTAG.Funnel.Services.Types;

namespace OpenTAG.Funnel.Services;

public class OpenTagMapper : IOpenTagMapper
{
    public OpenTagTemplate? MapJsonToTemplate(string? json)
    {
        if (string.IsNullOrEmpty(json))
            return null;

        try
        {
            var jsonObj = JsonDocument.Parse(json).RootElement;

            if (!jsonObj.TryGetProperty("versionedId", out var versionedId))
                return null;

            string? idString = versionedId.TryGetProperty("id", out var idProp) ?
                idProp.GetString() :
                null;
            int version = versionedId.TryGetProperty("version", out var versionProp) ?
                versionProp.GetInt32() :
                0;

            if (string.IsNullOrEmpty(idString))
                return null;

            var template = new OpenTagTemplate
            {
                Id = Guid.Parse(idString),
                Version = version,
                Fields = []
            };

            if (!jsonObj.TryGetProperty("fields", out var fieldsArray))
                return template;

            foreach (var field in fieldsArray.EnumerateArray())
            {
                string? apiName = field.TryGetProperty("apiName", out var apiNameProp) ?
                    apiNameProp.GetString() :
                    null;  
                
                string? displayName = field.TryGetProperty("displayName", out var displayNameProp) ?
                    displayNameProp.GetString() :
                    null;

                string? description = field.TryGetProperty("description", out var descProp) &&
                                      descProp.ValueKind != JsonValueKind.Null ? descProp.GetString() : null;

                string? fieldType = field.TryGetProperty("fieldType", out var fieldTypeProp) ?
                    fieldTypeProp.GetString() :
                    null;

                string? defaultValue = field.TryGetProperty("defaultValue", out var defaultValueProp) &&
                                       defaultValueProp.ValueKind != JsonValueKind.Null ? defaultValueProp.GetString() : null;

                if (!string.IsNullOrEmpty(apiName))
                {
                    template.Fields.Add(new OpenTagTemplateField
                    {
                        ApiName = apiName,
                        DisplayName = displayName ?? apiName,
                        Description = description ?? string.Empty,
                        Type = fieldType ?? string.Empty,
                        Value = defaultValue ?? string.Empty
                    });
                }
            }

            return template;
        }
        catch (Exception)
        {
            return null;
        }
    }
    
    public OpenTagId? MapJsonToTagId(string json)
    {
        if (string.IsNullOrEmpty(json))
            return null;

        try
        {
            var jsonObj = JsonDocument.Parse(json).RootElement;
            
            string? rawId = jsonObj.TryGetProperty("guid", out var idProp) ?
                idProp.GetString() :
                null;
            
            if (!Guid.TryParse(rawId, out Guid id))
                return null;

            string? rawOrgId = jsonObj.TryGetProperty("organisationId", out var organisationIdProp) ? 
                organisationIdProp.GetString() : 
                null;
            
            if (!Guid.TryParse(rawOrgId, out var organisationId))
                return null;
            
            string? name = jsonObj.TryGetProperty("name", out var nameProp) ?
                nameProp.GetString() :
                null;
            
            string? privateName = jsonObj.TryGetProperty("privateName", out var privateNameProp) ?
                privateNameProp.GetString() :
                null;
            
            var tagId = new OpenTagId
            {
                Id = id,
                OrganisationId = organisationId,
                Document = JsonDocument.Parse(json),
                Name = name ?? string.Empty,
                PrivateName = privateName ?? string.Empty
            };

            return tagId;
        }
        catch (Exception)
        {
            return null;
        }
    }
}