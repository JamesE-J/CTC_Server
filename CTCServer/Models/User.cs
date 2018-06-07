using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml;

namespace CTCServer.Models
{
    public class User
    {
        [Key]
        public Guid UserKey { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Team { get; set; }
        public int Score { get; set; }
        public string Zone { get; set; }
        public User()
        {
            Score = 0;
        }
    }

    public class Zone
    {
        [Key]
        public Guid ZoneKey { get; set; }
        public string ZoneNumber { get; set; }
        public int ZoneDominance { get; set; }
        public Zone()
        {

        }
    }

    public class UserDatabaseAccess
    {
        public static void updateDominance(int zoneNumber, string team, Guid UserKey)
        {
            Zone zone = new Zone();
            using (var db = new UserContext())
            {
                if (team == "Red Team")
                {
                    var query = from z in db.Zones
                                where z.ZoneNumber == zoneNumber.ToString()
                                select z;
                    foreach (var result in query)
                    {
                        zone = result;
                        db.Zones.Remove(result);
                    }
                    db.SaveChanges();
                    zone.ZoneDominance = zone.ZoneDominance + 1;
                    db.Zones.Add(zone);
                    db.SaveChanges();
                }
                else
                {
                    var query = from z in db.Zones
                                where z.ZoneNumber == zoneNumber.ToString()
                                select z;
                    foreach (var result in query)
                    {
                        zone = result;
                        db.Zones.Remove(result);
                    }
                    db.SaveChanges();
                    zone.ZoneDominance = zone.ZoneDominance - 1;
                    db.Zones.Add(zone);
                    db.SaveChanges();
                }
                addScore(UserKey);
            }
        }
        public static string[] getZoneState()
        {
            string[] returnArray = new string[8];
            using (var db = new UserContext())
            {
                for (int i = 0; i < 8; i++)
                {
                    var query = from z in db.Zones
                                where z.ZoneNumber == i.ToString()
                                select z;
                    foreach (var result in query)
                    {
                        returnArray[i] = result.ZoneDominance.ToString();
                    }
                }
            }
            return returnArray;
        }
        public static bool checkForZones()
        {
            using (var db = new UserContext())
            {
                for (int i = 0; i < 8; i++)
                {
                    Boolean exists = false;
                    var query = from z in db.Zones
                                where z.ZoneNumber == i.ToString()
                                select z;
                    foreach (var result in query)
                    {
                        exists = true;
                    }
                    if (exists != true)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public static string[] initialiseZones()
        {
            string[] returnArray = new string[8];
            using (var db = new UserContext())
            {
                var query = from z in db.Zones
                            select z;
                foreach (var result in query)
                {
                    db.Zones.Remove(result);
                }
                db.SaveChanges();
                for (int i = 0; i < 8; i = i + 1)
                {
                    Zone zone = new Zone();
                    zone.ZoneKey = Guid.NewGuid();
                    zone.ZoneNumber = i.ToString();
                    zone.ZoneDominance = 50;
                    returnArray[i] = zone.ZoneDominance.ToString();
                    db.Zones.Add(zone);
                    db.SaveChanges();
                }
            }
            return returnArray;
        }
        public static bool newUser(User user)
        {
            if (user.UserName != null && user.Password != null)
            {
                user.UserKey = Guid.NewGuid();
                using (var db = new UserContext())
                {
                    int redTeamNumbers = getRedTeamNumbers();
                    int blueteamNumbers = getBlueTeamNumbers();
                    int teamNumberDifferance = 0;
                    if(redTeamNumbers > blueteamNumbers)
                    {
                        teamNumberDifferance = redTeamNumbers - blueteamNumbers;
                        if (teamNumberDifferance > blueteamNumbers / 10)
                        {
                            user.Team = "Blue Team";
                        }
                    }
                    else if (redTeamNumbers < blueteamNumbers)
                    {
                        teamNumberDifferance = blueteamNumbers - redTeamNumbers;
                        if (teamNumberDifferance > redTeamNumbers / 10)
                        {
                            user.Team = "Red Team";
                        } 
                    }
                    db.Users.Add(user);
                    db.SaveChanges();
                    var query = from u in db.Users
                                where u.UserKey == user.UserKey
                                select u;
                    foreach (var result in query)
                    {
                        if (result.UserName == user.UserName)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public static int getRedTeamNumbers()
        {
            int teamNumber = 0;
            using (var db = new UserContext())
            {
                var query = from u in db.Users
                            where u.Team == "Red Team"
                            select u;
                foreach (var result in query)
                {
                    teamNumber = teamNumber + 1;
                }
            }
            return teamNumber;
        }
        public static string[] getTopScores()
        {
            String[] returnArray = new String[10];
            int index = 0;
            using (var db = new UserContext())
            {
                var query = (from u in db.Users.OrderByDescending(s => s.Score).Take(10).Select(s => s.Score).Distinct()
                            from i in db.Users
                            where u == i.Score
                            select i).ToList();
                foreach (var result in query)
                {
                    returnArray[index] = result.UserName + "," + result.Score + "," + result.Team;
                    index++;
                }
            }
            return returnArray;
        }
        public static int getBlueTeamNumbers()
        {
            int teamNumber = 0;
            using (var db = new UserContext())
            {
                var query = from u in db.Users
                            where u.Team == "Blue Team"
                            select u;
                foreach (var result in query)
                {
                    teamNumber = teamNumber + 1;
                }
            }
            return teamNumber;
        }
        public static void addScore(Guid UserKey)
        {
            using (var db = new UserContext())
            {
                User user = new User();
                var query = from u in db.Users
                            where u.UserKey == UserKey
                            select u;
                foreach (var result in query)
                {
                    user = result;
                    user.Score = result.Score + 1;
                    db.Users.Remove(result);
                }
                db.SaveChanges();
                db.Users.Add(user);
                db.SaveChanges();
            }
        }
        public static bool checkUserName(string pUserName)
        {
            using (var db = new UserContext())
            {
                var query = from u in db.Users where 
                            u.UserName == pUserName select u;
                foreach (var result in query)
                {
                    if (result.UserName == pUserName)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public static bool checkPassword(User user)
        {
            using (var db = new UserContext())
            {
                var query = from u in db.Users
                            where u.UserName == user.UserName
                            select u;
                foreach (var result in query)
                {
                    if (result.Password == user.Password)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public static Guid getUserKey(User user)
        {
            using (var db = new UserContext())
            {
                var query = from u in db.Users
                            where u.UserName == user.UserName
                            select u;
                foreach (var result in query)
                {
                    if (result.UserName == user.UserName)
                    {
                        return result.UserKey;
                    }
                }
            }
            return Guid.Empty;
        }
        public static string getUserTeam(User user)
        {
            using (var db = new UserContext())
            {
                var query = from u in db.Users
                            where u.UserName == user.UserName
                            select u;
                foreach (var result in query)
                {
                    if (result.UserName == user.UserName)
                    {
                        return result.Team;
                    }
                }
            }
            return "Error: no team assigned";
        }
    }
}