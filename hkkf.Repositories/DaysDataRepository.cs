using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using hkkf.Common;
using hkkf.Models;
using NHibernate.Linq;

namespace hkkf.Repositories
{
   public class DaysDataRepository: NHibernateRepository<DaysData, int>
    {
       public DaysData GetDataByDate(DateTime date)
       {
           return GetSession().Linq<DaysData>()
               .Where(p => p.EndDateTime >= date && p.BeginDateTime <= date)
               .FirstOrDefault();
       }
       public DaysData Getdata()
       {
            return GetSession().Linq<DaysData>()
               .FirstOrDefault();
       }
    

    }
}
