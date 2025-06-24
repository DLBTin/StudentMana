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
    public partial class StudentEditWindow : Window
    {
        private Student _student;

        public StudentEditWindow(Student student)
        {
            InitializeComponent();
            _student = student;

            StudentCodeTextBox.Text = student.StudentCode;
            FullNameTextBox.Text = student.User.FullName;
            EmailTextBox.Text = student.User.Email;
            DateOfBirthPicker.SelectedDate = student.DateOfBirth.ToDateTime(new TimeOnly());
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new StudentManagementContext())
            {
                var student = context.Students.Find(_student.StudentId);
                if (student != null)
                {
                    student.StudentCode = StudentCodeTextBox.Text;
                    student.DateOfBirth = DateOnly.FromDateTime(DateOfBirthPicker.SelectedDate ?? DateTime.Now);
                    student.User.FullName = FullNameTextBox.Text;
                    student.User.Email = EmailTextBox.Text;

                    context.SaveChanges();
                }
            }

            DialogResult = true;
            Close();
        }
    }
}
