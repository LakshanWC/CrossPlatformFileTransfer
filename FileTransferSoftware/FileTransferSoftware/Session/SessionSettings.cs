using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileTransferSoftware.Session
{
    public class SessionSettings
    {
        private static SessionSettings sessionSettingsInstent;
        private SessionSettings() {}

        //settings
        public Boolean isHttpListenerOn { get; set; }


        public static SessionSettings getSessionSettingsInstent()
        {
            if(sessionSettingsInstent == null)
            {
                 sessionSettingsInstent = new SessionSettings();
            }
            return sessionSettingsInstent;
        } 


    }
}
