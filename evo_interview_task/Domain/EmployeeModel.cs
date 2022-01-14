using System;

namespace evo_interview_task.Models {
    internal class EmployeeModel {
        public string Country { get; set; }
        public string Department { get; set; }
        public string Email { get; set; }
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

        public EmployeeModel WithHRStatus() {
            var withHRStatus = (EmployeeModel)this.MemberwiseClone();
            withHRStatus.HRStatus = TerminatedAt.HasValue ? Constants.Strings.TerminatedStatus : InTraining ? Constants.Strings.InTrainingStatus : Constants.Strings.ActiveStatus;
            return withHRStatus;
        }

        public EmployeeModel WithCompanyEmail() {
            var withEmail = (EmployeeModel)this.MemberwiseClone();
            withEmail.CompanyEmail = $"{FirstName[..1]}{LastName}{Constants.Strings.EmailDomain}";
            return withEmail;
        }


        public override string ToString() {
            return
            $"#---------------------------------------------------\n" +
            $"# Employee Id:    {EmployeeId}\n" +
            $"# Full name:      {FirstName} {LastName}\n" +
            $"# Position:       {Position}\n" +
            $"# Email:          {Email}\n" +
            $"# Company email:  {CompanyEmail}\n" +
            $"# Phone:          {Phone}\n" +
            $"# Status:         {HRStatus}\n" +
            $"# Department:     {Department}\n" +
            $"# Hired at        {HiredAt.ToString("d")}\n" +
            (TerminatedAt.HasValue ?
                $"# Terminated at   {TerminatedAt.Value.ToString("d")}\n"
                : String.Empty) +
            $"#---------------------------------------------------";
        }
    }
}
