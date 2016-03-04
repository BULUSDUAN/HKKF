using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using hkkf.web.Models;
using System.Web.UI;
using hkkf.Models;
using hkkf.Repositories;
using hkkf.Common.Validations;

namespace hkkf.Web.Controllers
{
    [SmartMasterPage]
    public class AccountController : Controller
    {
        //PersonRepository personRepository = new PersonRepository();
        private UserRepository userRepository = new UserRepository();
        public IFormsAuthenticationService FormsService { get; set; }
        public IMembershipService MembershipService { get; set; }
        private static readonly string UserNameSessionKey = "Name";
        #region Login
        public ActionResult Login()
        {

            HttpCookie c = HttpContext.Request.Cookies[UserNameSessionKey];
            HttpContext.Request.ContentType = "UTF-8";
            LogOnModel model = new LogOnModel();
            if (c != null && c.Value.IsNotNullAndEmpty())
                model.UserName = Server.UrlDecode(c.Value); // ldp: 某些浏览器的 cookie 不支持中文
            #if DEBUG
            //return Login(new LoginModel { UserName = "developer", Password = "developer", RememberMe = true }, RouteData.Values["returnUrl"] as string);
            #endif
            return View("Login");
        }

        public ActionResult GetValidateCode()
        {
            ValidateCode vCode = new ValidateCode();
            string code = vCode.CreateValidateCode(5);
            Session["ValidateCode"] = code;
            byte[] bytes = vCode.CreateValidateGraphic(code);
            return File(bytes, @"image/jpeg");
        }

        [HttpPost]
        [OutputCache(Duration = 0, VaryByParam = "*", NoStore = true, Location = OutputCacheLocation.None)]
        [DisableClientCache]
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings",
            Justification = "Needs to take same parameter type as Controller.Redirect()")]
        public ActionResult Login(LogOnModel model, string returnUrl, string subAction, string subAction2)
        {

            FormsAuthentication.SignOut();
            Session.Abandon();

            if (model != null)
            {
                if (Session["ValidateCode"] != null && model.ValidateCode != Session["ValidateCode"].ToString())
                {
                    Response.Write("<script>alert('验证码错误!')</script>");
                    return View("Login");
                }
            }
            else
            {
                var m = TempData["User"] as LogOnModel;
                if (m != null) model = m;
            }

            if (ModelState.IsValid == false) return View("Login");

            User user = userRepository.GetByNameAndPassword(model.UserName.Trim(), model.Password.Trim());

            if (user == null)
            {
                Response.Write("<script>alert('用户名或密码错误!')</script>");
                return View("Login");
            }

            //Person person = personRepository.GetByDatabaseID(user.ID);


            //保存用户名;
            {
                HttpCookie c = new HttpCookie(UserNameSessionKey, Server.UrlEncode(model.UserName));// ldp: 某些浏览器的 cookie 不支持中文
                HttpContext.Response.Cookies.Add(c);
            }
            //保存 Authentication 
            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
              1,
              user.Name, //user id
              DateTime.Now,
              DateTime.Now.AddMinutes(60),  // expiry
              false,  //do not remember
              user.ID.ToString(), //emp.Roles.ToDelimitedString(",", true, r => r.Name),
              "/");
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(authTicket));
            Response.ContentType = "UTF-8";
            Response.Cookies.Add(cookie);

            //return Redirect("~/Admin/Home/Index");
            // 不要再使用以下方式保存用户
            //Session["User"] = user;
            //Session[UserID_SessionKey] = user.ID;



            if (!String.IsNullOrEmpty(returnUrl))
            {
                if (returnUrl[0] != '/') returnUrl = "/" + returnUrl;
                return Redirect(returnUrl);
            }
            else
                return Redirect("~/" + user.Type.HomePageUrl);
        }
        #endregion


        public ActionResult ChangePassword()
        {

            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(string Password, string surePwd, string YuanPwd)
        {

            var user = this.User as  hkkf.web.MvcApplication.Principal;
            
            var thisuser = userRepository.GetByDatabaseID(user.ID);
           if (YuanPwd != thisuser.Password)
            {
                ViewData["alertMessage"] = "原密码输入不正确，不能修改";
                return View();
            }
            else
            {
                thisuser.Password = surePwd;
                userRepository.Update(thisuser);
                ViewData["alertMessage"] = "修改成功！";
            }

            return View();
        }
    }
}
