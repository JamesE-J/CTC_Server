using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CTCServer.Models;

namespace CTCServer.Controllers
{
    public class ZoneController : ApiController
    {
        [ActionName("Initialise")]
        public string GET()
        {
            if(UserDatabaseAccess.checkForZones())
            {
                string[] returnArray = UserDatabaseAccess.getZoneState();
                string returnString = null;
                for (int i = 0; i < returnArray.Length; i++)
                {
                    returnString = returnString + "," + returnArray[i];
                }
                return returnString;
            }
            else
            {
                string[] returnArray = UserDatabaseAccess.initialiseZones();
                string returnString = null;
                for (int i = 0; i < returnArray.Length; i++)
                {
                    returnString = returnString + "," + returnArray[i];
                }
                return returnString;
            }
        }
        [ActionName("Update")]
        public string POST([FromBody]User user)
        {
            string[] zoneStates = UserDatabaseAccess.getZoneState();
            int zoneState = Int32.Parse(zoneStates[Int32.Parse(user.Zone)]);
            if (user.Team == "Blue Team")
            {
                if(zoneState > 0){
                    UserDatabaseAccess.updateDominance(Int32.Parse(user.Zone), user.Team, user.UserKey);
                }
            }
            else
            {
                if (zoneState < 100)
                {
                    UserDatabaseAccess.updateDominance(Int32.Parse(user.Zone), user.Team, user.UserKey);
                }
            }
            string[] returnArray = UserDatabaseAccess.getZoneState();
            string returnString = null;
            for (int i = 0; i < returnArray.Length; i++)
            {
                returnString = returnString + "," + returnArray[i];
            }
            return returnString;
        }
    }
}
