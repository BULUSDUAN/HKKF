using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using hkkf.Models;
using hkkf.Repositories;

namespace hkkf.web.Controllers
{
    public class ServiceDemoController : Controller
    {
        //
        // GET: /ServiceDemo/
        ServiceRepository serviceRepository=new ServiceRepository();
        [HttpPost]
        public ActionResult Index()
        {
            Serviced service=new Serviced();
            service.name = "田鑫";
            service.UpDateTime = DateTime.Now;
            service.Descript = "田鑫测试定时";
            serviceRepository.Save(service);
            return Content("1");
        }

    }
}
