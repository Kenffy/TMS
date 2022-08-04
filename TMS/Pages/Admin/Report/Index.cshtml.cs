using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TMS.DataAccess;
using TMS.DataAccess.Data.Repository.IRepository;
using TMS.Models;
using TMS.Models.ViewModels;
using TMS.Utility;

namespace TMS.Pages.Admin.Report
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public IndexModel(IUnitOfWork unitOfWork, IWebHostEnvironment hostingEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostingEnvironment = hostingEnvironment;
        }


        [BindProperty]
        public ReportViewModel ReportObj { get; set; }

        public void OnGet(string date)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (date != null)
            {
                DateTime currdate = Convert.ToDateTime(date);
                ReportObj = new ReportViewModel()
                {
                    CurrentDate = currdate,
                    Tasks = _unitOfWork.Task.GetAll(includeProperties: "Priority,ApplicationUser", filter: c => c.UserId == claim.Value && c.Status == Constant.StatusClosed && c.ClosedOn.Month == currdate.Month && c.ClosedOn.Year == currdate.Year)
                };
            }
            else
            {
                DateTime currdate = DateTime.Today;
                ReportObj = new ReportViewModel()
                {
                    CurrentDate = currdate,
                    Tasks = _unitOfWork.Task.GetAll(includeProperties: "Priority,ApplicationUser", filter: c => c.UserId == claim.Value && c.Status == Constant.StatusClosed && c.ClosedOn.Month == currdate.Month && c.ClosedOn.Year == currdate.Year)
                };
            }
                       
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            return RedirectToPage("Index", new { date = ReportObj.CurrentDate.ToShortDateString()});
        }

        public IActionResult OnPostPrint()
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            DateTime currdate = ReportObj.CurrentDate;

            IEnumerable<CTask> Tasks = _unitOfWork.Task.GetAll(includeProperties: "Priority,ApplicationUser", filter: c => c.UserId == claim.Value && c.Status == Constant.StatusClosed && c.ClosedOn.Month == currdate.Month && c.ClosedOn.Year == currdate.Year);

            DataTable table = GetReportTable(Tasks);
            //MemoryStream mstream = GenerateReport(table, @"E:\Reports\test.pdf", "Report");

            MemoryStream mstream = GenerateReport(Tasks.ToList());

            return new FileStreamResult(mstream, "application/pdf");

            //return RedirectToPage("Index", new { date = ReportObj.CurrentDate.ToShortDateString() });
        }

        public MemoryStream GenerateReport(List<CTask> Tasks)
        {
            double hours = GetTotalOfHours(Tasks);

            DataTable table = GetReportTable(Tasks);
            //MemoryStream memoryStream = GenerateReport(table, hours);

            MemoryStream memoryStream = new MemoryStream();
            //FileStream fs = new FileStream(@"E:\Reports\test.pdf", FileMode.Create, FileAccess.Write, FileShare.None);
            Document pdfDoc = new Document(PageSize.A4, 50, 50, 50, 50);
            //PdfWriter writer = PdfWriter.GetInstance(pdfDoc, fs);
            PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);

            string webRootPath = _hostingEnvironment.WebRootPath;
            var logoPath = Path.Combine(webRootPath, @"images\logo\Logo.png");

            var titleFont = FontFactory.GetFont("Arial", 12, Font.BOLD);
            var titleFontBlue = FontFactory.GetFont("Arial", 14, Font.NORMAL);
            //var titleFontBlue = FontFactory.GetFont("Arial", 14, Font.NORMAL, BaseColor.DARK_GRAY);
            var boldTableFont = FontFactory.GetFont("Arial", 9, Font.BOLD);
            var bodyFont = FontFactory.GetFont("Arial", 9, Font.NORMAL);
            var EmailFont = FontFactory.GetFont("Arial", 8, Font.NORMAL);
            BaseColor TabelHeaderBackGroundColor = BaseColor.LIGHT_GRAY;


            Rectangle pageSize = writer.PageSize;
            // Open the Document for writing
            pdfDoc.Open();
            //Add elements to the document here

            #region Top table
            // Create the header table 
            PdfPTable headertable = new PdfPTable(3);
            headertable.HorizontalAlignment = 0;
            headertable.WidthPercentage = 100;
            headertable.SetWidths(new float[] { 100f, 300f, 120f });
            headertable.DefaultCell.Border = Rectangle.NO_BORDER;

            Image logo = Image.GetInstance(logoPath);

            logo.ScaleToFit(100, 15);

            {
                PdfPCell pdfCelllogo = new PdfPCell(logo);
                pdfCelllogo.Border = Rectangle.NO_BORDER;
                pdfCelllogo.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
                pdfCelllogo.BorderWidthBottom = 1f;
                headertable.AddCell(pdfCelllogo);
            }

            {
                PdfPCell middlecell = new PdfPCell();
                middlecell.Border = Rectangle.NO_BORDER;
                middlecell.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
                middlecell.BorderWidthBottom = 1f;
                headertable.AddCell(middlecell);
            }

            {
                PdfPTable nested = new PdfPTable(1);
                nested.DefaultCell.Border = Rectangle.NO_BORDER;
                PdfPCell nextPostCell1 = new PdfPCell(new Phrase("KenffySoft GmbH", titleFont));
                nextPostCell1.Border = Rectangle.NO_BORDER;
                nested.AddCell(nextPostCell1);
                PdfPCell nextPostCell2 = new PdfPCell(new Phrase("Rosengartenstraße 25", bodyFont));
                nextPostCell2.Border = Rectangle.NO_BORDER;
                nested.AddCell(nextPostCell2);
                PdfPCell nextPostCell3 = new PdfPCell(new Phrase("73614 Schorndorf", bodyFont));
                nextPostCell3.Border = Rectangle.NO_BORDER;
                nested.AddCell(nextPostCell3);
                PdfPCell nextPostCell4 = new PdfPCell(new Phrase("kenffysoft@gmail.com", EmailFont));
                nextPostCell4.Border = Rectangle.NO_BORDER;
                nested.AddCell(nextPostCell4);
                nested.AddCell("");
                PdfPCell nesthousing = new PdfPCell(nested);
                nesthousing.Border = Rectangle.NO_BORDER;
                nesthousing.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
                nesthousing.BorderWidthBottom = 1f;
                nesthousing.Rowspan = 5;
                nesthousing.PaddingBottom = 10f;
                headertable.AddCell(nesthousing);
            }

            PdfPTable Invoicetable = new PdfPTable(3);
            Invoicetable.HorizontalAlignment = 0;
            Invoicetable.WidthPercentage = 100;
            Invoicetable.SetWidths(new float[] { 220f, 180f, 120f });  // then set the column's __relative__ widths
            Invoicetable.DefaultCell.Border = Rectangle.NO_BORDER;


            {
                PdfPTable nested = new PdfPTable(1);
                nested.DefaultCell.Border = Rectangle.NO_BORDER;
                PdfPCell nextPostCell1 = new PdfPCell(new Phrase("", bodyFont));
                nextPostCell1.Border = Rectangle.NO_BORDER;
                nested.AddCell(nextPostCell1);
                PdfPCell nextPostCell2 = new PdfPCell(new Phrase("Voufo Kenfack, Marius Davy", titleFont));
                nextPostCell2.Border = Rectangle.NO_BORDER;
                nested.AddCell(nextPostCell2);
                PdfPCell nextPostCell3 = new PdfPCell(new Phrase("Musterstraße 20, 73884 Musterstadt", bodyFont));
                nextPostCell3.Border = Rectangle.NO_BORDER;
                nested.AddCell(nextPostCell3);
                PdfPCell nextPostCell4 = new PdfPCell(new Phrase("mustermann@example.com", EmailFont));
                nextPostCell4.Border = Rectangle.NO_BORDER;
                nested.AddCell(nextPostCell4);
                nested.AddCell("");
                PdfPCell nesthousing = new PdfPCell(nested);
                nesthousing.Border = Rectangle.NO_BORDER;
                //nesthousing.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
                //nesthousing.BorderWidthBottom = 1f;
                nesthousing.Rowspan = 5;
                nesthousing.PaddingBottom = 10f;
                Invoicetable.AddCell(nesthousing);
            }

            {
                PdfPCell middlecell = new PdfPCell();
                middlecell.Border = Rectangle.NO_BORDER;
                //middlecell.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
                //middlecell.BorderWidthBottom = 1f;
                Invoicetable.AddCell(middlecell);
            }


            {
                PdfPTable nested = new PdfPTable(1);
                nested.DefaultCell.Border = Rectangle.NO_BORDER;
                PdfPCell nextPostCell1 = new PdfPCell(new Phrase("TMS Report", titleFontBlue));
                nextPostCell1.Border = Rectangle.NO_BORDER;
                nested.AddCell(nextPostCell1);
                PdfPCell nextPostCell2 = new PdfPCell(new Phrase(DateTime.Now.ToString("MMMM") + " " + DateTime.Now.Year.ToString(), bodyFont));
                nextPostCell2.Border = Rectangle.NO_BORDER;
                nested.AddCell(nextPostCell2);
                PdfPCell nextPostCell3 = new PdfPCell(new Phrase("Printed on: " + DateTime.Now.ToShortDateString(), bodyFont));
                nextPostCell3.Border = Rectangle.NO_BORDER;
                nested.AddCell(nextPostCell3);
                nested.AddCell("");
                PdfPCell nesthousing = new PdfPCell(nested);
                nesthousing.Border = Rectangle.NO_BORDER;
                //nesthousing.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
                //nesthousing.BorderWidthBottom = 1f;
                nesthousing.Rowspan = 5;
                nesthousing.PaddingBottom = 10f;
                Invoicetable.AddCell(nesthousing);
            }


            pdfDoc.Add(headertable);
            Invoicetable.PaddingTop = 10f;

            pdfDoc.Add(Invoicetable);

            //Create body table
            PdfPTable itemTable = new PdfPTable(5);

            itemTable.HorizontalAlignment = 0;
            itemTable.WidthPercentage = 100;
            itemTable.SetWidths(new float[] { 5, 25, 25, 20, 25 });  // then set the column's __relative__ widths
            itemTable.SpacingAfter = 40;
            itemTable.DefaultCell.Border = Rectangle.BOX;
            PdfPCell cell1 = new PdfPCell(new Phrase("NO", boldTableFont));
            cell1.BackgroundColor = TabelHeaderBackGroundColor;
            cell1.HorizontalAlignment = Element.ALIGN_CENTER;
            itemTable.AddCell(cell1);
            PdfPCell cell2 = new PdfPCell(new Phrase("NAME", boldTableFont));
            cell2.BackgroundColor = TabelHeaderBackGroundColor;
            cell2.HorizontalAlignment = 1;
            itemTable.AddCell(cell2);
            PdfPCell cell3 = new PdfPCell(new Phrase("CUSTOMER", boldTableFont));
            cell3.BackgroundColor = TabelHeaderBackGroundColor;
            cell3.HorizontalAlignment = Element.ALIGN_CENTER;
            itemTable.AddCell(cell3);
            PdfPCell cell4 = new PdfPCell(new Phrase("STATUS", boldTableFont));
            cell4.BackgroundColor = TabelHeaderBackGroundColor;
            cell4.HorizontalAlignment = Element.ALIGN_CENTER;
            itemTable.AddCell(cell4);
            PdfPCell cell5 = new PdfPCell(new Phrase("HOURS", boldTableFont));
            cell5.BackgroundColor = TabelHeaderBackGroundColor;
            cell5.HorizontalAlignment = Element.ALIGN_CENTER;
            itemTable.AddCell(cell5);

            //foreach (DataRow row in dt.Rows)
            for (int i = 0; i < table.Rows.Count; i++)
            {
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    var _phrase = new Phrase();
                    _phrase.Add(new Chunk(table.Rows[i][j].ToString(), bodyFont));
                    PdfPCell descCell = new PdfPCell(_phrase);
                    descCell.HorizontalAlignment = 0;
                    descCell.PaddingLeft = 10f;
                    itemTable.AddCell(descCell);
                }
            }

            // Table footer
            PdfPCell totalAmtCell1 = new PdfPCell(new Phrase(""));
            totalAmtCell1.Border = Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER;
            itemTable.AddCell(totalAmtCell1);
            PdfPCell totalAmtCell2 = new PdfPCell(new Phrase(""));
            totalAmtCell2.Border = Rectangle.TOP_BORDER; //Rectangle.NO_BORDER; //Rectangle.TOP_BORDER;
            itemTable.AddCell(totalAmtCell2);
            PdfPCell totalAmtCell3 = new PdfPCell(new Phrase(""));
            totalAmtCell3.Border = Rectangle.TOP_BORDER; //Rectangle.NO_BORDER; //Rectangle.TOP_BORDER;
            itemTable.AddCell(totalAmtCell3);
            PdfPCell totalAmtStrCell = new PdfPCell(new Phrase("Total Of Hours", boldTableFont));
            totalAmtStrCell.Border = Rectangle.TOP_BORDER;   //Rectangle.NO_BORDER; //Rectangle.TOP_BORDER;
            totalAmtStrCell.HorizontalAlignment = 1;
            itemTable.AddCell(totalAmtStrCell);
            PdfPCell totalAmtCell = new PdfPCell(new Phrase(hours.ToString(), boldTableFont));
            totalAmtCell.HorizontalAlignment = 1;
            itemTable.AddCell(totalAmtCell);


            PdfPCell cell = new PdfPCell(new Phrase("", bodyFont));
            cell.Colspan = 5;
            cell.HorizontalAlignment = 1;
            itemTable.AddCell(cell);
            pdfDoc.Add(itemTable);


            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            //BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, true);
            PdfContentByte cb = new PdfContentByte(writer);
            cb = writer.DirectContent;
            cb.BeginText();
            cb.SetFontAndSize(bf, 8);
            cb.SetTextMatrix(pageSize.GetLeft(120), 20);
            cb.ShowText("Report Task Management System ");
            cb.EndText();

            //Move the pointer and draw line to separate footer section from rest of page
            cb.MoveTo(40, pdfDoc.PageSize.GetBottom(50));
            cb.LineTo(pdfDoc.PageSize.Width - 40, pdfDoc.PageSize.GetBottom(50));
            cb.Stroke();
            pdfDoc.Close();

            MemoryStream mstream = new MemoryStream();

            byte[] byteInfo = memoryStream.ToArray();
            mstream.Write(byteInfo, 0, byteInfo.Length);
            mstream.Position = 0;

            return mstream;
            #endregion

        }

        public DataTable GetReportTable(IEnumerable<CTask> Tasks)
        {
            DataTable table = new DataTable();

            if (Tasks != null)
            {
                // Define Columns
                table.Columns.Add("No");
                table.Columns.Add("Name");
                table.Columns.Add("Customer");
                //table.Columns.Add("Requested Date");
                table.Columns.Add("Status");
                table.Columns.Add("Hours");
                

                int count = 0;

                foreach (CTask task in Tasks)
                {
                    count++;
                    table.Rows.Add(count.ToString(),
                                    task.Name,
                                    task.Customer,
                                    //task.RequestDate.ToShortDateString(),
                                    task.Status,
                                    task.TotalOfHours.ToString());
                }
            }

            return table;
        }

        public double GetTotalOfHours(List<CTask> tasks)
        {
            // Compute total of Hours
            double count = 0;

            foreach (CTask task in tasks)
            {
                count += task.TotalOfHours;
            }

            return count;
        }

    }
}