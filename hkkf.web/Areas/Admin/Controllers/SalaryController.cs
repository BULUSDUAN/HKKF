using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using hkkf.Models;
using hkkf.Repositories;
using JieNuo.Data;
using JieNuo.Data.Exceptions;

namespace hkkf.web.Areas.Admin.Controllers
{
    [NavigationRoot("学生管理")]
    [SmartAuthorize]
    [SmartMasterPage]
    public class SalaryController : Controller
    {
        //
        // GET: /Admin/Student/    
    }
}
