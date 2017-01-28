using System;
using System.Net;
using System.Data;
using System.IO;
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
                returnString = "";
                try
                {
                    DataSet ds = new DataSet();
                    ds.ReadXml(new StringReader(series));
                    //DataTable dt = ds.Tables[0];
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        pipe.Send(GraphiteFormatter.Format(dr[0].ToString(), Convert.ToInt32(dr[1].ToString())));

                    }
                    returnString = ds.Tables[0].Rows.Count + " values sent";   
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
