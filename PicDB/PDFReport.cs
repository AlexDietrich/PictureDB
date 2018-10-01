using System;
using System.Collections.Generic;
using System.Linq;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PicDB.ViewModels;

namespace PicDB
{
    // ReSharper disable once InconsistentNaming
    internal class PDFReport
    {
        public void PdfReport(PictureViewModel pvm)
        {
            // Create a new PDF document
            PdfDocument document = new PdfDocument();
            document.Info.Title = pvm.FileName;

            // Create an empty page
            PdfPage page = document.AddPage();

            // Get an XGraphics object for drawing
            XGraphics gfx = XGraphics.FromPdfPage(page);

            // Create a font
            XFont font = new XFont("Verdana", 10);

            // Create Image
            var image = XImage.FromFile(pvm.FilePath);

            // Draw the text
            gfx.DrawString("Filename: " + pvm.FileName, font, XBrushes.Black,
                new XRect(10, 0, page.Width, 10),
                XStringFormats.TopLeft);
            gfx.DrawString("Photographer: ", font, XBrushes.Black,
                new XRect(10, 20, page.Width, 20),
                XStringFormats.TopLeft);
            gfx.DrawString("Name: " + pvm.Photographer.FirstName + " " + pvm.Photographer.LastName, font, XBrushes.Black,
                new XRect(10, 30, page.Width, 30),
                XStringFormats.TopLeft);
            gfx.DrawString("Birthday: " + pvm.Photographer.BirthDay, font, XBrushes.Black,
                new XRect(10, 40, page.Width, 40),
                XStringFormats.TopLeft);
            gfx.DrawString("Notes: " + pvm.Photographer.Notes, font, XBrushes.Black,
                new XRect(10, 50, page.Width, 50),
                XStringFormats.TopLeft);

            gfx.DrawImage(image, 100, 100, page.Width - 200, 200);

            gfx.DrawString("IPTC: ", font, XBrushes.Black,
                new XRect(10, 410, page.Width, 420),
                XStringFormats.TopLeft);
            gfx.DrawString("ByLine: " + pvm.IPTC.ByLine + " " + pvm.Photographer.LastName, font, XBrushes.Black,
                new XRect(10, 420, page.Width, 430),
                XStringFormats.TopLeft);
            gfx.DrawString("Caption: " + pvm.IPTC.Caption + " " + pvm.Photographer.LastName, font, XBrushes.Black,
                new XRect(10, 430, page.Width, 440),
                XStringFormats.TopLeft);
            gfx.DrawString("Headline: " + pvm.IPTC.Headline, font, XBrushes.Black,
                new XRect(10, 440, page.Width, 250),
                XStringFormats.TopLeft);
            gfx.DrawString("CopyrightNotice: " + pvm.IPTC.CopyrightNotice, font, XBrushes.Black,
                new XRect(10, 450, page.Width, 260),
                XStringFormats.TopLeft);
            gfx.DrawString("Keywords: " + pvm.IPTC.Keywords, font, XBrushes.Black,
                new XRect(10, 460, page.Width, 270),
                XStringFormats.TopLeft);

            gfx.DrawString("EXIF: ", font, XBrushes.Black,
                new XRect(10, 490, page.Width, 420),
                XStringFormats.TopLeft);
            gfx.DrawString("ExposureProgram: " + pvm.EXIF.ExposureProgram, font, XBrushes.Black,
                new XRect(10, 500, page.Width, 430),
                XStringFormats.TopLeft);
            gfx.DrawString("ExposureProgramResource: " + pvm.EXIF.ExposureProgramResource , font, XBrushes.Black,
                new XRect(10, 510, page.Width, 440),
                XStringFormats.TopLeft);
            gfx.DrawString("ExposureTime: " + pvm.EXIF.ExposureTime, font, XBrushes.Black,
                new XRect(10, 520, page.Width, 250),
                XStringFormats.TopLeft);
            gfx.DrawString("Flash: " + pvm.EXIF.Flash, font, XBrushes.Black,
                new XRect(10, 530, page.Width, 260),
                XStringFormats.TopLeft);
            gfx.DrawString("FNumber: " + pvm.EXIF.FNumber, font, XBrushes.Black,
                new XRect(10, 540, page.Width, 270),
                XStringFormats.TopLeft);
            gfx.DrawString("ISORating: " + pvm.EXIF.ISORating, font, XBrushes.Black,
                new XRect(10, 550, page.Width, 270),
                XStringFormats.TopLeft);
            gfx.DrawString("ISOValue: " + pvm.EXIF.ISOValue, font, XBrushes.Black,
                new XRect(10, 560, page.Width, 270),
                XStringFormats.TopLeft);
            gfx.DrawString("Make: " + pvm.EXIF.Make, font, XBrushes.Black,
                new XRect(10, 570, page.Width, 270),
                XStringFormats.TopLeft);

            document.Save(GlobalInformation.Path + "\\Reports\\report" + pvm.ID + ".pdf");
        }

