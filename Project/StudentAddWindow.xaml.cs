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
    public partial class StudentAddWindow : Window
    {
        public StudentAddWindow()
        {
            InitializeComponent();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new StudentManagementContext())
            {
                var user = new User
                {
                    Username = StudentCodeTextBox.Text,
                    PasswordHash = "hashed",
                    Role = "student",
                    CreatedDate = DateTime.Now,
                    FullName = FullNameTextBox.Text,
                    Email = EmailTextBox.Text
                };
                context.Users.Add(user);
                context.SaveChanges();

                var student = new Student
                {
                    UserId = user.UserId,
                    StudentCode = StudentCodeTextBox.Text,
                    DateOfBirth = DateOnly.FromDateTime(DateOfBirthPicker.SelectedDate ?? DateTime.Now)
                };
                context.Students.Add(student);
                context.SaveChanges();
            }

            DialogResult = true;
            Close();
        }
    }
}
