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
    /// <summary>
    /// Interaction logic for AdminMainWindow.xaml
    /// </summary>
    public partial class AdminMainWindow : Window
    {
        public AdminMainWindow()
        {
            InitializeComponent();
        }

        private void StudentButton_Click(object sender, RoutedEventArgs e) =>
            new StudentManagementView().Show();

        private void CourseButton_Click(object sender, RoutedEventArgs e) =>
            new CourseManagementWindow().Show();

        private void InstructorButton_Click(object sender, RoutedEventArgs e) =>
            new InstructorManagementWindow().Show();

        private void EnrollmentButton_Click(object sender, RoutedEventArgs e) =>
            new EnrollmentManagementWindow().Show();
    }
}
