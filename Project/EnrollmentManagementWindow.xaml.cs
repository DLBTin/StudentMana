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
    /// Interaction logic for EnrollmentManagementWindow.xaml
    /// </summary>
    public partial class EnrollmentManagementWindow : Window
    {
        public EnrollmentManagementWindow()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData(string? studentId = null, string? courseId = null, DateTime? from = null, DateTime? to = null)
        {
            using (var context = new StudentManagementContext())
            {
                var query = context.Enrollments.AsQueryable();

                if (!string.IsNullOrEmpty(studentId) && int.TryParse(studentId, out var sId))
                    query = query.Where(en => en.StudentId == sId);

                if (!string.IsNullOrEmpty(courseId) && int.TryParse(courseId, out var cId))
                    query = query.Where(en => en.CourseId == cId);

                if (from.HasValue && to.HasValue)
                    query = query.Where(en => en.EnrollmentDate >= from.Value && en.EnrollmentDate <= to.Value);

                EnrollmentDataGrid.ItemsSource = query.ToList();
            }
        }

        private void SearchEnrollment_Click(object sender, RoutedEventArgs e)
        {
            var studentId = StudentIdSearchBox.Text;
            var courseId = CourseIdSearchBox.Text;
            var from = EnrollmentDateFrom.SelectedDate;
            var to = EnrollmentDateTo.SelectedDate;

            LoadData(studentId, courseId, from, to);
        }

        private void AddEnrollment_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new EnrollmentAddWindow();
            if (addWindow.ShowDialog() == true)
            {
                LoadData();
            }
        }

        private void EditEnrollment_Click(object sender, RoutedEventArgs e)
        {
            if (EnrollmentDataGrid.SelectedItem is Enrollment selectedEnrollment)
            {
                var editWindow = new EnrollmentEditWindow(selectedEnrollment);
                if (editWindow.ShowDialog() == true)
                {
                    LoadData();
                }
            }
        }

        private void DeleteEnrollment_Click(object sender, RoutedEventArgs e)
        {
            if (EnrollmentDataGrid.SelectedItem is Enrollment selectedEnrollment)
            {
                using (var context = new StudentManagementContext())
                {
                    var enrollment = context.Enrollments.Find(selectedEnrollment.EnrollmentId);
                    if (enrollment != null)
                    {
                        context.Enrollments.Remove(enrollment);
                        context.SaveChanges();
                        LoadData();
                    }
                }
            }
        }

        private void ReportEnrollment_Click(object sender, RoutedEventArgs e)
        {
            var window = new EnrollmentReportViewWindow("Month", DateTime.Now);
            window.Show();
        }
    }
}
