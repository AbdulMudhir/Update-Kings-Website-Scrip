using System;

namespace AnikWebsiteUpdate
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {


        
            KeatsHelper keatsHelper = new KeatsHelper(Factory.GetExcel(), Factory.GetKeatsSelenium());

            keatsHelper.StartProcess(75);

          







        }
    }
}
