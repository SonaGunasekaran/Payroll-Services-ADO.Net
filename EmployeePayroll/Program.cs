using System;

namespace EmployeePayroll
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Payroll service database!");
            EmployeeRepo emp = new EmployeeRepo();
            EmployeeData data = new EmployeeData();
            Console.WriteLine("1.Retrive all data\n2.Update salary\n3.Retirieve Date using Name");
            Console.Write("Enter your choice:");
            int choice = Convert.ToInt32(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    emp.GetAllData();
                    break;
                case 2:
                    emp.UpdateSalary();
                    break;
                case 3:

                    emp.RetrieveDataOnDateRange();
                    break;
                default:
                    break;

            }
        }
    }
}
