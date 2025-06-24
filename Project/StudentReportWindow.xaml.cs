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
    /// <summary>
    /// Interaction logic for StudentReportWindow.xaml
    /// </summary>
    public partial class StudentReportWindow : Window
    {
        public StudentReportWindow(string filterType, DateTime selectedDate)
        {
            InitializeComponent();

            // Tiêu đề báo cáo
            ReportPeriodRun.Text = $"{filterType} - {selectedDate:dd/MM/yyyy}";
            GeneratedOnRun.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

            LoadReportData(filterType, selectedDate);
        }

        private void LoadReportData(string filterType, DateTime selectedDate)
        {
            using (var context = new StudentManagementContext())
            {
                var studentsQuery = context.Students.AsQueryable();

                // Áp dụng bộ lọc thời gian
                if (filterType == "Month")
                    studentsQuery = studentsQuery.Where(s => s.User.CreatedDate.Month == selectedDate.Month && s.User.CreatedDate.Year == selectedDate.Year);
                else if (filterType == "Quarter")
                    studentsQuery = studentsQuery.Where(s => (s.User.CreatedDate.Month - 1) / 3 + 1 == (selectedDate.Month - 1) / 3 + 1 && s.User.CreatedDate.Year == selectedDate.Year);
                else if (filterType == "Year")
                    studentsQuery = studentsQuery.Where(s => s.User.CreatedDate.Year == selectedDate.Year);

                var students = studentsQuery.Select(s => new
                {
                    s.StudentId,
                    Name = s.User.FullName,
                    Email = s.User.Email,
                    CreatedDate = s.User.CreatedDate
                }).ToList();

                // Build Table
                var tableRowGroup = StudentReportTable.RowGroups[0];
                foreach (var student in students)
                {
                    var row = new TableRow();
                    row.Cells.Add(new TableCell(new Paragraph(new Run(student.StudentId.ToString()))));
                    row.Cells.Add(new TableCell(new Paragraph(new Run(student.Name ?? ""))));
                    row.Cells.Add(new TableCell(new Paragraph(new Run(student.Email ?? ""))));
                    row.Cells.Add(new TableCell(new Paragraph(new Run(student.CreatedDate.ToString("dd/MM/yyyy")))));
                    tableRowGroup.Rows.Add(row);
                }
            }
        }
    }
}
