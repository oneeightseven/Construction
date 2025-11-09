using ClosedXML.Excel;
using Construction.Models.Dtos;
using Construction.Service.Interfaces;

namespace Construction.Service.Helpers
{
    public class ExcelHelper : IExcelHelper
    {
        public MemoryStream GenerateDetailing(List<WorkDto> works)
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Детализация работ");

            var headerStyle = workbook.Style;
            headerStyle.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
            headerStyle.Font.FontColor = XLColor.White;
            headerStyle.Font.Bold = true;
            headerStyle.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            headerStyle.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            headerStyle.Border.OutsideBorder = XLBorderStyleValues.Thin;
            headerStyle.Border.OutsideBorderColor = XLColor.DarkGray;

            string[] headers = { "Номер", "Дата заявки", "Срок", "Дата завершения", "Город", "ТЦ", "Бренд", "Статус", "Объект", "Клиент", "Дата создания", "Сумма ИТОГО" };

            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cell(1, i + 1).Value = headers[i];
                worksheet.Cell(1, i + 1).Style = headerStyle;
            }

            var dataStyle = workbook.Style;
            dataStyle.Border.OutsideBorder = XLBorderStyleValues.Thin;
            dataStyle.Border.InsideBorder = XLBorderStyleValues.Thin;
            dataStyle.Border.BottomBorder = XLBorderStyleValues.Thin;
            dataStyle.Border.BottomBorderColor = XLColor.LightGray;

            int row = 2;
            foreach (var obj in works)
            {
                worksheet.Cell(row, 1).Value = obj.Id;
                worksheet.Cell(row, 2).Value = obj.DateBid?.ToString();
                worksheet.Cell(row, 3).Value = obj.Term?.ToString();
                worksheet.Cell(row, 4).Value = obj.CompletionDate?.ToString();
                worksheet.Cell(row, 5).Value = obj.City?.Name;
                worksheet.Cell(row, 6).Value = obj.ShoppingMall?.Name;
                worksheet.Cell(row, 7).Value = obj.Brand?.Name;
                worksheet.Cell(row, 8).Value = obj.Status?.Name;
                worksheet.Cell(row, 9).Value = obj.ConstructionObject?.Name;
                worksheet.Cell(row, 10).Value = obj.Client?.Name;
                worksheet.Cell(row, 11).Value = obj.DateOfCreation?.ToString();
                worksheet.Cell(row, 12).Value = obj.Summ;

                if (row % 2 == 0)
                {
                    worksheet.Row(row).Style.Fill.BackgroundColor = XLColor.FromArgb(242, 242, 242);
                }

                worksheet.Cell(row, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell(row, 12).Style.NumberFormat.Format = "#,##0.00";
                worksheet.Cell(row, 12).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                row++;
            }

            worksheet.Columns().AdjustToContents();

            worksheet.SheetView.FreezeRows(1);

            worksheet.Range(1, 1, 1, headers.Length).SetAutoFilter();

            if (works.Any())
            {
                worksheet.Cell(row, 11).Value = "ИТОГО:";
                worksheet.Cell(row, 11).Style.Font.Bold = true;
                worksheet.Cell(row, 11).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                worksheet.Cell(row, 12).FormulaA1 = $"SUM(L2:L{row - 1})";
                worksheet.Cell(row, 12).Style.Font.Bold = true;
                worksheet.Cell(row, 12).Style.Fill.BackgroundColor = XLColor.FromArgb(226, 239, 218);
                worksheet.Cell(row, 12).Style.NumberFormat.Format = "#,##0.00";
            }

            var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            return stream;
        }
    }
}
