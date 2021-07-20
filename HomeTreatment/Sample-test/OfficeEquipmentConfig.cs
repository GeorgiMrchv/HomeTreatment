using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeTreatment.Web.Sample_test
{
    public class OfficeEquipmentConfig
    {
        public DeskConfig DeskConfig { get; set; }

        public MonitorConfig MonitorConfig { get; set; }

    }

    public class DeskConfig
    {
        public int Width { get; set; }

        public int Height { get; set; }

        public bool IsLifting { get; set; }

        public decimal Price { get; set; }
    }

    public class MonitorConfig
    {
        public string Color { get; set; }

    }
}
