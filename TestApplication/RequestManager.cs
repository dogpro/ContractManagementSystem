using System;
using System.Collections.Generic;
using System.Linq;
using TestApplication.Models;
using static TestApplication.Models.ReportModels;

namespace TestApplication
{
    public class RequestManager : IRequestManager
    {
        private UserContext _dbContext;

        public RequestManager(UserContext dbContext)
        {
            _dbContext = dbContext;
        }

        public decimal GetContractAmountForCurrentYear()
        {
            string sql = "SELECT SUM(ContractAmount) AS TotalAmount " +
                         "FROM Contracts " +
                         "WHERE strftime('%Y', SigningDate) = strftime('%Y', 'now');";

            try
            {
                return _dbContext.Database.SqlQuery<decimal>(sql).FirstOrDefault();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return -1;
            }
        }

        public List<ContractAmountByRussianResult> GetContractAmountByRussian()
        {
            string sql = "SELECT LegalEntity.CompanyName, SUM(Contracts.ContractAmount) AS TotalAmount " +
                 "FROM Contracts " +
                 "INNER JOIN LegalEntity ON Contracts.LegalEntityId = LegalEntity.LegalEntityId " +
                 "WHERE LegalEntity.Country = 'Россия' " +
                 "GROUP BY LegalEntity.CompanyName;";

            try
            {
                return _dbContext.Database.SqlQuery<ContractAmountByRussianResult>(sql).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public List<string> GetPersonEmailsForRecentContracts()
        {
            string sql = "SELECT DISTINCT Individual.Email " +
                             "FROM Contracts " +
                             "JOIN Individual ON Contracts.IndividualId = Individual.IndividualId " +
                             "WHERE ContractAmount > 40000 AND date(SigningDate) >= date('now', '-30 days')";

            try
            {
                return _dbContext.Database.SqlQuery<string>(sql).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public int UpdateContractsStatusForElderlyIndividuals()
        {
            string sql = "UPDATE Contracts " +
                         "SET Status = '0' " +
                         "WHERE IndividualId IN (SELECT IndividualId " +
                         "    FROM Individual " +
                         "    WHERE Age >= 60) AND Status = '1'";

            try
            {
                return _dbContext.Database.ExecuteSqlCommand(sql);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return -1;
            }
        }

        public List<MoscowResident> GenerateReportForMoscowResidents()
        {
            string sql = "SELECT i.FirstName || ' ' || i.LastName || ' ' || i.Patronymic AS FullName, " +
                     "i.Email, i.Phone, i.DateOfBirth " +
                     "FROM Contracts c " +
                     "JOIN Individual i ON c.IndividualId = i.IndividualId " +
                     "JOIN LegalEntity le ON c.LegalEntityId = le.LegalEntityId " +
                     "WHERE le.City = 'Москва' AND c.Status = '1'";

            try
            {
                return _dbContext.Database.SqlQuery<MoscowResident>(sql).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public bool IsDatabaseConnected()
        {
            try
            {
                _dbContext.Database.Connection.Open();
                _dbContext.Database.Connection.Close();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка подключения к базе данных: {ex.Message}");
                return false;
            }
        }
    }
}
