using AutoMapper;
using evo_interview_task.Data;
using evo_interview_task.Models;
using evo_interview_task.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace evo_interview_task {
    internal class Program {
        static async Task Main(string[] args) {

            //TODO Add DI 
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<EmployeeDto, EmployeeModel>();
                cfg.CreateMap<EmployeeModel, EmployeeDto>();
            });
            var mapper = new Mapper(config);
            var employeeRepo = new EmployeeRepository();
            var employeeService = new EmployeeService(employeeRepo, mapper);

            var csvDbLoader = new CsvDbLoader(mapper, employeeRepo, employeeService);
            await csvDbLoader.ShowMainMenu();
        }

        private class CsvDbLoader {
            private readonly EmployeeRepository employeeRepo;
            private readonly IEmployeeService employeeService;
            private readonly Mapper mapper;

            public CsvDbLoader(Mapper mapper, EmployeeRepository employeeRepo, EmployeeService employeeService) {
                this.employeeRepo = employeeRepo;
                this.employeeService = employeeService;
                this.mapper = mapper;
            }
            public async Task ShowMainMenu() {
                while (true) {
                    Console.Clear();
                    Console.WriteLine("Select option:");
                    Console.WriteLine("1.Load data from CSV");
                    Console.WriteLine("2.Query data");
                    Console.Write("Select option: ");
                    if (!await ProcessInput()) {
                        break;
                    };
                }
            }

            Task<bool> ProcessInput() => Console.ReadLine() switch {
                "1" => HandleLoadData(),
                "2" => HandleQueryDataOption(),
                _ => HandleInvalidOption()
            };

            Task<bool> HandleInvalidOption() {
                Console.Clear();
                Console.WriteLine("Wrong option selected. Press Enter to continue;");
                Console.ReadLine();
                return Task.FromResult(true);
            }

            Task HandleNoData() {
                Console.Clear();
                Console.WriteLine("No data loaded. Unable to run query.");
                Console.WriteLine("Press Enter to continue.");
                Console.ReadLine();
                return ShowMainMenu();
            }
            async Task<bool> HandleQueryDataOption() {
                if (await employeeRepo.GetEmployeeCountAsync() == 0) {
                    await HandleNoData();
                    return true;
                }
                while (true) {
                    await QueryData();
                    Console.WriteLine("Press Enter to to run another query, ESC to quit or any other key to return to main menu");
                    switch (Console.ReadKey().Key) {
                        case ConsoleKey.Escape:
                            return false;
                        case ConsoleKey.Enter:
                            break;
                        default:
                            return true;
                    }
                }
            }

            async Task<bool> HandleLoadData() {
                Console.Clear();
                var recordsLoaded = await LoadData();
                Console.WriteLine($"Records {recordsLoaded} loaded to database successfully.");
                Console.WriteLine("Press ESC to quit or any other key to return to main menu.");
                switch (Console.ReadKey().Key) {
                    case ConsoleKey.Escape:
                        return false;
                    default:
                        return true;
                }
            }

            async Task QueryData() {
                Console.Clear();
                Console.Write("Please enter search string (partial or full email, first or last name: ");
                var searchQuery = Console.ReadLine();
                var employees = await employeeService.QueryEmployees(searchQuery, 5);
                Console.WriteLine($"{employees.Count()} records found.");
                foreach (var employee in employees) {
                    Console.WriteLine(employee.ToString());
                }
            }

            async Task<int> LoadData() {

                Console.WriteLine("Reading csv file");
                var records = EmployeeCsvReader.ReadEmployeesFromCSV("Resources\\EmployeeDataTest.csv");
                Console.WriteLine($"{records.Count()} records found in csv file");
                Console.WriteLine("Loading records to database");
                var mappedRecords = records.Select(x => mapper.Map<EmployeeModel>(x).WithCompanyEmail().WithHRStatus());

                await employeeService.LoadEmployees(mappedRecords);
                return records.Count();
            }
        }
    }
}
