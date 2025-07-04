using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using SmileSCommissionDataImport.Controllers;
using SmileSCommissionDataImport.Models;

namespace SmileSCommissionDataImport.Utils
{
    public class ValidationHelpers
    {
        private readonly CommissionCalculateEntities _context;

        public ValidationHelpers()
        {
            _context = new CommissionCalculateEntities();
        }

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

        public ValidationResult ValidateInt(string columnName,object value,bool isRequire)
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
                    Value = value != null ? Convert.ToInt32(value) : (dynamic)null,
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

        public ValidationResult ValidateFloat(string columnName,object value,bool isRequire)
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
                    Value = value != null ? Convert.ToDouble(value) : (dynamic)null,
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

        public ValidationResult ValidateDate(string columnName,object value,bool isRequire)
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

                DateTime? myDate = null;

                if(value != null)
                {
                    if(value.ToString().Contains("-"))
                    {
                        var dArr = value.ToString().Split('-');
                        myDate = new DateTime(Convert.ToInt16(dArr[0]),Convert.ToInt16(dArr[1]),
                            Convert.ToInt16(dArr[2]));
                    }
                    else if(value.ToString().Contains("/"))
                    {
                        var dArr = value.ToString().Split('/');
                        myDate = new DateTime(Convert.ToInt16(dArr[0]),Convert.ToInt16(dArr[1]),
                            Convert.ToInt16(dArr[2]));
                    }
                    else
                    {
                        myDate = DateTime.FromOADate(Convert.ToInt32(value));
                    }
                }

                // Return value
                return new ValidationResult
                {
                    IsValid = true,
                    Value = myDate,
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

        public List<string> ValidateAppId(List<string> valid)
        {
            try
            {
                // TODO: Prepare list for query

                var existing = _context.ValidApp_Id.SqlQuery(
                    string.Format("SELECT App_Id FROM dbo.ValidApp_Id WHERE App_Id IN ('{0}')",
                        string.Join("','",valid.ToArray()))).ToList();

                // Remove exiting item form prepare list
                foreach(var ex in existing)
                {
                    valid.RemoveAll(x => x.Equals(ex.App_Id.Trim()));
                }

                return valid;
            }
            catch(Exception e)
            {
                return valid;
            }
        }

        public List<string> ValidateEmployeeSS(List<string> valid)
        {
            try
            {
                // TODO: Prepare list for query

                var existing = _context.ValidEmployeeSS.SqlQuery(
                    string.Format("SELECT Code FROM dbo.ValidEmployeeSS WHERE Code IN ('{0}')",
                        string.Join("','",valid.ToArray()))).ToList();

                // Remove exiting item form prepare list
                foreach(var ex in existing)
                {
                    valid.RemoveAll(x => x.Equals(ex.Code.Trim()));
                }

                return valid;
            }
            catch(Exception e)
            {
                return valid;
            }
        }

        public List<string> ValidateArea(List<string> valid)
        {
            try
            {
                // TODO: Prepare list for query

                var existing = _context.ValidArea.SqlQuery(
                    string.Format("SELECT * FROM dbo.ValidArea WHERE Code IN ('{0}')",
                        string.Join("','",valid.ToArray()))).ToList();

                // Remove exiting item form prepare list
                foreach(var ex in existing)
                {
                    valid.RemoveAll(x => x.Equals(ex.Code.Trim()));
                }

                return valid;
            }
            catch(Exception e)
            {
                return valid;
            }
        }

        public List<string> ValidateBranch(List<string> valid)
        {
            try
            {
                // TODO: Prepare list for query

                var existing = _context.ValidBranch.SqlQuery(
                    string.Format("SELECT * FROM dbo.ValidBranch WHERE Code IN ('{0}')",
                        string.Join("','",valid.ToArray()))).ToList();

                // Remove exiting item form prepare list
                foreach(var ex in existing)
                {
                    valid.RemoveAll(x => x.Equals(ex.Code.Trim()));
                }

                return valid;
            }
            catch(Exception e)
            {
                return valid;
            }
        }

        public List<string> ValidateProvince(List<string> valid)
        {
            try
            {
                // TODO: Prepare list for query

                var existing = _context.ValidProvince.SqlQuery(
                    string.Format("SELECT * FROM dbo.ValidProvince WHERE Code IN ('{0}')",
                        string.Join("','",valid.ToArray()))).ToList();

                // Remove exiting item form prepare list
                foreach(var ex in existing)
                {
                    valid.RemoveAll(x => x.Equals(ex.Code.Trim()));
                }

                return valid;
            }
            catch(Exception e)
            {
                return valid;
            }
        }

        public List<string> ValidateDistrict(List<string> valid)
        {
            try
            {
                // TODO: Prepare list for query

                var existing = _context.ValidDistrict.SqlQuery(
                    string.Format("SELECT * FROM dbo.ValidDistrict WHERE Code IN ('{0}')",
                        string.Join("','",valid.ToArray()))).ToList();

                // Remove exiting item form prepare list
                foreach(var ex in existing)
                {
                    valid.RemoveAll(x => x.Equals(ex.Code.Trim()));
                }

                return valid;
            }
            catch(Exception e)
            {
                return valid;
            }
        }

        public List<string> ValidateHC(List<string> valid)
        {
            try
            {
                // TODO: Prepare list for query

                var existing = _context.ValidHC.SqlQuery(
                    string.Format("SELECT * FROM dbo.ValidHC WHERE HCM_ID IN ('{0}')",
                        string.Join("','",valid.ToArray()))).ToList();

                // Remove exiting item form prepare list
                foreach(var ex in existing)
                {
                    valid.RemoveAll(x => x.Equals(ex.HCM_ID.Trim()));
                }

                return valid;
            }
            catch(Exception e)
            {
                return valid;
            }
        }

        public List<string> ValidateTeam(List<string> valid)
        {
            try
            {
                // TODO: Prepare list for query

                var existing = _context.ValidTeam.SqlQuery(
                    string.Format("SELECT * FROM dbo.ValidTeam WHERE Code IN ('{0}')",
                        string.Join("','",valid.ToArray()))).ToList();

                // Remove exiting item form prepare list
                foreach(var ex in existing)
                {
                    valid.RemoveAll(x => x.Equals(ex.Code.Trim()));
                }

                return valid;
            }
            catch(Exception e)
            {
                return valid;
            }
        }

        public List<string> ValidateZebra(List<string> valid)
        {
            try
            {
                // TODO: Prepare list for query

                var existing = _context.ValidZebra.SqlQuery(
                    string.Format("SELECT * FROM dbo.ValidZebra WHERE Code IN ('{0}')",
                        string.Join("','",valid.ToArray()))).ToList();

                // Remove exiting item form prepare list
                foreach(var ex in existing)
                {
                    valid.RemoveAll(x => x.Equals(ex.Code.Trim()));
                }

                return valid;
            }
            catch(Exception e)
            {
                return valid;
            }
        }

        public List<string> ValidateClaimCode(List<string> valid)
        {
            try
            {
                // TODO: Prepare list for query

                var existing = _context.ValidClaim_Id.SqlQuery(
                    string.Format("SELECT * FROM dbo.ValidClaim_Id WHERE Claim_Id IN ('{0}')",
                        string.Join("','",valid.ToArray()))).ToList();

                // Remove exiting item form prepare list
                foreach(var ex in existing)
                {
                    valid.RemoveAll(x => x.Equals(ex.Claim_Id.Trim()));
                }

                return valid;
            }
            catch(Exception e)
            {
                return valid;
            }
        }
    }
}