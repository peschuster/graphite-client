using Xunit;

namespace Graphite.Test
{
    public class HostnameTest
    {
        [Fact]
        public void ValidHostnamesTest()
        {
            string[] input = { "www.example.com", "example123.com", "server01.example.de", "4aa850f4.carbon.hostedgraphite.com" };

            foreach (string name in input)
            {
                Assert.True(Helpers.IsHostname(name), name);
            }
        }


        [Fact]
        public void InvalidHostnamesTest()
        {
            string[] input = { ".com", "192.168.0.104", "-abc.net", "de." };

            foreach (string name in input)
            {
                Assert.False(Helpers.IsHostname(name), name);
            }
        }
    }
}
