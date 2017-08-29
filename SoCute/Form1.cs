using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SoCute
{
    public partial class Form1 : Form
    {
        BackgroundWorker bw = new BackgroundWorker();
        bool work = false;
        string language = "eng";

        public Form1()
        {
            InitializeComponent();

            DirectoryInfo di = new DirectoryInfo(@"./tesseract/tessdata");
            foreach(FileInfo f_info in di.EnumerateFiles())
            {
                string data = f_info.Name.Remove(f_info.Name.IndexOf("."));
                if(comboLanguage.Items.Contains(data) == false)
                {
                    comboLanguage.Items.Add(data);
                }                
            }
            comboLanguage.SelectedItem = "eng";

            progressProcess.Minimum = 0;
            progressProcess.Maximum = 100;

            bw.WorkerSupportsCancellation = true;
            bw.WorkerReportsProgress = true;

            bw.DoWork += Bw_DoWork;
            bw.ProgressChanged += Bw_ProgressChanged;
            bw.RunWorkerCompleted += Bw_RunWorkerCompleted;
        }

        private void Bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if ((e.Cancelled == true))
            {
                MessageBox.Show("Canceled!");
            }

            else if (!(e.Error == null))
            {
                MessageBox.Show("Error: " + e.Error.Message);
            }

            else
            {
                MessageBox.Show("Done!");
            }
        }

        private void Bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressProcess.Value = e.ProgressPercentage;
        }

        private void Bw_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            while(work == true)
            {
                if ((worker.CancellationPending == true))
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    // Perform a time consuming operation and report progress.
                    foreach(object opath in listSup.Items)
                    {
                        if(opath.GetType() == typeof(string))
                        {
                            string path = (string)opath;
                            DoJob(path, worker);
                        }
                    }

                    work = false;
                }
            }
        }

        private void btnSupFolder_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "SUP files from VobEdit|*.sup";
            ofd.Multiselect = true;
            DialogResult dr = ofd.ShowDialog();
            if(dr == DialogResult.OK)
            {
                foreach(string s in ofd.FileNames)
                {
                    listSup.Items.Add(s);
                }
            }
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            if (bw.IsBusy != true)
            {
                work = true;
                bw.RunWorkerAsync();
            }
        }

        
        private void DoJob(string path, BackgroundWorker backworker)
        {
            //Extraction des BMP
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = Application.StartupPath + "/suptools/DVDSupDecode.exe";
            startInfo.Arguments = "-bitmap " + "\"" + path + "\"";
            process.StartInfo = startInfo;
            process.Start();

            process.WaitForExit();
            process.Close();
            process.Dispose();

            //Utilisation de tesseract et création du ASS à la volée
            float fps = 25f;
            FileInfo fileTXT = new FileInfo(path);
            string TXT = fileTXT.FullName.Replace(".sup", ".txt");
            int lineCount = File.ReadLines(TXT).Count();
            StreamReader sr = new StreamReader(TXT);

            //Prépare le ASS
            string ASS = fileTXT.FullName.Replace(".sup", ".ass");
            StreamWriter sw = new StreamWriter(ASS);
            PrepareASS(sw);

            //On lit le fichier TXT et on écrit au fur et à mesure le fichier ASS
            int counter = 0;
            while(sr.EndOfStream == false)
            {
                string s = sr.ReadLine();

                if (s.ToLower().StartsWith("framerate"))
                {
                    fps = Convert.ToSingle(s.Substring("FrameRate=".Length));
                }

                if (s.StartsWith("{"))
                {
                    Regex regex = new Regex(@"\{(\d+)\}\{(\d+)\}\{([^\}]+)}");
                    Match match = regex.Match(s);
                    if (match.Success)
                    {
                        //Valeurs
                        int start = Convert.ToInt32(match.Groups[1].Value);
                        int end = Convert.ToInt32(match.Groups[2].Value);
                        string bmp = match.Groups[3].Value;

                        //ASS
                        string assstartline = GetAssStartLine(fps, start, end);

                        //Tesseract BMP
                        string assendline = GetAssEndLine(fileTXT.Directory, bmp);

                        sw.WriteLine(assstartline + assendline);
                    }
                }

                counter += 1;
                backworker.ReportProgress(counter * 100 / lineCount);
            }

            sw.Close();
            sr.Close();
        }

        private string GetAssStartLine(float fps, int startframe, int endframe)
        {
            float start = Convert.ToSingle(startframe);
            float end = Convert.ToSingle(endframe);

            float start_ms = start / fps * 1000f;
            float end_ms = end / fps * 1000f;

            int start_hour = (int)(start_ms / 3600000);
            int start_min = (int)((start_ms - 3600000 * start_hour) / 60000);
            int start_sec = (int)((start_ms - 3600000 * start_hour - 60000 * start_min) / 1000);
            int start_cSec = (int)(start_ms - 3600000 * start_hour - 60000 * start_min - 100 * start_sec);
            string s_start = start_hour + ":";
            s_start += (start_min < 10 ? "0" + start_min : "" + start_min) + ":";
            s_start += (start_sec < 10 ? "0" + start_sec : "" + start_sec) + ".";
            s_start += start_cSec < 10 ? "0" + start_cSec : "" + start_cSec;

            int end_hour = (int)(end_ms / 3600000);
            int end_min = (int)((end_ms - 3600000 * end_hour) / 60000);
            int end_sec = (int)((end_ms - 3600000 * end_hour - 60000 * end_min) / 1000);
            int end_cSec = (int)(end_ms - 3600000 * end_hour - 60000 * end_min - 100 * end_sec);
            string s_end = end_hour + ":";
            s_end += (end_min < 10 ? "0" + end_min : "" + end_min) + ":";
            s_end += (end_sec < 10 ? "0" + end_sec : "" + end_sec) + ".";
            s_end += end_cSec < 10 ? "0" + end_cSec : "" + end_cSec;

            return "Dialogue: 0," + s_start + "," + s_end + ",Default,,0,0,0,,";
        }

        private string GetAssEndLine(DirectoryInfo folder, string relative_bitmap_file)
        {
            string data = "";

            //On s'occupe d'abord de l'image car elle est invalide (0dpi et mauvaise taille)
            string temp = folder.FullName + "/" + relative_bitmap_file;
            string input = folder.FullName + "/" + relative_bitmap_file.Replace(".bmp", ".png");
            using (FileStream fr = File.OpenRead(temp))
            {
                Bitmap r = Imaging.ReDraw(new Bitmap(Image.FromFile(temp)));
                Bitmap b = ResizeImage(r, 1280, 720);
                b.Save(input, ImageFormat.Png);
                b.Dispose();
            }

            //On supprime l'image en BMP
            if (File.Exists(temp) == true)
            {
                //File.Delete(temp);
            }

            //On s'occupe maintenant de la sortie
            string output = folder.FullName + "/outputBMPtoDATA";

            using (FileStream fr = File.OpenRead(input))
            {
                using (FileStream fw = File.OpenWrite(output))
                {
                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                    startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    startInfo.FileName = Application.StartupPath + "/tesseract/tesseract.exe";
                    startInfo.Arguments = "\"" + input + "\" \"" + output + "\" -l " + language;
                    process.StartInfo = startInfo;
                    process.Start();

                    process.WaitForExit();
                }
            }            

            using (StreamReader sr = new StreamReader(File.OpenRead(output + ".txt")))
            {
                data = sr.ReadLine();
                while (sr.EndOfStream == false)
                {
                    data += "\\N" + sr.ReadLine();
                }
                Console.WriteLine(data);
            }

            //On supprime l'image en PNG
            if (File.Exists(input) == true)
            {
                //File.Delete(input);
            }

            //On supprime le fichier de texte
            if (File.Exists(output) == true)
            {
                //File.Delete(output);
            }

            return data;
        }

        private void PrepareASS(StreamWriter sw)
        {
            sw.AutoFlush = true;

            sw.WriteLine("[Script Info]");
            sw.WriteLine("; Script generated by SoCute alpha");
            sw.WriteLine("; By TW2@GitHub");
            sw.WriteLine("ScriptType: v4.00+");
            sw.WriteLine("WrapStyle: 0");
            sw.WriteLine("PlayResX: 720");
            sw.WriteLine("PlayResY: 404");
            sw.WriteLine("");
            sw.WriteLine("[V4+ Styles]");
            sw.WriteLine("Format: Name, Fontname, Fontsize, PrimaryColour, SecondaryColour, OutlineColour, BackColour, Bold, Italic, Underline, StrikeOut, ScaleX, ScaleY, Spacing, Angle, BorderStyle, Outline, Shadow, Alignment, MarginL, MarginR, MarginV, Encoding");
            sw.WriteLine("Style: Default,Arial,20,&H00FFFFFF,&H000000FF,&H00000000,&H00000000,0,0,0,0,100,100,0,0,1,2,2,2,10,10,10,1");
            sw.WriteLine("");
            sw.WriteLine("[Events]");
            sw.WriteLine("Format: Layer, Start, End, Style, Name, MarginL, MarginR, MarginV, Effect, Text");
        }

        private void comboLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            language = comboLanguage.SelectedItem.ToString();
        }

        //@https://stackoverflow.com/questions/1922040/resize-an-image-c-sharp
        public Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

    }
}
