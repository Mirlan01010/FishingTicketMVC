using BLL.Models.StatisticModels;
using BLL.Services;
using DAL.Migrations;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IO;
using System.Text.Unicode;
using Ticket.Models;

namespace Ticket.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IStatisticService _statisticService;

        public HomeController(ILogger<HomeController> logger, IStatisticService statisticService)
        {
            _logger = logger;
            _statisticService = statisticService;
        }
        [Authorize(Roles = ("Admin"))]

        public async Task<IActionResult> Index()
        {
            var obj = new Sell
            {
                ForDay = await _statisticService.GetForDay(),
                ForMonth = await _statisticService.GetForMonth(),
            };
            return View(obj);
        }
        public async Task<IActionResult> Type()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Analize([FromBody] DateInterval interval)
        {
            var obj = new WaterBoydType
            {
                TypeSellsList = await _statisticService.GetTicketTypeSells(interval),
                WaterBodySellsList = await _statisticService.GetWaterBodySells(interval)
            };
            return Json(obj);
        }

        [HttpPost]
        public async Task<IActionResult> DownloadPdf(DateInterval interval)
        {
            
            
            using (MemoryStream ms = new MemoryStream())
            {
                var obj = await _statisticService.GetTicketTypeSells(interval);
                Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                PdfWriter writer = PdfWriter.GetInstance(document, ms);
                document.Open();

                string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
                BaseFont baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

                Font font = new Font(baseFont, 12, Font.NORMAL);
                Font titleFont = new Font(baseFont,12, Font.NORMAL,BaseColor.BLACK);

               
                var img = iTextSharp.text.Image.GetInstance("wwwroot/img/KRimg.png");
                img.Alignment = Element.ALIGN_CENTER;
                img.ScaleToFit(85.05f, 85.05f);
                document.Add(img);
                Paragraph paragraph = new Paragraph("Министерство сельского хозяйство КР\n\n", font);
                paragraph.Alignment = Element.ALIGN_CENTER;
                document.Add(paragraph);

                

                // Создаем таблицу
                PdfPTable table = new PdfPTable(3);

                string[] headers = { "Виды билета", "Количество проданных билетов", "Цена проданных билетов" };
                foreach (var header in headers)
                {
                    PdfPCell headerCell = new PdfPCell(new Phrase(header, titleFont));
                    headerCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    headerCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    headerCell.VerticalAlignment = Element.ALIGN_CENTER;
                    headerCell.BorderWidth = 1f;
                    table.AddCell(headerCell);
                }

               
                foreach(var item in obj)
                {
                    PdfPCell dataCell = new PdfPCell(new Phrase(item.Name, font));
                    dataCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    dataCell.VerticalAlignment = Element.ALIGN_CENTER;
                    dataCell.BorderWidth = 1f;
                    table.AddCell(dataCell);
                    PdfPCell dataCell1 = new PdfPCell(new Phrase(item.TotalCount.ToString(), font));
                    dataCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    dataCell.VerticalAlignment = Element.ALIGN_CENTER;
                    dataCell.BorderWidth = 1f;
                    table.AddCell(dataCell1);
                    PdfPCell dataCell2 = new PdfPCell(new Phrase(item.TotalPrice.ToString(), font));
                    dataCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    dataCell.VerticalAlignment = Element.ALIGN_CENTER;
                    dataCell.BorderWidth = 1f;
                    table.AddCell(dataCell2);
                    
                }

                document.Add(table);
                Paragraph date1 = new Paragraph($"Отчёт:\n", font);
                document.Add(date1);
                Paragraph date = new Paragraph($"От: {interval.Start}\n", font);
                document.Add(date);
                Paragraph datend = new Paragraph($"До: {interval.End} !\n\n", font);
                document.Add(datend);
                document.Close();
                writer.Close();
                var constant = ms.ToArray();
                return File(constant, "application/pdf", "Report.pdf");
            }

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //HelperMethods
        public async Task<IActionResult> GetPdf()
        {

            var interval = new DateInterval
            {
                Start = new DateOnly(2024,05, 12),
                End = new DateOnly(2024, 05, 18)
            };
            using (MemoryStream ms = new MemoryStream())
            {
                var obj = await _statisticService.GetTicketTypeSells(interval);
                Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                PdfWriter writer = PdfWriter.GetInstance(document, ms);
                document.Open();

                // Указываем путь к файлу шрифта Arial
                string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
                BaseFont baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

                // Создаем шрифт для текста
                Font font = new Font(baseFont, 12, Font.NORMAL);

                // Создаем абзац с русским текстом и двумя пустыми строками
                Paragraph paragraph = new Paragraph("Привет\n\n", font);
                paragraph.Alignment = Element.ALIGN_CENTER;
                document.Add(paragraph);

                // Добавляем информацию о датах
                Paragraph date = new Paragraph($"Начальная дата: {interval.Start} Конечная дата: {interval.End} !\n\n", font);
                document.Add(date);

                // Создаем таблицу
                PdfPTable table = new PdfPTable(4);

                // Добавляем ячейки заголовков
                string[] headers = { "Date", "Name", "Surname", "Address" };
                foreach (var header in headers)
                {
                    PdfPCell headerCell = new PdfPCell(new Phrase(header, font));
                    headerCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    headerCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    headerCell.VerticalAlignment = Element.ALIGN_CENTER;
                    headerCell.BorderWidth = 1f;
                    table.AddCell(headerCell);
                }

                // Добавляем данные в таблицу
                //for (int i = 0; i < 10; i++)
                //{
                //    for (int j = 0; j < 4; j++)
                //    {
                //        PdfPCell dataCell = new PdfPCell(new Phrase((i + j).ToString(), font));
                //        dataCell.HorizontalAlignment = Element.ALIGN_CENTER;
                //        dataCell.VerticalAlignment = Element.ALIGN_CENTER;
                //        dataCell.BorderWidth = 1f;
                //        table.AddCell(dataCell);
                //    }
                //}
                foreach (var item in obj)
                {
                    PdfPCell dataCell = new PdfPCell(new Phrase(item.Name, font));
                    dataCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    dataCell.VerticalAlignment = Element.ALIGN_CENTER;
                    dataCell.BorderWidth = 1f;
                    table.AddCell(dataCell);
                    PdfPCell dataCell1 = new PdfPCell(new Phrase(item.TotalCount.ToString(), font));
                    dataCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    dataCell.VerticalAlignment = Element.ALIGN_CENTER;
                    dataCell.BorderWidth = 1f;
                    table.AddCell(dataCell1);
                    PdfPCell dataCell2 = new PdfPCell(new Phrase(item.TotalPrice.ToString(), font));
                    dataCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    dataCell.VerticalAlignment = Element.ALIGN_CENTER;
                    dataCell.BorderWidth = 1f;
                    table.AddCell(dataCell2);
                    PdfPCell dataCell3 = new PdfPCell(new Phrase(0.ToString(), font));
                    dataCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    dataCell.VerticalAlignment = Element.ALIGN_CENTER;
                    dataCell.BorderWidth = 1f;
                    table.AddCell(dataCell3);
                }

                document.Add(table);
                document.Close();
                writer.Close();
                var constant = ms.ToArray();
                return File(constant, "application/pdf", "firstpdf.pdf");
            }

        }
    }
}
