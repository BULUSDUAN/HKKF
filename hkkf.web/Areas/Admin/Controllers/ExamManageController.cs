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
    [NavigationRoot("试卷管理")]
    [SmartAuthorize]
    [SmartMasterPage]
    public class ExamManageController : Controller
    {
        //
        // GET: /Admin/ExamManage/
        private ExamTypeRepository examTypeRepository=new ExamTypeRepository();
        private ExamPagesRepository examPagesRepository=new ExamPagesRepository();
        private ShopRepository shopRepository = new ShopRepository();
        public ActionResult ExamIndex(QueryInfo queryInfo,int[] ids, string ETypeName, string subAction,int? ETypeid)
        {
            if (subAction == "delete")
            {
                foreach (int userid in ids)
                {
                    examTypeRepository.Delete(userid);
                }
                ViewBag.message = "删除成功！";
            }
            ViewBag.ETypeName = ETypeid.ToString();
            var data= examTypeRepository.GetPagedData(queryInfo, ETypeName);
            return View(data);
        }

        public ActionResult AddExamType(int? id)
        {
            if (id != null)
            {
                var exam = examTypeRepository.GetByDatabaseID(id.Value);
                ViewBag.Edit = "1";
                return View(exam);

            }
            return View();
        }

        [HttpPost]
        public ActionResult AddExamType(int? id, FormCollection collection, string alertMessage, string IsEdit)
        {

            ExamType _examType = new ExamType();
            try
            {

                if (IsEdit == "1")
                {

                    var examEdit = examTypeRepository.GetByDatabaseID(id.Value);
                    TryUpdateModel(examEdit, collection);
                    examTypeRepository.Update(examEdit);
                }
                else
                {
                    TryUpdateModel(_examType, collection);
                    if (examTypeRepository.ExistExamName(_examType.EName))
                    {
                        alertMessage = "添加失败 此类型已存在！";
                        return View(_examType);
                    }
                    examTypeRepository.Save(_examType);
                }

                alertMessage = "操作成功！";
                ViewData["alertMessage"] = alertMessage;
                return RedirectToAction("ExamIndex");
            }
            catch (RuleException ex)
            {
                throw new RuleException(ex.Message, ex);
            }
        }


        public ActionResult ExamPageManange(QueryInfo queryInfo,int[] ids, string Pname, string subAction,int? id)
        {
            if (subAction == "delete")
            {
                foreach (int userid in ids)
                {
                    examPagesRepository.Delete(userid);
                 
                }
                ViewBag.message = "删除成功！";
            }
            ViewBag.ETypeid = id.ToString();
            var data = examPagesRepository.GetPagedData(queryInfo, Pname, id);
            return View(data);
        }

        public ActionResult AddExamPageType(int? id, string IsEdit, int? EtypeID)
        {
            if (EtypeID!=null)
            {
                var examType = examTypeRepository.GetByDatabaseID(EtypeID.Value);
                ViewBag.ETypeid = examType.ID.ToString();
                ViewBag.ETypeName = examType.EName;
            }
          
            if (id != null)
            {
                var exam = examPagesRepository.GetByDatabaseID(id.Value);
                ViewBag.Edit = "1";
                return View(exam);

            }
            return View();
        }
        [HttpPost]
        public ActionResult AddExamPageType(int? id, FormCollection collection, string alertMessage, string _Shop, string IsEdit, string ETypeid)
        {
            ExamPages _examType = new ExamPages();
            try
            {

                if (IsEdit == "1")
                {

                    var examEdit = examPagesRepository.GetByDatabaseID(id.Value);
                    TryUpdateModel(examEdit, collection);
                    if (ETypeid.IsNotNullAndEmpty())
                    {
                        examEdit.ETypeID = examTypeRepository.GetByDatabaseID(Convert.ToInt32(ETypeid));
                    }
                   
                    examPagesRepository.Update(examEdit);
                    ViewData["alertMessage"] = "修改成功！";
                    return View("AddExamPageType");
                }
                else
                {
                    TryUpdateModel(_examType, collection);
                    var examType1=  examTypeRepository.GetByDatabaseID(Convert.ToInt32(ETypeid));
                    _examType.ETypeID = examType1;
                    if (_Shop.IsNotNullAndEmpty())
                    {
                        _examType._Shop = shopRepository.GetShopByShopName(_Shop);
                    }
                    
                    if (examPagesRepository.ExistExamPageName(_examType.Title))
                    {
                        ViewData["alertMessage"] = "添加失败 此标题已存在！";
                        return View(_examType);
                    }
                    examPagesRepository.Save(_examType);
                }

                alertMessage = "操作成功！";
                ViewData["alertMessage"] = alertMessage;
                return View("AddExamPageType", _examType);
            }
            catch (RuleException ex)
            {
                throw new RuleException(ex.Message, ex);
            }
        }
    }
}
