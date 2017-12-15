using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;

namespace WebApiCore.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public abstract class BaseController : Controller
    {
        string FromQueryOrForm(string key)
        {
            if (Request.Method == "POST")
                return FromForm(key);
            else
                return FromQuery(key);
        }

        string FromQuery(string key)
        {
            if (Request.Query.Any())
            {
                var dict = Request.Query.ToDictionary(x => x.Key, x => x.Value);
                var start = dict[key].FirstOrDefault();
                return start;
            }
            return null;
        }

        string FromForm(string key)
        {
            if (Request.Form.Any())
            {
                var dict = Request.Form.ToDictionary(x => x.Key, x => x.Value);
                var start = dict[key].FirstOrDefault();
                return start;
            }
            return null;
        }

        public int Getstart()
        {
            string val = null;
                val = FromQueryOrForm("start");

            if (val != null)
                return Convert.ToInt32(val);

            return 1;
        }
        public int GetPageSize()
        {
            string val = null;
            val = FromQueryOrForm("length");

            if (val != null)
                return Convert.ToInt32(val);
            return 10;
        }
        public int GetPageNumber()
        {
            string val = null;
            val = FromQueryOrForm("draw");

            if (val != null)
                return Convert.ToInt32(val);

            return 1;
        }

        public string GetSearchedText()
        {
            string val = null;
            val = FromQueryOrForm("search[value]");

            if (val != null)
                return Convert.ToString(val);

            return "";
        }
        public string GetSortColumn()
        {
            var sortcolumn = string.Empty;
            var val1 = FromQueryOrForm("order[0][column]");
            var val2 = FromQueryOrForm("columns[" + val1 + "][data]");

            if (val2 != null)
                sortcolumn =  Convert.ToString(val2);

            return sortcolumn;
        }

        public string GetSortOrder()
        {
            var sortorder = string.Empty;
            var val = FromQueryOrForm("order[0][dir]");

            if (val != null)
                sortorder = Convert.ToString(val);
            
            return sortorder;
        }

        public IQueryable<TEntity> OrderBy<TEntity>(IQueryable<TEntity> source, string orderByProperty,
                          bool desc)
        {
            string command = desc ? "OrderByDescending" : "OrderBy";
            var type = typeof(TEntity);
            var property = type.GetProperty(orderByProperty, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
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