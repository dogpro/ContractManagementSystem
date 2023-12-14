using System.Collections.Generic;
using TestApplication.Models;

namespace TestApplication
{
    public interface IRequestManager
    {
        decimal GetContractAmountForCurrentYear();
        decimal GetContractAmountByRussian();
        List<string> GetPersonEmailsForRecentContracts();
        int UpdateContractsStatusForElderlyIndividuals();
        List<ReportModels.MoscowResident> GenerateReportForMoscowResidents();
    }
}