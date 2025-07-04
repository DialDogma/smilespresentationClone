using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;

namespace SmileSCommissionModelDesigner.Helper
{
    internal class ExcelHelper
    {
        public ExcelHelper()
        {
        }

        /// <summary>
        /// Get Row list
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        public static List<string> GetRows(ExcelWorksheet sheet, int columnIndex)
        {
            var lstResult = new List<String>();
            var start = sheet.Dimension.Start;
            var end = sheet.Dimension.End;
            for (int row = start.Row; row <= end.Row; row++)
            { // Row by row...
                lstResult.Add(sheet.Cells[row, columnIndex].Text);
            }

            return lstResult;
        }

        /// <summary>
        /// Get Header Column
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns></returns>
        public static List<String> GetHeaderColumns(ExcelWorksheet sheet)
        {
            List<string> columnNames = new List<string>();
            foreach (var firstRowCell in sheet.Cells[sheet.Dimension.Start.Row, sheet.Dimension.Start.Column, 1, sheet.Dimension.End.Column])
                columnNames.Add(firstRowCell.Text);
            return columnNames;
        }

        /// <summary>
        /// Return Datatable
        /// </summary>
        /// <param name="path"></param>
        /// <param name="hasHeader"></param>
        /// <returns></returns>
        public static DataTable GetDataTableFromExcel(ExcelWorksheet sheet, bool hasHeader = true)
        {
            DataTable tbl = new DataTable();
            foreach (var firstRowCell in sheet.Cells[1, 1, 1, sheet.Dimension.End.Column])
            {
                tbl.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
            }
            var startRow = hasHeader ? 2 : 1;
            for (int rowNum = startRow; rowNum <= sheet.Dimension.End.Row; rowNum++)
            {
                var wsRow = sheet.Cells[rowNum, 1, rowNum, sheet.Dimension.End.Column];
                DataRow row = tbl.Rows.Add();
                foreach (var cell in wsRow)
                {
                    row[cell.Start.Column - 1] = cell.Text;
                }
            }
            return tbl;
        }
    }
}