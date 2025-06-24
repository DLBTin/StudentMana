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
    public partial class InstructorAddWindow : Window
    {
        public InstructorAddWindow()
        {
            InitializeComponent();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new StudentManagementContext())
            {
                var user = new User
                {
                    Username = EmployeeCodeTextBox.Text,
                    PasswordHash = "hashed",
                    Role = "instructor",
                    CreatedDate = DateTime.Now,
                    FullName = FullNameTextBox.Text,
                    Email = EmailTextBox.Text
                };
                context.Users.Add(user);
                context.SaveChanges();

                var instructor = new Instructor
                {
                    UserId = user.UserId,
                    EmployeeCode = EmployeeCodeTextBox.Text
                };
                context.Instructors.Add(instructor);
                context.SaveChanges();
            }

            DialogResult = true;
            Close();
        }
    }
}
