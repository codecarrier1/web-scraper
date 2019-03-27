using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.IO;

namespace scrapping_with_kuto
{
    public partial class MainForm : Form
    {

        public string raceUrl = "http://www.equibase.com/static/entry/index.html?SAP=TN";
        public List<Dictionary<string, string>> tracks;
        public List<Dictionary<string, string>> races;
        public List<Dictionary<string, string>> horses;
        public WebBrowser[] browsers;
        public bool isCalled = false;
        public int indexHorse = 0;
        public Thread[] threads,threadBrowser3;

        public void SplashStart()
        {
            Application.Run(new Splash());
        }

        public MainForm()
        {
            
            //Thread t = new Thread(new ThreadStart(SplashStart));
            //t.Start();
            //Thread.Sleep(5000);
            InitializeComponent();            
            //t.Abort();
           
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //LETS LOAD THE TEAMS TO COMOBO BOX
            //this.Cursor = Cursors.WaitCursor;
            //------Load Races-----------------
            
            webBrowser1.Navigate(raceUrl);
            webBrowser1.ScriptErrorsSuppressed = true;
            //MessageBox.Show("sdf");
            webBrowser1.Visible = false;

            webBrowser3.ScriptErrorsSuppressed = true;
            //------------------

        }

        public void SetTrack()
        {
            MessageBox.Show("Loading Completed!");
            cmbTrack.Items.Clear();
            cmbTrack.Tag = tracks;
            foreach(Dictionary<string,string> track in tracks)
            {
                cmbTrack.Items.Add(track["name"]);
            }
        }

        public void SetRaces()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Tag = races;
            webBrowser2.AllowNavigation = false;
            this.Cursor = Cursors.Default;
            foreach (Dictionary<string,string> race in races)
            {
                string[] row = new string[] { race["name"], race["purse"], race["type"], race["distance"], race["surface"], race["starter"], race["post"]};
                dataGridView1.Rows.Add(row);
            }
            this.Cursor = Cursors.Default;
        }

