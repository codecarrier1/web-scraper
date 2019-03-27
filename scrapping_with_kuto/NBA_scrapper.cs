using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace scrapping_with_kuto
{
    /// <summary>
    /// http://www.espn.com/nba/teams
    /// </summary>
    class NBA_scrapper
    {
        private static string baseUrl = "http://www.equibase.com/";

        /// <summary>
        /// Gets the team list.
        /// </summary>
        /// <returns></returns>
        public static List<Dictionary<string, string>> getTeamList()
        {
            // lets rewrte the getTeams list

            // http://www.espn.com/nba/team/roster/_/name/atl

            string URL1 = "http://www.espn.com/nba/team/roster/_/name/atl";
            string URL = "http://www.equibase.com/static/entry/index.html?SAP=TN";
            string HTML = httpGet(URL);

            //import kuto
            ktf.Kuto scrapper = new ktf.Kuto(HTML);

            //snip the data limints

            scrapper = scrapper.Extract("<select", "</select>");
            scrapper = scrapper.Extract("</option>", ""); //remove the rist item

            List<Dictionary<string, string>> teams = new List<Dictionary<string, string>>();

            while (scrapper.Contains("<option"))
            {

                //scrap the info
                Dictionary<string, string> teamInfo = new Dictionary<string, string>();

                string url = "http:" + scrapper.Extract("value=\"", "\">").ToString().Trim();
                string name = scrapper.Extract("\">", "</").ToString().Trim();

                teamInfo.Add("name", name);
                teamInfo.Add("url", url);


                teams.Add(teamInfo);

                scrapper = scrapper.Extract("</option>", ""); //removes the first item

            }


            return teams;
        }

        public static List<Dictionary<string, string>> getTrackList(string HTML)
        {
            // lets rewrte the getTeams list

            // http://www.espn.com/nba/team/roster/_/name/atl

            //import kuto
            ktf.Kuto scrapper = new ktf.Kuto(HTML);
            scrapper = scrapper.Extract("</table","</table>");
            scrapper = scrapper.Extract("</thead>","");
            HTML = scrapper.ToString();

            List<Dictionary<string, string>> races = new List<Dictionary<string, string>>();
            while (scrapper.Contains("<tr>"))
            {

                //scrap the info
                Dictionary<string, string> raceInfo = new Dictionary<string, string>();

                
                string name = scrapper.Extract("<td>", "</td>").Extract(">","<").ToString();
                string url = baseUrl + scrapper.Extract("</td>", "").Extract("<a", ">").Extract("/",".").ToString().Trim()+".html";

                raceInfo.Add("name", name);
                raceInfo.Add("url", url);

                if(scrapper.Extract("</td>","").Extract("<td>","</td>").ToString().Contains("<a"))
                    races.Add(raceInfo);

                scrapper = scrapper.Extract("</tr>", ""); //removes the first item
                
                if (scrapper.ToString() == "</tbody>")
                    break;
                

            }

            return races;
        }

        /// <summary>
        /// Gets the players.
        /// </summary>
        /// <param name="team">The team.</param>
        /// <returns></returns>
        public static List<Dictionary<string, string>> GetRaces(string HTML)
        {

            ktf.Kuto scrapper = new ktf.Kuto(HTML);
            scrapper = scrapper.Extract("<table", "</table>");
            scrapper = scrapper.Extract("</thead>", "");
            HTML = scrapper.ToString();

            List<Dictionary<string, string>> races = new List<Dictionary<string, string>>();
            while (scrapper.Contains("<tr>"))
            {

                //scrap the info
                Dictionary<string, string> raceInfo = new Dictionary<string, string>();


                string name = "Race " + scrapper.Extract("Race", "</a>").ToString();
                string url = baseUrl + "static/entry/" + scrapper.Extract("entry\\", "html").ToString().Trim() + "html";

                scrapper = scrapper.Extract("</td>", "");

                string purse = "$" + scrapper.Extract("$", "</td>").ToString();
                scrapper = scrapper.Extract("</td>", "");

                string type;
                if(scrapper.Extract("<td","</td>").Contains("<a"))
                    type = scrapper.Extract("<td>", "</td>").Extract(">","<").ToString();
                else
                    type = scrapper.Extract("<td>", "</td>").ToString();
                scrapper = scrapper.Extract("</td>", "");

                string distance = scrapper.Extract("<td>", "</td>").ToString();
                scrapper = scrapper.Extract("</td>", "");
                
                string surface = scrapper.Extract("<td>", "</td>").ToString();
                scrapper = scrapper.Extract("</td>", "");

                string temp = scrapper.ToString();

                string starter = scrapper.Extract(">", "</td>").ToString();
                scrapper = scrapper.Extract("</td>", "");

                string post = scrapper.Extract(">", "</td>").ToString();
                scrapper = scrapper.Extract("</td>", "");


                raceInfo.Add("name", name);
                raceInfo.Add("url", url);
                raceInfo.Add("purse", purse);
                raceInfo.Add("type", type);
                raceInfo.Add("distance", distance);
                raceInfo.Add("surface", surface);
                raceInfo.Add("starter", starter);
                raceInfo.Add("post", post);


                races.Add(raceInfo);

                scrapper = scrapper.Extract("</tr>", ""); //removes the first item

                if (scrapper.ToString() == "</tbody>")
                    break;


            }


            return races;
        }

        public static string GetBirthday(string HTML)
        {
            ktf.Kuto scrapper = new ktf.Kuto(HTML);
            string birthday = scrapper.Extract("foaled", "<").ToString();
            return birthday;
        }


        public static List<Dictionary<string, string>> GetHorses(string HTML)
        {

            bool claiming = false;
            ktf.Kuto scrapper = new ktf.Kuto(HTML);
            scrapper = scrapper.Extract("<table", "</table>");
            if (scrapper.Extract("<thead>", "</thead>").Contains("Claim"))
                claiming = true;
            scrapper = scrapper.Extract("</thead>", "");
            HTML = scrapper.ToString();

            List<Dictionary<string, string>> horses = new List<Dictionary<string, string>>();
            while (scrapper.Contains("<tr>"))
            {

                //scrap the info
                Dictionary<string, string> horseInfo = new Dictionary<string, string>();

                if(!scrapper.Extract("<tr","</tr").Contains("Scratched"))
                {
                    string ps = scrapper.Extract("<td", "</td>").Extract("<div", "/div>").Extract(">", "<").ToString();
                    if((ps.Length==2)&&(ps[1]>='A' && ps[1]<='Z'))
                    {
                        ps = ps.Remove(1);
                    }
                    if(ps.Length == 3)
                    {
                        ps = ps.Remove(2);
                    }
                    scrapper = scrapper.Extract("</td>", "");

                    string pp = scrapper.Extract("<td", "</td>").Extract("<b>", "</b>").ToString();
                    scrapper = scrapper.Extract("</td>", "");

                    //string temp = scrapper.Extract("<td", "</td>").ToString();

                    string name = scrapper.Extract("<td", "</td>").Extract("<a", "/a>").Extract(">", "<").ToString();

                    string url = "http:" + scrapper.Extract("<td", "</td>").Extract("http:", '"'.ToString()).ToString().Trim();
                    url = url.Replace("amp;", "");

                    if (claiming == true)
                        scrapper = scrapper.Extract("</td>", "").Extract("</td>", "").Extract("</td>", "").Extract("</td>", "").Extract("</td>", "").Extract("</td>", "").Extract("</td>", "").Extract("</td>", "");
                    else
                        scrapper = scrapper.Extract("</td>", "").Extract("</td>", "").Extract("</td>", "").Extract("</td>", "").Extract("</td>", "").Extract("</td>", "").Extract("</td>", "");

                    string ml = scrapper.Extract(">", "<").ToString();
                    
                    scrapper = scrapper.Extract("</td>", "");

                    horseInfo.Add("pp", ps);
                    horseInfo.Add("horse", name);
                    horseInfo.Add("url", url);
                    horseInfo.Add("ml", ml);
                    
                    horses.Add(horseInfo);
                }

                scrapper = scrapper.Extract("</tr>", ""); //removes the first item

                if (scrapper.ToString() == "</tbody>")
                    break;

            }
            return horses;

            
        }


        /// <summary>
        /// lOADS WEBSITE html
        /// </summary>
        /// <param name="URL">The URL.</param>
        /// <returns></returns>
        static string httpGet(string URL)
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = client.GetAsync(URL).Result)
                {
                    Thread.Sleep(2000);
                    using (HttpContent content = response.Content)
                    {
                        string result = content.ReadAsStringAsync().Result;
                        return result;
                    }
                }
            }

        }
    }
}
