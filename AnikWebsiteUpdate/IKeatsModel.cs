namespace AnikWebsiteUpdate
{
    public interface IKeatsModel
    {
        public string ModuleCode { get; set; }
        public string ModuleLead { get; set; }

        public string ModulePeriod { get; set; }
        public string ModuleTitle { get; set; }
        public ITutor Tutor { get; }
        public ITutor GetTutor();


        public string HeroBannerTemplate { get;  }
        public string KeyContactTemplate { get; }


    }
}