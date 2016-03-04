using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using hkkf.Models;
using hkkf.Repositories;
using JieNuo.Data;
using hkkf.web.Areas.Service.Common;

namespace hkkf.web.Areas.Finance.Controllers
{
    [NavigationRoot("财务人员管理")]
    [SmartAuthorize]
    [SmartMasterPage]
    public class HomeController : Controller
    {
        //
        // GET: /Finance/Home/

        ShopRepository shopRepository = new ShopRepository();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ShopIndex(QueryInfo queryInfo, int[] ids, string name, string alertMessage, string subAction)
        {
            User user = this.Users();
            Kf_DepartMent departMent = user.DepartMent;  
            PagedData<Shop> data = shopRepository.GetData(queryInfo, null, name,departMent);
            return View(data);

        }

    }
}
