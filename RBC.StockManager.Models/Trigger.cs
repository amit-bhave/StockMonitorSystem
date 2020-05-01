using RBC.StockManager.Models.Enums;

namespace RBC.StockManager.Models
{
    public class Trigger
    {
        public TriggerType Type { get; set; }
        public double Threshold { get; set; }
        public TriggerDirection Direction { get; set; }
        public double Sensitivity { get; set; }
        public bool ClientNotified { get; set; }
    }
}
