namespace AnikWebsiteUpdate
{
    public interface IKeatsSeleniumHelper
    {

        string GetCourseID(string moduleCode);
        IKeatsModel courseModel { get; set; }
        bool RemoveLectureCapture();
        bool RemoveRecentActivity();
        bool UpdateAnnouncement();
        bool UpdateHeroBanner();
        bool UpdateKeyContact();
        bool UpdateModuleResourcesLink();
        bool UpdateQuickMailBlock();
        bool UpdateReadingList();
        bool login(string username, string password);
        bool login();
        string _courseURl { get; set; }
    }
}