using JasperFx.Core.Reflection;

namespace JasperFx.Core.Descriptions;

/// <summary>
/// Just a serializable, readonly view of system configuration to be used for diagnostic purposes
/// </summary>
public class OptionsDescription
{
    public string Subject { get; set; }
    public List<OptionsValue> Properties { get; set; } = new();

    public Dictionary<string, OptionsDescription> Children = new();
    
    // For serialization
    public OptionsDescription()
    {
    }

    public OptionsDescription(object subject)
    {
        if (subject == null)
        {
            throw new ArgumentNullException(nameof(subject));
        }

        var type = subject.GetType();
        foreach (var property in type.GetProperties().Where(x => !x.HasAttribute<IgnoreDescriptionAttribute>()))
        {
            if (property.HasAttribute<ChildDescriptionAttribute>())
            {
                var child = property.GetValue(subject);
                if (child == null) continue;

                var childDescription = new OptionsDescription(child);
                Children[property.Name] = childDescription;
                
                continue;
            }
            
            if (property.PropertyType != typeof(string) && property.PropertyType.IsEnumerable()) continue;
            Properties.Add(OptionsValue.Read(property, subject));
        }
    }
}