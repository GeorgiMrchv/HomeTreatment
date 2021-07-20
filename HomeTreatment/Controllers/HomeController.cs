using HomeTreatment.Web.Sample_test;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace HomeTreatment.Web
{
    public class HomeController : Controller
    {
        public readonly OfficeEquipmentConfig _officeEquipmentConfig;

        public HomeController(IOptions<OfficeEquipmentConfig> officeEquipmentConfig)
        {
           _officeEquipmentConfig = officeEquipmentConfig.Value;
        }
        public IActionResult Index()
        {
            var timi = _officeEquipmentConfig;
            return View();
        }
    }
}


//IOption - pattern koito izpolzcvame za konfiguraciq. Vmesto da raz4itam na OfficeEquipmentConfig