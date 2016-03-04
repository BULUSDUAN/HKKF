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
    public class StudentController : Controller
    {
        //
        // GET: /Admin/Student/
        StudentRepository StudentRepository = new StudentRepository();
        ShopTypeRepositories ShopTypeRepository = new ShopTypeRepositories();
        public ActionResult StudentIndex(QueryInfo queryInfo, int[] ids, string StudentName, string StudentType, String Sex, string alertMessage, string subAction)
        {
            ViewBag.TypeList = ShopTypeRepository.GetAll().ToList().Select(p => new SelectListItem { Text = p.Name, Value = p.ID.ToString() });
            if (subAction == "delete")
            {
                foreach (int StudentID in ids)
                {
                    StudentRepository.Delete(StudentID);
                }
               alertMessage = "删除成功！";
            }
            PagedData<Student> data = StudentRepository.GetStudent(queryInfo,StudentName,StudentType,Sex);
            return View(data);
        }


        public ActionResult AddStudent(int? id)
        {
            ViewBag.TypeList = ShopTypeRepository.GetAll().ToList().Select(p => new SelectListItem { Text = p.Name, Value = p.ID.ToString() });
            if (id != null)
            {
                var Student = StudentRepository.GetByDatabaseID(id.Value);
                ViewBag.Edit = "1";
                return View(Student);

            }
            return View();
        }
        [HttpPost]
        public ActionResult AddStudent(FormCollection collection, string alertMessage, string IsEdit, int? id)
        {

            Student Student = new Student();
            ViewBag.TypeList = ShopTypeRepository.GetAll().ToList()
                .Select(p => new SelectListItem 
                {
                 Text=p.Name,
                 Value=p.ID.ToString()
                }); 
            try
            {

                if (IsEdit == "1")
                {
                    var shopEdit = StudentRepository.GetByDatabaseID(id.Value);
                    TryUpdateModel(shopEdit, collection);
                    StudentRepository.Update(shopEdit);
                   // alertMessage = "修改成功";
                   //  ViewBag.message = alertMessage;
                   // return View(shopEdit);
                }
                else
                {
                    TryUpdateModel(Student, collection);
                    if (StudentRepository.ExistStudentName(Student.StudentName))
                    {
                        alertMessage = "添加失败 该学生已存在！";
                        ViewBag.alertMessage = alertMessage;
                        return View(Student);
                    }
                    else
                    {
                        alertMessage = "添加成功！";
                    }
                    StudentRepository.Save(Student);
                }
                return RedirectToAction("StudentIndex", new{ alertMessage = alertMessage });
            }
            catch (RuleException ex)
            {
                throw new RuleException(ex.Message, ex);
            }
        }

    }

}
