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
    public partial class EnrollmentEditWindow : Window
    {
        private Enrollment _enrollment;

        public EnrollmentEditWindow(Enrollment enrollment)
        {
            InitializeComponent();
            _enrollment = enrollment;

            StudentIdTextBox.Text = _enrollment.StudentId.ToString();
            CourseIdTextBox.Text = _enrollment.CourseId.ToString();
            EnrollmentDatePicker.SelectedDate = _enrollment.EnrollmentDate;
            GradeTextBox.Text = _enrollment.Grade?.ToString();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new StudentManagementContext())
            {
                var enrollment = context.Enrollments.Find(_enrollment.EnrollmentId);
                if (enrollment != null)
                {
                    enrollment.StudentId = int.Parse(StudentIdTextBox.Text);
                    enrollment.CourseId = int.Parse(CourseIdTextBox.Text);
                    enrollment.EnrollmentDate = EnrollmentDatePicker.SelectedDate ?? DateTime.Now;
                    enrollment.Grade = decimal.TryParse(GradeTextBox.Text, out var g) ? g : (decimal?)null;

                    context.SaveChanges();
                }
            }

            DialogResult = true;
            Close();
        }
    }
}
