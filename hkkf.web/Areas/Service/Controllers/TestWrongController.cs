using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using hkkf.Models;
using hkkf.Repositories;
using JieNuo.Data;

namespace hkkf.web.Areas.Service.Controllers
{
    [NavigationRoot("客服考试错题查询")]
    [SmartAuthorize]
    [SmartMasterPage]
    public class TestWrongController : Controller
    {
        //
        // GET: /Service/TestWrong/
        WrongRepository  wrongRepository=new WrongRepository();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TestWrong(QueryInfo queryInfo, DateTime? startDate, string userName)
        {
            TempData["startDate"] = startDate;
            PagedData<Wrong> data = wrongRepository.Wrong(queryInfo, startDate, userName);
            return View(data);
        }
    }
}
