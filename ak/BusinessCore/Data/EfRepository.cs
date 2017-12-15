//-----------------------------------------------------------------------
// <copyright file="EfRepository.cs" company="AccountGo">
// Copyright (c) AccountGo. All rights reserved.
// <author>Marvin Perez</author>
// <date>1/11/2015 9:48:38 AM</date>
// </copyright>
//-----------------------------------------------------------------------

using BusinessCore.Data;
using BusinessCore.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Principal;

namespace BusinessCore.Data
{
    public partial class EfRepository<T> : IRepository<T> where T : BaseEntity
    {
        #region Fields
        private readonly string companyIdpropertyName = "CompanyId";
        private readonly IDbContext _context;
        private DbSet<T> _entities;
        private readonly Security.AppPrincipal _principal;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="context">Object context</param>
        public EfRepository(IDbContext context,
                            Security.AppPrincipal principal)
        {
            this._context = context;
            this._principal = principal;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get entity by identifier
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Entity</returns>
        public virtual T GetById(int id)
        {
            //see some suggested performance optimization (not tested)
            //http://stackoverflow.com/questions/11686225/dbset-find-method-ridiculously-slow-compared-to-singleordefault-on-id/11688189#comment34876113_11688189
            return this.Table.FirstOrDefault(t => t.Id == id);
        }

        /// <summary>
        /// Insert entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void Insert(T entity)
        {
            //try
            //{
            if (entity == null)
                throw new ArgumentNullException("entity");

            this.Entities.Add(entity);

            this._context.SaveChanges();

            //}
            //catch (DbEntityValidationException dbEx)
            //{
            //    var msg = string.Empty;

            //    foreach (var validationErrors in dbEx.EntityValidationErrors)
            //        foreach (var validationError in validationErrors.ValidationErrors)
            //            msg += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;

            //    var fail = new Exception(msg, dbEx);
            //    //Debug.WriteLine(fail.Message, fail);
            //    throw fail;
            //}
        }

        /// <summary>
        /// Insert entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual void Insert(IEnumerable<T> entities)
        {
            //try
            //{
            if (entities == null)
                throw new ArgumentNullException("entities");
            var hasCompanyId = HasCompanyId();
            foreach (var entity in entities)
            {
                this.Entities.Add(entity);
            }
            this._context.SaveChanges();
            //}
            //catch (DbEntityValidationException dbEx)
            //{
            //    var msg = string.Empty;

            //    foreach (var validationErrors in dbEx.EntityValidationErrors)
            //        foreach (var validationError in validationErrors.ValidationErrors)
            //            msg += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;

            //    var fail = new Exception(msg, dbEx);
            //    //Debug.WriteLine(fail.Message, fail);
            //    throw fail;
            //}
        }

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void Update(T entity)
        {
            //try
            //{
            if (entity == null)
                throw new ArgumentNullException("entity");

            this._context.SaveChanges();
            //}
            //catch (DbEntityValidationException dbEx)
            //{
            //    var msg = string.Empty;

            //    foreach (var validationErrors in dbEx.EntityValidationErrors)
            //        foreach (var validationError in validationErrors.ValidationErrors)
            //            msg += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);

            //    var fail = new Exception(msg, dbEx);
            //    //Debug.WriteLine(fail.Message, fail);
            //    throw fail;
            //}
        }

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void Delete(T entity)
        {
            //try
            //{
            if (entity == null)
                throw new ArgumentNullException("entity");

            this.Entities.Remove(entity);

            this._context.SaveChanges();
            //}
            //catch (DbEntityValidationException dbEx)
            //{
            //    var msg = string.Empty;

            //    foreach (var validationErrors in dbEx.EntityValidationErrors)
            //        foreach (var validationError in validationErrors.ValidationErrors)
            //            msg += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);

            //    var fail = new Exception(msg, dbEx);
            //    //Debug.WriteLine(fail.Message, fail);
            //    throw fail;
            //}
        }

        /// <summary>
        /// Delete entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual void Delete(IEnumerable<T> entities)
        {
            //try
            //{
            if (entities == null)
                throw new ArgumentNullException("entities");

            foreach (var entity in entities)
                this.Entities.Remove(entity);

            this._context.SaveChanges();
            //}
            //catch (DbEntityValidationException dbEx)
            //{
            //    var msg = string.Empty;

            //    foreach (var validationErrors in dbEx.EntityValidationErrors)
            //        foreach (var validationError in validationErrors.ValidationErrors)
            //            msg += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);

            //    var fail = new Exception(msg, dbEx);
            //    //Debug.WriteLine(fail.Message, fail);
            //    throw fail;
            //}
        }

        public IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> queryable = Entities;
            if (HasCompanyId())
            {
                if (_principal != null)
                    queryable = queryable.Where(MakeCurrentUserCompanyFilter(companyIdpropertyName, _principal.CompanyId));
            }

            foreach (Expression<Func<T, object>> includeProperty in includeProperties)
            {
                queryable = queryable.Include<T, object>(includeProperty);
            }

            return queryable;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a table
        /// </summary>
        public virtual IQueryable<T> Table
        {
            get
            {
                if (HasCompanyId())
                {
                    if (_principal != null)
                        return this.Entities.Where(MakeCurrentUserCompanyFilter(companyIdpropertyName, _principal.CompanyId));
                }
                return this.Entities;
            }
        }

        /// <summary>
        /// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
        /// </summary>
        public virtual IQueryable<T> TableNoTracking
        {
            get
            {
                if (HasCompanyId())
                {
                    if (_principal != null)
                        return this.Entities.AsNoTracking().Where(MakeCurrentUserCompanyFilter(companyIdpropertyName, _principal.CompanyId));
                }
                return this.Entities.AsNoTracking();
            }
        }

        /// <summary>
        /// Entities
        /// </summary>
        protected virtual DbSet<T> Entities
        {
            get
            {
                if (_entities == null)
                {
                    _entities = _context.Set<T>();
                }
                return _entities;
            }
        }

        #endregion

        #region HACK: Current User Company filter

        //private BusinessCore.Domain.Security.User _currentUser;

        //private BusinessCore.Domain.Security.User CurrentUser
        //{
        //    get
        //    {
        //        if (_currentUser == null)
        //            _currentUser = _context.Set<BusinessCore.Domain.Security.User>().Where(u => u.Username == WindowsIdentity.GetCurrent().Name).FirstOrDefault();
        //        return _currentUser;
        //    }
        //}

        bool HasCompanyId()
        {
            return typeof(T).GetProperty(companyIdpropertyName) != null;
        }

        static Expression<Func<T, bool>> MakeCurrentUserCompanyFilter(string propertyName, object value)
        {
            var type = typeof(T);

            var property = type.GetProperty(propertyName);

            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var constantValue = Expression.Constant(value);

            var equality = Expression.Equal(propertyAccess, constantValue);

            return Expression.Lambda<Func<T, bool>>(equality, parameter);
        }

        #endregion
    }
}

