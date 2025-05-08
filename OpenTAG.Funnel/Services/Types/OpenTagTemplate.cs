namespace OpenTAG.Funnel.Services.Types;

public class OpenTagTemplate
{
    public Guid Id { get; set; }

    public int Version { get; set; }

    public string Name { get; set; }

    public List<OpenTagTemplateField> Fields { get; set; }   
}