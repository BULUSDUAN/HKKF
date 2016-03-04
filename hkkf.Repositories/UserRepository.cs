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
    public class UserRepository : NHibernateRepository<User, int>
    {

        //用于选择用户内容
        public IEnumerable<User> GetData(int kf_DepartMentID,int userTypeID)
        {
            if (userTypeID == Convert.ToInt32(UserEnmType.Person))
            {
                return GetSession()
              .Linq<User>()
              .Where(u => u.UserStateID != UserEnmState.离职)
              .Where(u => u.Type.ID == userTypeID)
              .WhereIf(u => u.DepartMent.ID == kf_DepartMentID, kf_DepartMentID != 1);
            }
            else
            {
                return GetSession()
                .Linq<User>()
                .Where(u => u.UserStateID != UserEnmState.离职)
                .Where(u => u.Type.ID == userTypeID);
            }          
        }

        ShopRepository shopRepository=new ShopRepository();
        public User GetByNameAndPassword(string name, string password)
        {
            return GetSession().Linq<User>()
                .Where(u => u.Name == name && u.Password == password)
                .FirstOrDefault();
        }

        public User GetByName(string name)
        {
            return GetSession().Linq<User>()
                .WhereIf(u => u.Name == name, name.IsNotNullAndEmpty())
                .FirstOrDefault();
        }

        public Boolean ExistUserName(string userName)
        {
            return GetSession().Linq<User>()
                .Where(u => u.Name == userName).Any();
        }

        public Boolean ExistEmail(string email)
        {
            return GetSession().Linq<User>()
                .Where(u => u.Email == email).Any();
        }

        public User Type(int userId)
        {
            return GetSession().Get<User>(userId);
        }

        /// <summary>
        //public PagedData<User> GetUser(QueryInfo queryInfo, int? typeId, string name,string username,string iskf)
        //{
        //    return GetSession()
        //        .Linq<User>()
        //        //.Where(u => u.Type != "1".ToEnum<UserEnmType>() && u.Type != "2".ToEnum<UserEnmType>())
        //        // .WhereIf(u => u.Type == typeId.ToString().ToEnum<UserEnmType>(), typeId != null)
        //        .WhereIf(u => u.Name.Contains(name.Trim()), name.IsNotNullAndEmpty())
        //        .WhereIf(u => u.userName.Contains(username.Trim()), username.IsNotNullAndEmpty())
        //        //.WhereIf(u=>u.Type.ID==typeId,iskf.IsNotNullAndEmpty())
        //        .Page(queryInfo);
        //}
      
        public PagedData<User> GetUserData(QueryInfo queryInfo, string name,string typeID,Kf_DepartMent kf_DepartMent)
        {
            return GetSession()
                .Linq<User>()
                .WhereIf(u=>u.Type.ID==Convert.ToInt64(typeID),typeID!=null)
                .WhereIf(u => u.userName.Contains(name.Trim()), name.IsNotNullAndEmpty())
                .WhereIf(u => u.DepartMent == kf_DepartMent, kf_DepartMent.ID != 1)
                .Page(queryInfo);
        }
    }
}
