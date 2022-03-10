using System.ComponentModel;

namespace FileExtensionChanger
{
    public partial class FileExtensionChanger : Form
    {
        //fileExtensionChanger by Starlyn1232

        //Variable to find the paths and our thread manager (BackgroundWorker)

        private FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
        private BackgroundWorker thread = new BackgroundWorker();

        public FileExtensionChanger()
        {
            InitializeComponent();
        }

        //Let's set the folderFinder at LoadEvent

        private void FileExtensionChanger_Load(object sender, EventArgs e)
        {
            folderBrowserDialog.ShowNewFolderButton = true;
            folderBrowserDialog.Description = "Select the folder with containing files.";
        }

        //Event for Start button

        private void btnFolderFinder_Click(object sender, EventArgs e)
        {
            //Let's select a path

            DialogResult dialog = folderBrowserDialog.ShowDialog();

            //Only if you selected, we work, that's set

            if (dialog == DialogResult.OK)
            {
                //We save and show the selectedPath at textBox (FolderFinder)

                tbFolderFinder.Text = folderBrowserDialog.SelectedPath;

                //Let's check if we have the condition to enable the Start Button

                checkFields(ref tbCurrentExt, ref tbFinalExt);
            }
        }

        //Start Button Event

        private void btnStart_Click(object sender, EventArgs e)
        {
            //The program will check extension (*.extension) automatically, we don't need more "."!

            if (tbCurrentExt.Text.Contains(".") || tbFinalExt.Text.Contains("."))
                MessageBox.Show("You mustn't use \".\", the program understand the extension already!", "FileExtensionChanger Error.", MessageBoxButtons.OK, MessageBoxIcon.Error);

            else
            {
                //We the the thread and start it

                thread.DoWork += jobThread;
                thread.RunWorkerCompleted += completedJobThread;
                thread.RunWorkerAsync();
            }
        }

        //MainThread details

        private void jobThread(object sender, DoWorkEventArgs e)
        {
            //Let's disable the UI using the created function

            manageUI(false, btnStart,btnFolderFinder,btnClearFields,tbCurrentExt,tbFinalExt);

            //We'll use some variables to process the files's extension

            string currentExt = "." + tbCurrentExt.Text;
            string finalExt = "." + tbFinalExt.Text;

            //We won't use this for now, but you can implement it!

            string errorLog = "";

            //Let's filter the list before starting

            string[] filterFiles = Directory.GetFiles(tbFolderFinder.Text);
            List<string> foundFiles = new List<string>();

            //Okay, we have all files from Folder, but we need to process only the ones
            //which has the looking extension, conditions!

            foreach (string file in filterFiles)
                if (file.Substring(file.LastIndexOf(".") + 1) == tbCurrentExt.Text)
                    foundFiles.Add(file);

            //File's counters

            int[] counter = { 0, foundFiles.Count };

            //If folder is empty, just exit

            if (counter[1] == 0)
            {
                //Error sound

                System.Media.SystemSounds.Exclamation.Play();

                MessageBox.Show(String.Format("No files found! (.{0})", tbCurrentExt.Text));
                manageUI(true, btnStart, btnFolderFinder, btnClearFields, tbCurrentExt, tbFinalExt);
                return;
            }

            //Let's prepare progressBar values for better UI presentation

            int[] progressInt = { (100 / counter[1]), (100 / counter[1]) };

            foreach (string file in foundFiles)
            {
                //We wait 0.1 seconds by each file

                Thread.Sleep(30);

                //Change the progressBar value while we iterate the files

                progressBarChanger(progressInt[0]);
                progressInt[0] += progressInt[1];

                //Why try> Because maybe at future you'd like to have a error logger
                //with file and reason's details if it fails.

                try
                {
                    File.Move(file, file.Replace(currentExt, finalExt));
                    counter[0]++;
                }

                //For now commented, later we upgrade it

                catch
                {
                    //errorLog += "\nError with file: " + file.Replace(tbFolderFinder.Text, "");
                }
            }

            //Reset the progressBar

            Thread.Sleep(400);
            progressBarChanger(0);

            //Final Sound

            System.Media.SystemSounds.Hand.Play();

            //Final message with details

            MessageBox.Show(String.Format("\nTotal files found: {0}" +
                "\n\nSuccess: {1}/{2}\nFailures: {3}/{4}", counter[1], counter[0], counter[1], (counter[1] - counter[0]), counter[1]), "Process details");

            //Let's enable back the UI, why we use it? We want to avoid calling the thread twice for example through the UI.
            //That will crash the Program, and we don't want that

            manageUI(true, btnStart, btnFolderFinder, btnClearFields, tbCurrentExt, tbFinalExt);
        }

        //After getting the job done, let's remove the jobThread from "thread's delegate"

        private void completedJobThread(object sender, RunWorkerCompletedEventArgs e)
        {
            thread.DoWork -= jobThread;
        }

        //We manage the progressBar with "invoke" to make it works with BackgroundWorker properly

        private void progressBarChanger(int porcert)
        {
            progressBar.Invoke(new Action(() => { progressBar.Value = porcert; }));
        }

        //TextChangedEvent for textBoxes events

        private void tbCurrentExt_TextChanged(object sender, EventArgs e)
        {
            checkFields(ref tbCurrentExt, ref tbFinalExt);
        }

        private void tbFinalExt_TextChanged(object sender, EventArgs e)
        {
            checkFields(ref tbFinalExt, ref tbCurrentExt);
        }

        //Here we don't need to use "Invoke" because this will occur only
        //on non-thread-needed situations

        private void checkFields(ref TextBox textBox, ref TextBox textBox2)
        {
            //We want to btnClearFields only when is really needed

            if(textBox.Text == "" && textBox2.Text == "")
                btnClearFields.Enabled = false;

            else
                btnClearFields.Enabled = true;

            //Let's verify if we must the btnStart available

            if (textBox.Text.Length > 0 && textBox2.Text.Length > 0)
                if (tbFolderFinder.Text.Length > 0)
                    btnStart.Enabled = true;

                else
                    btnStart.Enabled = false;

            else
                btnStart.Enabled = false;
        }

        //Finally we have the function that allow to manage the UI while we work
        //or not with the thread

        private void manageUI(bool enableUI, params Control[] controls)
        {
            foreach (Control control in controls)
                control.Invoke(new Action(() => { control.Enabled = enableUI; }));
        }

        //Fields cleaner

        private void btnClearFields_Click(object sender, EventArgs e)
        {
            tbCurrentExt.Clear();
            tbFinalExt.Clear();
        }

        //Confirm Exit at FormClosing Event

        private void FileExtensionChanger_FormClosing(object sender, FormClosingEventArgs e)
        {
            //We ask

            DialogResult dialog = MessageBox.Show("Are you sure?", "Exit Confirmation.", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            //We act (Or not)

            if(dialog != DialogResult.Yes)
                e.Cancel = true;
        }
    }
}
