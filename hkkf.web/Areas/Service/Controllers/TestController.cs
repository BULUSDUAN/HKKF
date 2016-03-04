using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using hkkf.Models;
using hkkf.Repositories;
using hkkf.web.Areas.Service.Common;
using NHibernate.Mapping;

namespace hkkf.web.Areas.Service.Controllers
{
    [NavigationRoot("客服考试")]
    [SmartAuthorize]
    [SmartMasterPage]
    public class TestController : Controller
    {
        //
        // GET: /Service/Test/
       TestproblemRepository testproblemRepository=new TestproblemRepository();
       ExamPagesRepository examPagesRepository = new ExamPagesRepository();
       kfGradeRepository  kfGrade=new kfGradeRepository();
       ShopRepository shopTypeRepositories=new ShopRepository();
       WrongRepository Wrongrepository=new WrongRepository();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Test()
        {

            return View();
        }
        
        public ActionResult TestProblem()
        {
            var uname = this.Users().userName;
            ViewBag.Uname = uname;
            ViewBag.startTime = DateTime.Now;
            ViewBag.TypeList = shopTypeRepositories.GetAll().ToList();        
            return View();
        }

        private int i = 0;

        public string Submit()
        {
            var list = new List<TestProblem>();
            list = testproblemRepository.NoAnswer(this.Users().ID, DateTime.Now);
            if (list.Count != 0)
            {
                string answer = string.Empty;
                foreach (var it in list)
                {
                    answer = answer + it.tihao + ",";
                }
                
                return answer;
            }

            return "";
        }

        public void SaveGrade()
        {
            

        }
        [HttpPost]
        public ActionResult TestProblem(string subAction,string count,string dianpu)
        {
            var list50 =new List<TestProblem>();           
            if (subAction=="开始考试")
            {
                ViewBag.TypeList = shopTypeRepositories.GetAll().ToList();        
                var uname = this.Users().userName;           
                list50 = testproblemRepository.GetList50(DateTime.Now,this.Users().ID);       
                ViewBag.Uname = uname;
                ViewBag.startTime = DateTime.Now;
                if (list50.Count != 0)
                {
                    ViewBag.message = "今天已经考过试！";
                    return View();
                }
                DateTime time = new DateTime();
                string[] str = dianpu.Split(',');
                int ID = 0;
                int TH = 1;
                Random ran=new Random();
                int r=0;
                List<int> li=new List<int>();
                while (examPagesRepository.GetproblemByShopTaoCount()>i)
                {
                    r = ran.Next(examPagesRepository.GetMinID(), examPagesRepository.GetMaxID());
                    if (!li.Contains(r))
                    {
                        if (examPagesRepository.GetExam(r)!=null)
                        {
                            TestProblem testProblem = new TestProblem();
                            testProblem.startTime = DateTime.Now;
                            time = testProblem.startTime;
                            testProblem.endTime = testProblem.startTime.AddMinutes(60);
                            testProblem.userid = this.Users();
                            testProblem.problemid = examPagesRepository.GetByDatabaseID(r);
                            testProblem.tihao = TH;
                            testproblemRepository.Save(testProblem);
                            i++;                         
                            TH++;
                            li.Add(r);
                        }
                       
                    }
                   
                }



                for (int j = 0; j < str.Length - 1; j++)
                {
                    ID = shopTypeRepositories.GetShopsByShopName(str[j]).ID;
                    foreach (var ite in examPagesRepository.GetproblemByShop(ID))
                    {
                        TestProblem testProblem = new TestProblem();
                        testProblem.startTime = DateTime.Now;
                        time = testProblem.startTime;
                        testProblem.endTime = testProblem.startTime.AddMinutes(60);
                        testProblem.userid = this.Users();
                        testProblem.problemid = ite;
                        testProblem.tihao = TH;
                        testproblemRepository.Save(testProblem);
                        i++;
                        TH++;
                    }
                }
                ViewBag.coun = i;
                ViewBag.message = "考试开始！";            
                list50 = testproblemRepository.GetList50(time, this.Users().ID);    
            }
           
            return View(list50);
        }

        public ActionResult Testend()
        {
            kfGrade kf = new kfGrade();
            kf.userid = this.Users();
            kf.time = DateTime.Now;
            double ii = Wrongrepository.GetWrongByAnswer(kf.time, this.Users().ID);
            double cou = testproblemRepository.GetMaxID();
            kf.grade = (Math.Round(ii / cou * 100)).ToString() + "%";
            kfGrade.Save(kf);
            kfGrade grade=testproblemRepository.GetGradeByID(this.Users().ID, DateTime.Now);
            if (grade==null)
            {
                ViewBag.grade = "0";
                return View();
            }
            else
            {
                ViewBag.grade = grade.grade;
                return View(grade);
            }
            
           
          
           
        }
       
       
        public string Grade(int id, string answer,string othoranswer,int check,int ye,string ioi)
        {
          
            char[] arr = answer.ToCharArray();
            TestProblem test=new TestProblem();
            if (ye==1)
            {
                 test = testproblemRepository.AddthisanswerByproblem(this.Users().ID, int.Parse(ioi));
            }
            else
            {
                test = testproblemRepository.AddthisanswerByproblem(this.Users().ID, id);
            }
            if (check==1)
            {
                test.thisanswer = string.Empty;
                for (int i = 0; i < answer.Length; i++)
                {
                    test.thisanswer = test.thisanswer + arr[i] + ",";
                }
            }
            else
            {
                test.thisanswer = answer;   
            }                 
                testproblemRepository.Update(test);                      
            if (answer != testproblemRepository.GetAnswerByproblem(id).TrueAnswer)
            {
                var model = Wrongrepository.GetWrongByAnswer(id, DateTime.Now, this.Users().ID);
                if (model != null)
                {
                    Wrongrepository.Delete(model);
                }
                Wrong wrong = new Wrong();
                wrong.userid = this.Users();
                wrong.time=DateTime.Now;
                wrong.wrong = examPagesRepository.GetByDatabaseID(id);
                Wrongrepository.Save(wrong);
               
            }
            else
            {
                var model = Wrongrepository.GetWrongByAnswer(id, DateTime.Now, this.Users().ID);
                if (model!=null)
                {
                    Wrongrepository.Delete(model);
                }  
            }
           
            TestProblem te = new TestProblem();
            if (othoranswer != null)
            {
                if (ye==1)
                {
                    te = testproblemRepository.GetthisanwserByproblem(this.Users().ID,id);
                }
                else
                {
                    te = testproblemRepository.GetthisanwserByproblem(this.Users().ID, int.Parse(othoranswer));    
                }
               
            }     
            return te.thisanswer;
        }
        public JsonResult TestAnswer(string id,string answer)
        {
            ExamPages proble=testproblemRepository.Getproblem(int.Parse(id));
            
            
           return Json(proble,JsonRequestBehavior.AllowGet);
        }   
    }
}
