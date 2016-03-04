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
   public class WorkDateRepository:NHibernateRepository<UserWorkDate,int>
    {
       //根据排班的日期生成UserWorkDate，并且返回这个时间段的listUserWorkDate
       public List<UserWorkDate> PaiBan(DateTime localStartDate, DateTime localEndDate)
       {
           List<UserWorkDate> listUserWorkDate = this.GetAll()
               .Where(it => it.WorkDate <= localEndDate)
               .Where(it => it.WorkDate >= localStartDate)
               .ToList();

           bool ifCheck = false;
           //如果原来没有排过班，则插入排班的日期。
           for (DateTime Date = localStartDate; Date <= localEndDate; Date = Date.AddDays(1))
           {
               //此处是为了减少和数据库的连接次数，所以取出来后来判断。
               UserWorkDate workDate = listUserWorkDate.Where(it => it.WorkDate == Date).FirstOrDefault();
               if (workDate == null)
               {
                   UserWorkDate LocalWorkDate = new UserWorkDate();
                   LocalWorkDate.WorkDate = Date;
                   LocalWorkDate.Year = Date.Year;
                   LocalWorkDate.Month = Date.Month;
                   LocalWorkDate.UpdateTime = System.DateTime.Now.Date;
                   this.Save(LocalWorkDate);
                   ifCheck = true;
               }
           }
           //排班过了以后，再取出来一次。
           if (ifCheck == true)
           {
               listUserWorkDate = this.GetAll()
                .Where(it => it.WorkDate <= localEndDate)
                .Where(it => it.WorkDate >= localStartDate)
                .ToList();
           }
           return listUserWorkDate; 
       }

       public PagedData<UserWorkDate> GetUserWorkDate(QueryInfo queryInfo,DateTime startDate,DateTime endDate)
        {
            if (startDate != null && endDate != null)
            {
                
                return GetSession().Linq<UserWorkDate>()
                    // .WhereIf(p => p.StudentName.Contains(Name), Name.IsNotNullAndEmpty())
                    // .WhereIf(p => p.Type.Name.Contains(StudentType), StudentType.IsNotNullAndEmpty())
                    // .WhereIf(p=>p.Sex.ToString().Contains(Sex.ToString()),Sex.ToString().IsNotNullAndEmpty())
                    //   .WhereIf(p=>p.Sex==Sex,!Sex.GetType().IsNullableType())
                    //.Where(p => p.Sex == b)
                      .Where(p => p.WorkDate>=startDate)
                      .Where(p => p.WorkDate<=endDate)                  
                      .Page(queryInfo);
            }
            else
            { 
               return GetSession().Linq<UserWorkDate>()
                    // .WhereIf(p => p.StudentName.Contains(Name), Name.IsNotNullAndEmpty())
                    // .WhereIf(p => p.Type.Name.Contains(StudentType), StudentType.IsNotNullAndEmpty())
                    // .WhereIf(p=>p.Sex.ToString().Contains(Sex.ToString()),Sex.ToString().IsNotNullAndEmpty())
                    //   .WhereIf(p=>p.Sex==Sex,!Sex.GetType().IsNullableType())
                    //.Where(p => p.Sex == b)
                     // .Where(p => p.WorkDate.Date >= startDate.Date)
                     // .Where(p => p.WorkDate.Date <= endDate.Date)
                      .Page(queryInfo);
            }
                    
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
