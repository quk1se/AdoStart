﻿using ado1.Views;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlTypes;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ado1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SqlConnection connection;
        private DAL.DAO.ProductGroupDAO productGroupDAO;
        public ObservableCollection<String> columns { get; set; } = new();
        public ObservableCollection<DAL.Entity.ProductGroup> ProductGroups { get; set; } = new();

        public MainWindow()
        {
            InitializeComponent();
            connection = null!;
            this.DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                connection = new(App.ConnectionString);
                connection.Open();
                productGroupDAO = new(connection);
                LoadGroups();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Close();
            }
        }

        private void LoadGroups()
        {
            try
            {
                foreach (var group in productGroupDAO.GetAll())
                {
                    ProductGroups.Add(group);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Query error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            connection?.Dispose();
        }

        private void CreateGroup_Click(object sender, RoutedEventArgs e)
        {
            using SqlCommand command = new();
            command.Connection = connection;
            command.CommandText =
                @"CREATE TABLE ProductGroups (
	                Id			UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	                Name		NVARCHAR(50)     NOT NULL,
	                Description NTEXT            NOT NULL,
                    Picture     NVARCHAR(50)     NULL
                )";
            try
            {
                command.ExecuteNonQuery();
                MessageBox.Show("Table Created");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Create error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void InsertGroup_Click(object sender, RoutedEventArgs e)
        {
            using SqlCommand command = new();
            command.Connection = connection;
            command.CommandText =
                @"INSERT INTO ProductGroups
	                ( Id, Name,	Description, Picture )
                VALUES
                ( '089015F4-31B5-4F2B-BA05-A813B5419285', N'Інструменти',     N'Ручний інструмент для побутового використання', N'tools.png' ),
                ( 'A6D7858F-6B75-4C75-8A3D-C0B373828558', N'Офісні товари',   N'Декоративні товари для офісного облаштування', N'office.jpg' ),
                ( 'DEF24080-00AA-440A-9690-3C9267243C43', N'Вироби зі скла',  N'Творчі вироби зі скла', N'glass.jpg' ),
                ( '2F9A22BC-43F4-4F73-BAB1-9801052D85A9', N'Вироби з дерева', N'Композиції та декоративні твори з деревини', N'wood.jpg' ),
                ( 'D6D9783F-2182-469A-BD08-A24068BC2A23', N'Вироби з каменя', N'Корисні та декоративні вироби з натурального каменю', N'stone.jpg' )";
            try
            {
                command.ExecuteNonQuery();
                MessageBox.Show("Data inserted");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Insertation error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GroupCount_Click(object sender, RoutedEventArgs e)
        {
            using SqlCommand command = new();
            command.Connection = connection;
            command.CommandText = "SELECT COUNT(*) FROM ProductGroups";
            try
            {
                int cnt = Convert.ToInt32(
                    command.ExecuteScalar());
                MessageBox.Show($"Table has {cnt} rows");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Query error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem item)
            {
                if (item.Content is DAL.Entity.ProductGroup group)
                {
                    CrudGroupsWindow cgw = new(group with { });
                    bool? result = cgw.ShowDialog();
                    if (result == false)
                    {
                        if (MessageBox.Show("Confirm it?", "Data delete", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            if (productGroupDAO.Delete(group))
                            {
                                ProductGroups.Remove(group);
                            }
                            else
                                MessageBox.Show("Troubles with your db");
                        }
                    }
                    else if (result == true)
                    {
                        if (productGroupDAO.SaveAndUpdate(cgw.ProductGroup!))
                        {
                            int index = ProductGroups.IndexOf(group);
                            ProductGroups.Remove(group);
                            ProductGroups.Insert(index, cgw.ProductGroup!);
                            MessageBox.Show("Save data");
                        }
                        else
                            MessageBox.Show("Troubles with your db");
                    }
                }
            }
        }



        private void AddGroupButton_Click(object sender, RoutedEventArgs e)
        {
            DAL.Entity.ProductGroup newGroup = new()
            {
                Id = Guid.NewGuid(),

            };
            CrudGroupsWindow dialog = new(newGroup);
            bool? dialogResult = dialog.ShowDialog();
            if (dialogResult ?? false)
            {
                try
                {
                    productGroupDAO.Add(newGroup);
                    ProductGroups.Add(newGroup);
                    MessageBox.Show("Save data");
                }
                catch (Exception ex)
                {
                    Title = ex.Message;
                    MessageBox.Show("Troubles with your db");
                }
            }
        }
    }
}
