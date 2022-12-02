using System.Linq.Expressions;
using JasperFx.Core.Reflection;
using NSubstitute;
using Shouldly;

namespace JasperFx.Core.Tests.Reflection;

public class ReflectionExtensionsTester
{
    private ICallback _callback;
    private Expression<Func<PropertyHolder, object>> _expression;
    private ICallback _uncalledCallback;

    public ReflectionExtensionsTester()
    {
        _expression = ph => ph.Age;
        _callback = Substitute.For<ICallback>();
        _uncalledCallback = Substitute.For<ICallback>();
    }

    [Theory]
    [InlineData(typeof(SimpleClass), true)]
    [InlineData(typeof(Simple2Class), true)]
    [InlineData(typeof(SimpleClass3), false)]
    public void has_default_constructor(Type target, bool hasCtor)
    {
        target.HasDefaultConstructor().ShouldBe(hasCtor);
    }

    [Fact]
    public void is_anonymous_type()
    {
        var o = new { Name = "Miller" };
        o.IsAnonymousType().ShouldBeTrue();

        "who?".IsAnonymousType().ShouldBeFalse();
        string data = null;
        data.IsAnonymousType().ShouldBeFalse();
    }

    public class PropertyHolder
    {
        public int Age { get; set; }
    }

    public interface ICallback
    {
        void Callback();
    }


    public class SimpleClass
    {
    }

    public class Simple2Class
    {
        public Simple2Class(string name)
        {
        }

        public Simple2Class()
        {
        }
    }

    public class SimpleClass3
    {
        public SimpleClass3(string name)
        {
        }
    }
}