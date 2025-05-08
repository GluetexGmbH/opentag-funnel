using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using OpenTAG.Funnel.Services.Interfaces;
using OpenTAG.Funnel.Services.Types;

namespace OpenTAG.Funnel.Services;

public class OpenTagAdapter : IOpenTagAdapter
{
    private const string TemplateUrl = "https://devapi.myshade.io/api/client/templates/";
    private const string TagUrl = "https://devapi.myshade.io/api/client/ids/";
    private readonly HttpClient httpClient;
    private readonly ISettingsService settingsService;
    private readonly ILogger<OpenTagAdapter> logger;
    private readonly IOpenTagMapper openTagMapper;
    
    public OpenTagAdapter(
        HttpClient httpClient, 
        ISettingsService settingsService,
        ILogger<OpenTagAdapter> logger, IOpenTagMapper openTagMapper)
    {
        this.httpClient = httpClient;
        this.settingsService = settingsService;
        this.logger = logger;
        this.openTagMapper = openTagMapper;
    }

    private async Task<HttpRequestMessage> GenerateRequest(HttpMethod method, string url)
    {
        string? jwtToken = await settingsService.GetSettingAsync("JwtToken");
        
        if (string.IsNullOrEmpty(jwtToken))
            throw new InvalidOperationException("JWT token not set.");
        
        var request = new HttpRequestMessage(method, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
        return request;
    }
    
    public async Task<OpenTagTemplate?> LoadTemplate()
    {
        string? templateId = await settingsService.GetSettingAsync("TemplateId");
        if (string.IsNullOrEmpty(templateId))
            return null;
        
        string url = $"{TemplateUrl}{templateId}";
        var request = await GenerateRequest(HttpMethod.Get, url);
        var response = await httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("Failed to load template: {StatusCode}", response.StatusCode);
            return null;
        }
       
        string json = await response.Content.ReadAsStringAsync();
        var template = openTagMapper.MapJsonToTemplate(json);

        return template;
    }
    
    public async Task<OpenTagId?> LoadId(string? tagId)
    {
        if (string.IsNullOrEmpty(tagId))
            return null;

        string url = $"{TagUrl}{tagId}";
        var request = await GenerateRequest(HttpMethod.Get, url);
        var response = await httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("Failed to load ID: {StatusCode}", response.StatusCode);
            return null;
        }
        
        string json = await response.Content.ReadAsStringAsync();
        var tagIdObject = openTagMapper.MapJsonToTagId(json);

        return tagIdObject;
    }
    
    public async Task<bool> SaveTag(OpenTagTemplate loadedTemplate, OpenTagId loadedId)
    {
        var templateJsonFields = new Dictionary<string, object>();
        foreach (var field in loadedTemplate.Fields)
            templateJsonFields[field.ApiName] = field.Value;
        
        templateJsonFields["order"] = 0;
        templateJsonFields["versionedId"] = new
        {
            id = loadedTemplate.Id.ToString(),
            version = loadedTemplate.Version
        };
        
        var tagId = new
        {
            name = loadedId.Name,
            guid = loadedId.Id,
            privateName = loadedId.PrivateName,
            organisationId = loadedId.OrganisationId,
            shadeLinkIds = new List<Guid>(),
            templates = new List<object>()
            {
                templateJsonFields
            }
        };

        string serialize = JsonSerializer.Serialize(tagId);
        string url = $"{TagUrl}{loadedId.Id}";
        
        var request = await GenerateRequest(HttpMethod.Put, url);
        request.Content = new StringContent(serialize, System.Text.Encoding.UTF8, "application/json");
        
        var response = await httpClient.SendAsync(request);
        if (response.IsSuccessStatusCode)
            return true;
        
        logger.LogError("Failed to save tag: {StatusCode}", response.StatusCode);
        return false;
    }
}