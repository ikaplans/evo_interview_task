using CsvHelper;
using CsvHelper.Configuration;
using evo_interview_task.Models.Mapping;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace evo_interview_task.Data {
    internal class EmployeeCsvReader {
        public static IEnumerable<EmployeeDto> ReadEmployeesFromCSV(string filePath) {

            var config = new CsvConfiguration(CultureInfo.InvariantCulture) {
                HasHeaderRecord = true,
                Delimiter = ";",
                HeaderValidated = null,
                MissingFieldFound = null
            };
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, config);
            csv.Context.RegisterClassMap<CsvToEmployeeDtoMap>();
            return csv.GetRecords<EmployeeDto>().ToList();

        }

    }
}
