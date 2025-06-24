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
    /// Interaction logic for CourseManagementWindow.xaml
    /// </summary>
    public partial class CourseManagementWindow : Window
    {
        public CourseManagementWindow()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData(string? code = null, string? name = null, int? credits = null)
        {
            using (var context = new StudentManagementContext())
            {
                var query = context.Courses.AsQueryable();

                if (!string.IsNullOrEmpty(code))
                    query = query.Where(c => c.Code.Contains(code));
                if (!string.IsNullOrEmpty(name))
                    query = query.Where(c => c.Name.Contains(name));
                if (credits.HasValue)
                    query = query.Where(c => c.Credits == credits.Value);

                CourseDataGrid.ItemsSource = query.ToList();
            }
        }

        private void SearchCourse_Click(object sender, RoutedEventArgs e)
        {
            var code = CourseCodeSearchBox.Text;
            var name = CourseNameSearchBox.Text;
            var credits = int.TryParse(CreditsSearchBox.Text, out var result) ? result : (int?)null;

            LoadData(code, name, credits);
        }

        private void AddCourse_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new CourseAddWindow();
            if (addWindow.ShowDialog() == true)
            {
                LoadData();
            }
        }

        private void EditCourse_Click(object sender, RoutedEventArgs e)
        {
            if (CourseDataGrid.SelectedItem is Course selectedCourse)
            {
                var editWindow = new CourseEditWindow(selectedCourse);
                if (editWindow.ShowDialog() == true)
                {
                    LoadData();
                }
            }
        }

        private void DeleteCourse_Click(object sender, RoutedEventArgs e)
        {
            if (CourseDataGrid.SelectedItem is Course selectedCourse)
            {
                using (var context = new StudentManagementContext())
                {
                    var course = context.Courses.Find(selectedCourse.CourseId);
                    if (course != null)
                    {
                        context.Courses.Remove(course);
                        context.SaveChanges();
                        LoadData();
                    }
                }
            }
        }

        private void ReportCourse_Click(object sender, RoutedEventArgs e)
        {
            var window = new CourseReportViewWindow("Month", DateTime.Now);
            window.Show();
        }
    }
}
