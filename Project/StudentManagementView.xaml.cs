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
    /// Interaction logic for StudentManagementView.xaml
    /// </summary>
    public partial class StudentManagementView : Window
    {
        public StudentManagementView()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData(string? code = null, string? name = null, DateTime? from = null, DateTime? to = null)
        {
            using (var context = new StudentManagementContext())
            {
                var query = context.Students.AsQueryable();

                if (!string.IsNullOrEmpty(code))
                    query = query.Where(s => s.StudentCode.Contains(code));
                if (!string.IsNullOrEmpty(name))
                    query = query.Where(s => s.User.FullName.Contains(name));
                if (from.HasValue && to.HasValue)
                    query = query.Where(s => s.User.CreatedDate >= from.Value && s.User.CreatedDate <= to.Value);

                StudentDataGrid.ItemsSource = query.ToList();
            }
        }

        private void SearchStudent_Click(object sender, RoutedEventArgs e)
        {
            var code = StudentCodeSearchBox.Text;
            var name = StudentNameSearchBox.Text;
            var from = StudentDateFrom.SelectedDate;
            var to = StudentDateTo.SelectedDate;

            LoadData(code, name, from, to);
        }

        private void AddStudent_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new StudentAddWindow();
            if (addWindow.ShowDialog() == true)
            {
                LoadData();
            }
        }

        private void EditStudent_Click(object sender, RoutedEventArgs e)
        {
            if (StudentDataGrid.SelectedItem is Student selectedStudent)
            {
                var editWindow = new StudentEditWindow(selectedStudent);
                if (editWindow.ShowDialog() == true)
                {
                    LoadData();
                }
            }
        }

        private void DeleteStudent_Click(object sender, RoutedEventArgs e)
        {
            if (StudentDataGrid.SelectedItem is Student selectedStudent)
            {
                using (var context = new StudentManagementContext())
                {
                    var student = context.Students.Find(selectedStudent.StudentId);
                    if (student != null)
                    {
                        context.Students.Remove(student);
                        context.SaveChanges();
                        LoadData();
                    }
                }
            }
        }

        private void ReportStudent_Click(object sender, RoutedEventArgs e)
        {
            var window = new StudentReportWindow("Month", DateTime.Now);
            window.Show();
        }
    }
}
