using AutoMapper;
using evo_interview_task.Data;
using evo_interview_task.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace evo_interview_task.Services {
    internal class EmployeeService : IEmployeeService {
        private readonly EmployeeRepository employeeRepository;
        private readonly Mapper mapper;

        public EmployeeService(EmployeeRepository employeeRepository, Mapper mapper) {
            this.employeeRepository = employeeRepository;
            this.mapper = mapper;
        }

        public async Task<int> LoadEmployees(IEnumerable<EmployeeModel> records) {

            await employeeRepository.UpsertEmployeesAsync(records.Select(x => mapper.Map<EmployeeDto>(x)));
            return records.Count();
        }

        public async Task<IEnumerable<EmployeeModel>> QueryEmployees(string searchQuery, int limit) {
            var employees = await employeeRepository.QueryEmployeesAsync(
                (emp) =>
                    //HACK: Specifying StringComparison does not work due to SQLite library limitations
                    emp.Email.ToLower().Contains(searchQuery.ToLower())
                    || emp.FirstName.ToLower().Contains(searchQuery.ToLower())
                    || emp.LastName.ToLower().Contains(searchQuery.ToLower())
                , limit);
            return employees.Select(x => mapper.Map<EmployeeModel>(x));

        }
    }
}