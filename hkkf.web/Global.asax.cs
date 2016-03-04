using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using hkkf.Common;
using hkkf.Common.MVC;
using hkkf.Models;
using JieNuo.Web.Mvc;
using JieNuo.Web.Mvc.Validation;

namespace hkkf.web
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public MvcApplication()
        {
            ThreadPool.QueueUserWorkItem(Init);
            AuthenticateRequest += new EventHandler(MvcApplication_AuthenticateRequest);
        }

        //实现GenericPrincipal,表示一般用户        
        public class Principal : GenericPrincipal
        {
            public int ID { get; set; }

            public Principal(int id, IIdentity identity, string[] roles)
                : base(identity, roles)
            {
                this.ID = id;
            }
        }
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
        void MvcApplication_AuthenticateRequest(object sender, EventArgs e)
        {
            var pathAndQuery = Context.Request.Url.PathAndQuery;
            if (pathAndQuery.EndsWith(".gif")) return;
            else if (pathAndQuery.EndsWith(".jpg")) return;
            else if (pathAndQuery.EndsWith(".png")) return;
            else if (pathAndQuery.EndsWith(".js")) return;
            else if (pathAndQuery.EndsWith(".css")) return;
            else if (pathAndQuery.EndsWith(".ico")) return;

            HttpCookie authCookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                int userID = int.Parse(authTicket.UserData);

                var userPrincipal = new Principal(userID, new GenericIdentity(authTicket.Name), new string[] { });
                Context.User = userPrincipal;
            }
        }
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // 路由名称
                "{controller}/{action}/{id}", // 带有参数的 URL
                new {controller = "Account", action = "Login", id = UrlParameter.Optional},
                new[] {"hkkf.Web.Controllers"} // 参数默认值 // 参数默认值
                );

        }

        protected void Application_Start()
        {
            Init(1);
            ModelMetadataProviders.Current = new LJZCDataAnnotationsModelMetadataProvider();

            ModelValidatorProviders.Providers.Add(new ValidatableModelValidatorProvider());

            //ModelBinder
            var domainModelBinder = new DomainModelBinder();
            foreach (Type type in typeof(DomainModel).Assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(DomainModel))))
                ModelBinders.Binders.Add(type, domainModelBinder);
            var presentationDomainModelBinder = new PresentationDomainModelBinder();
            foreach (Type type in typeof(MvcApplication).Assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(PresentationModel))))
                ModelBinders.Binders.Add(type, presentationDomainModelBinder);

            ModelBinders.Binders.Add(typeof(Type), new TypeModelBinder());

            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
        public void Init(object obj)
        {
            NHibernateHelper.AddAssemblyAndBuilderSessionFactory(typeof(DomainModel).Assembly);
        }
    }
}