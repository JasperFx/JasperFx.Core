using System.Linq.Expressions;
using JasperFx.Core.Descriptions;
using JasperFx.Core.Reflection;
using Shouldly;

namespace JasperFx.Core.Tests.Descriptions;

public class reading_descriptions
{
    public readonly Target theTarget = new();

    private OptionsValue read(Expression<Func<Target, object>> expression)
    {
        return OptionsValue.Read(theTarget, expression);
    }
    
    [Fact]
    public void read_a_string()
    {
        // Our dog's name who's sleeping in my office...
        theTarget.Name = "Chewie";

        var property = read(x => x.Name);
        property.Name.ShouldBe("Name");
        property.Type.ShouldBe(PropertyType.Text);
        property.Subject = $"{typeof(Target).FullNameInCode()}.{nameof(Target.Name)}";
        property.RawValue.ShouldBe(theTarget.Name);
        property.Value.ShouldBe("Chewie");
    }
    
    [Fact]
    public void read_a_null_value()
    {
        theTarget.Name = null;

        var property = read(x => x.Name);
        property.Name.ShouldBe("Name");
        property.Type.ShouldBe(PropertyType.None);
        property.Subject = $"{typeof(Target).FullNameInCode()}.{nameof(Target.Name)}";
        property.RawValue.ShouldBe(null);
        property.Value.ShouldBe("None");
    }

    [Fact]
    public void read_an_integer()
    {
        var property = read(x => x.Age);
        property.Name.ShouldBe("Age");
        property.Type.ShouldBe(PropertyType.Numeric);
        property.Subject = $"{typeof(Target).FullNameInCode()}.{nameof(Target.Age)}";
        property.RawValue.ShouldBe(theTarget.Age);
        property.Value.ShouldBe(theTarget.Age.ToString());
    }

    [Fact]
    public void read_an_enum()
    {
        var property = read(x => x.Color);
        property.Name.ShouldBe("Color");
        property.Type.ShouldBe(PropertyType.Enum);
        property.Subject = $"{typeof(Target).FullNameInCode()}.{nameof(Target.Color)}";
        property.RawValue.ShouldBe(theTarget.Color);
        property.Value.ShouldBe(theTarget.Color.ToString());
    }
    
    [Fact]
    public void read_a_boolean()
    {
        theTarget.IsTrue = true;
        var property = read(x => x.IsTrue);
        property.Name.ShouldBe("IsTrue");
        property.Type.ShouldBe(PropertyType.Boolean);
        property.Subject = $"{typeof(Target).FullNameInCode()}.{nameof(Target.IsTrue)}";
        property.RawValue.ShouldBe(theTarget.IsTrue);
        property.Value.ShouldBe(theTarget.IsTrue.ToString());
    }

    [Fact]
    public void read_a_uri()
    {
        theTarget.IsTrue = true;
        var property = read(x => x.Uri);
        property.Name.ShouldBe("Uri");
        property.Type.ShouldBe(PropertyType.Uri);
        property.Subject = $"{typeof(Target).FullNameInCode()}.{nameof(Target.Uri)}";
        property.RawValue.ShouldBe(theTarget.Uri);
        property.Value.ShouldBe(theTarget.Uri.ToString());
    }

    [Fact]
    public void read_a_time_span()
    {
        theTarget.IsTrue = true;
        var property = read(x => x.Duration);
        property.Name.ShouldBe("Duration");
        property.Type.ShouldBe(PropertyType.TimeSpan);
        property.Subject = $"{typeof(Target).FullNameInCode()}.{nameof(Target.Duration)}";
        property.RawValue.ShouldBe(theTarget.Duration);
        property.Value.ShouldBe(theTarget.Duration.ToDisplay());
    }

    [Fact]
    public void read_in_description()
    {
        theTarget.Name = "Shiner"; // our previous family dog
        var description = new OptionsDescription(theTarget);
        
        description.Properties.Select(x => x.Name)
            .ToArray()
            .ShouldBe(new string[]{"Name", "IsTrue", "Age", "Color", "Uri", "Duration"});
        
        description.Children["YesThis"].Properties.Select(x => x.Name)
            .ShouldHaveTheSameElementsAs("Number", "Suffix");
    }
}

public enum Color
{
    Red, Blue, Green
}

public class Target
{
    public string Name { get; set; }
    public bool IsTrue { get; set; }
    public int Age { get; set; } = 51;
    public Color Color { get; set; } = Color.Blue;
    public Uri Uri { get; set; } = "local://durable".ToUri();

    public TimeSpan Duration { get; set; } = 25.Milliseconds();
    
    // I want this skipped
    public string[] Strings { get; set; }
    
    [IgnoreDescription]
    public Thing NotThis { get; set; }

    [ChildDescription]
    public Thing YesThis { get; set; } = new Thing
    {
        Number = 4, Suffix = "Jr"
    };
}

public class Thing
{
    public int Number { get; set; } = 5;
    public string Suffix { get; set; } = "Esq.";
}