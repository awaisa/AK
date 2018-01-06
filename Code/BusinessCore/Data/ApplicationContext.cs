using BusinessCore.Domain;
using BusinessCore.Domain.Auditing;
using BusinessCore.Domain.Financials;
using BusinessCore.Domain.Items;
using BusinessCore.Domain.Purchases;
using BusinessCore.Domain.Sales;
using BusinessCore.Domain.Security;
using BusinessCore.Domain.TaxSystem;
using BusinessCore.EntityMappings;
using BusinessCore.Security;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Security.Principal;

namespace BusinessCore.Data
{
    public class ApplicationContext : DbContext, IDbContext
    {
        public IAppPrincipal _appPrincipal;
        public IAppPrincipal AppPrincipal
        {
            get { return _appPrincipal; }
            set { _appPrincipal = value; }
        }
        public ApplicationContext(IAppPrincipal principal, DbContextOptions options) : base(options)
        {
            _appPrincipal = principal;
        }
        #region DbSets

        public DbSet<AccountClass> AccountClasses { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<GeneralLedgerHeader> GeneralLedgerHeaders { get; set; }
        public DbSet<GeneralLedgerLine> GeneralLedgerLines { get; set; }
        public DbSet<JournalEntryHeader> JournalEntryHeaders { get; set; }
        public DbSet<JournalEntryLine> JournalEntryLines { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Party> Parties { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Measurement> Measurements { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemCategory> ItemCategories { get; set; }
        public DbSet<ItemBrand> ItemBrands { get; set; }
        public DbSet<ItemModel> ItemModels { get; set; }
        public DbSet<SalesQuoteHeader> SalesQuoteHeaders { get; set; }
        public DbSet<SalesQuoteLine> SalesQuoteLines { get; set; }
        public DbSet<SalesOrderHeader> SalesOrderHeaders { get; set; }
        public DbSet<SalesOrderLine> SalesOrderLines { get; set; }
        public DbSet<SalesDeliveryHeader> SalesDeliveryHeaders { get; set; }
        public DbSet<SalesDeliveryLine> SalesDeliveryLines { get; set; }
        public DbSet<SalesReceiptHeader> SalesReceiptHeaders { get; set; }
        public DbSet<SalesReceiptLine> SalesReceiptLines { get; set; }
        public DbSet<SalesInvoiceHeader> SalesInvoiceHeaders { get; set; }
        public DbSet<SalesInvoiceLine> SalesInvoiceLines { get; set; }
        public DbSet<PurchaseOrderHeader> PurchaseOrderHeaders { get; set; }
        public DbSet<PurchaseOrderLine> PurchaseOrderLines { get; set; }
        public DbSet<PurchaseReceiptHeader> PurchaseReceiptHeaders { get; set; }
        public DbSet<PurchaseReceiptLine> PurchaseReceiptLines { get; set; }
        public DbSet<PurchaseInvoiceHeader> PurchaseInvoiceHeaders { get; set; }
        public DbSet<PurchaseInvoiceLine> PurchaseInvoiceLines { get; set; }
        public DbSet<SequenceNumber> SequenceNumbers { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanySetting> CompanySettings { get; set; }
        public DbSet<FinancialYear> FiscalYears { get; set; }
        public DbSet<Tax> Taxes { get; set; }
        public DbSet<TaxGroup> TaxGroups { get; set; }
        public DbSet<ItemTaxGroup> ItemTaxGroups { get; set; }
        public DbSet<TaxGroupTax> TaxGroupTax { get; set; }
        public DbSet<ItemTaxGroupTax> ItemTaxGroupTax { get; set; }
        public DbSet<GeneralLedgerSetting> GeneralLedgerSettings { get; set; }
        public DbSet<PaymentTerm> PaymentTerms { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<AuditableEntity> AuditableEntities { get; set; }
        public DbSet<AuditableAttribute> AuditableAttributes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<SecurityRole> SecurityRoles { get; set; }
        public DbSet<SecurityPermission> SecurityPermissions { get; set; }
        public DbSet<SecurityUserRole> SecurityUserRoles { get; set; }
        public DbSet<SecurityGroup> SecurityGroups { get; set; }
        public DbSet<SecurityRolePermission> SecurityRolePermissions { get; set; }
        public DbSet<MainContraAccount> MainContraAccounts { get; set; }
        #endregion

        #region Methods
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            //modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.OneToManyCascadeDeleteConvention>();
            modelBuilder.AddEntityConfigurationsFromAssembly(GetType().Assembly);

            foreach (var type in modelBuilder.Model.GetEntityTypes())
            {
                var method = SetGlobalQueryMethod.MakeGenericMethod(type.ClrType);
                method.Invoke(this, new object[] { modelBuilder });
            }

            base.OnModelCreating(modelBuilder);
        }
        static readonly MethodInfo SetGlobalQueryMethod = typeof(ApplicationContext).GetMethods(BindingFlags.Public | BindingFlags.Instance)
                                                        .Single(t => t.IsGenericMethod && t.Name == "SetGlobalQuery");
        public void SetGlobalQuery<T>(ModelBuilder builder) where T : BaseEntity
        {
            builder.Entity<T>().HasKey(e => e.Id);
            //Debug.WriteLine("Adding global query for: " + typeof(T));
            builder.Entity<T>().HasQueryFilter(e => !e.Deleted);
        }

        public override int SaveChanges()
        {
            //var user = "System";
            //if (string.IsNullOrEmpty(_principal?.Username))
            //    SaveAuditLog(user);
            //else
            //    SaveAuditLog(_principal?.Username);

            // CAN BE USE IN THE FUTURE : Track Created and Modified fields Automatically with Entity Framework Code First

            var entities = ChangeTracker.Entries().Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified || x.State == EntityState.Deleted));

            //var currentUsername = HttpContext.Current != null && HttpContext.Current.User != null
            //    ? HttpContext.Current.User.Identity.Name
            //    : "Anonymous";
            int? userId = _appPrincipal?.UserId == 0 ? null : _appPrincipal?.UserId;
            foreach (var entity in entities)
            {
                if (entity.State == EntityState.Added)
                {
                    //((BaseEntity)entity.Entity).Id = 0; Set it to 0 so it won't break Identity constraints (Auto Increment) in DB
                    ((BaseEntity)entity.Entity).CreatedOn = DateTime.Now;
                    ((BaseEntity)entity.Entity).CreatedById = userId;
                    ((BaseEntity)entity.Entity).ModifiedOn = DateTime.Now;
                    ((BaseEntity)entity.Entity).ModifiedById = userId;

                    #region New entities insert against current user's companyId
                    var companyBaseEntity = entity.Entity as ICompanyBaseEntity;
                    if (companyBaseEntity != null)
                    {
                        if (_appPrincipal != null)
                            companyBaseEntity.CompanyId = (int)_appPrincipal?.CompanyId;
                    }
                    #endregion
                }
                else if(entity.State == EntityState.Modified)
                {
                    ((BaseEntity)entity.Entity).ModifiedOn = DateTime.Now;
                    ((BaseEntity)entity.Entity).ModifiedById = userId;
                }
                else if (entity.State == EntityState.Deleted)
                {
                    entity.State = EntityState.Modified;
                    ((BaseEntity)entity.Entity).Deleted = true;
                    ((BaseEntity)entity.Entity).ModifiedOn = DateTime.Now;
                    ((BaseEntity)entity.Entity).ModifiedById = userId;
                }
            }

            var ret = base.SaveChanges();

            //UpdateAuditLogRecordId();

            return ret;
        }

        /// <summary>
        /// Get DbSet
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <returns>DbSet</returns>
        public new DbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity
        {
            return base.Set<TEntity>();
        }
        #endregion
    }
}
