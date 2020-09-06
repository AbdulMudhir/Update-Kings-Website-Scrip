using System;
using System.Collections.Generic;
using System.Text;

namespace AnikWebsiteUpdate
{
    public class KeatsModel : IKeatsModel
    {
        public string ModuleCode { get; set; }

        public string ModuleTitle { get; set; }

        public string ModuleLead { get; set; }
        public string ModulePeriod { get; set; }

        public ITutor Tutor { get { return GetTutor();  } }

        public string KeyContactTemplate { get {
            
            return @$"
              <p>
        <b>Module leader</b>:&nbsp;{Tutor.FirstName} {Tutor.LastName}<br>
        <b>Email</b>: <a href=""mailto: {Tutor.FirstName}.{Tutor.LastName}@kcl.ac.uk"">{Tutor.FirstName}.{Tutor.LastName}@kcl.ac.uk</a><br>
        <b> Office Hours:</b>&nbsp; TBC </p>
          <p><b>Administrative support:</b> <a href=""mailto: email"">emailplaceholder</a></p>

 ";

            } }

        public string HeroBannerTemplate { get { return @$"
        <main>
    <div class=""hero"" title=""Home page""><img src=""https://keats.kcl.ac.uk/pluginfile.php/5888455/mod_lightboxgallery/gallery_images/0/Foundation%20banner.png"" alt=""River Thames"" width=""1200"" height=""240"" class=""img-responsive atto_image_button_text-bottom"">
        <div class=""hero-box"">
            <h1><span style = ""color: inherit; font-family: inherit;"" > {ModuleTitle} </span></h1>
        </div>
    </div>
</main>
 
"; } }

        public ITutor GetTutor()
        {
            var nameArray = ModuleLead.Split(",");

            return new Tutor()
            {
                FirstName = nameArray[nameArray.Length -1],
                LastName = nameArray[0]
            };

            
        }
    }
}
