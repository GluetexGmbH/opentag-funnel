using System.Text.Json;

namespace OpenTAG.Funnel.Services.Types;

public class OpenTagId
{
    
    public Guid Id { get; set; }

    public Guid OrganisationId { get; set; }
    
    public string Name { get; set; }
    
    public string PrivateName { get; set; }

    public JsonDocument Document { get; set; }

}