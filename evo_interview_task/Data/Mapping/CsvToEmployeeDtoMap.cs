using CsvHelper;
using CsvHelper.Configuration;
using evo_interview_task.Data;
using System;

namespace evo_interview_task.Models.Mapping {
    internal class CsvToEmployeeDtoMap : ClassMap<EmployeeDto> {
        public CsvToEmployeeDtoMap() {
            Map(x => x.Country);
            Map(x => x.Department);
            Map(x => x.Email);
            Map(x => x.EmployeeId).Name(Constants.EmployeeCsvFieldNames.EmployeeId);
            Map(x => x.FirstName);
            Map(x => x.HiredAt);
            Map(x => x.InTraining).Convert(ConvertIntraining);
            Map(x => x.LastName);
            Map(x => x.Phone);
            Map(x => x.Position);
            Map(x => x.TerminatedAt);
        }

        public bool ConvertIntraining(ConvertFromStringArgs employeeRow) =>
           employeeRow.Row?.GetField(Constants.EmployeeCsvFieldNames.InTraining)?.ToLower() switch {
               Constants.EmployeeCsvFieldValues.InTrainingTrue => true,
               Constants.EmployeeCsvFieldValues.InTrainingFalse => false,
               _ => throw new ArgumentException($"Invalid string value for {Constants.EmployeeCsvFieldNames.InTraining}", nameof(employeeRow)),
           };
    }
}