        public void PdfReport(string tags)
        {
            var bl = new BusinessLayer();

            var tagDict = bl.GetTagCount();

            tags = tags.ToLower(); 

            string[] tagArray = tags.Split(';');

            // Create a new PDF document
            PdfDocument document = new PdfDocument();
            document.Info.Title = tags;

            // Create an empty page
            PdfPage page = document.AddPage();

            // Get an XGraphics object for drawing
            XGraphics gfx = XGraphics.FromPdfPage(page);

            // Create a font
            XFont font = new XFont("Verdana", 10);


            gfx.DrawString("Tag: Count", font, XBrushes.Black,
                new XRect(10, 0, page.Width, 10),
                XStringFormats.TopLeft);

            gfx.DrawString("____________________", font, XBrushes.Black,
                new XRect(10, 0, page.Width, 10),
                XStringFormats.TopLeft);

            int textHeight = 20;

            if (String.IsNullOrWhiteSpace(tags))
            {
                foreach (KeyValuePair<string, int> valuePair in tagDict)
                {

                    // Draw the text
                    gfx.DrawString(valuePair.Key + ": " + valuePair.Value, font, XBrushes.Black,
                        new XRect(10, textHeight, page.Width, 10),
                        XStringFormats.TopLeft);

                    textHeight += 10;

                }
            }
            else
            {
                foreach (KeyValuePair<string, int> valuePair in tagDict)
                {
                    if (tagArray.Contains((valuePair.Key).ToLower()))
                    {
                        // Draw the text
                        gfx.DrawString(valuePair.Key + ": " + valuePair.Value, font, XBrushes.Black,
                            new XRect(10, textHeight, page.Width, 10),
                            XStringFormats.TopLeft);

                        textHeight += 10;
                    }
                }
            }



            if (String.IsNullOrWhiteSpace(tags))
            {
                document.Save(GlobalInformation.Path + "\\Reports\\reportalltags.pdf");
            }
            else
            {
                document.Save(GlobalInformation.Path + "\\Reports\\report" + tags + ".pdf");
            }

        }

        private List<PictureViewModel> GetFilteredPictureList(PictureListViewModel plvm, string tags)
        {

            List<PictureViewModel> filteredPictures = new List<PictureViewModel>();

            //load picture list into new list
            foreach (PictureViewModel picture in plvm.List)
            {
                filteredPictures.Add(picture);
            }

            //get array of Tags
            string[] tagArray = tags.Split(' ');

            //filter list
            foreach (string tag in tagArray)
            {
                foreach (PictureViewModel picture in plvm.List)
                {
                    if (picture.IPTC.Keywords != null)
                    {
                        if (!picture.IPTC.Keywords.Contains(tag) && filteredPictures.Contains(picture))
                        {
                            filteredPictures.Remove(picture);
                        }
                    }
                }
            }
            return filteredPictures;
        }
    }
}
