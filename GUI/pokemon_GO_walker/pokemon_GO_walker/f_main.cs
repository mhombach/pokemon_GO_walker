using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Threading;
using System.IO;
using System.Diagnostics;

namespace pokemon_GO_walker
{
    public partial class f_main : Form
    {
        // Declare global variables
        static TelnetConnection tc1; // Telnetconnection (Player 1)
        static TelnetConnection tc2; // Telnetconnection (Player 2)
        Point loc; // Pointer for Form-Movement
        int player1_autowalk_counter; // Counter for Autowalk (Player 1)
        int player2_autowalk_counter; // Counter for Autowalk (Player 2)
        bool resetConfigFlag = false; // Flag for global config-reset
        Random rnd = new Random(); // Random-Object for Autowalk


        public f_main()
        {
             InitializeComponent();
        }

        private void L_titlebar_MouseDown(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                loc = e.Location;
            }
        }

        private void L_titlebar_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                f_main.ActiveForm.Left += (e.Location.X - loc.X);
                f_main.ActiveForm.Top += (e.Location.Y - loc.Y);
            }
        }

        private void L_titlebar_close_Click(object sender, EventArgs e)
        {
            f_main.ActiveForm.Close();
        }

        private void f_main_Load(object sender, EventArgs e)
        {
            loadConfig();
            LinkLabel.Link link = new LinkLabel.Link();
            link.LinkData = "http://q.gs/14246747/pokemon-go-walker";
            L_menu_website.Links.Add(link);

            TAB_player1_manual.BackColor = Color.Transparent;
            TABPAGE_player1_manual.BackColor = Color.Transparent;
            TABPAGE_player1_autowalk.BackColor = Color.Transparent;
            TABPAGE_player1_bookmarks.BackColor = Color.Transparent;
            TABPAGE_player1_log.BackColor = Color.Transparent;
            TAB_player1_manual.Visible = true;
        }

        private void B_player1_autowalk_toggle_Click(object sender, EventArgs e)
        {
            if(TIMER_player1_autowalk.Enabled)
            {
                TIMER_player1_autowalk.Enabled = false;
                T_player1_lat.ReadOnly = false;
                T_player1_lon.ReadOnly = false;
                L_player1_autowalk_status.Text = "OFF";
                L_player1_autowalk_status.ForeColor = Color.Red;
            }
            else
            {
                T_player1_lat.ReadOnly = true;
                T_player1_lon.ReadOnly = true;
                player1_autowalk_counter = int.Parse(T_player1_autowalk_change.Text);
                TIMER_player1_autowalk.Interval = 1000 * int.Parse(T_player1_autowalk_seconds.Text);
                TIMER_player1_autowalk.Enabled = true;
                L_player1_autowalk_status.Text = "ON";
                L_player1_autowalk_status.ForeColor = Color.Chartreuse;
            }
        }

        public void loadConfig()
        {
            // load Player 1 config 
            if (Properties.Settings.Default.T_player1_ip != "") { T_player1_ip.Text = Properties.Settings.Default.T_player1_ip; }
            if (Properties.Settings.Default.T_player1_port != "") { T_player1_port.Text = Properties.Settings.Default.T_player1_port; }
            if (Properties.Settings.Default.T_player1_lat != "") { T_player1_lat.Text = Properties.Settings.Default.T_player1_lat; }
            if (Properties.Settings.Default.T_player1_lon != "") { T_player1_lon.Text = Properties.Settings.Default.T_player1_lon; }
            if (Properties.Settings.Default.T_player1_step != "") { T_player1_step.Text = Properties.Settings.Default.T_player1_step; }
            if (Properties.Settings.Default.T_player1_autowalk_seconds != "") { T_player1_autowalk_seconds.Text = Properties.Settings.Default.T_player1_autowalk_seconds; }
            if (Properties.Settings.Default.T_player1_autowalk_step != "") { T_player1_autowalk_step.Text = Properties.Settings.Default.T_player1_autowalk_step; }
            if (Properties.Settings.Default.C_player1_autowalk_direction != "") { C_player1_autowalk_direction.Text = Properties.Settings.Default.C_player1_autowalk_direction; }
            if (Properties.Settings.Default.T_player1_autowalk_change != "") { T_player1_autowalk_change.Text = Properties.Settings.Default.T_player1_autowalk_change; }
        }

        private void saveConfig()
        {
            // Save player 1 settings
            Properties.Settings.Default.T_player1_ip = T_player1_ip.Text;
            Properties.Settings.Default.T_player1_port = T_player1_port.Text;
            Properties.Settings.Default.T_player1_lat = T_player1_lat.Text;
            Properties.Settings.Default.T_player1_lon = T_player1_lon.Text;
            Properties.Settings.Default.T_player1_step = T_player1_step.Text;
            Properties.Settings.Default.T_player1_autowalk_seconds = T_player1_autowalk_seconds.Text;
            Properties.Settings.Default.T_player1_autowalk_step = T_player1_autowalk_step.Text;
            Properties.Settings.Default.C_player1_autowalk_direction = C_player1_autowalk_direction.Text;
            Properties.Settings.Default.T_player1_autowalk_change = T_player1_autowalk_change.Text;

            Properties.Settings.Default.Save(); // Saves settings in application configuration file
        }

        private void openConnection(int p)
        {
            writeLog(p, @"Will try to open connection...");
            string answer = "";
            if (p == 1)
            {
                tc1 = new TelnetConnection(T_player1_ip.Text, int.Parse(T_player1_port.Text));
                answer = tc1.Read();
            }
            else if (p == 2)
            {
                //tc2 = new TelnetConnection(t_ip2.Text, int.Parse(t_port2.Text));
                //answer = tc2.Read();
            }
            answer = answer.Replace("\r", ".");
            answer = answer.Replace("\n", ".");
            if (answer == "MockGeoFix: type 'help' for a list of commands..OK..")
            {
                writeLog(p, "Connection is open :)");
            }
        }

        private void writeLog(int p, string s, bool append = false)
        {
            if (p == 1)
            {
                if (append == false)
                {
                    RTB_player1_log.AppendText(Environment.NewLine);
                }
                RTB_player1_log.AppendText(DateTime.Now.ToShortTimeString() + ": " + s);
            }
            else if (p == 2)
            {
                if (append == false)
                {
                    //RTB_player2_log.AppendText(Environment.NewLine);
                }
                //RTB_player2_log.AppendText(DateTime.Now.ToShortTimeString() + ": " + s);
            }
        }

        private void sendGps(int p, string lat, string lon)
        {
            string GPS_lon = lon.Replace(",", ".");
            string GPS_lat = lat.Replace(",", ".");
            string tc_return = "";
            if (p == 1)
            {
                tc_return = tc1.writeRead(@"geo fix " + GPS_lon + " " + GPS_lat);
            }
            else if (p == 2)
            {
                tc_return = tc2.writeRead(@"geo fix " + GPS_lon + " " + GPS_lat);
            }

            writeLog(p, "New Location (Lat/Long): " + GPS_lat + " / " + GPS_lon);
            //string answer = tc.Read();
            writeLog(p, "Status:" + tc_return);
        }

        private void goUp(int p)
        {
            decimal pos_lat = 0;
            string newLat = "";
            string newLon = "";

            if (p == 1)
            {
                // Latitude
                pos_lat = decimal.Parse(T_player1_lat.Text);
                pos_lat = pos_lat + decimal.Parse(T_player1_step.Text);
                newLat = pos_lat.ToString();
                T_player1_lat.Text = newLat;

                // Longitude is same as t_longitude
                newLon = T_player1_lon.Text;
            }
            else if (p == 2)
            {
                // Latitude
                /*pos_lat = decimal.Parse(t_latitude2.Text);
                pos_lat = pos_lat + decimal.Parse(t_step2.Text);
                newLat = pos_lat.ToString();
                t_latitude2.Text = newLat;

                // Longitude is same as t_longitude
                newLong = t_longitude2.Text;*/
            }

            // Send
            sendGps(p, newLat, newLon);
        }

        private void goDown(int p)
        {
            decimal pos_lat = 0;
            string newLat = "";
            string newLon = "";

            if (p == 1)
            {
                // Latitude
                pos_lat = decimal.Parse(T_player1_lat.Text);
                pos_lat = pos_lat - decimal.Parse(T_player1_step.Text);
                newLat = pos_lat.ToString();
                T_player1_lat.Text = newLat;

                // Longitude is same as t_longitude
                newLon = T_player1_lon.Text;
            }
            else if (p == 2)
            {
                // Latitude
                /*pos_lat = decimal.Parse(t_latitude2.Text);
                pos_lat = pos_lat - decimal.Parse(t_step2.Text);
                newLat = pos_lat.ToString();
                t_latitude2.Text = newLat;

                // Longitude is same as t_longitude
                newLong = t_longitude2.Text;*/
            }

            // Send
            sendGps(p, newLat, newLon);
        }

        private void goLeft(int p)
        {
            decimal pos_long = 0;
            string newLat = "";
            string newLon = "";

            if (p == 1)
            {
                // Latitude is same as t_latitude
                newLat = T_player1_lat.Text;

                // Longitude
                pos_long = decimal.Parse(T_player1_lon.Text);
                pos_long = pos_long - decimal.Parse(T_player1_step.Text);
                newLon = pos_long.ToString();
                T_player1_lon.Text = newLon;
            }
            else if (p == 2)
            {
                // Latitude is same as t_latitude
                /*newLat = t_latitude2.Text;

                // Longitude
                pos_long = decimal.Parse(t_longitude2.Text);
                pos_long = pos_long - decimal.Parse(t_step2.Text);
                newLong = pos_long.ToString();
                t_longitude2.Text = newLong;*/
            }


            // Send
            sendGps(p, newLat, newLon);
        }

        private void goRight(int p)
        {
            decimal pos_long = 0;
            string newLat = "";
            string newLon = "";

            if (p == 1)
            {
                // Latitude is same as t_latitude
                newLat = T_player1_lat.Text;

                // Longitude
                pos_long = decimal.Parse(T_player1_lon.Text);
                pos_long = pos_long + decimal.Parse(T_player1_step.Text);
                newLon = pos_long.ToString();
                T_player1_lon.Text = newLon;
            }
            else if (p == 2)
            {
                // Latitude is same as t_latitude
                /* newLat = t_latitude2.Text;

                // Longitude
                pos_long = decimal.Parse(t_longitude2.Text);
                pos_long = pos_long + decimal.Parse(t_step2.Text);
                newLong = pos_long.ToString();
                t_longitude2.Text = newLong;*/
            }


            // Send
            sendGps(p, newLat, newLon);
        }

        private void B_resetConfig_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reset();
            resetConfigFlag = true;
            Application.Restart();
        }

        private void B_player1_connect_Click(object sender, EventArgs e)
        {
            openConnection(1);
        }

        private void T_hotkey_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                // Left side
                case Keys.W:
                    writeLog(1, "Hotkey -> W");
                    goUp(1);
                    break;
                case Keys.S:
                    writeLog(1, "Hotkey -> S");
                    goDown(1);
                    break;
                case Keys.A:
                    writeLog(1, "Hotkey -> A");
                    goLeft(1);
                    break;
                case Keys.D:
                    writeLog(1, "Hotkey -> D");
                    goRight(1);
                    break;

                // Right side
               /* case Keys.NumPad8:
                    writeLog(2, "Hotkey -> 8");
                    goUp(2);
                    break;
                case Keys.NumPad2:
                    writeLog(2, "Hotkey -> 2");
                    goDown(2);
                    break;
                case Keys.NumPad5:
                    writeLog(2, "Hotkey -> 5");
                    goDown(2);
                    break;
                case Keys.NumPad4:
                    writeLog(2, "Hotkey -> 4");
                    goLeft(2);
                    break;
                case Keys.NumPad6:
                    writeLog(2, "Hotkey -> 6");
                    goRight(2);
                    break;*/
                default:
                    Console.WriteLine("Unknown key Pressed");
                    break;
            }
        }

        private void B_player1_setGps_Click(object sender, EventArgs e)
        {
            sendGps(1, T_player1_lat.Text, T_player1_lon.Text);
        }

        private void B_player1_refreshGps_Click(object sender, EventArgs e)
        {
            sendGps(1, T_player1_lat.Text, T_player1_lon.Text);
        }

        private void B_player1_UP_Click(object sender, EventArgs e)
        {
            goUp(1);
        }

        private void B_player1_DOWN_Click(object sender, EventArgs e)
        {
            goDown(1);
        }

        private void B_player1_LEFT_Click(object sender, EventArgs e)
        {
            goLeft(1);
        }

        private void B_player1_RIGHT_Click(object sender, EventArgs e)
        {
            goRight(1);
        }

        private void TIMER_player1_autowalk_Tick(object sender, EventArgs e)
        {
            decimal pos_lat, pos_lon;
            string newLat, newLon;

            switch (C_player1_autowalk_direction.Text)
            {
                case "UP":
                    pos_lat = decimal.Parse(T_player1_lat.Text);
                    pos_lat = pos_lat + decimal.Parse(T_player1_autowalk_step.Text);
                    newLat = pos_lat.ToString();
                    T_player1_lat.Text = newLat;

                    // Longitude is same as t_longitude
                    newLon = T_player1_lon.Text;

                    sendGps(1, newLat, newLon);
                    break;

                case "DOWN":
                    pos_lat = decimal.Parse(T_player1_lat.Text);
                    pos_lat = pos_lat - decimal.Parse(T_player1_autowalk_step.Text);
                    newLat = pos_lat.ToString();
                    T_player1_lat.Text = newLat;

                    // Longitude is same as t_longitude
                    newLon = T_player1_lon.Text;

                    sendGps(1, newLat, newLon);
                    break;

                case "LEFT":
                    newLat = T_player1_lat.Text;

                    // Longitude
                    pos_lon = decimal.Parse(T_player1_lon.Text);
                    pos_lon = pos_lon - decimal.Parse(T_player1_autowalk_step.Text);
                    newLon = pos_lon.ToString();
                    T_player1_lon.Text = newLon;

                    sendGps(1, newLat, newLon);
                    break;

                case "RIGHT":
                    newLat = T_player1_lat.Text;

                    // Longitude
                    pos_lon = decimal.Parse(T_player1_lon.Text);
                    pos_lon = pos_lon + decimal.Parse(T_player1_autowalk_step.Text);
                    newLon = pos_lon.ToString();
                    T_player1_lon.Text = newLon;

                    sendGps(1, newLat, newLon);
                    break;
            }

            if (C_player1_autowalk_direction.Text != "")
            {
                player1_autowalk_counter--;
                if (player1_autowalk_counter <= 0)
                {
                    writeLog(1, "Autowalk: changing direction");
                    switch (rnd.Next(0, 4))
                    {
                        case 0:
                            C_player1_autowalk_direction.Text = "UP";
                            break;
                        case 1:
                            C_player1_autowalk_direction.Text = "DOWN";
                            break;
                        case 2:
                            C_player1_autowalk_direction.Text = "LEFT";
                            break;
                        case 3:
                            C_player1_autowalk_direction.Text = "RIGHT";
                            break;
                    }
                    player1_autowalk_counter = int.Parse(T_player1_autowalk_change.Text);
                }
                writeLog(1, "Autowalk: " + C_player1_autowalk_direction.ToString() + " steps until changing direction");
            }
        }

        private void f_main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (resetConfigFlag == false)
            {
                saveConfig();
            }
        }

        private void button1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                // Left side
                case Keys.W:
                    writeLog(1, "Hotkey -> W");
                    goUp(1);
                    break;
                case Keys.S:
                    writeLog(1, "Hotkey -> S");
                    goDown(1);
                    break;
                case Keys.A:
                    writeLog(1, "Hotkey -> A");
                    goLeft(1);
                    break;
                case Keys.D:
                    writeLog(1, "Hotkey -> D");
                    goRight(1);
                    break;

                // Right side
                /* case Keys.NumPad8:
                     writeLog(2, "Hotkey -> 8");
                     goUp(2);
                     break;
                 case Keys.NumPad2:
                     writeLog(2, "Hotkey -> 2");
                     goDown(2);
                     break;
                 case Keys.NumPad5:
                     writeLog(2, "Hotkey -> 5");
                     goDown(2);
                     break;
                 case Keys.NumPad4:
                     writeLog(2, "Hotkey -> 4");
                     goLeft(2);
                     break;
                 case Keys.NumPad6:
                     writeLog(2, "Hotkey -> 6");
                     goRight(2);
                     break;*/
                default:
                    Console.WriteLine("Unknown key Pressed");
                    break;
            }
        }

        private void B_hotkey_Leave(object sender, EventArgs e)
        {
            L_hotkey_status .ForeColor = Color.Red;
            L_hotkey_status.Text = "Hotkeys are inactive";
        }

        private void B_hotkey_Click(object sender, EventArgs e)
        {
            L_hotkey_status.ForeColor = Color.Chartreuse;
            L_hotkey_status.Text = "Hotkeys are active";
        }

        private void B_menu_Click(object sender, EventArgs e)
        {
            if(GROUP_menu.Visible == true)
            {
                GROUP_menu.Enabled = false;
                GROUP_menu.Visible = false;
            }
            else
            {
                GROUP_menu.Enabled = true;
                GROUP_menu.Visible = true;
            }
        }

        private void L_menu_website_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(e.Link.LinkData as string);
        }

        private void B_menu_close_Click(object sender, EventArgs e)
        {
            GROUP_menu.Enabled = false;
            GROUP_menu.Visible = false;
        }
    }

    public class MyTabControl : TabControl
    {
        private bool bColorSet = false;
        private Color myBackColor;

        public MyTabControl()
        {
        }

        public override Color BackColor
        {
            get
            {
                if (bColorSet)
                    return this.myBackColor;

                return base.BackColor;
            }
            set
            {
                bColorSet = true;

                if (value == Color.Transparent)
                {
                    typeof(Control).InvokeMember("SetStyle",
                          BindingFlags.DeclaredOnly | BindingFlags.Public |
                          BindingFlags.NonPublic | BindingFlags.Instance |
                          BindingFlags.InvokeMethod,
                          null, this,
                          new object[] { ControlStyles.SupportsTransparentBackColor, true });
                }

                this.myBackColor = value;
            }
        }
    }
}
