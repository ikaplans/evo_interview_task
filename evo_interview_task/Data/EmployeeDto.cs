using SQLite;
using System;

namespace evo_interview_task.Data {
    internal class EmployeeDto {
        public string Country { get; set; }
        public string Department { get; set; }
        public string Email { get; set; }
        [PrimaryKey]
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public DateTime HiredAt { get; set; }
        public bool InTraining { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Position { get; set; }
        public DateTime? TerminatedAt { get; set; }
        public string HRStatus { get; set; }
        public string CompanyEmail { get; set; }
    }
}
