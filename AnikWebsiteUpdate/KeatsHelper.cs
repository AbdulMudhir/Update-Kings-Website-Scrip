using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace AnikWebsiteUpdate
{
    public class KeatsHelper
    {
        

        private readonly Excel _excel;

        private string[] _columns = 
            new string[8] {"AJ", "AK", "AL", "AM", "AN", "AO", "AP", "AQ" };

        private string[] _moduleInfoColumn = new string[4] { "B", "J", "P", "F" };

        private readonly IKeatsSeleniumHelper _web;

        public KeatsHelper(Excel excel, IKeatsSeleniumHelper web )
        {
            _excel = excel;
            _web = web;
        }


        public void StartProcess(int start)
        {

            //using overloaded method to login as the credential were saved in the chrome profile
            _web.login();

            // only going up to 5 for starting, will be navigating downwards
            for (var row = start; row >  (start-9); row--)
            {

                if (isValidRow(row))
                {

                    if (!isRowCompleted(row))
                    {


                        IKeatsModel courseModule = GetCourseModule(row);

                        

                        _web.courseModel = courseModule;

                        var courseID = _web.GetCourseID(courseModule.ModuleCode);

                        if (courseID != null)
                        {
                            _web._courseURl = courseID;

                            foreach (string column in _columns)
                            {

                                if (column.Equals("AJ"))
                                {
                                    _web.UpdateAnnouncement();

                                    _excel.UpdateCell(column + row, "Complete");
                                }

                                if (column.Equals("AK"))
                                {
                                    var keyContactField = _web.UpdateKeyContact();

                                    if (!keyContactField)
                                    {
                                        _excel.UpdateCell(column + row, "Key Contact Missing");
                                    }
                                    else
                                    {
                                        _excel.UpdateCell(column + row, "Complete");
                                    }
                                }

                                if(column.Equals("AL"))
                                {
                                    _web.UpdateHeroBanner();
                                    _excel.UpdateCell(column + row, "Complete");
                                }

                                if(column.Equals("AM"))
                                {
                                    _web.UpdateReadingList();
                                    _excel.UpdateCell(column + row, "Complete");
                                }
                                if (column.Equals("AN"))
                                {

                                    _excel.UpdateCell(column + row, "started");
                                }

                                if(column.Equals("AO"))
                                {
                                    _web.RemoveRecentActivity();
                                    _excel.UpdateCell(column + row, "Complete");
                                }

                                if(column.Equals("AQ"))
                                {
                                    _web.UpdateQuickMailBlock();
                                    _excel.UpdateCell(column + row, "Complete");

                                }

                                if (column.Equals("AP"))
                                {
                                    _web.RemoveLectureCapture();
                                    _excel.UpdateCell(column + row, "Complete");
                                }

                                else
                                {

                                }
                            }
                        }
                    }
                }
            }
        }



        private bool isRowCompleted(int row)
        {

           if(_excel.GetCell(_columns[0]+row).Equals("Complete"))
            {
                return true;
            }
            return false;
        }

        private bool isValidRow(int row)
        {
            //if (_excel.GetCell(_moduleInfoColumn[3] + row).Equals("Full year"))
            //{
            //    return true;
            //}
            return true;
        }


        private IKeatsModel GetCourseModule(int row)
        {

            return new KeatsModel()
            {
                ModuleCode = _excel.GetCell(_moduleInfoColumn[0] + row),
                ModuleLead = _excel.GetCell(_moduleInfoColumn[2] + row),
                ModuleTitle = _excel.GetCell(_moduleInfoColumn[1] + row),
                ModulePeriod = _excel.GetCell(_moduleInfoColumn[3] + row)
            };

        }
    }
}
