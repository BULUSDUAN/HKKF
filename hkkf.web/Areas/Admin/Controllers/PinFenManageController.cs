using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using hkkf.Repositories;
using JieNuo.Data;
using hkkf.Models;
using hkkf.web.Areas.Service.Common;

namespace hkkf.web.Areas.Admin.Controllers
{
    [NavigationRoot("店铺分配管理")]
    [SmartAuthorize]
    [SmartMasterPage]
    public class PinFenManageController : Controller
    {
        //
        // GET: /Admin/PinFenManage/
        private PinFenRepository pinFenRepository=new PinFenRepository();
        private UserRepository UserRepository = new UserRepository();

        public ActionResult PinFenByUserIndex(QueryInfo queryInfo, string UserName)
        {
            var dataUser = this.UserRepository.GetUserData(queryInfo, UserName, "1",this.Users().DepartMent);
            return View(dataUser);
        }     

    }
}
