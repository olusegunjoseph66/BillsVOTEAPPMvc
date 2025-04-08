//using OfficeOpenXml;

using ClosedXML.Excel;

namespace VOTEAPPMvc.Services
{
    // Services/ReportService.cs
    public class ReportService : IReportService
    {
        private readonly IVoteService _voteService;

        public ReportService(IVoteService voteService)
        {
            _voteService = voteService;
        }

        public async Task<byte[]> GenerateExcelReport(int topicId)
        {
            var result = await _voteService.GetVoteResult(topicId);
            if (result == null) return null;

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Voting Results");

            // Header
            worksheet.Cell(1, 1).Value = "Topic";
            worksheet.Cell(1, 2).Value = result.TopicTitle;
            worksheet.Range(1, 1, 1, 2).Style.Font.Bold = true;

            // Data
            worksheet.Cell(3, 1).Value = "Option";
            worksheet.Cell(3, 2).Value = "Count";
            worksheet.Cell(3, 3).Value = "Percentage";
            worksheet.Range(3, 1, 3, 3).Style.Font.Bold = true;

            worksheet.Cell(4, 1).Value = "Hi";
            worksheet.Cell(4, 2).Value = result.YesCount;
            worksheet.Cell(4, 3).Value = result.TotalVotes > 0 ? (result.YesCount * 100.0) / result.TotalVotes : 0;

            worksheet.Cell(5, 1).Value = "Na";
            worksheet.Cell(5, 2).Value = result.NoCount;
            worksheet.Cell(5, 3).Value = result.TotalVotes > 0 ? (result.NoCount * 100.0) / result.TotalVotes : 0;

            worksheet.Cell(6, 1).Value = "Total";
            worksheet.Cell(6, 2).Value = result.TotalVotes;
            worksheet.Cell(6, 3).Value = 100;
            worksheet.Range(6, 1, 6, 3).Style.Font.Bold = true;

            // Formatting
            worksheet.Range(4, 3, 6, 3).Style.NumberFormat.Format = "0.00%";
            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
    }
}
