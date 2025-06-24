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
    public partial class InstructorReportViewWindow : Window
    {
        public InstructorReportViewWindow(string filterType, DateTime selectedDate)
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
                var instructorsQuery = context.Instructors.AsQueryable();

                if (filterType == "Month")
                    instructorsQuery = instructorsQuery.Where(i => i.Courses.Any(c => c.Enrollments.Any(en => en.EnrollmentDate.Month == selectedDate.Month && en.EnrollmentDate.Year == selectedDate.Year)));
                else if (filterType == "Quarter")
                    instructorsQuery = instructorsQuery.Where(i => i.Courses.Any(c => c.Enrollments.Any(en => (en.EnrollmentDate.Month - 1) / 3 + 1 == (selectedDate.Month - 1) / 3 + 1 && en.EnrollmentDate.Year == selectedDate.Year)));
                else if (filterType == "Year")
                    instructorsQuery = instructorsQuery.Where(i => i.Courses.Any(c => c.Enrollments.Any(en => en.EnrollmentDate.Year == selectedDate.Year)));

                var instructors = instructorsQuery.Select(i => new
                {
                    i.InstructorId,
                    i.User.FullName,
                    i.User.Email,
                    TotalCourses = i.Courses.Count
                }).ToList();

                var tableRowGroup = InstructorReportTable.RowGroups[0];
                foreach (var item in instructors)
                {
                    var row = new TableRow();
                    row.Cells.Add(new TableCell(new Paragraph(new Run(item.InstructorId.ToString()))));
                    row.Cells.Add(new TableCell(new Paragraph(new Run(item.FullName ?? ""))));
                    row.Cells.Add(new TableCell(new Paragraph(new Run(item.Email ?? ""))));
                    row.Cells.Add(new TableCell(new Paragraph(new Run(item.TotalCourses.ToString()))));
                    tableRowGroup.Rows.Add(row);
                }
            }
        }
    }
}
