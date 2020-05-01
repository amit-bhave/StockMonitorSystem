using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RBC.StockManager.Models.Enums
{
    public enum TriggerType
    {
        Buy,
        Sell
    }

    public enum TriggerDirection
    {
        FromAbove,
        FromBelow
    }
}
