using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitcoinPriceMonitor.Profile
{
    public interface IProfileLoader
    {
        void Subscribe(ILoadProfileListener listener);
    }
}
