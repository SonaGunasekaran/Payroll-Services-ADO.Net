using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeePayroll
{
    class PayRollData
    {
        public double BasicPay;
        public double Deduction;
        public double TaxablePay;
        public double IncomeTax;
        public double NetPay;
        public PayRollData(double basicPay)
        {
            this.Deduction = 0.2 * basicPay;
            this.TaxablePay = this.Deduction - basicPay;
            this.IncomeTax = 0.1 * TaxablePay;
            this.NetPay = basicPay - this.IncomeTax;
        }
    }
}
