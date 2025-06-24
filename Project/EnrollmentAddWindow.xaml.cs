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
    public partial class EnrollmentAddWindow : Window
    {
        public EnrollmentAddWindow()
        {
            InitializeComponent();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new StudentManagementContext())
            {
                var newEnrollment = new Enrollment
                {
                    StudentId = int.Parse(StudentIdTextBox.Text),
                    CourseId = int.Parse(CourseIdTextBox.Text),
                    EnrollmentDate = EnrollmentDatePicker.SelectedDate ?? DateTime.Now,
                    Grade = decimal.TryParse(GradeTextBox.Text, out var g) ? g : (decimal?)null
                };
                context.Enrollments.Add(newEnrollment);
                context.SaveChanges();
            }

            DialogResult = true;
            Close();
        }
    }
}
