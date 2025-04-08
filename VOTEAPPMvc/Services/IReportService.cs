namespace VOTEAPPMvc.Services
{
    public interface IReportService
    {
        Task<byte[]> GenerateExcelReport(int topicId);
    }
}
