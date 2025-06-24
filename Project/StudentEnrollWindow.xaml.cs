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
    public partial class StudentEnrollWindow : Window
    {
        private User _user;

        public StudentEnrollWindow(User user)
        {
            InitializeComponent();
            _user = user;

            LoadAvailableCourses();
        }

        private void LoadAvailableCourses()
        {
            using (var context = new StudentManagementContext())
            {
                var student = context.Students.First(s => s.UserId == _user.UserId);

                var enrolledCourseIds = context.Enrollments
                    .Where(en => en.StudentId == student.StudentId)
                    .Select(en => en.CourseId).ToList();

                var courses = context.Courses
                    .Include(c => c.Instructor).ThenInclude(i => i.User)
                    .Where(c => !enrolledCourseIds.Contains(c.CourseId))
                    .ToList();

                AvailableCoursesGrid.ItemsSource = courses;
            }
        }

        private void EnrollButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedCourse = AvailableCoursesGrid.SelectedItem as Course;
            if (selectedCourse == null)
            {
                MessageBox.Show("Please select a course to enroll.");
                return;
            }

            using (var context = new StudentManagementContext())
            {
                var student = context.Students.First(s => s.UserId == _user.UserId);

                var newEnrollment = new Enrollment
                {
                    StudentId = student.StudentId,
                    CourseId = selectedCourse.CourseId,
                    EnrollmentDate = System.DateTime.Now
                };
                context.Enrollments.Add(newEnrollment);
                context.SaveChanges();
            }

            MessageBox.Show($"Enrolled in course: {selectedCourse.Name}");
            this.Close();
        }
    }
}
