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
   public  class ExamTypeRepository : NHibernateRepository<ExamType, int>
    {

       public PagedData<ExamType> GetPagedData(QueryInfo queryInfo, string name)
       {
           return GetSession().Linq<ExamType>()
               .WhereIf(p => p.EName.Contains(name), name.IsNotNullAndEmpty())
               .Page(queryInfo);
       }

       public bool ExistExamName(string EName)
       {
           return GetSession().Linq<ExamType>()
               .Where(p => p.EName == EName)
               .Any();
       }

    }
}
