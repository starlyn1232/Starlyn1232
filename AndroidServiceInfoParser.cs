using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace AndroidServiceInfoParser
{
    public partial class Parser : Form
    {
        //Variables

        private string serialNo = "";
        private Process process = new Process();

        public Parser()
        {
            InitializeComponent();
        }

        //Load event

        private void Form1_Load(object sender, EventArgs e)
        {
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;

            this.MinimumSize = Size;
            this.MaximumSize = Size;

            MessageBox.Show("Welcome to the Android Service Info Parser!", "Coded by Starlyn1232",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //Functions

        private string CMD(string exe, string cmd, bool readBuffer, bool WaitExit)
        {
            process.StartInfo.FileName = exe;
            process.StartInfo.Arguments = cmd;
            process.Start();

            if (WaitExit)
                process.WaitForExit();

            if (readBuffer)
            {
                Thread.Sleep(250);

                string[] buffer = { process.StandardOutput.ReadToEnd(), process.StandardError.ReadToEnd() };

                if (buffer[0].Length == 0 && buffer[1].Length > 0)
                    buffer[0] = buffer[1];

                return buffer[0];
            }

            return "we don't like to wait!";
        }

        private string ADBCmd(string cmd, bool WaitExit)
        {
            cmd = (checkRoot.Checked) ? cmd.Replace("shell","shell su -c") : cmd;

            return CMD(Directory.GetCurrentDirectory() + "\\adb.exe", cmd, true, WaitExit);
        }

        private string ADBCmdCPP(string cmd, bool WaitExit)
        {
            cmd = (checkRoot.Checked) ? cmd.Replace("shell", "shell su -c") : cmd;

            //Why to use an C++ exe instead an own function? Just for pure fun!

            return CMD(Directory.GetCurrentDirectory() + "\\ADBService.exe",
                Directory.GetCurrentDirectory() + "\\adb.exe " + cmd, true, WaitExit);
        }

        private enum ADBMode
        {
            CSHARP, CPP
        }

        private string ADBConnected(string cmd, ADBMode mode, bool WaitExit)
        {
            if (!adbFastDetect(false, false))
            {
                infoCaller("\n\nDevice disconnected!", infoIndex.info1);
                return "failed";
            }

            string buffer = "";

            if (mode == ADBMode.CSHARP)
                buffer = ADBCmd(String.Format("-s {0} {1}", serialNo, cmd), WaitExit);

            else
                buffer = ADBCmdCPP(String.Format("-s {0} {1}", serialNo, cmd), WaitExit);

            if (buffer.Contains(String.Format("error: device '{0}' not found", serialNo)))
            {
                infoCaller("\n\nDevice disconnected!", infoIndex.info1);
                return "failed";
            }

            else
                return buffer;
        }

        //We'll invoke to allow the properly use while we use a BackGroundWorker

        private enum infoIndex
        {
            info1 = 1, info2 = 2, info3 = 3
        }

        private void infoCaller(string txt, infoIndex info)
        {
            if (info == infoIndex.info1)
                infoBox.Invoke(new Action(() => { infoBox.AppendText(txt); }));

            else if (info == infoIndex.info2)
                infoBox2.Invoke(new Action(() => { infoBox2.AppendText(txt); }));

            else if (info == infoIndex.info3)
                infoBox3.Invoke(new Action(() => { infoBox3.AppendText(txt); }));
        }

        private void infoCaller(infoIndex info)
        {
            if (info == infoIndex.info1)
                infoBox.Invoke(new Action(() => { infoBox.Clear(); }));

            else if (info == infoIndex.info2)
                infoBox2.Invoke(new Action(() => { infoBox2.Clear(); }));

            else if (info == infoIndex.info3)
                infoBox3.Invoke(new Action(() => { infoBox3.Clear(); }));
        }

        //ADB fast check

        private bool adbFastDetect(bool showTxt, bool refill)
        {
            void infoCaller(string txt)
            {
                if (showTxt)
                    this.infoCaller(txt, infoIndex.info1);
            }

            char[] fixBuffer = { ' ', '\n', '\r' };

            serialNo = ADBCmd("get-serialno", true);
            serialNo = serialNo.Trim(fixBuffer);

            if (serialNo.Contains("offline"))
            {
                infoCaller("\n\nOffline device! (Try to unplug and plug back the cable)");
                return false;
            }

            else if (serialNo.Contains("device unauthorized"))
            {
                infoCaller("\n\nDevice detected but adb request wasn't accepted!");
                return false;
            }

            else if (serialNo.Contains("no devices/emulators found"))
            {
                infoCaller("\n\nDevice wasn't detected!");
                return false;
            }

            else if (serialNo.Contains("more than one device/emulator"))
            {
                infoCaller("\n\nMore than one device detected!");
                return false;
            }

            else
            {
                if (refill)
                {
                    txtDevice.Invoke(new Action(() => { txtDevice.Text = serialNo; }));

                    txtAndroid.Invoke(new Action(() => {
                        txtAndroid.Text = ADBCmd("shell getprop ro.build.version.release", true).Trim(fixBuffer);
                    }));
                }

                return true;
            }
        }

        //UI manager

        private enum UIModes
        {
            connected, disconnected
        }

        private void UIManager(UIModes mode, bool Success)
        {
            if (mode == UIModes.disconnected)
            {
                btnConnect.Invoke(new Action(() => { btnConnect.Enabled = true; }));

                btnDisconnect.Invoke(new Action(() => { btnDisconnect.Enabled = false; }));
                btnCallService.Invoke(new Action(() => { btnCallService.Enabled = false; }));

                cbServiceList.Invoke(new Action(() => { cbServiceList.Enabled = false; }));
                cbServiceList.Invoke(new Action(() => { cbServiceList.Items.Clear(); }));

                btnADBReboot.Invoke(new Action(() => { btnADBReboot.Enabled = false; }));
                btnRestartADB.Invoke(new Action(() => { btnRestartADB.Enabled = false; }));
                btnRebootReco.Invoke(new Action(() => { btnRebootReco.Enabled = false; }));

                txtDevice.Invoke(new Action(() => { txtDevice.Clear(); }));
                txtAndroid.Invoke(new Action(() => { txtAndroid.Clear(); }));
                txtFoundServices.Invoke(new Action(() => { txtFoundServices.Clear(); }));
                txtArgs.Invoke(new Action(() => { txtArgs.Clear(); }));

                txtArgs.Invoke(new Action(() => { txtArgs.Enabled = false; }));

                checkRoot.Invoke(new Action(() => { checkRoot.Enabled = false; }));
            }

            else
            {
                btnConnect.Invoke(new Action(() => { btnConnect.Enabled = false; }));
                btnDisconnect.Invoke(new Action(() => { btnDisconnect.Enabled = true; }));

                if (Success)
                {
                    btnCallService.Invoke(new Action(() => { btnCallService.Enabled = true; }));

                    btnADBReboot.Invoke(new Action(() => { btnADBReboot.Enabled = true; }));
                    btnRestartADB.Invoke(new Action(() => { btnRestartADB.Enabled = true; }));
                    btnRebootReco.Invoke(new Action(() => { btnRebootReco.Enabled = true; }));
                    btnCleanWrapper.Invoke(new Action(() => { btnCleanWrapper.Enabled = true; }));

                    txtArgs.Invoke(new Action(() => { txtArgs.Enabled = true; }));

                    checkRoot.Invoke(new Action(() => { checkRoot.Enabled = true; }));
                }
            }
        }

        private void FastUI(bool enable)
        {
            btnCallService.Invoke(new Action(() => { btnCallService.Enabled = enable; }));
            btnADBReboot.Invoke(new Action(() => { btnADBReboot.Enabled = enable; }));
            btnRestartADB.Invoke(new Action(() => { btnRestartADB.Enabled = enable; }));
            btnRebootReco.Invoke(new Action(() => { btnRebootReco.Enabled = enable; }));
            btnCleanWrapper.Invoke(new Action(() => { btnCleanWrapper.Enabled = enable; }));
            btnDisconnect.Invoke(new Action(() => { btnDisconnect.Enabled = enable; }));

            checkRoot.Invoke(new Action(() => { checkRoot.Enabled = enable; }));
        }

        private struct StringCounterData
        {
            public int[] CharPositions;
            public int counter;
            public int pattern;
        }

        //Let's play

        private StringCounterData CountAndPos(string txt, char character)
        {
            StringCounterData data = new StringCounterData();

            data.CharPositions = new int[] { -1 };
            data.counter = -1;
            data.pattern = -1;

            //Check if at least has one char at string

            int found = txt.Count(f => f == character);

            if (found < 1)
                return data;

            List<int> indexs = new List<int>();

            //We'll save the position of each char

            for (int i = 0; i < txt.Length; i++)
                if (txt[i] == character)
                    indexs.Add(i);

            //Save found characters quantity and positions to variable

            data.counter = found;
            data.CharPositions = new int[indexs.Count];

            for (int i = 0; i < data.counter; i++)
                data.CharPositions[i] = indexs[i];

            //Let's make code smarter

            if(data.counter > 3)
            {
                //Let's check the length pattern between each index

                int[] check2 = new int[data.counter];

                for(int i = (data.counter - 1); i > 0; i--)
                    check2[i] = data.CharPositions[i] - data.CharPositions[i - 1];

                //Final comparision

                if (check2[1] == check2[2] || check2[1] == (check2[2]-1))
                    if (check2[1] == check2[3] || check2[1] == (check2[3] - 1))
                        data.pattern = check2[1];
            }

            //Return the struct

            return data;
        }

        private int AnyToInt(object obj)
        {
            try
            {
                return Convert.ToInt32(obj);
            }

            catch
            {
                return -1;
            }
        }

        private delegate void JobCaller();

        private void ThreadCaller(JobCaller call)
        {
            FastUI(false);

            Thread thread = new Thread(new ThreadStart(call));
            thread.Start();
        }

        //Let's rock it

        private void btnStart_Click(object sender, EventArgs e)
        {
            //Detector 

            bool detected = false;

            //Let's save the list here

            List<string> serviceList = new List<string>();

            void Job(object sender2, EventArgs e2)
            {
                UIManager(UIModes.connected, false);

                infoCaller(infoIndex.info1);
                infoCaller("\nDetecting adb devices: ", infoIndex.info1);

                if (!adbFastDetect(true, true))
                {
                    UIManager(UIModes.disconnected, false);
                    return;
                }

                else
                    detected = true;

                infoCaller("\n\nDevice found: " + serialNo, infoIndex.info1);

                infoCaller("\n\nReading service list: ", infoIndex.info1);

                int[] help = { 0, 0, 0 };
                string buffer = ADBCmdCPP("shell service list", false);
                StringReader reader = new StringReader(buffer);
                char[] fixBuffer = { '0','1', '2', '3'
                        ,'4','5','6','7','8','9','\n','\r','\t',' ',':','[',']' };
                buffer = "";

                while ((buffer = reader.ReadLine()) != null)
                {
                    //We'll skip the first line, at the same time verify the correct syntax

                    if (help[2] == 0)
                    {
                        if (!buffer.Contains("Found") && !buffer.Contains("services:"))
                        {
                            infoCaller("\nNo items found!", infoIndex.info1);
                            break;
                        }

                        else
                        {
                            help[2] = 1;
                            continue;
                        }
                    }

                    //Service item

                    try
                    {
                        //Check if the service list start with numbers

                        help[1] = buffer.IndexOf(":");

                        for (int i = help[1]; i > 0; i--)
                        {
                            if (buffer[i] == ' ' || buffer[i] == '\t')
                            {
                                help[0] = i;
                                break;
                            }
                        }

                        //Let's filter it!

                        buffer = buffer.Substring(help[0], (help[1]));
                        buffer = buffer.Trim(fixBuffer);

                        serviceList.Add(buffer);
                    }

                    catch
                    {
                        //Catched? Isn't going to the birthday party.
                    }
                }
            }

            void JobCompleted(object sender2, RunWorkerCompletedEventArgs e2)
            {
                cbServiceList.Items.Clear();

                if (!detected)
                    return;

                if (serviceList.Count > 0)
                {
                    txtFoundServices.Invoke(new Action(() => { txtFoundServices.Text = serviceList.Count.ToString(); }));

                    infoCaller(String.Format("\n\nService items found: {0}", serviceList.Count)
                        , infoIndex.info1);

                    for (int i = 0; i < serviceList.Count; i++)
                        cbServiceList.Items.Add(serviceList[i]);

                    //Let's sort and enable comboBox

                    cbServiceList.Enabled = true;
                    cbServiceList.SelectedIndex = 0;

                    UIManager(UIModes.connected, true);
                }

                else
                {
                    cbServiceList.Enabled = false;
                    infoCaller("\nNo items found!", infoIndex.info1);
                    UIManager(UIModes.disconnected, false);
                }
            }

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += Job;
            worker.RunWorkerCompleted += JobCompleted;
            worker.RunWorkerAsync();
        }

        private void btnCallService_Click(object sender, EventArgs e)
        {
            if (txtArgs.Text.Length == 0)
                MessageBox.Show("You can't call a service with an empty argument!", "Please try again.", MessageBoxButtons.OK, MessageBoxIcon.Error);

            else
            {
                string[] data = { (string)cbServiceList.SelectedItem, txtArgs.Text, txtArgs.Text, "" };

                void function()
                {
                    CleanAll();

                    infoCaller(String.Format("\nService name: {0}\nArguments: {1}",
                        data[0].Replace(" ", ""), data[1]), infoIndex.info1);

                    if (checkRoot.Checked)
                        infoCaller("\n\nRoot mode detected! (Accept SU request)", infoIndex.info1);

                    data[2] = ADBConnected(String.Format("shell service call {0} {1}",
                        data[0], data[1]), ADBMode.CPP, false);

                    if (data[2] == "failed")
                        UIManager(UIModes.disconnected, false);

                    else
                    {
                        if (!data[2].Contains("Result: Parcel("))
                            infoCaller("\n\nBad service response! (Does not exists)", infoIndex.info1);

                        else if (data[2].ToLower().Contains("no data available"))
                            infoCaller("\n\nThere is not information to parse!", infoIndex.info1);

                        else if (data[2].ToLower().Contains("no data available"))
                            infoCaller("\n\nThere is not information to parse!", infoIndex.info1);

                        else
                        {
                            StringCounterData counter1 = CountAndPos(data[2], '\n');

                            if (counter1.counter == -1)
                                infoCaller("\n\nInformation incorrectly received! v1", infoIndex.info1);

                            else if (counter1.pattern == -1)
                            {
                                try
                                {
                                    if (data[2].Contains("'"))
                                        data[2] = data[2].Substring(data[2].IndexOf("'")+1,8);

                                    infoCaller(String.Format("\nWrong data received: \n\n{0}", data[2]), infoIndex.info2);
                                }

                                catch
                                {

                                }

                                infoCaller("\n\nInformation incorrectly received! v2", infoIndex.info1);
                            }

                            else
                            {
                                infoCaller("\n\nInformation correctly received!", infoIndex.info1);
                                infoCaller(String.Format("\nRaw Data: \n\n{0}",data[2]), infoIndex.info2);

                                //We'll declare dinamically our variables, I just like saving

                                infoCaller(String.Format("\n\nLines: {0}\n\n",counter1.counter.ToString())
                                    ,infoIndex.info2);

                                string[] dataWrapper2 = new string[counter1.counter-1];

                                try
                                {
                                    infoCaller("\nFixed Data v1: \n\n", infoIndex.info3);

                                    char[] filterLine = {'\t','\n','\r' };

                                    for(int i=0;i< counter1.counter-1; i++)
                                    {
                                        dataWrapper2[i] = data[2].Substring(counter1.CharPositions[i], counter1.pattern).Trim(filterLine);
                                        dataWrapper2[i] = dataWrapper2[i].Substring(dataWrapper2[i].IndexOf("'") + 1,16);

                                        infoCaller(dataWrapper2[i] + "\n", infoIndex.info3);
                                    }

                                    int Len = 0;
                                    string ultraWrapper = "";

                                    infoCaller("\nFixed Data v2: \n\n", infoIndex.info3);

                                    for (int i = 0; i < counter1.counter - 1; i++)
                                    {
                                        Len = dataWrapper2[i].Length;
                                        ultraWrapper = "";

                                        for(int j = 0;j < Len; j++)
                                        {
                                            if(j%2==0 && dataWrapper2[i][j] != '.')
                                                ultraWrapper += dataWrapper2[i][j];
                                        }

                                        infoCaller(ultraWrapper + "\n", infoIndex.info3);
                                    }

                                    infoCaller("\nFixed Data v3 : \n\n", infoIndex.info3);

                                    for (int i = 0; i < counter1.counter - 1; i++)
                                    {
                                        Len = dataWrapper2[i].Length;
                                        ultraWrapper = "";

                                        for (int j = 0; j < Len; j++)
                                        {
                                            if (j % 2 == 0 && dataWrapper2[i][j] != '.')
                                                ultraWrapper += dataWrapper2[i][j];
                                        }

                                        infoCaller(ultraWrapper, infoIndex.info3);
                                    }
                                }

                                catch(Exception ex)
                                {
                                    infoCaller("\n\nFatal error: \n\n" + ex.Message,infoIndex.info1);
                                }
                            }
                        }

                        FastUI(true);
                    }
                }

                ThreadCaller(function);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            UIManager(UIModes.disconnected, false);

            infoCaller(infoIndex.info1);
            infoCaller(String.Format("\nDevice disconnected! ({0})",serialNo), infoIndex.info1);
        }

        private void btnADBReboot_Click(object sender, EventArgs e)
        {
            void function()
            {
                if (ADBConnected("reboot", ADBMode.CPP, false) == "failed")
                    UIManager(UIModes.disconnected, false);

                else
                {
                    infoCaller("\n\nDevice restarted!", infoIndex.info1);
                    FastUI(true);
                }
            }

            ThreadCaller(function);
        }

        private void btnRebootReco_Click(object sender, EventArgs e)
        {
            void function()
            {
                if (ADBConnected("reboot recovery", ADBMode.CPP, false) == "failed")
                    UIManager(UIModes.disconnected, false);

                else
                {
                    infoCaller("\n\nDevice restarted to recovery mode!", infoIndex.info1);
                    FastUI(true);
                }
            }

            ThreadCaller(function);
        }

        private void btnRestartADB_Click(object sender, EventArgs e)
        {
            void function()
            {
                ADBCmd("kill-server", false);

                infoCaller("\n\nADB server Restarted!", infoIndex.info1);

                FastUI(true);
            }

            ThreadCaller(function);
        }

        private void CleanAll()
        {
            for (int i = 1; i <= 3; i++)
                infoCaller((infoIndex)i);
        }

        private void btnCleanWrapper_Click(object sender, EventArgs e)
        {
            CleanAll();
        }

        private void Parser_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Are you sure?", "Exit confirmation.", MessageBoxButtons.YesNo, MessageBoxIcon.Information) != DialogResult.Yes)
                e.Cancel = true;
        }
    }
}
