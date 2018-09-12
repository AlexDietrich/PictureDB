using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using BIF.SWE2.Interfaces.ViewModels;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace PicDB
{
    // ReSharper disable once InconsistentNaming
    internal class PDFReport
    {
        public void CreateReport(IPictureListViewModel pictures , string tags)
        {

        }

        public void CreateReport(IPictureViewModel picture)
        {
            if (!File.Exists(picture.FilePath))
            {
                throw new FileNotFoundException();
            }

            var report = new PdfDocument();
            var page = report.AddPage();
            var filename = $"Report{DateTime.Now.Ticks}.pdf";

            var gfx = XGraphics.FromPdfPage(page);



            var image = XImage.FromFile(picture.FilePath);
            
            gfx.DrawImage(image, 50, 50, 500, 300);

            gfx.DrawString("Filename: " + picture.FileName, new XFont("Times New Roman", 12), XBrushes.Black, new XRect(50, 400, page.Width, page.Height), XStringFormat.TopLeft);
            gfx.DrawString("Keywords: " + picture.IPTC.Keywords, new XFont("Times New Roman", 12), XBrushes.Black, new XRect(50, 414, page.Width, page.Height), XStringFormat.TopLeft);
            gfx.DrawString("Copyright: " + picture.IPTC.CopyrightNotice, new XFont("Times New Roman", 12), XBrushes.Black, new XRect(50, 428, page.Width, page.Height), XStringFormat.TopLeft);
            gfx.DrawString("Camera Producer: " + picture.Camera.Producer, new XFont("Times New Roman", 12), XBrushes.Black, new XRect(50, 440, page.Width, page.Height), XStringFormat.TopLeft);
            gfx.DrawString("Camera Make: " + picture.Camera.Make, new XFont("Times New Roman", 12), XBrushes.Black, new XRect(50, 454, page.Width, page.Height), XStringFormat.TopLeft);
            gfx.DrawString("Headline: " + picture.IPTC.Headline, new XFont("Times New Roman", 12), XBrushes.Black, new XRect(50, 468, page.Width, page.Height), XStringFormat.TopLeft);


            report.Save(Path.GetTempPath()+filename);
        }
    }
}
