using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSystem
{
    internal class Student
    {
        public int RollID { get; set; }
        public string StudentName { get; set; }
        public int StudentGrade { get; set; }

        public static List<Student> StudentsList = new List<Student>();

    }
}
