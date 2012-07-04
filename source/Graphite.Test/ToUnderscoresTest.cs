using System.Collections.Generic;
using Xunit;

namespace Graphite.Test
{
    public class ToUnderscoresTest
    {
        [Fact]
        public void ActionTest()
        {
            Dictionary<string, string> input = new Dictionary<string, string>
            {
                { "GetData", "Get_Data" },
                { "GetDAta", "Get_DAta" },
                { "GETDAta", "GETDAta" },
            };

            foreach (var item in input)
            {
                Assert.Equal(item.Value, item.Key.ToUnderscores());
            }
        }
    }
}
