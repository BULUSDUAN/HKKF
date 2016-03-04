using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Exceptions;
using JieNuo.Data.NHibernates;
using JieNuo.ComponentModel;
using JieNuo.Data.Exceptions;
using System.Data.SqlClient;
using NHibernate.Linq;
using Spring.Context;

namespace hkkf.Common
{
    abstract public class NHibernateRepository<TEntity, TKey> : IRepository<TEntity>
        where TEntity : class
    {
        private static TypeInfoAttribute typeInfo;

        static NHibernateRepository()
        {
            typeInfo = AttributeHelper.GetNoInherit<TypeInfoAttribute>(typeof(TEntity)).FirstOrDefault();
            if (typeInfo == null) typeInfo = TypeInfoAttribute.Default;
        }

        virtual public TEntity GetByDatabaseID(TKey id)
        {
            ISession session = GetSession();
            var v = session.Get<TEntity>(id);
            return v;
        }

        virtual public TEntity GetByBusinessID(object id)
        {
            ISession session = GetSession();
            return session.Linq<TEntity>()
                .Where(typeInfo.BusinessIdProperty + " = @0", id)
                .FirstOrDefault();
        }

        virtual public string GetDisplayText(object id)
        {
            ISession session = GetSession();
            return session.Linq<TEntity>()
                .Where(typeInfo.BusinessIdProperty + " = @0", id)
                .Select(typeInfo.TextProperty)
                .Cast<object>()
                .FirstOrDefault()
                .ToString();
        }

        #region CreateNew

        virtual public TEntity CreateNew()
        {
            return CreateNew(typeof(TEntity));
        }

        virtual public TEntity CreateNew<T>()
            where T : TEntity
        {
            return CreateNew(typeof(T));
        }

        virtual public TEntity CreateNew(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (type != typeof(TEntity) && type.IsSubclassOf(typeof(TEntity)) == false)
                throw new ArgumentOutOfRangeException("参数 type 必须是 {0} 或其派生类".FormatWith(typeof(TEntity)));

            try
            {
                return Activator.CreateInstance(type) as TEntity;
            }
            catch (Exception ex)
            {
                throw new RuleException("无法创建类型 {0} 的实例".FormatWith(type), ex);
            }
        }
        #endregion

        virtual public IEnumerable<TEntity> GetAll()
        {
            ISession session = GetSession();
            ICriteria criteria = session.CreateCriteria<TEntity>();
            return criteria.List<TEntity>();
        }

        virtual public void Update(TEntity t)
        {
            ISession session = GetSession();
            using (session.BeginTransaction())
            {
                try
                {
                    session.Update(t);
                    session.Flush();
                    session.Transaction.Commit();
                }
                catch(Exception ex)
                {
                    session.Transaction.Rollback();
                    throw new RuleException(ex.Message, ex);
                }
            }
        }
        
        virtual public void SaveOrUpdate(TEntity t)
        {
            ISession session = GetSession();
            using (session.BeginTransaction())
            {
                try
                {
                    session.Transaction.Commit();
                    session.SaveOrUpdate(t);
                    session.Flush();
                }
                catch (Exception ex)
                {
                    session.Transaction.Rollback();
                    throw new RuleException(ex.Message, ex);
                }
            }
        }

        virtual public TKey Save(TEntity t)
        {
            ISession session = GetSession();

            using (ITransaction tran = session.BeginTransaction())
            {
                try
                {
                    object obj = session.Save(t);
                    tran.Commit();

                    session.Flush();
                    return (TKey)obj;
                }
                catch (GenericADOException ex)
                {
                    tran.Rollback();
                    if (ex.Message.Contains("could not insert") && ex.InnerException is SqlException)
                        throw new CouldNotInsertException(ex.InnerException);
                    throw new RuleException(ex.Message, ex);
                }
                catch (StaleObjectStateException ex)
                {
                    tran.Rollback();
                    throw new RuleException("相关数据被其他用户修改，系统已刷新数据，请确认后再次尝试！", ex);
                }
                catch(Exception ex)
                {
                    //TODO:错误 提示丢失信息
                    session.Transaction.Rollback();
                    string message = ex.Message;
                    if (ex.InnerException != null) message += ex.InnerException.Message;
                    throw new RuleException(message, ex);
                }
            }
        }

        virtual public void Delete(TEntity t)
        {
            ISession session = GetSession();

            using (session.BeginTransaction())
            {
                try
                {
                    session.Delete(t);
                    session.Transaction.Commit();
                }
                catch (GenericADOException ex)
                {
                    if (ex.Message.Contains("could not delete") && ex.InnerException != null)
                        throw new CouldNotDeleteException(ex.InnerException);
                    throw new RuleException(ex.Message, ex);
                }
                catch (Exception ex)
                {
                    session.Transaction.Rollback();
                    throw new RuleException(ex.Message, ex);
                }
            }
        }

        virtual public void Delete(TKey key)
        {
            ISession session = GetSession();
            using (session.BeginTransaction())
            {
                try
                {
                    TEntity entity = session.Load<TEntity>(key);
                    session.Delete(entity);
                    session.Transaction.Commit();
                }
                catch (Exception ex)
                {
                    session.Transaction.Rollback();
                    throw new RuleException(ex.Message, ex);
                }
            }            
        }

        public void Refresh(TEntity t)
        {
            ISession session = GetSession();
            session.Refresh(t);
        }


        public void Merge<T>(T obj)
        {
            ISession session = GetSession();
            session.Merge(obj);
        }


        virtual public void BeginUpdate(TEntity t)
        {
            ISession session = GetSession();
            if (session.Transaction != null && session.Transaction.IsActive) throw new Exception("当前Session已有事务正在进行中");

            ITransaction tran = session.BeginTransaction();
            session.Update(t);
        }

        virtual public void EndUpdate(TEntity t)
        {
            ISession session = GetSession();
            ITransaction tran = session.Transaction;
            if (tran == null) throw new Exception("当前Session不存在更新事务");

            //if (t is ILifecycle)
            //    (t as ILifecycle).OnUpdate(session);

            try
            {
                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw new RuleException(ex.Message, ex);
            }
        }


        protected virtual ISession GetSession()
        {
            return NHibernateHelper.GetCurrentSession();
        }

    }
}
