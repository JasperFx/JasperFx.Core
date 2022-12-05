using Shouldly;

namespace JasperFx.Core.Tests
{
    public class DisposableExtensionsTests
    {
        [Fact]
        public void swallow_exceptions()
        {
            var blowsUpDisposing = new BlowsUpDisposing();
            blowsUpDisposing.SafeDispose();

            blowsUpDisposing.DisposeWasCalled.ShouldBeTrue();
        }

        public class BlowsUpDisposing : IDisposable
        {
            public void Dispose()
            {
                DisposeWasCalled = true;
                throw new Exception("You stink!");
            }

            public bool DisposeWasCalled { get; set; }
        }
    }
}