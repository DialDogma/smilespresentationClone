using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmileSDataCenterV2.Models;
using SmileSDataCenterV2.WebServices;

namespace SmileSDataCenterV2.Helper
{
    public class ValidationHelper
    {
        public class ValidationResult
        {
            public bool IsValid { get; set; }
            public dynamic Value { get; set; }
            public string InvalidMessage { get; set; }
        }

        public List<String> ValidateColumn(object[,] rows,List<String> columns)
        {
            var missingColumn = new List<string>();

            for(int i = 0;i < columns.Count;i++)
            {
                try
                {
                    if(rows[0,i].ToString() != columns[i])
                    {
                        missingColumn.Add(columns[i]);
                    }
                }
                catch(Exception e)
                {
                    missingColumn.Add(columns[i]);
                }
            }

            return missingColumn;
        }

        public ValidationResult ValidateString(string columnName,object value,bool isRequire)
        {
            try
            {
                // Check require
                if(isRequire)
                {
                    if(value == null || value.ToString().Trim() == "")
                    {
                        // If null return that
                        return new ValidationResult
                        {
                            IsValid = false,
                            Value = null,
                            InvalidMessage = "Require Column " + columnName
                        };
                    }
                }

                // Return value
                return new ValidationResult
                {
                    IsValid = true,
                    Value = value != null ? value.ToString() : null,
                    InvalidMessage = null
                };
            }
            catch(Exception e)
            {
                // Exception when convert
                return new ValidationResult
                {
                    IsValid = false,
                    Value = null,
                    InvalidMessage = "Invalid Type of column " + columnName
                };
            }
        }

        public List<string> ValidateUser(List<string> valid)
        {
            try
            {
                using(var _context = new DataCenterV1Entities())
                {
                    var existing = _context.AspNetUsers.SqlQuery(
                        string.Format("SELECT * FROM dbo.AspNetUsers WHERE UserName IN ('{0}')",
                         string.Join("','",valid.ToArray()))).ToList();
                    foreach(var ex in existing)
                    {
                        valid.RemoveAll(x => x.Equals(ex.UserName.Trim()));
                    }

                    return valid;
                }
            }
            catch(Exception e)
            {
                return valid;
            }
        }

        public ValidationResult ValidatePassword(string columnName,object value,bool isRequire)
        {
            try
            {
                if(isRequire)
                {
                    if(value == null || value.ToString().Trim() == "")
                    {
                        // If null return that
                        return new ValidationResult
                        {
                            IsValid = false,
                            Value = null,
                            InvalidMessage = "Require Column " + columnName
                        };
                    }
                }
                if(value.ToString().Length < 4)
                {
                    // If < 4 return that
                    return new ValidationResult
                    {
                        IsValid = false,
                        Value = null,
                        InvalidMessage = "Lenght less than 4 " + columnName
                    };
                }

                // Return value
                return new ValidationResult
                {
                    IsValid = true,
                    Value = value,
                    InvalidMessage = null
                };
            }
            catch(Exception e)
            {
                // Exception when convert
                return new ValidationResult
                {
                    IsValid = false,
                    Value = null,
                    InvalidMessage = "Invalid Type of column " + columnName
                };
            }
        }
    }
}