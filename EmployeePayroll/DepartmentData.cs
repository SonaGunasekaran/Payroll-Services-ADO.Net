using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeePayroll
{
    class DepartmentData
    {
        public int DepartmentId;
        public string Department;
        public DepartmentData(int depId, string dep)
        {
            this.DepartmentId = depId;
            this.Department = dep;
        }
    }
}
