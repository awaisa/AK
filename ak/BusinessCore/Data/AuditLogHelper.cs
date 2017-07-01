using BusinessCore.Domain.Auditing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System.Linq;
using System.Reflection;

namespace BusinessCore.Data
{
    public class AuditLogHelper
    {
        static ApplicationContext _context;
        static AuditableEntity _auditableEntity = null;

        public static IDictionary<DateTime, EntityEntry> addedEntities = new Dictionary<DateTime, EntityEntry>();

        public static List<AuditLog> GetChangesForAuditLog(EntityEntry dbEntry, string username, ApplicationContext context)
        {
            _context = context;
            var result = new List<AuditLog>();

            // Get the Table() attribute, if one exists
            Type entryType = dbEntry.Entity.GetType();

            TableAttribute tableAttr = entryType.GetTypeInfo().GetCustomAttributes(typeof(TableAttribute), false).SingleOrDefault() as TableAttribute;
            //TableAttribute tableAttr = entryType.GetCustomAttributes(typeof(TableAttribute), false).SingleOrDefault() as TableAttribute;

            if (tableAttr == null)
            {
                tableAttr = entryType.GetTypeInfo().BaseType.GetTypeInfo().GetCustomAttributes(typeof(TableAttribute), false).SingleOrDefault() as TableAttribute;
                //tableAttr = entryType.BaseType.GetCustomAttributes(typeof(TableAttribute), false).SingleOrDefault() as TableAttribute;
            }

            // Get table name (if it has a Table attribute, use that, otherwise get the pluralized name)
            string tableName = tableAttr != null ? tableAttr.Name : dbEntry.Entity.GetType().Name;

            if (tableName.IndexOf("_") > 0)
                tableName = tableName.Substring(0, tableName.IndexOf("_"));

            FillAuditableEntityAndAttributes(tableName);

            try
            {
                // Get primary key value (If you have more than one key column, this will need to be adjusted)
                string keyName = dbEntry.Entity.GetType().GetProperties().Single(p => p.GetCustomAttributes(typeof(KeyAttribute), false).Count() > 0).Name;

                DateTime changeTime = DateTime.UtcNow;

                if (dbEntry.State == EntityState.Added)
                {
                    //For Inserts, just add the whole record
                    if (IsEntityAuditable(tableName))
                    {
                        string dbEntryObject = ObjectFieldsValues(dbEntry);

                        result.Add(CreateAuditLog(username,
                            changeTime,
                            AuditEventTypes.Added,
                            tableName,
                            dbEntry.CurrentValues.GetValue<object>(keyName).ToString(),
                            string.Empty,
                            string.Empty,
                            dbEntryObject));

                        addedEntities.Add(new KeyValuePair<DateTime, EntityEntry>(changeTime, dbEntry));
                    }
                }
                else if (dbEntry.State == EntityState.Deleted)
                {
                    if (IsEntityAuditable(tableName))
                    {
                        string dbEntryObject = ObjectFieldsValues(dbEntry);

                        result.Add(CreateAuditLog(username,
                        changeTime,
                        AuditEventTypes.Deleted,
                        tableName,
                        dbEntry.CurrentValues.GetValue<object>(keyName).ToString(),
                        string.Empty,
                        string.Empty,
                        dbEntryObject));
                    }
                }
                else if (dbEntry.State == EntityState.Modified)
                {
                    foreach (var propertyName in dbEntry.OriginalValues.Properties)
                    {
                        if (IsAttributeAuditable(tableName, propertyName.Name))
                        {
                            // For updates, we only want to capture the columns that actually changed
                            if (!object.Equals(dbEntry.OriginalValues.GetValue<object>(propertyName), dbEntry.CurrentValues.GetValue<object>(propertyName)))
                            {
                                result.Add(CreateAuditLog(username,
                                    changeTime,
                                    AuditEventTypes.Modified,
                                    tableName,
                                    dbEntry.CurrentValues.GetValue<object>(keyName).ToString(),
                                    propertyName.Name,
                                    dbEntry.OriginalValues.GetValue<object>(propertyName) == null ? null : dbEntry.OriginalValues.GetValue<object>(propertyName).ToString(),
                                    dbEntry.CurrentValues.GetValue<object>(propertyName) == null ? null : dbEntry.CurrentValues.GetValue<object>(propertyName).ToString()));
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return result;
        }

        #region Private Methods
        private static AuditLog CreateAuditLog(string username, DateTime changeTime, AuditEventTypes type, string tableName, string recordId, string fieldname, string originalvalue, string newvalue)
        {
            var auditLog = new AuditLog()
            {
                UserName = username,
                AuditEventDateUTC = changeTime,
                AuditEventType = (int)type,
                TableName = tableName,
                RecordId = recordId,
                FieldName = fieldname,
                OriginalValue = originalvalue,
                NewValue = newvalue
            };

            return auditLog;
        }

        private static bool IsEntityAuditable(string tablename)
        {
            bool auditable = true;

            if (_auditableEntity == null)
                auditable = false;

            return auditable;
        }

        private static bool IsAttributeAuditable(string tablename, string columnname)
        {
            bool auditable = true;

            if (null == _auditableEntity.AuditableAttributes
                .Where(attr => attr.AttributeName == columnname && attr.AuditableEntity.EntityName == tablename)
                .FirstOrDefault())
                auditable = false;

            return auditable;
        }

        private static string ObjectFieldsValues(EntityEntry dbEntry)
        {
            string record = string.Empty;

            foreach(var propertyName in dbEntry.CurrentValues.Properties)
            {
                record += string.Format("|{0}:{1}", propertyName, dbEntry.CurrentValues.GetValue<object>(propertyName) == null ? string.Empty : dbEntry.CurrentValues.GetValue<object>(propertyName).ToString());
            }

            return record;
        }

        private static void FillAuditableEntityAndAttributes(string entityName)
        {
           _auditableEntity = _context.AuditableEntities.Where(e => e.EntityName == entityName).FirstOrDefault();
        }
        #endregion
    }
}
