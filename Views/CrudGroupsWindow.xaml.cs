using ado1.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Documents.DocumentStructures;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ado1.Views
{
    /// <summary>
    /// Interac tion logic for CrudGroupsWindow.xaml
    /// </summary>
    public partial class CrudGroupsWindow : Window
    {
        public DAL.Entity.ProductGroup? ProductGroup { get; set; }
        private string name;
        private string description;
        private string picture;

        public CrudGroupsWindow(DAL.Entity.ProductGroup productGroup)
        {
            InitializeComponent();
            this.ProductGroup = productGroup;
            this.DataContext = this.ProductGroup;
            name = this.ProductGroup.Name;
            description = this.ProductGroup.Description;
            picture = this.ProductGroup.Picture;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProductGroup?.Name == "" || ProductGroup?.Description == "" || ProductGroup?.Picture == "")
            {
                MessageBox.Show("Enter normal data!");
            }
            else
            {
                DialogResult = true;
            }

        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            ProductGroup = null;
            Close();
        }

        private void NameTxtBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (NameTxtBox.Text == name)
            {
                SaveButton.IsEnabled = false;
            }
            else
            {
                SaveButton.IsEnabled = true;
            }
        }

        private void DescrTxtBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DescrTxtBox.Text == description)
            {
                SaveButton.IsEnabled = false;
            }
            else
            {
                SaveButton.IsEnabled = true;
            }
        }

        private void PictTxtBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (PictTxtBox.Text == picture)
            {
                SaveButton.IsEnabled = false;
            }
            else
            {
                SaveButton.IsEnabled = true;
            }
        }
    }
}
