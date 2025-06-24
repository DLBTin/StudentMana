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
    public partial class CourseReportViewWindow : Window
    {
        public CourseReportViewWindow(string filterType, DateTime selectedDate)
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
                var coursesQuery = context.Courses.AsQueryable();

                if (filterType == "Month")
                    coursesQuery = coursesQuery.Where(c => c.Enrollments.Any(en => en.EnrollmentDate.Month == selectedDate.Month && en.EnrollmentDate.Year == selectedDate.Year));
                else if (filterType == "Quarter")
                    coursesQuery = coursesQuery.Where(c => c.Enrollments.Any(en => (en.EnrollmentDate.Month - 1) / 3 + 1 == (selectedDate.Month - 1) / 3 + 1 && en.EnrollmentDate.Year == selectedDate.Year));
                else if (filterType == "Year")
                    coursesQuery = coursesQuery.Where(c => c.Enrollments.Any(en => en.EnrollmentDate.Year == selectedDate.Year));

                var courses = coursesQuery.Select(c => new
                {
                    c.CourseId,
                    c.Code,
                    c.Name,
                    c.Credits,
                    StudentCount = c.Enrollments.Count
                }).ToList();

                var tableRowGroup = CourseReportTable.RowGroups[0];
                foreach (var item in courses)
                {
                    var row = new TableRow();
                    row.Cells.Add(new TableCell(new Paragraph(new Run(item.CourseId.ToString()))));
                    row.Cells.Add(new TableCell(new Paragraph(new Run(item.Name ?? ""))));
                    row.Cells.Add(new TableCell(new Paragraph(new Run(item.Credits.ToString()))));
                    row.Cells.Add(new TableCell(new Paragraph(new Run(item.StudentCount.ToString()))));
                    tableRowGroup.Rows.Add(row);
                }
            }
        }
    }
}
