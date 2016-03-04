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
    public class UserWorkDayOrNightRepository : NHibernateRepository<UserWorkDayOrNight, int>
    {
       public IEnumerable<UserWorkDayOrNight> GetUserWorkDateNoXiuBan()
        {
            return GetSession().Linq<UserWorkDayOrNight>()
                // .WhereIf(p => p.StudentName.Contains(Name), Name.IsNotNullAndEmpty())
                // .WhereIf(p => p.Type.Name.Contains(StudentType), StudentType.IsNotNullAndEmpty())
                // .WhereIf(p=>p.Sex.ToString().Contains(Sex.ToString()),Sex.ToString().IsNotNullAndEmpty())
                //   .WhereIf(p=>p.Sex==Sex,!Sex.GetType().IsNullableType())
                //.Where(p => p.Sex == b)
                      .Where(p => p.ID != 3);              
                                
        }
       public bool ExistWorkDate(DateTime Date)
       {
           return GetSession().Linq<UserWorkDate>()
               .Where(u => u.WorkDate == Date).Any();
       }
       public UserWorkDate getUserWorkDate(DateTime Date)
       {
           return GetSession().Linq<UserWorkDate>()
               .Where(u => u.WorkDate == Date)
               .FirstOrDefault();
       }
      // // public bool ExistStudentNameNotThisID(string StudentName,int id)
      // // {
      //     // return GetSession().Linq<Student>()
      //       //   .WhereIf(u => u.Name == StudentName).Any(),u.id<>id);
      ////  }
      //  public bool ExistStudentID(int id)
      //  {
      //      return GetSession().Linq<Student>()
      //         .Where(u => u.ID == id).Any();
      //  }
    }
}
