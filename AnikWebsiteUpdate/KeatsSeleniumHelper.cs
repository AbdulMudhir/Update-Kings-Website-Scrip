using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Internal;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AnikWebsiteUpdate
{
    public class KeatsSeleniumHelper : IKeatsSeleniumHelper
    {

      
        private readonly IWebDriver _driver;
        private string _keatsUrl;
        private string _courseQueryURL;

        public string _courseURl { get; set; }


        public IKeatsModel courseModel { get; set; }



        public KeatsSeleniumHelper(IWebDriver driver, string keatsUrl)
        {
            _driver = driver;
            _keatsUrl = keatsUrl;
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);
            _driver.Navigate().GoToUrl(_keatsUrl);
            _courseQueryURL = "https://keats.kcl.ac.uk/course/search.php?search=";


        }
        public bool login(string username, string password)
        {
            // only needed when profile needs to be setup
            var loginField = _driver.FindElement(By.CssSelector("#inputName"));
            var passwordField = _driver.FindElement(By.CssSelector("#inputPassword"));

            loginField.SendKeys(username);
            passwordField.SendKeys(password);

            var submit = _driver.FindElement(By.CssSelector("#submit"));

            submit.Click();
         

            return false;
        }

        public bool UpdateAnnouncement()
        {

            NavigateToCourse();



            var announcement = GetElementInSectionBox("Announcements");
            var discussion = GetElementInSectionBox("Discussion & Advice Forum");

            if (announcement == null)
            {
                AddAnnouncement();
            }

            if(discussion == null)
            {
                AddDiscussion();
            }

            RemoveNewsForum();



            return false;
        }

        private void AddDiscussion()
        {
            var option = GetSharingCartItem("Discussion & Advice Forum", "Copy to course");

            if (option != null)
            {
                option.Click();

                var moveTarget = _driver.FindElement(By.ClassName("move_target"));

                moveTarget.Click();


            }

        }

        public void AddAnnouncement()
        {
            var option = GetSharingCartItem("Announcements", "Copy to course");

            if(option != null)
            {
                option.Click();

                var moveTarget = _driver.FindElement(By.ClassName("move_target"));

                moveTarget.Click();

                MoveSectionElementBox("Announcements", 0);

            }


        }


        public IWebElement GetSharingCartItem(string cartItemName, string selectOption)
        {
            var AnnouncementSharingCart = _driver.FindElements(By.ClassName("sc-indent-0"));

            foreach (var element in AnnouncementSharingCart)
            {
                if (element.Text.Equals(cartItemName))
                {

                    var options = element.FindElements(By.ClassName("iconsmall"));

                    foreach (var option in options)
                    {

                        var altTag = option.GetAttribute("alt");

                        if (altTag.Equals(selectOption))
                        {

                            return option;
                        }

                    }

                   
                }
            }

            return null;

        }


        public void MoveSectionElementBox(string elementToMove, int position)
        {
            var element = GetElementInSectionBox(elementToMove);

            if (element != null)
            {

                var moveButton = element.FindElement(By.ClassName("moodle-core-dragdrop-draghandle"));

                moveButton.Click();
              


                var unsortedList = _driver.FindElement(By.ClassName("dragdrop-keyboard-drag"));
              
               
                var childrenElements = unsortedList.FindElements(By.TagName("li"));

                var childrenElement = childrenElements[position];

                if (elementToMove.ToLower().Equals("announcement".ToLower()))
                {
                    if (childrenElement.Text.ToLower().Equals(@"To the top of section "" General """.ToLower()))
                    {
                        childrenElement.Click();
                    }
                    else
                    {
                        _driver.Navigate().Refresh();
                    }
                }
                else
                {
                    childrenElements[position].Click();
                }
            }

              
        }



        private IWebElement GetElementInSectionBox(string elementToFind)
        {
            var editField = _driver.FindElement(By.ClassName("section"));

            var elements = editField.FindElements(By.ClassName("activity"));


            foreach (var element in elements)
            {

                var elementStringFormated = element.Text.Trim().Replace('\u0009'.ToString(), "").ToLower();



                if (elementStringFormated.Contains(elementToFind.ToLower()))
                {
                    Console.WriteLine(elementToFind);


                    return element;
                }

                else
                {

                }
            }


            return null;
        }

        public void RemoveNewsForum()
        {

            var element = GetElementInSectionBox("News Forum");

            if (element != null)
            {
                var editMenu = element.FindElement(By.ClassName("dropdown-toggle"));

                editMenu.Click();
                var deleteOption = element.FindElement(By.ClassName("editing_delete"));

                deleteOption.Click();


                var confirmationBox = _driver.FindElement(By.ClassName("modal-content"));

                NewsForumConfirmationBox(confirmationBox);


            }

         
        }


        public void NewsForumConfirmationBox(IWebElement element)
        {
            var confirmationMessage = element.FindElement(By.ClassName("modal-body"));

            if(confirmationMessage.Text.ToLower().Contains("News Forum".ToLower()))
            {

                var yesBox = element.FindElement(By.ClassName("btn-primary"));

                var noBox = element.FindElement(By.ClassName("btn-secondary"));

                yesBox.Click();

                var boxShadow = _driver.FindElement(By.ClassName("modal-backdrop"));

                if(boxShadow.Displayed)
                {
                    _driver.Navigate().Refresh();
                }

                Console.WriteLine("News Forum Removed");
            }

            
        }



        private void TurnOnEditing()
        {
            var buttonDiv = _driver.FindElement(By.ClassName("singlebutton"));

            var editButton = buttonDiv.FindElement(By.ClassName("btn"));

            if(!editButton.Text.Equals("Turn editing off"))
            {
                editButton.Click();
            }

            
        }

        public bool UpdateKeyContact()
        {
            NavigateToCourse();

            var keyContact = GetWebElementByText("block_html", "Key Contacts");

            if (keyContact != null)
            {
                var keyContactOption = keyContact.FindElement(By.ClassName("dropdown-toggle"));

                keyContactOption.Click();


                keyContact.FindElement(By.ClassName("editing_edit")).Click();


                _driver.FindElement(By.ClassName("collapse_group")).Click();


                _driver.FindElement(By.ClassName("atto_html_button")).Click();


                var code = _driver.FindElement(By.ClassName("CodeMirror-code"));

                code.Click();

                Actions action = new Actions(_driver);

                action.KeyDown(Keys.Control).SendKeys("a").KeyUp(Keys.Control).B‌​uild().Perform();


                System.Windows.Forms.Clipboard.SetText(courseModel.KeyContactTemplate);


                action.KeyDown(Keys.Control).SendKeys("v").KeyUp(Keys.Control).B‌​uild().Perform();


                _driver.FindElement(By.ClassName("btn-primary")).Click();

                return true;

            }
            return false;
        }

        private IWebElement GetHeroBannerImg()
        {
            try
            {
                var imgElement = _driver.FindElement(By.ClassName("atto_image_button_text-bottom"));
                var imgUrl = imgElement.GetAttribute("src").ToLower();


                if (imgUrl.Equals("https://keats.kcl.ac.uk/pluginfile.php/5888455/mod_lightboxgallery/gallery_images/0/Foundation%20banner.png".ToLower())
                    || imgUrl.Contains("Banner%20of%20the%20river%20Color%20overlay%20%282%29.png".ToLower())
                    || imgUrl.Contains(@"Banner%20of%20the%20river%20Color%20overlay".ToLower())

                    )
                {
                    return imgElement;
                        }
                else
                {
                    return null;
                }
            }
            catch(NoSuchElementException)
            {
                return null;
            }
        }
        public bool UpdateHeroBanner()
        {
            NavigateToCourse();

          

            if (GetHeroBannerImg() == null)
            {

                var heroBannerOption = _driver.FindElement(By.XPath("//a[@title =  'Edit section']"));


                heroBannerOption.Click();


                _driver.FindElement(By.ClassName("collapse_group")).Click();


                _driver.FindElement(By.ClassName("atto_html_button")).Click();


                var code = _driver.FindElement(By.ClassName("CodeMirror-code"));


                var childrenElements = code.FindElements(By.XPath(@".//*"));

                childrenElements[0].Click();

                Actions action = new Actions(_driver);

                action.SendKeys(Keys.PageUp).B‌​uild().Perform();


                System.Windows.Forms.Clipboard.SetText(courseModel.HeroBannerTemplate);

                action.KeyDown(Keys.Control).SendKeys("v").KeyUp(Keys.Control).B‌​uild().Perform();

                _driver.FindElement(By.ClassName("btn-primary")).Click();
            }
            return false;
        }
        public bool UpdateReadingList()
        {
            NavigateToCourse();

            var readingList = GetWebElementByText("p-3", "My Reading Lists");

            if (readingList != null)
            {
                _driver.FindElement(By.ClassName("container-fluid")).Click();

          


                var actionMenu = readingList.FindElement(By.ClassName("dropdown-toggle"));
                actionMenu.Click();

                var deleteButton = readingList.FindElement(By.ClassName("editing_delete"));




                deleteButton.Click();

                NewPageConfirmationDeletion("My Reading Lists");


              


            }

            var readingInSection = GetElementInSectionBox("Reading List");
            var AnnouncementInSection = GetElementInSectionBox("Announcements");

            if (readingInSection != null && AnnouncementInSection != null)
            {
                var readListLink = readingInSection.FindElement(By.ClassName("aalink")).GetAttribute("href");
                var announcementLink = AnnouncementInSection.FindElement(By.ClassName("aalink")).GetAttribute("href");

                UpdateReadingLinkinResources(readListLink, announcementLink);
            }
            else
            {
                if(AnnouncementInSection == null)
                {
                    AddAnnouncement();
                }
                if (readingInSection == null)
                {
                    AddReadingLink();
                }

                UpdateReadingList();

            }

            return false;
        }

        private void AddReadingLink()
        {
            var option = GetSharingCartItem("Reading List", "Copy to course");

            if (option != null)
            {
                option.Click();

                var moveTarget = _driver.FindElement(By.ClassName("move_target"));

                moveTarget.Click();

                MoveSectionElementBox("Reading List", 1);

            }
        }

        public void UpdateReadingLinkinResources(string link, string announcementLink)
        {
            var resources = GetWebElementByText("p-3", "Module Resources");

            var actionMenu = resources.FindElement(By.ClassName("action-menu"));

            actionMenu.Click();

            resources.FindElement(By.ClassName("editing_edit")).Click();


            _driver.FindElement(By.ClassName("collapse_group")).Click();


            _driver.FindElement(By.ClassName("atto_html_button")).Click();


            var code = _driver.FindElement(By.ClassName("CodeMirror-code"));

            
            var readingLinkTemplate = @$"<p><a href=""{announcementLink}"" target=""_blank"">Announcements</a><br>Assessments<br>Handbook<br><a href=""https://mytimetable.kcl.ac.uk/"" target=""_blank"">Timetable</a><br><a href=""{link}"" target=""_blank"">Reading List</a></p>";

           

            code.Click();

            Actions action = new Actions(_driver);

            action.KeyDown(Keys.Control).SendKeys("a").KeyUp(Keys.Control).B‌​uild().Perform();


            System.Windows.Forms.Clipboard.SetText(readingLinkTemplate);


            action.KeyDown(Keys.Control).SendKeys("v").KeyUp(Keys.Control).B‌​uild().Perform();


            _driver.FindElement(By.ClassName("btn-primary")).Click();


        }

        public void NewPageConfirmationDeletion(string element)
        {
            var confirmationBox = _driver.FindElement(By.CssSelector("#modal-body"));



            if(confirmationBox.Text.Contains(element))
            {

                var yesButton = _driver.FindElement(By.ClassName("btn-primary"));
                yesButton.Click();
            }

        }





        public IWebElement GetWebElementByText(string classname , string text)
        {
           var elements =  _driver.FindElements(By.ClassName(classname));

            foreach(var element in elements)
            {
                if (!element.Text.Contains("Add a block"))
                {
                    if (element.Text.Contains(text))
                    {
                        return element;
                    }
                }
            }

            return null;

        }

        public bool UpdateModuleResourcesLink()
        {
            throw new NotImplementedException();
        }



        public bool RemoveRecentActivity()
        {
            NavigateToCourse();


            var recentActivity = GetWebElementByText("p-3", "Recent activity");

            if(recentActivity != null)
            {

                var actionMenu = recentActivity.FindElement(By.ClassName("dropdown-toggle"));
                actionMenu.Click();

                var deleteButton = recentActivity.FindElement(By.ClassName("editing_delete"));

                deleteButton.Click();

                NewPageConfirmationDeletion("Recent activity");
            }

            return false;
        }


        public bool RemoveLectureCapture()
        {
            NavigateToCourse();


            var lectureCapture = GetElementInSectionBox("Lecture Capture");

            if(lectureCapture != null)
            {

                var confirmBox = lectureCapture.FindElement(By.ClassName("dropdown-toggle"));
                confirmBox.Click();

                lectureCapture.FindElement(By.ClassName("editing_update")).Click();


                RemoveLectureCaptureInfo();
            }
            else
            {
                AddNewLectureCapture();
                RemoveLectureCapture();
            }

            return false;
        }


        private void AddNewLectureCapture()
        {

            var option = GetSharingCartItem("Lecture Capture Recordings", "Copy to course");

            if (option != null)
            {
                option.Click();

                var moveTarget = _driver.FindElement(By.ClassName("move_target"));

                moveTarget.Click();

                MoveSectionElementBox("Lecture Capture Recordings", 2);

            }

        }

        private void RemoveLectureCaptureInfo()
        {
            _driver.FindElement(By.ClassName("moreless-toggler")).Click();

            _driver.FindElement(By.CssSelector("#id_introeditoreditable")).Clear();

            var avalibiltiyElement = _driver.FindElement(By.XPath("//a[@aria-controls ='id_modstandardelshdr']"));

            if (avalibiltiyElement != null)
            {
                avalibiltiyElement.Click();

                var avaliabiltiyBox = _driver.FindElement(By.CssSelector("#id_modstandardelshdr"));



                var selectBox = avaliabiltiyBox.FindElement(By.CssSelector("#id_visible"));

                selectBox.Click();

                selectBox.FindElement(By.CssSelector("#id_visible > option:nth-child(2)")).Click();

                _driver.FindElement(By.CssSelector("#id_submitbutton2")).Click();
            }
        }


        private void NavigateToCourse()
        {
            _driver.Navigate().GoToUrl(_courseURl);

            TurnOnEditing();

        }

        public bool UpdateQuickMailBlock()
        {
            NavigateToCourse();

            var quickMail = GetQuickMail();

            if(quickMail == null)
            {
                AddQuickMail();
            }

            return false;
        }

        public IWebElement GetQuickMail()
        {
            try
            {
                return _driver.FindElement(By.ClassName("block_quickmail"));
            }
            catch(NoSuchElementException)
            {
                return null;
            }
        }

        private void AddQuickMail()
        {
            

            var addBlockElement = GetAddBlockElement();

            if(addBlockElement != null)
            {
               var options =  addBlockElement.FindElement(By.XPath("//select[@name='bui_addblock']"));
                options.Click();

                var quickMailOption = addBlockElement.FindElement(By.XPath("//select[@name='bui_addblock']/option[@value='quickmail']"));

                quickMailOption.Click();


                MoveQuickMail();

            }

        }


        private void MoveQuickMail()
        {
            var quickMail = GetQuickMail(); 
            if(quickMail != null)
            {
                quickMail.FindElement(By.ClassName("moodle-core-dragdrop-draghandle")).Click();

                var unsortedList = _driver.FindElement(By.ClassName("dragdrop-keyboard-drag"));

                var elementsInList = unsortedList.FindElements(By.TagName("li"));

                elementsInList[6].FindElement(By.TagName("a")).Click();
            }
            else
            {
                AddQuickMail();
            }
        }

        public IWebElement GetAddBlockElement()
        {
            var elements = _driver.FindElements(By.ClassName("p-3"));

            foreach (var element in elements)
            {
                
                    if (element.Text.Contains("Add a block"))
                    {
                        return element;
                    }
                
            }

            return null;

        }

        public IWebElement GetAvalibilityElement()
        {

            var elements = _driver.FindElements(By.ClassName("ftoggler"));

            foreach(var element in elements)
            {
                if(element.Text.Contains("Common module settings"))
                {
                    return element;
                }
            }

            return null;
        }


        public string GetCourseID(string moduleCode)
        {
            _driver.Navigate().GoToUrl(_courseQueryURL + moduleCode);

            var elements = _driver.FindElements(By.ClassName("dimmed"));


            string courseID = null;

            foreach(var element in elements)
            {
                if (element.Text.Contains("20~21"))
                {
                    courseID = element.FindElement(By.ClassName("dimmed"))
                        .GetAttribute("href");
                  
                    break;
                }
              
            }


            return courseID;
        }

        public bool login()
        {
            try
            {
                var loginButton = _driver.FindElement(By.CssSelector("#module-3059982 > div > div > div:nth-child(2) > div > div > div > h2 > a"));
                loginButton.Click();
            }
            catch(NoSuchElementException)
            {
              
            }

            return true;
        }
    }
}
