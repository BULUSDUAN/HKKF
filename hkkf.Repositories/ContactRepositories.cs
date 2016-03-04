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
    public class ContactRepositories : NHibernateRepository<Contact, int>
    {
        public PagedData<Contact> GetPagedData(QueryInfo queryInfo,string name)
        {
            return GetSession().Linq<Contact>()
                .WhereIf(p=>p.CName.Contains(name),name.IsNotNullAndEmpty())
                .Page(queryInfo);
        }

       
    }
}
