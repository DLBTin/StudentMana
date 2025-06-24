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
    public partial class CourseAddWindow : Window
    {
        public CourseAddWindow()
        {
            InitializeComponent();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new StudentManagementContext())
            {
                var newCourse = new Course
                {
                    Code = CodeTextBox.Text,
                    Name = NameTextBox.Text,
                    Credits = int.Parse(CreditsTextBox.Text),
                    InstructorId = int.Parse(InstructorIdTextBox.Text),
                };
                context.Courses.Add(newCourse);
                context.SaveChanges();
            }

            DialogResult = true;
            Close();
        }
    }
}
