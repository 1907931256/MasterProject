using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace CNCLib
{
    public interface IMachineComms
    {
        Task<MachinePosition> GetPositionAsync(CancellationToken ct);
        Task<MachinePosition> WaitforInPositionAsync(CancellationToken ct);
        Task WaitforStartCollectionAsync(CancellationToken ct);
        Task SendProgramAsync(string sourceFileName);
        Task<bool> ConnectAsync();
        Task<bool> DisconnectAsync();
    }
  
}
