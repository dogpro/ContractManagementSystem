using System.Collections.Generic;
using TestApplication.Models;
using static TestApplication.Models.ReportModels;

namespace TestApplication
{
    
    public interface IRequestManager
    {
        decimal GetContractAmountForCurrentYear();
        List<ContractAmountByRussianResult> GetContractAmountByRussian();
        List<string> GetPersonEmailsForRecentContracts();
        int UpdateContractsStatusForElderlyIndividuals();
        List<MoscowResident> GenerateReportForMoscowResidents();
    }
}