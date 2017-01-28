using System;
using System.Net;
using System.Xml;
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

        [SqlProcedure]
        public static void GraphiteSendSeries(string host, int port, string series, out string returnString)
        {
            IPAddress address = Helpers.ParseAddress(host);

            using (var pipe = new TcpPipe(address, port))
            {
                returnString = string.Empty;

                try
                {
                    var doc = new XmlDocument();
                    doc.LoadXml(series);

                    int count = 0;

                    foreach (XmlNode node in doc.DocumentElement)
                    {
                        if (node.ChildNodes.Count != 2)
                            continue;

                        string value = GraphiteFormatter.Format(node.FirstChild.InnerText, Convert.ToInt32(node.LastChild.InnerText));
                        if (pipe.Send(value))
                        {
                            count += 1;
                        }
                    }

                    returnString = count + " values sent";
                }
                catch (FormatException exception)
                {
                    SqlContext.Pipe.Send(exception.Message);

                    returnString = exception.Message;
                }
                catch (InvalidOperationException exception)
                {
                    SqlContext.Pipe.Send(exception.Message);

                    returnString = exception.Message;
                }
            }
        }
    }
}
