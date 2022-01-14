using SQLite;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace evo_interview_task.Data {
    internal class EmployeeRepository
    //TODO: Add caching
    //TODO: Add Unit of Work
    {
        private readonly SQLiteAsyncConnection database;
        public EmployeeRepository() {
            database = new SQLiteAsyncConnection("emp.db");
            database.CreateTableAsync<EmployeeDto>().GetAwaiter().GetResult();
        }

        public Task<int> InsertEmployeeAsync(EmployeeDto employeeRecord) {
            return database.InsertAsync(employeeRecord);
        }

        public Task UpsertEmployeesAsync(IEnumerable<EmployeeDto> employeeRecords) {
            return database.RunInTransactionAsync((syncConnection) => {
                //TODO Find a better way to upsert using the library functionality
                foreach (var employeeRecord in employeeRecords) {
                    syncConnection.Execute($@"
                    INSERT INTO {nameof(EmployeeDto)}(
                        {nameof(employeeRecord.EmployeeId)},
                        {nameof(employeeRecord.Country)},
                        {nameof(employeeRecord.Department)},
                        {nameof(employeeRecord.Email)},
                        {nameof(employeeRecord.FirstName)},
                        {nameof(employeeRecord.HiredAt)},
                        {nameof(employeeRecord.InTraining)},
                        {nameof(employeeRecord.LastName)},
                        {nameof(employeeRecord.Phone)},
                        {nameof(employeeRecord.Position)},
                        {nameof(employeeRecord.TerminatedAt)},
                        {nameof(employeeRecord.HRStatus)},
                        {nameof(employeeRecord.CompanyEmail)}
                    ) 
                    VALUES(?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
                    ON CONFLICT({nameof(employeeRecord.EmployeeId)}) 
                    DO UPDATE SET 
                        {nameof(employeeRecord.Country)}=excluded.{nameof(employeeRecord.Country)},
                        {nameof(employeeRecord.Department)}=excluded.{nameof(employeeRecord.Department)},
                        {nameof(employeeRecord.Email)}=excluded.{nameof(employeeRecord.Email)},
                        {nameof(employeeRecord.FirstName)}=excluded.{nameof(employeeRecord.FirstName)},
                        {nameof(employeeRecord.HiredAt)}=excluded.{nameof(employeeRecord.HiredAt)},
                        {nameof(employeeRecord.InTraining)}=excluded.{nameof(employeeRecord.InTraining)},
                        {nameof(employeeRecord.LastName)}=excluded.{nameof(employeeRecord.LastName)},
                        {nameof(employeeRecord.Phone)}=excluded.{nameof(employeeRecord.Phone)},
                        {nameof(employeeRecord.Position)}=excluded.{nameof(employeeRecord.Position)},
                        {nameof(employeeRecord.TerminatedAt)}=excluded.{nameof(employeeRecord.TerminatedAt)},
                        {nameof(employeeRecord.HRStatus)}=excluded.{nameof(employeeRecord.HRStatus)},
                        {nameof(employeeRecord.CompanyEmail)}=excluded.{nameof(employeeRecord.CompanyEmail)};"
                        ,
                        employeeRecord.EmployeeId, employeeRecord.Country,
                        employeeRecord.Department, employeeRecord.Email,
                        employeeRecord.FirstName, employeeRecord.HiredAt,
                        employeeRecord.InTraining, employeeRecord.LastName,
                        employeeRecord.Phone, employeeRecord.Position,
                        employeeRecord.TerminatedAt, employeeRecord.HRStatus,
                        employeeRecord.CompanyEmail
                    );
                }
            });
        }

        public Task<List<EmployeeDto>> GetAllEmployeesAsync() {
            return database.Table<EmployeeDto>().ToListAsync();
        }

        public Task<int> GetEmployeeCountAsync() {
            return database.Table<EmployeeDto>().CountAsync();
        }

        public Task<List<EmployeeDto>> QueryEmployeesAsync(Expression<Func<EmployeeDto, bool>> selectorFunc, int? limit = null) {
            var query = database.Table<EmployeeDto>().Where(selectorFunc);
            return (limit.HasValue ?
                          query.Take(limit.Value)
                        : query
                    ).ToListAsync();
        }
    }
}
