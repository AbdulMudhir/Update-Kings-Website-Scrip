using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnikWebsiteUpdate
{
    class Factory
    {

        public static Excel GetExcel()
        {
            
            return new Excel(@"C:\Users\Abdul\source\repos\AnikWebsiteUpdate\AnikWebsiteUpdate\updated.xlsx", "Module list checks");
        }

        public static IWebDriver GetChromeDriver()
        {
            return new ChromeDriver(GetChromeOptions());
        }

        public static ChromeOptions GetChromeOptions()
        {
            var options = new ChromeOptions();
            options.AddArguments(@"user-data-dir=C:\Users\Abdul\source\repos\AnikWebsiteUpdate\AnikWebsiteUpdate\ChromeProfile");
            return options;
        }
        public static IKeatsSeleniumHelper GetKeatsSelenium()
        {

            return new KeatsSeleniumHelper(GetChromeDriver(), "https://keats.kcl.ac.uk/my/");
        }
    }
}
