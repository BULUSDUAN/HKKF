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
   public class StudentRepository:NHibernateRepository<Student,int>
    {
       public PagedData<Student> GetStudent(QueryInfo queryInfo, string Name, String StudentType, String Sex)
        {
            if (Sex.IsNullOrEmpty())
            {
                return GetSession().Linq<Student>()
                    .WhereIf(p => p.StudentName.Contains(Name), Name.IsNotNullAndEmpty())
                    .WhereIf(p => p.Type.Name.Contains(StudentType), StudentType.IsNotNullAndEmpty())
                    // .WhereIf(p=>p.Sex.ToString().Contains(Sex.ToString()),Sex.ToString().IsNotNullAndEmpty())
                    //   .WhereIf(p=>p.Sex==Sex,!Sex.GetType().IsNullableType())
                   //.Where(p => p.Sex == b)
                   .Page(queryInfo);  
            }
            if (Sex.IsNotNullAndEmpty() && Sex.Contains("不限"))
            {
                return GetSession().Linq<Student>()
                  .WhereIf(p => p.StudentName.Contains(Name), Name.IsNotNullAndEmpty())
                   .WhereIf(p => p.Type.Name.Contains(StudentType), StudentType.IsNotNullAndEmpty())
                    // .WhereIf(p=>p.Sex.ToString().Contains(Sex.ToString()),Sex.ToString().IsNotNullAndEmpty())
                    //   .WhereIf(p=>p.Sex==Sex,!Sex.GetType().IsNullableType())
                   //.Where(p => p.Sex == true)
                  .Page(queryInfo);
            }
            if(Sex.IsNotNullAndEmpty()&&Sex.Contains("男"))
            {
                return GetSession().Linq<Student>()
                  .WhereIf(p => p.StudentName.Contains(Name), Name.IsNotNullAndEmpty())
                   .WhereIf(p => p.Type.Name.Contains(StudentType), StudentType.IsNotNullAndEmpty())
                    // .WhereIf(p=>p.Sex.ToString().Contains(Sex.ToString()),Sex.ToString().IsNotNullAndEmpty())
                    //   .WhereIf(p=>p.Sex==Sex,!Sex.GetType().IsNullableType())
                   .Where(p => p.Sex == true)
                  .Page(queryInfo);              
            }
            if(Sex.IsNotNullAndEmpty()&&Sex.Contains("女"))
            {
                return GetSession().Linq<Student>()
                  .WhereIf(p => p.StudentName.Contains(Name), Name.IsNotNullAndEmpty())
                   .WhereIf(p => p.Type.Name.Contains(StudentType), StudentType.IsNotNullAndEmpty())
                    // .WhereIf(p=>p.Sex.ToString().Contains(Sex.ToString()),Sex.ToString().IsNotNullAndEmpty())
                    //   .WhereIf(p=>p.Sex==Sex,!Sex.GetType().IsNullableType())
                   .Where(p => p.Sex == false)
                  .Page(queryInfo);              
            }
            return GetSession().Linq<Student>()
                  .WhereIf(p => p.StudentName.Contains(Name), Name.IsNotNullAndEmpty())
                   .WhereIf(p => p.Type.Name.Contains(StudentType), StudentType.IsNotNullAndEmpty())
                // .WhereIf(p=>p.Sex.ToString().Contains(Sex.ToString()),Sex.ToString().IsNotNullAndEmpty())
                //   .WhereIf(p=>p.Sex==Sex,!Sex.GetType().IsNullableType())
                   .Where(p => p.Sex == true)
                  .Page(queryInfo);
        }
        public bool ExistStudentName(string StudentName)
        {
            return GetSession().Linq<Student>()
                .Where(u => u.StudentName == StudentName).Any();
        }
       // public bool ExistStudentNameNotThisID(string StudentName,int id)
       // {
           // return GetSession().Linq<Student>()
             //   .WhereIf(u => u.Name == StudentName).Any(),u.id<>id);
      //  }
        public bool ExistStudentID(int id)
        {
            return GetSession().Linq<Student>()
               .Where(u => u.ID == id).Any();
        }
    }
}
