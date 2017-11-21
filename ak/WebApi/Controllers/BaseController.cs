using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Linq.Expressions;

namespace WebApiCore.Controllers
{
    public abstract class BaseController : Controller
    {
        public int Getstart()
        {
            if (Request.Method == "POST")
            {
                var dict = Request.Form.ToDictionary(x => x.Key, x => x.Value);

                var start = dict["start"].FirstOrDefault();
                if (start != null)
                    return Convert.ToInt32(start);
            }
            else
            {
                var dict = Request.Query.ToDictionary(x => x.Key, x => x.Value);

                var start = dict["start"].FirstOrDefault();
                if (start != null)
                    return Convert.ToInt32(start);
            }
            return 1;
        }
        public int GetPageSize()
        {
            if (Request.Method == "POST")
            {
                var dict = Request.Form.ToDictionary(x => x.Key, x => x.Value);

                var length = dict["length"].FirstOrDefault();
                if (length != null)
                    return Convert.ToInt32(length);
            }
            else
            {
                var dict = Request.Query.ToDictionary(x => x.Key, x => x.Value);

                var length = dict["length"].FirstOrDefault();
                if (length != null)
                    return Convert.ToInt32(length);
            }
            return 10;
        }
        public int GetPageNumber()
        {
            if (Request.Method == "POST")
            {
                var dict = Request.Form.ToDictionary(x => x.Key, x => x.Value);

                var draw = dict["draw"].FirstOrDefault();
                if (draw != null)
                    return Convert.ToInt32(draw);
            }
            else
            {
                var draw = Request.Query["draw"].FirstOrDefault();
                if (draw != null)
                    return Convert.ToInt32(draw);
            }

            return 1;
        }

        public string GetSearchedText()
        {
            if (Request.Method == "POST")
            {
                var dict = Request.Form.ToDictionary(x => x.Key, x => x.Value);

                var search = dict["search[value]"].FirstOrDefault();
                if (search != null)
                    return Convert.ToString(search);
            }
            else
            {
                var search = Request.Query["search[value]"].FirstOrDefault();
                if (search != null)
                    return Convert.ToString(search);
            }
            return "";
        }
        public string GetSortColumn()
        {
            var sortcolumn = string.Empty;
            if (Request.Method == "POST")
            {
                var dict = Request.Form.ToDictionary(x => x.Key, x => x.Value);

                sortcolumn = dict["columns[" + dict["order[0][column]"].FirstOrDefault() + "][data]"].FirstOrDefault();
            }
            else
            {
                var dict = Request.Query.ToDictionary(x => x.Key, x => x.Value);
                sortcolumn = dict["columns[" + dict["order[0][column]"].FirstOrDefault() + "][data]"].FirstOrDefault();
            }
            return sortcolumn;
        }

        public string GetSortOrder()
        {
            var sortorder = string.Empty;
            if (Request.Method == "POST")
            {
                var dict = Request.Form.ToDictionary(x => x.Key, x => x.Value);

                sortorder = dict["order[0][dir]"].FirstOrDefault();
            }
            else
            {
                var dict = Request.Query.ToDictionary(x => x.Key, x => x.Value);

                sortorder = dict["order[0][dir]"].FirstOrDefault();
            }
            return sortorder;
        }

        public IQueryable<TEntity> OrderBy<TEntity>(IQueryable<TEntity> source, string orderByProperty,
                          bool desc)
        {
            string command = desc ? "OrderByDescending" : "OrderBy";
            var type = typeof(TEntity);
            var property = type.GetProperty(orderByProperty);
            if (property == null) return source;
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);
            var resultExpression = Expression.Call(typeof(Queryable), command, new Type[] { type, property.PropertyType },
                                          source.Expression, Expression.Quote(orderByExpression));
            return source.Provider.CreateQuery<TEntity>(resultExpression);
        }

    }
}