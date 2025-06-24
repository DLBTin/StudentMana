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
    /// Interaction logic for InstructorManagementWindow.xaml
    /// </summary>
    public partial class InstructorManagementWindow : Window
    {
        public InstructorManagementWindow()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData(string? employeeCode = null, string? name = null)
        {
            using (var context = new StudentManagementContext())
            {
                var query = context.Instructors.AsQueryable();

                if (!string.IsNullOrEmpty(employeeCode))
                    query = query.Where(i => i.EmployeeCode.Contains(employeeCode));
                if (!string.IsNullOrEmpty(name))
                    query = query.Where(i => i.User.FullName.Contains(name));

                InstructorDataGrid.ItemsSource = query.ToList();
            }
        }

        private void SearchInstructor_Click(object sender, RoutedEventArgs e)
        {
            var code = InstructorCodeSearchBox.Text;
            var name = InstructorNameSearchBox.Text;

            LoadData(code, name);
        }

        private void AddInstructor_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new InstructorAddWindow();
            if (addWindow.ShowDialog() == true)
            {
                LoadData();
            }
        }

        private void EditInstructor_Click(object sender, RoutedEventArgs e)
        {
            if (InstructorDataGrid.SelectedItem is Instructor selectedInstructor)
            {
                var editWindow = new InstructorEditWindow(selectedInstructor);
                if (editWindow.ShowDialog() == true)
                {
                    LoadData();
                }
            }
        }

        private void DeleteInstructor_Click(object sender, RoutedEventArgs e)
        {
            if (InstructorDataGrid.SelectedItem is Instructor selectedInstructor)
            {
                using (var context = new StudentManagementContext())
                {
                    var instructor = context.Instructors.Find(selectedInstructor.InstructorId);
                    if (instructor != null)
                    {
                        context.Instructors.Remove(instructor);
                        context.SaveChanges();
                        LoadData();
                    }
                }
            }
        }

        private void ReportInstructor_Click(object sender, RoutedEventArgs e)
        {
            var window = new InstructorReportViewWindow("Month", DateTime.Now);
            window.Show();
        }
    }
}
