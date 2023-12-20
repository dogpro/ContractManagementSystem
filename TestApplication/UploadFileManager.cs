using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Xml.Serialization;
using TestApplication.Models;

namespace TestApplication
{
    public class UploadFileManager
    {
        /// <summary>
        /// Сохранение данных в файл JSON
        /// </summary>
        /// <param name="data">Лист данных</param>        
        public static void SaveToJson(List<ReportModels.MoscowResident> data)
        {
            try
            {
                var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });

                File.WriteAllText(ConstantValues.JsonFilePath, json);

                Console.WriteLine($"Файл {ConstantValues.JsonFilePath} сохранен.\n");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        /// <summary>
        /// Сохранение данных в файл XML
        /// </summary>
        /// <param name="data">Лист данных</param>
        public static void SaveToXml(List<ReportModels.MoscowResident> data)
        {
            try
            {
                var xmlSerializer = new XmlSerializer(typeof(List<ReportModels.MoscowResident>));

                using (var streamWriter = new StreamWriter(ConstantValues.XmlFilePath))
                {
                    xmlSerializer.Serialize(streamWriter, data);
                }

                Console.WriteLine($"Файл {ConstantValues.XmlFilePath} сохранен.\n");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}