using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfApp2.Data;
using WpfApp2.Models;
namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Role _userRole;
        public ObservableCollection<Сourse> Courses { get; set; }
        public ObservableCollection<Bid> Bids { get; set; }
        public ObservableCollection<BidStatus> BidsStatus { get; set; }
        private List<Bid> _originalBids;
        public MainWindow(Role role = null)
        {
            InitializeComponent();
            Courses = new ObservableCollection<Сourse>();
            BidsStatus = new ObservableCollection<BidStatus>();
            Bids = new ObservableCollection<Bid>();
            DataContext = this;
            _userRole = role;
            LoadCourses();
            CheckAccess();
        }
        private void CheckAccess()
        {
            if (_userRole == null)
            {
                BidsTab.Visibility = Visibility.Collapsed;
            }
            else if (_userRole.RoleName == "Гость")
            {
                BidsTab.Visibility = Visibility.Collapsed;
            }
            else
            {
                LoadBids();
                LoadStatuses();
                BidsTab.Visibility = Visibility.Visible;
            }
        }

        private void LoadCourses()
        {
            Courses.Clear();
            using (var context = new Task4Context()) // Замените на ваш контекст
            {
                var courses = context.Сourses
                    .Include(c => c.Direction)
                    .Include(c => c.Teacher)
                    .ToList();

                foreach (var course in courses)
                {
                    Courses.Add(course);
                }
            }
        }

        private void LoadStatuses()
        {
            BidsStatus.Clear();
            using (var context = new Task4Context()) // Замените на ваш контекст
            {
                var statuses = context.BidStatuses.ToList();
                foreach (var status in statuses)
                {
                    BidsStatus.Add(status);
                }
            }
        }

        private void LoadBids()
        {
            Bids.Clear();
            using (var context = new Task4Context()) // Замените на ваш контекст
            {
                var bids = context.Bids
                    .Include(b => b.Сourse)
                    .Include(b => b.User)
                    .Include(b => b.BidStatus)
                    .ToList();

                foreach (var bid in bids)
                {
                    Bids.Add(bid);
                }
                _originalBids = bids;
            }
        }

        private void StatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox cb && cb.SelectedValue is int statusId)
            {
                var filtered = Bids.Where(b => b.BidStatus?.BidStatusId == statusId).ToList();
                BidsDataGrid.ItemsSource = filtered;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BidsDataGrid.ItemsSource = Bids;
            StatusComboBox.SelectedIndex = -1;
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string query = SearchBox.Text.Trim();

            var filtered = Bids.Where(b =>
                b.BidId.ToString().Contains(query, StringComparison.OrdinalIgnoreCase) ||
                (b.User?.Name?.Contains(query, StringComparison.OrdinalIgnoreCase) == true)
            ).ToList();

            BidsDataGrid.ItemsSource = filtered;
        }

        private void BidsDataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var selectedBid = BidsDataGrid.SelectedItem as Bid;

            if (selectedBid != null)
            {
                using (var context = new Task4Context()) 
                {
                    var fullBid = context.Bids
                        .Include(b => b.User)
                            .ThenInclude(u => u.Company)
                        .Include(b => b.User)
                            .ThenInclude(u => u.Role)
                        .Include(b => b.Сourse)
                            .ThenInclude(c => c.Direction)
                        .Include(b => b.Сourse)
                            .ThenInclude(c => c.Teacher)
                        .Include(b => b.BidStatus)
                        .FirstOrDefault(b => b.BidId == selectedBid.BidId);

                    if (fullBid != null)
                    {
                        BidDetailWindow detailWindow = new BidDetailWindow(fullBid);
                        detailWindow.ShowDialog();
                    }
                }
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            LoadBids();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            CreateBidWindow createBidWindow = new CreateBidWindow();
            createBidWindow.Show();
        }

        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SortComboBox.SelectedItem == null) return;

            var selected = (ComboBoxItem)SortComboBox.SelectedItem;
            var sortType = selected.Tag.ToString();

            var sorted = sortType switch
            {
                "desc" => Bids.OrderByDescending(b => b.BidData).ToList(),
                "asc" => Bids.OrderBy(b => b.BidData).ToList(),
                _ => Bids.ToList()
            };

            BidsDataGrid.ItemsSource = sorted;
        }
    }
}
    
