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
    public partial class InstructorEditWindow : Window
    {
        private Instructor _instructor;

        public InstructorEditWindow(Instructor instructor)
        {
            InitializeComponent();
            _instructor = instructor;

            EmployeeCodeTextBox.Text = _instructor.EmployeeCode;
            FullNameTextBox.Text = _instructor.User.FullName;
            EmailTextBox.Text = _instructor.User.Email;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new StudentManagementContext())
            {
                var instructor = context.Instructors.Find(_instructor.InstructorId);
                if (instructor != null)
                {
                    instructor.EmployeeCode = EmployeeCodeTextBox.Text;
                    instructor.User.FullName = FullNameTextBox.Text;
                    instructor.User.Email = EmailTextBox.Text;

                    context.SaveChanges();
                }
            }

            DialogResult = true;
            Close();
        }
    }
}
