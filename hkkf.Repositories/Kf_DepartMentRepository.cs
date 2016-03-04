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
   public class Kf_DepartMentRepository:NHibernateRepository<Kf_DepartMent,int>
    {
       //如果是总部的，那么都取出来，如果是分部的，那么只取出分部的数据。
       public IEnumerable<Kf_DepartMent> GetData(int kf_DepartMentID)
       {
           return GetSession()
               .Linq<Kf_DepartMent>()
              .WhereIf(u => u.ID == kf_DepartMentID, kf_DepartMentID != 1);               
       }
    }
}
