using System;
using System.Collections.Generic;
using System.Data.Entity;
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

namespace _4kUP
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        agencyEntities db = new agencyEntities();
        public MainWindow()
        {
            InitializeComponent();
            db.Agents.Load();
            db.Clients.Load();
            clientsGrid.ItemsSource = db.Clients.Local.ToBindingList();
            agentsGrid.ItemsSource = db.Agents.Local.ToBindingList();
        }

        private void IntValuesProtect(object sender, TextCompositionEventArgs e)
        {
            if (int.TryParse(e.Text, out int _) == false) e.Handled = true;
        }

        private void AddClient(object sender, RoutedEventArgs e)
        {
            try
            {
                if (phoneBox.Text != null && phoneBox.Text != string.Empty || emailBox.Text != null && emailBox.Text != string.Empty)
                {
                    Clients temp = new Clients() { FirstName = firstNameBox.Text, MiddleName = middleNameBox.Text, LastName = lastNameBox.Text, Phone = phoneBox.Text, Email = emailBox.Text };
                    db.Clients.Add(temp);
                    db.SaveChanges();
                }
                else MessageBox.Show("Телефон либо почта должны быть заполнены");
            }
            catch
            {
                MessageBox.Show("Ошибка добавления");
                return;
            }

        }

        private void clientsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (clientsGrid.SelectedItem != null)
            {
                var temp = (Clients)clientsGrid.SelectedItem;
                firstNameBox.Text = temp.FirstName;
                middleNameBox.Text = temp.MiddleName;
                lastNameBox.Text = temp.LastName;
                phoneBox.Text = temp.Phone;
                emailBox.Text = temp.Email;
                changeBtn.IsEnabled = true;
                deleteBtn.IsEnabled = true;
            }
            else
            {
                changeBtn.IsEnabled = false;
                deleteBtn.IsEnabled = false;
            }
        }

        private void ChangeClient(object sender, RoutedEventArgs e)
        {
            var temp = (Clients)clientsGrid.SelectedItem;
            Clients result = (from p in db.Clients
                              where p.Id == temp.Id
                              select p).SingleOrDefault();

            result.FirstName = firstNameBox.Text;
            result.MiddleName = middleNameBox.Text;
            result.LastName = lastNameBox.Text;
            result.Phone = phoneBox.Text;
            result.Email = emailBox.Text;

            db.SaveChanges();
            clientsGrid.Items.Refresh();

        }

        private void DeleteClient(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите удалить запись?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var temp = (Clients)clientsGrid.SelectedItem;
                db.Clients.Remove(temp);
                db.SaveChanges();
                clientsGrid.Items.Refresh();
                firstNameBox.Clear();
                middleNameBox.Clear();
                lastNameBox.Clear();
                phoneBox.Clear();
                emailBox.Clear();
            }

            else return;
        }

        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DeleteAgent(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите удалить запись?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var temp = (Agents)agentsGrid.SelectedItem;
                db.Agents.Remove(temp);
                db.SaveChanges();
                agentsGrid.Items.Refresh();
                firstANameBox.Clear();
                middleANameBox.Clear();
                lastANameBox.Clear();
                taxBox.Clear();
            }

            else return;
        }

        private void AddAgent(object sender, RoutedEventArgs e)
        {
            try
            {
                Agents temp = new Agents() { FirstName = firstANameBox.Text, MiddleName = middleANameBox.Text, LastName = lastANameBox.Text, DealShare = Convert.ToDouble(taxBox.Text) };
                db.Agents.Add(temp);
                db.SaveChanges();
            }
            catch
            {
                MessageBox.Show("Ошибка добавления");
                return;
            }
        }

        private void ChangeAgent(object sender, RoutedEventArgs e)
        {
            var temp = (Agents)agentsGrid.SelectedItem;
            Agents result = (from p in db.Agents
                              where p.Id == temp.Id
                              select p).SingleOrDefault();

            result.FirstName = firstNameBox.Text;
            result.MiddleName = middleNameBox.Text;
            result.LastName = lastNameBox.Text;
            result.DealShare= Convert.ToDouble(taxBox.Text);

            db.SaveChanges();
            clientsGrid.Items.Refresh();
        }

        private void agentsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (agentsGrid.SelectedItem != null )
            {
                var temp = (Agents)agentsGrid.SelectedItem;
                firstANameBox.Text = temp.FirstName;
                middleANameBox.Text = temp.MiddleName;
                lastANameBox.Text = temp.LastName;
                taxBox.Text = temp.DealShare.ToString();
                changeABtn.IsEnabled = true;
                deleteABtn.IsEnabled = true;
            }
            else
            {
                changeABtn.IsEnabled = false;
                deleteABtn.IsEnabled = false;
            }
        }
    }
}
