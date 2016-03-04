using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using hkkf.Common;
using hkkf.Models;
using JieNuo.Data;
using NHibernate.Linq;
using NHibernate.Properties;

namespace hkkf.Repositories
{
    public class TestproblemRepository : NHibernateRepository<TestProblem, int>
    {
        public int GetCount(int userid,int promblemID)
        {
            return GetSession()
                .Linq<TestProblem>()
                .Where(e => e.userid.ID==userid)
                .Where(e => e.problemid.ID == promblemID)
                .Count();

        }

        public PagedData<kfGrade> Score(QueryInfo queryInfo, DateTime? time, string name)
        {
            return GetSession()
                .Linq<kfGrade>()
                .WhereIf(e => e.userid.Name == name,name.IsNotNullAndEmpty())
                .WhereIf(e => e.time > time.Value.Date && time.Value.Date.AddDays(1) >e.time, time!=null)
                .Page(queryInfo);
        } 
        public List<TestProblem> NoAnswer(int userid,DateTime time)
        {
            return GetSession()
                .Linq<TestProblem>()
                .Where(p => p.userid.ID == userid)
                .Where(p => p.startTime <= time && p.endTime >= time)
                .Where(p => p.thisanswer == null)
                .ToList();

        }
        public int GetMaxID()
        {
            return GetSession()
                .Linq<TestProblem>()
                .ToList()
                .OrderByDescending(p => p.tihao)
                .FirstOrDefault().tihao;
        }
        public List<TestProblem> GetList50(DateTime dateTime,int ID)
        {
            return GetSession()
                .Linq<TestProblem>()
                .Where(p => p.startTime > dateTime.Date && p.startTime < dateTime.Date.AddDays(1))
                .Where(p=>p.userid.ID==ID)
                .ToList();
        }

        public ExamPages Getproblem(int id)
        {
            return GetSession()
                .Linq<ExamPages>()
                .Where(p => p.ID == id)
                .FirstOrDefault();

        }

        public ExamPages GetAnswerByproblem(int id)
        {
            return GetSession()
                .Linq<ExamPages>()
                .Where(p => p.ID == id)
                .FirstOrDefault();

        }

        public TestProblem GetthisanwserByproblem(int ID, int problem)
        {
            return GetSession()
                .Linq<TestProblem>()
                .Where(p => p.userid.ID == ID)
                .Where(p => p.problemid.ID == problem)
                .FirstOrDefault();
        }
        public TestProblem AddthisanswerByproblem(int userid,int problem)
        {
            return GetSession()
                .Linq<TestProblem>()
                .Where(p => p.userid.ID == userid)
                .Where(p => p.problemid.ID == problem)
              
                .FirstOrDefault();


        }
        public kfGrade GetGradeByID(int id,DateTime time)
        {
            return GetSession()
                .Linq<kfGrade>()
                .Where(p => p.userid.ID == id)
                .Where(P => P.time > time.Date && time.Date.AddDays(1) > P.time)
                .FirstOrDefault();
        }
    }
}
