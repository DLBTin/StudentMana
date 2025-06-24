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
    public partial class CourseEditWindow : Window
    {
        private Course _course;

        public CourseEditWindow(Course course)
        {
            InitializeComponent();
            _course = course;

            CodeTextBox.Text = _course.Code;
            NameTextBox.Text = _course.Name;
            CreditsTextBox.Text = _course.Credits.ToString();
            InstructorIdTextBox.Text = _course.InstructorId.ToString();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new StudentManagementContext())
            {
                var course = context.Courses.Find(_course.CourseId);
                if (course != null)
                {
                    course.Code = CodeTextBox.Text;
                    course.Name = NameTextBox.Text;
                    course.Credits = int.Parse(CreditsTextBox.Text);
                    course.InstructorId = int.Parse(InstructorIdTextBox.Text);
                    context.SaveChanges();
                }
            }

            DialogResult = true;
            Close();
        }
    }
}
