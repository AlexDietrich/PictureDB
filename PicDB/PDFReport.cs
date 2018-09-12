using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
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
            var report = new PdfDocument();
            var page = report.AddPage();
            var filename = $"Report{DateTime.Now.Ticks}.pdf";

            var gfx = XGraphics.FromPdfPage(page);

            if (!File.Exists(picture.FilePath))
            {
                throw new FileNotFoundException();
            }

            var image = XImage.FromFile(picture.FilePath);

            gfx.DrawImage(image, 50, 50, 500, 500);

            report.Save(Path.GetTempPath());
        }
    }
}