        public void SetHorses()
        {
            //MessageBox.Show("Set Horses");
            //horses = new List<Dictionary<string, string>>();

            threads = new Thread[horses.Count];

            dataGridView2.Rows.Clear();
            dataGridView2.Tag = horses;
            
            
            foreach(Dictionary<string,string> horse in horses)
            {
                /*
                this.Cursor = Cursors.WaitCursor;
                browsers[horses.IndexOf(horse)] = new WebBrowser();
                browsers[horses.IndexOf(horse)].AllowNavigation = true;
                browsers[horses.IndexOf(horse)].Visible = false;
                browsers[horses.IndexOf(horse)].ScriptErrorsSuppressed = true;
                
                browsers[horses.IndexOf(horse)].DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(funcAsync);

                 void funcAsync(object sender, WebBrowserDocumentCompletedEventArgs e)
                {

                    if (e.Url.AbsolutePath != (sender as WebBrowser).Url.AbsolutePath)
                        return;
                    MessageBox.Show("a");
                    /*
                    string htmlcode = ((WebBrowser)sender).Document.Body.InnerHtml;
                    //foreach(DataGridViewRow )
                    if(dataGridView2.Rows.Count < horses.Count)
                    {
                        horse.Add("birthday", NBA_scrapper.GetBirthday(htmlcode));

                        //-----------Calculate Biorhythms----------------------------

                        DateTime today = DateTime.Today;
                        DateTime birth = Convert.ToDateTime(horse["birthday"]);

                        TimeSpan dayAlive = today.Subtract(birth);

                        int phy = (int) (Math.Sin(2 * Math.PI * dayAlive.Days / 23) * 100);
                        int emo = (int)(Math.Sin(2 * Math.PI * dayAlive.Days / 28) * 100);
                        int intel = (int)(Math.Sin(2 * Math.PI * dayAlive.Days / 33) * 100);
                        int aes = (int)(Math.Sin(2 * Math.PI * dayAlive.Days / 43) * 100);
                        int awa = (int)(Math.Sin(2 * Math.PI * dayAlive.Days / 48) * 100);
                        int spi = (int)(Math.Sin(2 * Math.PI * dayAlive.Days / 53) * 100);
                        int intu = (int)(Math.Sin(2 * Math.PI * dayAlive.Days / 38) * 100);

                        int[] bio = { phy, emo, intel, aes, awa, spi, intu };

                        //string[] row = new string[] { horse["pp"], horse["horse"], horse["ml"], horse["birthday"],
                        //phy.ToString(), emo.ToString(), intel.ToString(), aes.ToString(), awa.ToString(), spi.ToString(), intu.ToString()};

                        //-----------------------------------------------------------
                        horse.Add("phy", phy.ToString());
                        horse.Add("emo", emo.ToString());
                        horse.Add("intel", intel.ToString());
                        horse.Add("aes", aes.ToString());
                        horse.Add("awa", awa.ToString());
                        horse.Add("spi", spi.ToString());
                        horse.Add("intu", intu.ToString());
                        int index = dataGridView2.Rows.Add(Convert.ToInt32(horse["pp"]), horse["horse"], horse["ml"], horse["birthday"],
                            phy, emo, intel, aes, awa, spi, intu);
                        for(int i = 4; i < 11; i++)
                        {
                            if (bio[i - 4] > 90)
                                dataGridView2.Rows[index].Cells[i].Style.ForeColor = Color.Green;
                            if (bio[i - 4] <-90)
                                dataGridView2.Rows[index].Cells[i].Style.ForeColor = Color.Red;
                            if (bio[i - 4] == 0)
                                dataGridView2.Rows[index].Cells[i].Style.ForeColor = Color.Yellow;
                        }
                        dataGridView2.Sort(dataGridView2.Columns[0], ListSortDirection.Ascending);
                    }
                    
                    threads[horses.IndexOf(horse)] = null;
                }
                */
                threads[horses.IndexOf(horse)] = new Thread(() => webBrowser4.Navigate(horse["url"]));
                
            }
            //MessageBox.Show(threads.Count().ToString());
            threads[0].Start();
            this.Cursor = Cursors.Default;
        }

        public void SetBio()
        {
            DateTime today = DateTime.Today;
            Dictionary<string, string> horse = horses[indexHorse];
                DateTime birth = Convert.ToDateTime(horse["birthday"]);

                TimeSpan dayAlive = today.Subtract(birth);

                int phy = (int)(Math.Sin(2 * Math.PI * dayAlive.Days / 23) * 100);
                int emo = (int)(Math.Sin(2 * Math.PI * dayAlive.Days / 28) * 100);
                int intel = (int)(Math.Sin(2 * Math.PI * dayAlive.Days / 33) * 100);
                int aes = (int)(Math.Sin(2 * Math.PI * dayAlive.Days / 43) * 100);
                int awa = (int)(Math.Sin(2 * Math.PI * dayAlive.Days / 48) * 100);
                int spi = (int)(Math.Sin(2 * Math.PI * dayAlive.Days / 53) * 100);
                int intu = (int)(Math.Sin(2 * Math.PI * dayAlive.Days / 38) * 100);

                int[] bio = { phy, emo, intel, aes, awa, spi, intu };

                //string[] row = new string[] { horse["pp"], horse["horse"], horse["ml"], horse["birthday"],
                //phy.ToString(), emo.ToString(), intel.ToString(), aes.ToString(), awa.ToString(), spi.ToString(), intu.ToString()};

                //-----------------------------------------------------------
                horse.Add("phy", phy.ToString());
                horse.Add("emo", emo.ToString());
                horse.Add("intel", intel.ToString());
                horse.Add("aes", aes.ToString());
                horse.Add("awa", awa.ToString());
                horse.Add("spi", spi.ToString());
                horse.Add("intu", intu.ToString());
                int index = dataGridView2.Rows.Add(Convert.ToInt32(horse["pp"]), horse["horse"], horse["ml"], horse["birthday"],
                    phy, emo, intel, aes, awa, spi, intu);
                for (int i = 4; i < 11; i++)
                {
                    if (bio[i - 4] > 90)
                        dataGridView2.Rows[index].Cells[i].Style.ForeColor = Color.Green;
                    if (bio[i - 4] < -90)
                        dataGridView2.Rows[index].Cells[i].Style.ForeColor = Color.Red;
                    if (bio[i - 4] == 0)
                        dataGridView2.Rows[index].Cells[i].Style.ForeColor = Color.Yellow;
                }
                dataGridView2.Sort(dataGridView2.Columns[0], ListSortDirection.Ascending);
   
        }

