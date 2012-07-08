using System;
using System.Net;
using Microsoft.SqlServer.Server;

namespace Graphite.TSql
{
    public class GraphiteProcedures
    {
        [SqlProcedure]
        public static void GraphiteSend(string host, int port, string key, int value)
        {
            IPAddress address = Helpers.ParseAddress(host);

            using (var pipe = new TcpPipe(address, port))
            {
                try
                {
                    pipe.Send(GraphiteFormatter.Format(key, value));
                }
                catch (InvalidOperationException exception)
                {
                    SqlContext.Pipe.Send(exception.Message);
                }
            }
        }
    }
}
