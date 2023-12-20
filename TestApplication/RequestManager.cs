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

        /// <summary>
        /// Конструктор подключения к базе данных
        /// </summary>
        /// <param name="dbContext">контекст подключения к БД</param>
        public RequestManager(UserContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Сумма всех заключенных договоров за текущий год
        /// </summary>
        /// <returns>Сумма договоров за год</returns>
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

        /// <summary>
        /// Сумма заключенных договоров по каждому контрагенту из России
        /// </summary>
        /// <returns>Лист объектов <CompanyName, TotalAmount></returns>
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

        /// <summary>
        /// Cписок e-mail уполномоченных лиц, заключивших договора за последние 30 дней, на сумму больше 40000
        /// </summary>
        /// <returns>Лист e-mails</returns>
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

        /// <summary>
        /// Изменение статуса договоров для физических лиц старше 60 лет включительно.
        /// </summary>
        /// <returns>Колличество изменненых записей</returns>
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

        /// <summary>
        /// Ищет записи физ.лиц у которых есть действующие договора по компаниям, расположенных в городе Москва.
        /// </summary>
        /// <returns>Лист объектов <ФИО, e-mail, моб. телефон, дату рождения></returns>
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

        /// <summary>
        /// Проверка подключения к БД
        /// </summary>
        /// <returns>
        ///     true - подключение успешно
        ///     false - проблемы с подключением
        /// </returns>
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