        private void cmbTrack_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView2.Rows.Clear();

            List<Dictionary<string, string>> tracks = (List<Dictionary<string, string>>)cmbTrack.Tag;
            if(cmbTrack.SelectedIndex>=0 && cmbTrack.SelectedIndex<cmbTrack.Items.Count)
            {
                Dictionary<string, string> track = tracks[cmbTrack.SelectedIndex];
                //this.Cursor = Cursors.WaitCursor;
                webBrowser2.AllowNavigation = true;
                webBrowser2.Navigate(track["url"]);
            }

        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (e.Url.AbsolutePath != (sender as WebBrowser).Url.AbsolutePath)
                return;
            string htmlcode = webBrowser1.Document.Body.InnerHtml;
            if(htmlcode.Contains("Captcha"))
            {
                webBrowser1.Visible = true;
                dataGridView2.Visible = false;
            }
            else
            {
                webBrowser1.Visible = false;
                dataGridView2.Visible = true;
                tracks = NBA_scrapper.getTrackList(htmlcode);
                if (cmbTrack.Items.Count == 0)
                    SetTrack();
            }
        }

        private void webBrowser2_DocumentCompleted_1(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (e.Url.AbsolutePath != (sender as WebBrowser).Url.AbsolutePath)
                return;
            string htmlcode = webBrowser2.Document.Body.InnerHtml;
            races = NBA_scrapper.GetRaces(htmlcode);

            //if (dataGridView1.RowCount == 0)
            SetRaces();
            webBrowser2.Stop();
        }

        private void webBrowser3_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (e.Url.AbsolutePath != (sender as WebBrowser).Url.AbsolutePath)
                return;
            string htmlcode = webBrowser3.Document.Body.InnerHtml;
            horses = new List<Dictionary<string, string>>();
            horses = NBA_scrapper.GetHorses(htmlcode);
            webBrowser3.AllowNavigation = false;
            webBrowser3.Stop();

            //Thread thread1 = new Thread(() => SetHorses());
            //thread1.Start();
            //Thread.Sleep(1000);
            SetHorses();
            
            
            //MessageBox.Show("asdf");
            
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex >= 0)
            {
                string url = races.ElementAt(e.RowIndex)["url"];
                this.Cursor = Cursors.WaitCursor;
                webBrowser3.AllowNavigation = true;
                horses = new List<Dictionary<string, string>>();
                indexHorse = 0;
                webBrowser3.Navigate(url);
            }
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        private void webBrowser4_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (e.Url.AbsolutePath != (sender as WebBrowser).Url.AbsolutePath)
                return;

            string htmlcode = webBrowser4.Document.Body.InnerHtml;
            string birthday = NBA_scrapper.GetBirthday(htmlcode);
            horses[indexHorse].Add("birthday", birthday);
            SetBio();
            indexHorse++;
            if (indexHorse < threads.Count())
            {
                threads[indexHorse].Start();
            }
            else
                MessageBox.Show("Loading Completed");
        }   
    }
}