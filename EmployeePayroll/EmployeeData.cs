using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeePayroll
{
    class EmployeeData
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Gender { get; set; }
        public double PhoneNumber { get; set; }
        public DateTime StartDate { get; set; }
        public string Address { get; set; }
        public string Department { get; set; }
        public double BasicPay { get; set; }
        public double NetPay { get; set; }
        public double Deductions { get; set; }
        public double IncomeTax { get; set; }
        public double TaxablePay { get; set; }
    }
}
