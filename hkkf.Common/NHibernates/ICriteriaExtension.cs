using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Criterion;

namespace NHibernate.Criterion
{
    using NHibernate;

    public static class ICriteriaExtension
    {
        public static ICriteria AddIf(this ICriteria criteria, ICriterion expression, bool condition)
        {
            if (condition) return criteria.Add(expression);
            else return criteria;
        }
    }
}
