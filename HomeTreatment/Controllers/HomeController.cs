using HomeTreatment.Data;
using HomeTreatment.Data.Repository;
using HomeTreatment.Web.Sample_test;
using HomeTreatment.Web.ViewModels;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System.Linq;
using System.Threading.Tasks;

namespace HomeTreatment.Web
{
    public class HomeController : Controller
    {
        public readonly OfficeEquipmentConfig _officeEquipmentConfig;
        private readonly IRepository _repository;


        public HomeController(IOptions<OfficeEquipmentConfig> officeEquipmentConfig, IRepository repository)
        {
            _repository = repository;
            _officeEquipmentConfig = officeEquipmentConfig.Value;
        }
        public async Task<IActionResult> Index()
        {
            var timi = _officeEquipmentConfig;           

            return View();
        }

        //Ajax Call Success
        //public JsonResult GetAllUsers()
        //{
        //    var allUsers = _repository.Set<User>();
        //    return Json(allUsers);
        //}
    }
}


//IOption - pattern koito izpolzcvame za konfiguraciq. Vmesto da raz4itam na OfficeEquipmentConfig