using System.Linq.Expressions;
using System.Reflection;
using JasperFx.Core.Reflection;

namespace JasperFx.Core.Descriptions;

public class OptionsValue
{
    // For serialization
    public OptionsValue()
    {
    }

    // make this containing object full name.name
    // Can be deep? That would cover DurabilitySettings
    // use this to tag fancier tooltips
    public string Subject { get; set; }
    public string Name { get; set; }
    public PropertyType Type { get; set; }
    
    // Maybe don't serialize this
    public object RawValue { get; set; }
    public string Value { get; set; }

    public static OptionsValue Read<T>(T subject, Expression<Func<T,object>> expression)
    {
        if (subject == null)
        {
            throw new ArgumentNullException(nameof(subject));
        }

        var property = ReflectionHelper.GetProperty(expression);

        return Read(property, subject);
    }

    public static OptionsValue Read(PropertyInfo property, object subject)
    {
        var value = property.GetValue(subject);
        
        var description = new OptionsValue
        {
            Subject = $"{subject.GetType().FullNameInCode()}.{property.Name}",
            RawValue = value,
            Value = value?.ToString(),
            Name = property.Name,
            Type = PropertyType.Text, // safest guess
        };

        if (value == null)
        {
            description.Type = PropertyType.None;
            description.Value = "None";
        }
        else if (value.GetType().IsNumeric())
        {
            description.Type = PropertyType.Numeric;
        }
        else if (value.GetType().IsEnum)
        {
            description.Type = PropertyType.Enum;
        }
        else if (value is bool)
        {
            description.Type = PropertyType.Boolean;
        }
        else if (value is Uri)
        {
            description.Type = PropertyType.Uri;
        }
        else if (value is TimeSpan time)
        {
            description.Type = PropertyType.TimeSpan;
            description.Value = time.ToDisplay();
        }
        
        return description;
    }
}