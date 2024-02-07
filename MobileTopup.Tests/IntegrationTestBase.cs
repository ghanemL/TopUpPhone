using MobileTopup.IntegrationTests;
using System.Net;
using System.Reflection;
using Xunit.Abstractions;

namespace MobileTopup.Tests
{
    public abstract class IntegrationTestBase : IDisposable
    {

        private readonly ICollection<IDisposable> _disposables = new List<IDisposable>();

        protected readonly SliceFixture SliceFixture;

        protected IntegrationTestBase()
        {
        }

        protected T WithDisposable<T>(Func<T> disposableFunc) where T : IDisposable
        {
            var disposable = disposableFunc();
            _disposables.Add(disposable);
            return disposable;
        }

        protected async Task<StringContent> WithStringContentFromManifestResourceStream(string name)
        {
            var reader = WithDisposable(() => new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(name)));
            return await WithDisposable(async () => new StringContent(await reader.ReadToEndAsync().ConfigureAwait(false))).ConfigureAwait(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            foreach (var disposable in _disposables)
            {
                try
                {
                    disposable?.Dispose();
                }
                catch (Exception)
                {
                    //NOOP
                }
            }
        }

        protected HttpResponseMessage NewEmptyJsonArray()
        {
            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = WithDisposable(() => new StringContent("[]"))
            };
        }
    }
}
