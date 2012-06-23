using System;
using Xunit;

namespace Graphite.Test
{
    public class UriTests
    {
        [Fact]
        public void AbsoluteUri()
        {
            Uri baseUri = new Uri("http://www.contoso.com/");
            Uri myUri = new Uri(baseUri, "catalog/shownew.htm?date=today");

            Assert.Equal("/catalog/shownew.htm", myUri.AbsolutePath);
        }
    }
}
