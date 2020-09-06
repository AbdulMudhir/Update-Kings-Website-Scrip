using ExcelDataReader;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AnikWebsiteUpdate
{
    public class Excel
    {
       

        private readonly string _path;
        private readonly string _sheet;
        private readonly ExcelPackage _package;


        public Excel(string path, string sheet)
        {
            _path = path;
            _sheet = sheet;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            _package = new ExcelPackage(new FileInfo(_path));






        }


        public string GetCell(string cell)
        {
         

                var sheet = _package.Workbook.Worksheets[_sheet];


                return sheet.Cells[cell].Text;


                


          
        }

        public string UpdateCell(string cell, string value)
        {
            

                var sheet = _package.Workbook.Worksheets[_sheet];

                sheet.Cells[cell].Value = value;


                _package.Save();

                return sheet.Cells[cell].Text;


        }

    }
}
