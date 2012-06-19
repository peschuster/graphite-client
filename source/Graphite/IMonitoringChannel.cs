using System.Threading.Tasks;

namespace Graphite
{
    public interface IMonitoringChannel
    {
        bool Report(int value);

        Task<bool> ReportAsync(int value);
    }
}