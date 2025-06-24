using Microsoft.EntityFrameworkCore;
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
    public partial class StudentMainWindow : Window
    {
        private User _user;

        public StudentMainWindow(User user)
        {
            InitializeComponent();
            _user = user;

            LoadEnrolledCourses();
        }

        private void LoadEnrolledCourses()
        {
            using (var context = new StudentManagementContext())
            {
                var student = context.Students
                    .First(s => s.UserId == _user.UserId);

                var courses = context.Enrollments
                    .Where(en => en.StudentId == student.StudentId)
                    .Include(en => en.Course).ThenInclude(c => c.Instructor).ThenInclude(i => i.User)
                    .ToList();

                EnrolledCoursesGrid.ItemsSource = courses;
            }
        }

        private void EnrollButton_Click(object sender, RoutedEventArgs e)
        {
            var enrollWindow = new StudentEnrollWindow(_user);
            enrollWindow.Closed += (s, args) => LoadEnrolledCourses();
            enrollWindow.Show();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            new LoginWindow(new AuthService()).Show();
            this.Close();
        }
    }
}
