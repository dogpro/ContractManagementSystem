using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Mime;

namespace TestApplication
{
    internal class Program
    {
        public static void Main()
        {
            using (var dbContext = new UserContext())
            {
                var requestManager = new RequestManager(dbContext);

                if (requestManager.IsDatabaseConnected())
                {
                    Console.WriteLine("Подключение к базе данных: ОК.\n");
                }
                else
                {
                    Console.WriteLine("Подключение к базе данных: не подключено.");
                    Console.ReadKey();
                    return;
                }

                while (true)
                {
                    Console.WriteLine("Меню:");
                    Console.WriteLine("1. Вывести сумму всех заключенных договоров за текущий год");
                    Console.WriteLine("2. Вывести сумму заключенных договоров по каждому контрагенту из России");
                    Console.WriteLine("3. Вывести список e-mail уполномоченных лиц, заключивших договора за последние 30 дней, на сумму больше 40000");
                    Console.WriteLine("4. Изменить статус договора для физических лиц с действующим договором и старше 60 лет");
                    Console.WriteLine("5. Создать отчет для физ. лиц, у которых есть действующие договора по компаниям в Москве");
                    Console.WriteLine("0. Выход");
                    
                    Console.Write("\nВвод: ");
                    var input = Console.ReadLine();

                    switch (input)
                    {
                        case "1":

                            Console.WriteLine($"Сумма всех заключенных договоров за текущий год: " +
                                              $"{requestManager.GetContractAmountForCurrentYear()}\n");
                            break;

                        case "2":
                            var resultDict = requestManager.GetContractAmountByRussian();

                            Console.WriteLine($"Сумма заключенных договоров по каждому контрагенту из России:");

                            if (resultDict == null)
                            {
                                break;
                            }

                            for (int i = 0 ; i < resultDict.Count; i++)
                            {
                                Console.WriteLine($"{i}. {resultDict[i].CompanyName} - {resultDict[i].TotalAmount} руб.");
                            }

                            Console.WriteLine();
                            break;

                        case "3":
                            Console.WriteLine("Список e-mail уполномоченных лиц, заключивших договора за последние 30 дней, на сумму больше 40000:");

                            var emailsList = requestManager.GetPersonEmailsForRecentContracts();

                            if (emailsList == null)
                            {
                                break;
                            }

                            if (emailsList.Count == 0)
                            {
                                Console.WriteLine("Данных по заданым параметрам нет.\n");
                                break;
                            }

                            foreach (var email in emailsList)
                            {
                                Console.WriteLine(email);
                            }

                            Console.WriteLine();
                            break;

                        case "4":
                            Console.WriteLine($"Операция завершена. Колличество измененных строк: {requestManager.UpdateContractsStatusForElderlyIndividuals()}\n");
                            break;

                        case "5":
                            var reportData = requestManager.GenerateReportForMoscowResidents();

                            if (reportData == null)
                            {
                                break;
                            }

                            if (reportData.Count == 0)
                            {
                                Console.WriteLine("Данных по заданым параметрам нет.\n");
                                break;
                            }

                            UploadFileManager.SaveToXml(reportData);
                            break;

                        case "0":
                            return;

                        default:
                            Console.WriteLine("Неверный ввод. Попробуйте снова.\n");
                            break;
                    }
                }
            }
        }
    }
}