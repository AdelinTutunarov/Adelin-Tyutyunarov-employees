using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace EmployeesPrpjects
{
    public partial class MainWindow : Window
    {
        private List<Employee> employees;
        private List<Project> projects;
        public MainWindow()
        {
            InitializeComponent();
            employees = new List<Employee>();
            projects = new List<Project>();
        }

        private void btnOpenFile_click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt|CSV files (*.csv)|*.csv";
            if(openFileDialog.ShowDialog() == true)
            {
                LoadInfoFromFile(openFileDialog.FileName);
            }
        }

        private void LoadInfoFromFile(string path)
        {
            Employee employee;
            DateTime dt;
            using (var reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] values = line.Split(',');
                    //txtInfo.Text += line + Environment.NewLine;
                    if (values[3] == "NULL")
                    {
                        dt = DateTime.Now;
                    }
                    else
                    {
                        //dt = DateTime.ParseExact(values[3], "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        dt = DateTime.Parse(values[3]);
                    }
                    employee = new Employee()
                    {
                        EmployeeId = int.Parse(values[0]),
                        ProjectId = int.Parse(values[1]),
                        DateFrom = DateTime.Parse(values[2]),
                        DateTo = dt
                    };
                    txtInfo.Text += $"EmployeeId:{employee.EmployeeId}, ProjectId:{employee.ProjectId}, Start Date:{employee.DateFrom}, End Date:{employee.DateTo}" + Environment.NewLine;
                    employees.Add(employee);
                }
            }

            LoadProjectsInfo(employees);
        }

        private void LoadProjectsInfo(List<Employee> employees)
        {
            Project project;
            for (int i = 0; i < employees.Count - 1; i++)
            {
                for (int j = i + 1; j < employees.Count; j++)
                {
                    if (employees[i].ProjectId == employees[j].ProjectId)
                    {
                        int days = CalculateDaysWorked(employees[i].DateFrom, employees[i].DateTo, employees[j].DateFrom, employees[j].DateTo);
                        if (days > 0)
                        {
                            txtProjects.Text += $"1st employee id:{employees[i].EmployeeId}, 2nd employee id{employees[j].EmployeeId}, Project id:{employees[i].ProjectId}, Days worked together:{days}" + Environment.NewLine;
                            project = new Project()
                            {
                                EmployeeId1 = employees[i].EmployeeId,
                                EmployeeId2 = employees[j].EmployeeId,
                                ProjectId = employees[i].ProjectId,
                                DaysWorked = days
                            };
                            projects.Add(project);
                        }
                    }
                }
            }
        }

        private int CalculateDaysWorked(DateTime emp1startDate, DateTime emp1endDate, DateTime emp2startDate, DateTime emp2endDate)
        {
            DateTime start = emp1startDate > emp2startDate ? emp1startDate : emp2startDate;
            DateTime end = emp1endDate < emp2endDate ? emp1endDate : emp2endDate;
            return (end - start).Days;
        }

        private void btnCalculate_click(object sender, RoutedEventArgs e)
        {
            if (employees.Any())
            {
                Project project = projects.OrderByDescending(p => p.DaysWorked).FirstOrDefault();
                txtResult.Text = "The pair of employees who have worked together on common project the most are:" 
                    + Environment.NewLine
                    + $"Employee with ID:{project.EmployeeId1} and Employee with ID:{project.EmployeeId2} on Project with ID:{project.ProjectId} for {project.DaysWorked} days.";
            }
            else
            {
                txtResult.Text = "You have to load a file with information to calculate the result";
            }
        }
    }
}
