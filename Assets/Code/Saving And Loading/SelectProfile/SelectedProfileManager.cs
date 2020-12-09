using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Code.Saving___Loading.Profile_System.SelectProfile
{
    public class SelectedProfileManager
    {
        public static SelectedProfile selectedProfile = null;
    }

    // The object which stores the information for the currently selected profile.
    public class SelectedProfile
    {
        public string profileId;
        public string profileName;
        public string fullProfileName;
        public string companyName;
        public string profileImage;
    }
}
