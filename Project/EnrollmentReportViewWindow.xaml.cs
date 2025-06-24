using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Project
{
    public partial class EnrollmentReportViewWindow : Window
    {
        public EnrollmentReportViewWindow(string filterType, DateTime selectedDate)
        {
            InitializeComponent();

            ReportPeriodRun.Text = $"{filterType} - {selectedDate:dd/MM/yyyy}";
            GeneratedOnRun.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

            LoadReportData(filterType, selectedDate);
        }

        private void LoadReportData(string filterType, DateTime selectedDate)
        {
            using (var context = new StudentManagementContext())
            {
                var enrollmentsQuery = context.Enrollments.AsQueryable();

                if (filterType == "Month")
                    enrollmentsQuery = enrollmentsQuery.Where(en => en.EnrollmentDate.Month == selectedDate.Month && en.EnrollmentDate.Year == selectedDate.Year);
                else if (filterType == "Quarter")
                    enrollmentsQuery = enrollmentsQuery.Where(en => (en.EnrollmentDate.Month - 1) / 3 + 1 == (selectedDate.Month - 1) / 3 + 1 && en.EnrollmentDate.Year == selectedDate.Year);
                else if (filterType == "Year")
                    enrollmentsQuery = enrollmentsQuery.Where(en => en.EnrollmentDate.Year == selectedDate.Year);

                var results = enrollmentsQuery.Select(en => new
                {
                    StudentId = en.Student.StudentId,
                    StudentName = en.Student.User.FullName,
                    CourseName = en.Course.Name,
                    EnrollmentDate = en.EnrollmentDate
                }).ToList();

                var tableRowGroup = EnrollmentReportTable.RowGroups[0];
                foreach (var item in results)
                {
                    var row = new TableRow();
                    row.Cells.Add(new TableCell(new Paragraph(new Run(item.StudentId.ToString()))));
                    row.Cells.Add(new TableCell(new Paragraph(new Run(item.StudentName ?? ""))));
                    row.Cells.Add(new TableCell(new Paragraph(new Run(item.CourseName ?? ""))));
                    row.Cells.Add(new TableCell(new Paragraph(new Run(item.EnrollmentDate.ToString("dd/MM/yyyy")))));
                    tableRowGroup.Rows.Add(row);
                }
            }
        }
    }
}
