using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using hkkf.Models;
using hkkf.Repositories;
using JieNuo.Data;
using hkkf.web.Areas.Service.Common;

namespace hkkf.web.Areas.Finance.Controllers
{
    [NavigationRoot("合同管理")]
    [SmartAuthorize]
    [SmartMasterPage]
    public class ContactController : Controller
    {
        //
        // GET: /Finance/Contact/
        UserRepository userRepository = new UserRepository();
        PersonShopRepository personShopRepository = new PersonShopRepository();
        ShopRepository shopRepository = new ShopRepository();
        ContactRepositories contactRepositories = new ContactRepositories();
        public ActionResult UserIndex(QueryInfo queryInfo, int[] ids, string name, string alertMessage, string subAction)
        {
            if (alertMessage != null)
            {
                ViewBag.message = alertMessage;
            }
            PagedData<User> data = userRepository.GetUserData(queryInfo, null,null,this.Users().DepartMent);
            return View(data);

        }

        public ActionResult ContactUpload(string name, int? userid)
        {
            ViewBag.TypeList = shopRepository.GetAll().ToList();
            ViewBag.userid = userid.ToString();
            return View();
        }
        [HttpPost]
        public ActionResult ContactUpload(string Shopid, string userid, string SubAction, HttpPostedFileBase pathdata)
        {
            ViewBag.TypeList = shopRepository.GetAll().ToList();
          
            var user = userRepository.GetByDatabaseID(Convert.ToInt32(userid));
            if (SubAction == "上传合同")
            {
                //服务器的哪个文件夹
                string mapPath = Server.MapPath("~/UploadFile/");
                if (pathdata != null)
                {
                    string FullPath = SaveFile(mapPath, pathdata);
                    if (FullPath != null)
                    {
                        if (Shopid != null)
                        {
                            string shop = Shopid.Substring(1, Shopid.Length - 1);
                            string[] ids = shop.Split(',');
                            int[] newids = Array.ConvertAll<string, int>(ids, delegate(string s) { return int.Parse(s); });
                            for (int i = 0; i < newids.Length; i++)
                            {
                                Contact contact = new Contact();
                                var shop1 = shopRepository.GetByDatabaseID(newids[i]);
                                contact.shop = shop1;
                                contact.CName = pathdata.FileName;
                                contact.FileUrl = FullPath;
                                contact._user = user;
                                contact.ContacTime = DateTime.Now;
                                contactRepositories.Save(contact);
                            }

                        }

                    }
                    return Content("<script>alert('上传成功!!');location.href='../Contact/UserIndex'</script>");

                }


            }
            return View();
        }


        public ActionResult ContactIndex(QueryInfo queryInfo,string name)
        {

            var list = contactRepositories.GetPagedData(queryInfo, name);
            return View(list);
        }

        public ActionResult ContactEdit(int id)
        {
            ViewBag.TypeList = shopRepository.GetAll().ToList();
            ViewBag.id = id;
            var data = contactRepositories.GetByDatabaseID(id);
            return View(data);
        } 
        [HttpPost]
        public ActionResult ContactEdit(string Shopid, int id, string SubAction, HttpPostedFileBase pathdata)
        {
            var data = contactRepositories.GetByDatabaseID(id);
            var user = data._user;
            contactRepositories.Delete(data);

            if (SubAction == "上传合同")
            {
                //服务器的哪个文件夹
                string mapPath = Server.MapPath("~/UploadFile/");
                if (pathdata != null)
                {
                    string FullPath = SaveFile(mapPath, pathdata);
                    if (FullPath != null)
                    {
                        if (Shopid != null)
                        {
                            string shop = Shopid.Substring(1, Shopid.Length - 1);
                            string[] ids = shop.Split(',');
                            int[] newids = Array.ConvertAll<string, int>(ids, delegate(string s) { return int.Parse(s); });
                            for (int i = 0; i < newids.Length; i++)
                            {
                                Contact contact = new Contact();
                                var shop1 = shopRepository.GetByDatabaseID(newids[i]);
                                contact.shop = shop1;
                                contact.CName = pathdata.FileName;
                                contact.FileUrl = FullPath;
                                contact._user = user;
                                contact.ContacTime = DateTime.Now;
                                contactRepositories.Save(contact);
                            }

                        }

                    }
                    return Content("<script>alert('上传成功!!');location.href='../ContactIndex'</script>");

                }


            }
            return View();
        } 
        




        //保存文件
        public string SaveFile(string mapPath, HttpPostedFileBase pathdata)
        {

            string year = DateTime.Now.Year.ToString();
            string month = DateTime.Now.Month.ToString();
            string day = DateTime.Now.Day.ToString();
            string path = string.Empty;

            //得到文件夹完全路径
            string pathMonth = mapPath + year + "-" + month;

            //判断文件是否存在
            if (!Directory.Exists(pathMonth))//判断文件夹是否存在
            {
                //创建月份文件夹
                Directory.CreateDirectory(pathMonth);
            }

            //得到日志文件的名称
            string filename = pathdata.FileName;

            //得到日志文件的完整路径
            path = pathMonth + "/" + filename;
            pathdata.SaveAs(path);
            return path;
        }
    }
}
