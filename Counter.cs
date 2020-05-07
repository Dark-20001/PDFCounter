using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Spire.Pdf;
using System.Drawing;

namespace PDFCounter
{
    public class Counter
    {
        public List<fileItem> ReadFiles(string[] files)
        {
            List<fileItem> fileItems = new List<fileItem>();

            foreach (string f in files)
            {
                fileItems.Add(ReadFile(f));
            }

            return fileItems;
        }

        public fileItem ReadFile(string file)
        {
            PdfDocument doc = new PdfDocument();

            try
            {
                doc.LoadFromFile(file);
                //bool isEncrypted = doc.Security.OriginalEncrypt;
                //return new fileItem(Path.GetFileNameWithoutExtension(file), 0, Convert.ToInt32(isEncrypted), file);

                List<Image> ListImage = new List<Image>();

                //实例化一个StringBuilder 对象
                StringBuilder content = new StringBuilder();

                foreach (PdfPageBase page in doc.Pages)
                {
                    //遍历文档所有PDF页面，提取文本

                    if (!File.Exists(file + ".txt"))
                    {
                        content.Append(page.ExtractText());
                    }
                    //Image[] images = page.ExtractImages();
                    //if (images != null && images.Length > 0)
                    //{
                    //    ListImage.AddRange(images);
                    //}
                }
                //save
                if (File.Exists(file + ".txt"))
                {
                }
                else
                {
                    StreamWriter sw = new StreamWriter(file + ".txt", false, Encoding.UTF8);
                    sw.Write(content.ToString());
                    sw.Close();
                }

                ////将获取到的图片保存到本地路径
                //string ph = Path.GetDirectoryName(file) + "\\" + Path.GetFileNameWithoutExtension(file)+"\\";
                //if (ListImage.Count > 0)
                //{
                //    for (int i = 0; i < ListImage.Count; i++)
                //    {
                //        Image image = ListImage[i];
                //        image.Save(ph+"image" + (i + 1).ToString() + ".png", System.Drawing.Imaging.ImageFormat.Png);
                //    }
                //}
                return new fileItem(Path.GetFileNameWithoutExtension(file), doc.Pages.Count, ListImage.Count, file);
            }
            catch (Exception e)
            {
                return new fileItem(Path.GetFileNameWithoutExtension(file), 0, 0, file);
            }
        }
    }

    public class fileItem
    {
        public string FileName { get; set; }
        public int Count { get; set; }
        public int ImageCount { get; set; }
        public string Path { get; set; }

        public fileItem(string _fileName, int _count, int _imagecount, string _path)
        {
            FileName = _fileName;
            Count = _count;
            ImageCount = _imagecount;
            Path = _path;
        }
    }
}
