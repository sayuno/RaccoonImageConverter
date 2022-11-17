using System.Drawing.Imaging;
using System.Net;

namespace RaccoonImageConverter
{
    public partial class frmImageConverter : Form
    {
        public frmImageConverter()
        {
            InitializeComponent();
        }


        private void btnFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog folderBrowser = new OpenFileDialog();
            // Set validate names and check file exists to false otherwise windows will
            // not let you select "Folder Selection."
            folderBrowser.ValidateNames = false;
            folderBrowser.CheckFileExists = false;
            folderBrowser.CheckPathExists = true;
            // Always default to Folder Selection.
            folderBrowser.FileName = "Folder Selection.";
            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                txtFile.Text = folderBrowser.FileName;
            }
        }

        private void btnOutput_Click(object sender, EventArgs e)
        {
            OpenFileDialog folderBrowser = new OpenFileDialog();
            // Set validate names and check file exists to false otherwise windows will
            // not let you select "Folder Selection."
            folderBrowser.ValidateNames = false;
            folderBrowser.CheckFileExists = false;
            folderBrowser.CheckPathExists = true;
            // Always default to Folder Selection.
            folderBrowser.FileName = "Folder Selection.";
            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                txtOutput.Text = Path.GetDirectoryName(folderBrowser.FileName);
            }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            var listImageUrls = new List<string>();
            if (!string.IsNullOrWhiteSpace(txtFile.Text))
            {
                using (StreamReader r = new StreamReader(txtFile.Text))
                {
                    string line;
                    while ((line = r.ReadLine()) != null)
                    {
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            var values = line.Split(';');
                            listImageUrls.AddRange(values);
                        }
                    }
                }
                GetFilesLabel(listImageUrls);
                this.Text = this.Text + " - Processing...";

                Parallel.ForEach(listImageUrls, (item, loop, itemIndex) =>
                {
                    SaveAsWebp(item);
                });
            }



            if (chkPng.Checked)
                ConvertToPng();

            this.Text = this.Text + " - Finished";
        }

        private void GetFilesLabel(List<string> listImageUrls)
        {
            lblFilesFound.Text = listImageUrls.Count.ToString() + " Files Found";
            lblFilesFound.ForeColor = Color.Green;
            lblFilesFound.Visible = true;
        }

        private void ConvertToPng()
        {
            string[] files = System.IO.Directory.GetFiles(txtOutput.Text, "*.webp");
            GetFilesLabel(files.ToList());
            foreach (string file in files)
            {
                var originalFile = file.Split('/').Last();
                var fileName = originalFile.Replace("webp", "png");
                var pathFileName = Path.Combine(txtOutput.Text, fileName);
                using (WebP webp = new WebP())
                {
                    try
                    {
                        Bitmap bmp = webp.Load(originalFile);
                        bmp.Save(fileName, ImageFormat.Png);
                        if (File.Exists(file) && file.Contains("webp"))
                            File.Delete(file);
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }



        }
        private void SaveAsWebp(string item)
        {
            using (WebClient client = new WebClient())
            {
                var fileName = item.Split('/').Last();
                var pathFileName = Path.Combine(txtOutput.Text, fileName);
                if (!File.Exists(pathFileName))
                    client.DownloadFile(new Uri(item), pathFileName);
            }
        }
    }
}