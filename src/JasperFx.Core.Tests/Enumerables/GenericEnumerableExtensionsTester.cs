using Shouldly;

namespace JasperFx.Core.Tests.Enumerables
{
    
    public class GenericEnumerableExtensionsTester
    {

        [Fact]
        public void invoke_action_on_each_enumerable_value()
        {
            IList<int> list = new List<int>{42,42};
            var result = new int[2];
            
            list.Each((item, index) => result[index] = item * index);
            result[0].ShouldBe(0);
            result[1].ShouldBe(42);
        }

        [Fact]
        public void Fill_a_value_into_a_list()
        {
            var list = new List<string>();

            list.Fill("a");

            list.Count.ShouldBe(1);
            list.Contains("a").ShouldBeTrue();

            // don't add it if it already exists
            list.Fill("a");
            list.Count.ShouldBe(1);
        }

        [Fact]
        public void FirstValue()
        {
            var objects = new[]
            {
                new TestObject(), new TestObject(),
                new TestObject()
            };
            objects.FirstValue(x => x.Child).ShouldBeNull();

            var theChild = new TestObject();
            objects[1].Child = theChild;
            objects[2].Child = new TestObject();

            objects.FirstValue(x => x.Child).ShouldBeSameAs(theChild);
        }
        
        [Fact]
        public void add_many_and_range()
        {
            var list = new List<string>();
            list.AddMany("a", "b", "c");

            list.ShouldHaveTheSameElementsAs("a", "b", "c");
        }

        [Fact]
        public void remove_all()
        {
            var list = new List<string> { "a", "c", "b" };
            list.Count().ShouldBe(3);
            Func<string, bool> whereEvaluator = item => item.CompareTo("c") < 0;
            list.RemoveAll(whereEvaluator);
            list.Count().ShouldBe(1);
            list.ShouldContain("c");
        }

    }
}