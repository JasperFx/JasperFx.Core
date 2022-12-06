using System.Collections;
using Shouldly;

namespace JasperFx.Core.Tests
{
    public class LightweightCacheTests
    {
        private LightweightCache<string, int> cache = new LightweightCache<string, int>();
        private const string Key = "someKey";
        public interface ICallback
        {
            string GetKeyCallback(int value);
            void OnAdditionCallback(int value);
        }



        [Fact]
        public void when_key_not_found_should_throw_by_default()
        {
            const string key = "nonexisting key";

            Exception<KeyNotFoundException>.ShouldBeThrownBy(() => cache[key].ShouldBe(0)).
                Message.ShouldBe("Key '{0}' could not be found".ToFormat(key));
        }

        [Fact]
        public void get_all_keys()
        {
            cache.Fill(Key, 42);
            cache.Count().ShouldBe(1);
            cache.Contains(Key).ShouldBeTrue();
        }

        [Fact]
        public void get_enumerator()
        {
            cache.Fill(Key, 42);
            cache.GetEnumerator().ShouldBeAssignableTo<IEnumerator<int>>();
            IEnumerable enumerable = cache;
            enumerable.GetEnumerator().ShouldBeAssignableTo<IEnumerator>();
            cache.Count().ShouldBe(1);
            cache.ShouldContain(42);
        }

        [Fact]
        public void can_remove()
        {
            cache[Key] = 42;
            cache.Contains(Key).ShouldBeTrue();
            cache.Remove(Key);
            cache.Contains(Key).ShouldBeFalse();
        }

        [Fact]
        public void store_and_fetch()
        {
            cache["a"] = 1;
            cache["a"].ShouldBe(1);

            cache["a"] = 2;
            cache["a"].ShouldBe(2);
        }

        [Fact]
        public void test_the_on_missing()
        {
            int count = 0;
            cache.OnMissing = key => ++count;


            cache["a"].ShouldBe(1);
            cache["b"].ShouldBe(2);
            cache["c"].ShouldBe(3);

            cache["a"].ShouldBe(1);
            cache["b"].ShouldBe(2);
            cache["c"].ShouldBe(3);

            cache.Count.ShouldBe(3);
        }

        [Fact]
        public void fill_only_writes_if_there_is_not_previous_value()
        {
            cache.Fill("a", 1);
            cache["a"].ShouldBe(1);

            cache.Fill("a", 2);
            cache["a"].ShouldBe(1); // did not overwrite
        }

        [Fact]
        public void WithValue_positive()
        {
            cache["b"] = 2;

            int number = 0;

            cache.WithValue("b", i => number = i);

            number.ShouldBe(2);
        }

        [Fact]
        public void WithValue_negative()
        {
            cache.WithValue("b", i => { throw new Exception("Should not be called"); });
        }
    }
}