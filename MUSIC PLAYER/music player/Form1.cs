using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace music_player
{
    public partial class Form1 : Form
    {
        List<string> paths = new List<string>();
        string[] files;
        public Form1()
        {
            InitializeComponent();
            trackBar1.Value = 50;
            label2.Text = trackBar1.Value.ToString() + "%";
        }
        private void axWindowsMediaPlayer1_Enter(object sender, EventArgs e)
        {

        }
        private void button7_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog1 = new OpenFileDialog();
            fileDialog1.Filter = ("Mp3|*.mp3|Mp4|*.mp4");
            fileDialog1.Multiselect = true;
            fileDialog1.CheckFileExists = true;
            if (fileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                files = fileDialog1.SafeFileNames;
                int i = 0;
                foreach (string filename in fileDialog1.FileNames)
                {
                    if (paths.Contains(filename))
                    {
                        MessageBox.Show(filename + " not added", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        paths.Add(filename);
                        listBox1.Items.Add(files[i]);
                        ++i;
                    }
                }
                if (listBox1.SelectedIndex == -1)
                {
                    listBox1.SetSelected(0, true);
                    axWindowsMediaPlayer1.URL = paths[listBox1.SelectedIndex];
                }
            }
            StreamWriter writeFile = new StreamWriter("../../playlist/lastPlaylist.txt");
            StreamWriter writePlaylist = null;
            if (listBox2.SelectedIndex != -1)
            {
                writePlaylist = new StreamWriter("../../playlist/" + listBox2.SelectedItem + ".txt");
            }
            for (int i = 0; i < listBox1.Items.Count; ++i)
            {
                writeFile.WriteLine(listBox1.Items[i].ToString());
                writeFile.WriteLine(paths[i].ToString());
                if (listBox2.SelectedIndex != -1)
                {
                    writePlaylist.WriteLine(listBox1.Items[i].ToString());
                    writePlaylist.WriteLine(paths[i]);
                }
            }
            int listbox2Index = 0;
            int listbox1Index = 0;
            if (listBox2.SelectedIndex != -1)
            {
                listbox2Index = listBox2.SelectedIndex;
                writePlaylist.Close();
            }
            if (listBox1.SelectedIndex != -1)
            {
                listbox1Index = listBox1.SelectedIndex;
            }
            writeFile.Close();
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            paths.Clear();
            Form1_Load(sender, e);
            listBox2.SelectedIndex = listbox2Index;
            listBox1.SelectedIndex = listbox1Index;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.pause();
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                string path = paths[listBox1.SelectedIndex];
                if (File.Exists(path))
                {
                    axWindowsMediaPlayer1.URL = paths[listBox1.SelectedIndex];
                }
                else
                {
                    MessageBox.Show("File does not exist!\nIt may be renamed, moved or deleted", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    button6_Click(sender, e);
                }
                try
                {
                    var file = TagLib.File.Create(paths[listBox1.SelectedIndex]);
                    var bin = (byte[])(file.Tag.Pictures[0].Data.Data);
                    if (pictureBox1.Image.Equals(Image.FromStream(new MemoryStream(bin))))
                    {
                        MessageBox.Show("Picture can't change");
                        pictureBox1.Image = Image.FromFile("A:/VISUAL STUDIO/Final Project/Final Project/creative-isolated-watercolor-tourism-illustration/2220.jpg");
                    }
                    else if ((pictureBox1.Image = Image.FromStream(new MemoryStream(bin))) == null)
                    {
                        MessageBox.Show("Picture can't change");
                    }
                    else
                    {
                        pictureBox1.Image = Image.FromStream(new MemoryStream(bin));
                    }
                }
                catch
                {

                }
            }
            if (listBox2.SelectedIndex != -1)
            {
                StreamWriter writePlaylist = new StreamWriter("../../playlist/" + listBox2.SelectedItem + ".txt");
                for (int i = 0; i < listBox1.Items.Count; ++i)
                {
                    writePlaylist.WriteLine(listBox1.Items[i].ToString());
                    writePlaylist.WriteLine(paths[i]);
                }
                writePlaylist.Close();
            }

        }
        private void button2_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.stop();
            progressBar1.Value = 0;
            start.Text = "00:00:00";
            end.Text = "00:00:00";
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == 0)
            {
                listBox1.SetSelected(listBox1.Items.Count - 1, true);
            }
            else
            {
                listBox1.SetSelected(listBox1.SelectedIndex - 1, true);
            }
            axWindowsMediaPlayer1.URL = paths[listBox1.SelectedIndex];
        }
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                listBox1.SetSelected(listBox1.SelectedIndex + 1, true);
                axWindowsMediaPlayer1.URL = paths[listBox1.SelectedIndex];
            }
            catch (Exception ex)
            {
                listBox1.SetSelected(0, true);
                axWindowsMediaPlayer1.URL = paths[listBox1.SelectedIndex];
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            StreamReader readPlaylist = new StreamReader("../../playlist/playlist.txt");
            int no = 0;
            while (!readPlaylist.EndOfStream)
            {
                string line = readPlaylist.ReadLine();
                listBox2.Items.Add(line);
                no++;
            }
            button11.Text = no.ToString();
            button11.Enabled = false;
            readPlaylist.Close();
            StreamReader readFile = new StreamReader("../../playlist/lastPlaylist.txt");
            while (!readFile.EndOfStream)
            {
                string line1 = readFile.ReadLine();
                string line2 = readFile.ReadLine();
                listBox1.Items.Add(line1);
                paths.Add(line2);
            }
            readFile.Close();
            axWindowsMediaPlayer1.uiMode = "none";
            if (listBox1.Items.Count > 0)
            {
                listBox1.SetSelected(0, true);
            }
            timer1.Start();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
            {
                button1.Enabled = false;
                button2.Enabled = false;
                button3.Enabled = false;
                button4.Enabled = false;
                button5.Enabled = false;
                button6.Enabled = false;
                progressBar1.Value = 0;
                end.Text = "00:00:00";
                start.Text = "00:00:00";
            }
            else
            {
                button1.Enabled = true;
                button2.Enabled = true;
                button3.Enabled = true;
                button4.Enabled = true;
                button5.Enabled = true;
                button6.Enabled = true;
            }
            if (listBox2.SelectedIndex == -1)
            {
                button9.Enabled = false;
            }
            else
            {
                button9.Enabled = true;
            }
            if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                progressBar1.Maximum = (int)axWindowsMediaPlayer1.Ctlcontrols.currentItem.duration;
                progressBar1.Value = (int)axWindowsMediaPlayer1.Ctlcontrols.currentPosition;
                try
                {
                    start.Text = axWindowsMediaPlayer1.Ctlcontrols.currentPositionString;
                    end.Text = axWindowsMediaPlayer1.Ctlcontrols.currentItem.durationString;
                    if (progressBar1.Maximum == progressBar1.Value)
                    {
                        timer1.Stop();
                        progressBar1.Value = 0;
                        button5_Click(sender, e);
                        timer1.Start();
                    }
                    string path = paths[listBox1.SelectedIndex];
                    if (!File.Exists(path))
                    {
                        timer1.Stop();
                        button6_Click(sender, e);
                        DialogResult result = MessageBox.Show(listBox1.SelectedItem + " does not exist!\nIt may be renamed, moved or deleted", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        timer1.Start();
                    }
                }
                catch
                {

                }
            }
        }
        private void button6_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                paths.RemoveAt(listBox1.SelectedIndex);
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
                progressBar1.Value = 0;
                start.Text = "00:00:00";
                end.Text = "00:00:00";
                axWindowsMediaPlayer1.Ctlcontrols.stop();
            }
            if (listBox1.Items.Count > 0)
            {
                listBox1.SetSelected(0, true);
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.play();
        }
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.settings.volume = trackBar1.Value;
            label2.Text = trackBar1.Value.ToString() + "%";
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            StreamWriter writeFile = new StreamWriter("../../playlist/lastPlaylist.txt");
            StreamWriter writePlaylist = null;
            if (listBox2.SelectedIndex != -1)
            {
                writePlaylist = new StreamWriter("../../playlist/" + listBox2.SelectedItem + ".txt");
            }
            for (int i = 0; i < listBox1.Items.Count; ++i)
            {
                writeFile.WriteLine(listBox1.Items[i].ToString());
                writeFile.WriteLine(paths[i].ToString());
                if (listBox2.SelectedIndex != -1)
                {
                    writePlaylist.WriteLine(listBox1.Items[i].ToString());
                    writePlaylist.WriteLine(paths[i]);
                }
            }
            if (listBox2.SelectedIndex != -1)
            {
                writePlaylist.Close();
            }
            writeFile.Close();
        }
        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text.Contains(" ") || textBox1.Text == "")
                {
                    MessageBox.Show("Playlist name can't be empty!");
                }
                else
                {
                    StreamReader readFile = new StreamReader("../../playlist/playlist.txt");
                    while (!readFile.EndOfStream)
                    {
                        string line = readFile.ReadLine();
                        if (line == textBox1.Text)
                        {
                            MessageBox.Show("Play list Updated!");
                            readFile.Close();
                            return;
                        }
                    }
                    readFile.Close();
                    StreamWriter writeFile = new StreamWriter("../../playlist/playlist.txt", append: true);
                    writeFile.WriteLine(textBox1.Text);
                    StreamWriter writePlaylist = new StreamWriter("../../playlist/" + textBox1.Text + ".txt");
                    for (int i = 0; i < listBox1.Items.Count; ++i)
                    {
                        writePlaylist.WriteLine(listBox1.Items[i].ToString());
                        writePlaylist.WriteLine(paths[i]);
                    }
                    writePlaylist.Close();
                    MessageBox.Show("Playlist saved successfully");
                    writeFile.Close();
                    string[] lines = File.ReadAllLines("../../playlist/playlist.txt");
                    File.WriteAllLines("../../playlist/playlist.txt", lines.Distinct().ToArray());
                    listBox2.Items.Clear();
                    listBox1.Items.Clear();
                    axWindowsMediaPlayer1.Ctlcontrols.stop();
                    paths.Clear();
                    Form1_Load(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            paths.Clear();
            axWindowsMediaPlayer1.Ctlcontrols.stop();
            axWindowsMediaPlayer1.URL = null;
            if (listBox2.SelectedIndex != -1)
            {
                StreamReader readFile = new StreamReader("../../playlist/" + listBox2.SelectedItem.ToString() + ".txt");
                while (!readFile.EndOfStream)
                {
                    string line1 = readFile.ReadLine();
                    string line2 = readFile.ReadLine();
                    listBox1.Items.Add(line1);
                    paths.Add(line2);
                }
                readFile.Close();
                if (listBox1.Items.Count > 0)
                {
                    listBox1.SetSelected(0, true);
                }
            }

        }
        private void button9_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1)
            {
                StreamWriter writeFile = new StreamWriter("../../playlist/playlist1.txt");
                StreamReader readFile = new StreamReader("../../playlist/playlist.txt");
                while (!readFile.EndOfStream)
                {
                    string line = readFile.ReadLine();
                    if (line != listBox2.SelectedItem.ToString())
                    {
                        writeFile.WriteLine(line);
                    }
                }
                writeFile.Close();
                readFile.Close();
                File.Delete("../../playlist/playlist.txt");
                File.Move("../../playlist/playlist1.txt", "../../playlist/playlist.txt");
                string filePath = "../../playlist/" + listBox2.SelectedItem.ToString() + ".txt";
                File.Delete(filePath);
                listBox2.Items.Remove(listBox2.SelectedItem.ToString());
                int no = int.Parse(button11.Text);
                no--;
                button11.Text = no.ToString();
            }
        }
        private void progressBar1_MouseDown(object sender, MouseEventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.currentPosition = axWindowsMediaPlayer1.currentMedia.duration * e.X / progressBar1.Width;
        }
    }
}
