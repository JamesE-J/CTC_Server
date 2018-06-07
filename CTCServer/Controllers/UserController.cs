using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CTCServer.Models;

namespace CTCServer.Controllers
{
    public class UserController : ApiController
    {
        [ActionName("New")]
        public string POST([FromBody]User user)
        {
            if (UserDatabaseAccess.checkUserName(user.UserName))
            {
                return "Error: Username already exists in database";
            }
            else
            {
                if (UserDatabaseAccess.newUser(user))
                {
                    return "Success: New User created";
                }
                else
                {
                    return "Error: Cannot Create Entry in Database";
                }
            }
        }
        [ActionName("Login")]
        public HttpResponseMessage GET([FromUri]User user)
        {

            if (UserDatabaseAccess.checkPassword(user))
            {
                Guid UserKey = UserDatabaseAccess.getUserKey(user);
                if (UserKey == Guid.Empty)
                {
                    return Request.CreateResponse(HttpStatusCode.Accepted,
                        "Error: Cannot find UserKey");
                }
                else
                {
                    string userTeam = UserDatabaseAccess.getUserTeam(user);

                    return Request.CreateResponse(HttpStatusCode.Accepted,
                        UserKey.ToString() + "," + userTeam);
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.Accepted,
                    "Error: Incorrect Password or Username");
            }
        }
        [ActionName("GetScores")]
        public HttpResponseMessage GET()
        {
            String[] resultArray = UserDatabaseAccess.getTopScores();
            string result = null;
            for (int i = 0; i < resultArray.Length; i++)
            {
                result = result + "," + resultArray[i];
            }
            return Request.CreateResponse(HttpStatusCode.Accepted, result);
        }
    }
}
