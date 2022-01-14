using evo_interview_task.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace evo_interview_task.Services {
    internal interface IEmployeeService {
        public Task<IEnumerable<EmployeeModel>> QueryEmployees(string searchQuery, int limit);
        public Task<int> LoadEmployees(IEnumerable<EmployeeModel> records);
    }
}
