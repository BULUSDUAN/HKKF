using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using hkkf.Common;
using hkkf.Models;
using JieNuo.Data;
using NHibernate.Linq;

namespace hkkf.Repositories
{
    public class ExamPagesRepository : NHibernateRepository<ExamPages, int>
    {
        public PagedData<ExamPages> GetPagedData(QueryInfo queryInfo, string pageName,int? Etype)
        {
            return GetSession().Linq<ExamPages>()
                .WhereIf(p => p.ETypeID.ID == Etype.Value, Etype!=null)
                .WhereIf(p => p.Title.Contains(pageName), pageName.IsNotNullAndEmpty())
                .Page(queryInfo);
        }
        public int GetMaxID()
        {
            return GetSession()
                .Linq<ExamPages>()
                .Where(p => p._Shop.ID == null)
                .ToList()
                .OrderByDescending(p => p.ID)
                .FirstOrDefault().ID;
        }
        public int GetMinID()
        {
            return GetSession()
                .Linq<ExamPages>()
                .Where(p => p._Shop.ID == null)
                .ToList()
                .OrderBy(p => p.ID)
                .FirstOrDefault().ID;
        }
        public ExamPages GetExam(int id)
        {
            return GetSession()
                .Linq<ExamPages>()
                .Where(p => p.ID == id)
                .FirstOrDefault();
        }
        public int GetproblemByShopTaoCount()
        {
            return GetSession().Linq<ExamPages>()
                .Where(p => p._Shop.ID == null)
                .Count();
        }
        public bool ExistExamPageName(string Title)
        {
            return GetSession().Linq<ExamPages>()
                .WhereIf(p => p.Title == Title, Title.IsNotNullAndEmpty())
                .Any();

        }
        public List<ExamPages> GetproblemByShop(int shopID)
        {
            return GetSession().Linq<ExamPages>()
                .Where(p => p._Shop.ID == shopID)
                .ToList();


        }

    }
}
