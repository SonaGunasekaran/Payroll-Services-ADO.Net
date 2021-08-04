using System;

namespace EmployeePayroll
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Payroll service database!");
                EmployeeRepo emp = new EmployeeRepo();
                emp.GetAllData();

        }
    }
}
