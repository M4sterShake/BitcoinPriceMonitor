using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitcoinPriceMonitor.Profile
{
    interface IProfileLoader
    {
        void Subscribe(ILoadProfileListener listener);
    }
}
