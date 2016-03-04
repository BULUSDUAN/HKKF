using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JieNuo.Data;
using hkkf.Common;
using hkkf.Models;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Mapping;


namespace hkkf.Repositories
{
    public class UserEnmTypeRepository : NHibernateRepository<User_Enm_Type, int>
    {
        public PagedData<User_Enm_Type> GetUserEnmType(QueryInfo queyInfo, string name)
        {
            return GetSession().Linq<User_Enm_Type>()
                .WhereIf(p => p.Name.Contains(name), name.IsNotNullAndEmpty())
                .Page(queyInfo);
        }



        public bool ExistUserEnmTypeName(string Name)
        {
            return GetSession().Linq<User_Enm_Type>()
               .Where(u => u.Name == Name).Any();
        }

        public bool ExistUserEnmTypeID(int id)
        {
            return GetSession().Linq<User_Enm_Type>()
               .Where(u => u.ID == id).Any();
        }
    }
}
