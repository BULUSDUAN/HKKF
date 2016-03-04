using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;


namespace hkkf.Common
{

    public static class Extensions
    {
        public static IQueryable<T> WhereDateBetween<T>(this IQueryable<T> queryable, Expression<Func<T, DateTime?>> expression,
            DateTime? startDate, DateTime? endDate)
        {
            if (queryable == null) throw new ArgumentNullException("queryable");
            if (expression == null) throw new ArgumentNullException("expression");

            if (startDate == null && endDate == null) return queryable;

            var body = expression.Body;
            var dateTimeValue = body;//Expression.Convert(body, typeof(DateTime));

            Expression afterStart = null,
                beforeEnd = null;
            if (startDate.HasValue)
                afterStart = Expression.GreaterThanOrEqual(dateTimeValue, Expression.Constant(startDate, typeof(DateTime?)));
            if (beforeEnd != null)
                beforeEnd = Expression.LessThan(dateTimeValue, Expression.Constant(endDate.Value.Date.AddDays(1), typeof(DateTime?)));

            Expression exp = Expression.NotEqual(body, Expression.Constant(null));

            if (afterStart != null) exp = Expression.And(exp, afterStart);
            if (beforeEnd != null) exp = Expression.And(exp, beforeEnd);

            var lambda = Expression.Lambda<Func<T, bool>>(exp, expression.Parameters);
            return queryable.Where(lambda);
        }
    }
}
