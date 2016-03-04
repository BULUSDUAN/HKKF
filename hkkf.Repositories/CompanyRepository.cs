using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using hkkf.Models;
using JieNuo.Data.NHibernates;
using NHibernate.Linq;

namespace hkkf.Repositories
{
    public class CompanyRepository : NHibernateRepository<Company, int>
    {
        public IDictionary GetAuto()
        {
            return GetSession()
                .Linq<Company>()
                .Skip(10).Take(10)
                .ToDictionary(e => e.ID.ToString(), e => e.Name);
        }
    }
}
