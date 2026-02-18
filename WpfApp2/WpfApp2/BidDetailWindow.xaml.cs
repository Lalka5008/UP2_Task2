using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfApp2.Data;
using WpfApp2.Models;
using Microsoft.EntityFrameworkCore;

namespace WpfApp2
{
    /// <summary>
    /// Логика взаимодействия для BidDetailWindow.xaml
    /// </summary>
    public partial class BidDetailWindow : Window
    {
        private Bid _bid;
        private List<BidStatus> _statuses;
        private List<Сourse> _courses;
        private List<Direction> _directions;
        private List<Teacher> _teachers;
        private List<Company> _companies;
        public BidDetailWindow(Bid bid )
        {
            InitializeComponent();
            _bid = bid;
            LoadStatuses();
            LoadData();
            LoadBidData();
        }
    
            private void LoadData()
        {
            using var context = new Task4Context(); // Замените на ваш контекст

            // Загружаем статусы
            _statuses = context.BidStatuses.ToList();
            StatusComboBox.ItemsSource = _statuses;

            // Загружаем курсы
            _courses = context.Сourses
                .Include(c => c.Direction)
                .Include(c => c.Teacher)
                .ToList();
            cbCourse.ItemsSource = _courses;

            // Загружаем направления
            _directions = context.Directions.ToList();
            cbDirection.ItemsSource = _directions;

            // Загружаем преподавателей
            _teachers = context.Teachers.ToList();
            cbTeacher.ItemsSource = _teachers;

            // Загружаем компании
            _companies = context.Companies.ToList();
            cbCompany.ItemsSource = _companies;
        }

        private void LoadStatuses()
        {
            using var context = new Task4Context(); // Замените на ваш контекст
            _statuses = context.BidStatuses.ToList();
            StatusComboBox.ItemsSource = _statuses;
            StatusComboBox.DisplayMemberPath = "BidStatusName";
            StatusComboBox.SelectedValuePath = "BidStatusId";
        }

        private void LoadBidData()
        {
            txtBidId.Text = "Номер заявки: " + _bid.BidId.ToString();
            txtUserName.Text = "ФИО клиента: " + (_bid.User?.Name ?? "Не указано");
            txtUserEmail.Text = "Email: " + (_bid.User?.Email ?? "Не указано");
            txtBidDate.Text = "Дата заявки: " + (_bid.BidData?.ToString("dd.MM.yyyy") ?? "Не указана");
            txtComment.Text = (_bid.Comment ?? "");
            txtSeats.Text = "Кол-во мест: " + (_bid.Seats?.ToString() ?? "0");
            txtTotalPrice.Text = "Общая стоимость: " + (_bid.TotalPrice?.ToString("N0") + "₽" ?? "0₽");

            // Выбираем текущий курс
            if (_bid.СourseId.HasValue && _courses != null)
            {
                var selectedCourse = _courses.FirstOrDefault(c => c.СourseId == _bid.СourseId.Value);
                if (selectedCourse != null)
                {
                    cbCourse.SelectedItem = selectedCourse;
                    UpdateCourseInfo(selectedCourse);

                    // Устанавливаем направление и преподавателя
                    if (selectedCourse.DirectionId.HasValue)
                    {
                        cbDirection.SelectedItem = _directions?.FirstOrDefault(d => d.DirectionId == selectedCourse.DirectionId.Value);
                    }

                    if (selectedCourse.TeacherId.HasValue)
                    {
                        cbTeacher.SelectedItem = _teachers?.FirstOrDefault(t => t.TeacherId == selectedCourse.TeacherId.Value);
                    }
                }
            }

            // Устанавливаем компанию клиента
            if (_bid.User?.CompanyId.HasValue == true)
            {
                cbCompany.SelectedItem = _companies?.FirstOrDefault(c => c.CompanyId == _bid.User.CompanyId.Value);
            }

            // Устанавливаем статус
            if (_bid.BidStatusId.HasValue)
            {
                StatusComboBox.SelectedValue = _bid.BidStatusId.Value;
            }
        }

        private void StatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StatusComboBox.SelectedValue is int newStatusId)
            {
                using var context = new Task4Context(); // Замените на ваш контекст
                var bidToUpdate = context.Bids.FirstOrDefault(b => b.BidId == _bid.BidId);
                if (bidToUpdate != null)
                {
                    bidToUpdate.BidStatusId = newStatusId;
                    context.SaveChanges();

                    // Обновляем отображение
                    _bid.BidStatusId = newStatusId;
                    var newStatus = _statuses.FirstOrDefault(s => s.BidStatusId == newStatusId);
                    _bid.BidStatus = newStatus;
                }
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using var context = new Task4Context(); // Замените на ваш контекст
                var bid = context.Bids
                    .Include(b => b.User)
                    .First(b => b.BidId == _bid.BidId);

                // Обновляем курс
                if (cbCourse.SelectedItem is Сourse selectedCourse)
                {
                    bid.СourseId = selectedCourse.СourseId;
                }

                // Обновляем статус
                if (StatusComboBox.SelectedValue is int statusId)
                {
                    bid.BidStatusId = statusId;
                }

                // Обновляем пользователя
                if (cbCompany.SelectedItem is Company selectedCompany && bid.User != null)
                {
                    bid.User.CompanyId = selectedCompany.CompanyId;
                }

                // Обновляем количество мест и комментарий
                bid.Seats = int.TryParse(txtSeats.Text.Replace("Кол-во мест: ", ""), out int seats) ? seats : bid.Seats;
                bid.Comment = txtComment.Text;

                // Пересчитываем общую стоимость
                if (bid.Seats.HasValue && bid.Сourse?.Price.HasValue == true)
                {
                    bid.TotalPrice = bid.Seats.Value * bid.Сourse.Price.Value;
                }

                context.SaveChanges();
                MessageBox.Show("Изменения сохранены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Course_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (cbCourse.SelectedItem is Сourse selectedCourse)
            {
                cbDirection.SelectedItem = _directions?.FirstOrDefault(d => d.DirectionId == selectedCourse.DirectionId);
                cbTeacher.SelectedItem = _teachers?.FirstOrDefault(t => t.TeacherId == selectedCourse.TeacherId);
                UpdateCourseInfo(selectedCourse);

                // Обновляем общую стоимость
                if (_bid.Seats.HasValue && selectedCourse.Price.HasValue)
                {
                    _bid.TotalPrice = _bid.Seats.Value * selectedCourse.Price.Value;
                    txtTotalPrice.Text = "Общая стоимость: " + (_bid.TotalPrice?.ToString("N0") + "₽" ?? "0₽");
                }
            }
        }

        private void UpdateCourseInfo(Сourse course)
        {
            txtFreeSeats.Text = (course.Free?.ToString() ?? "0");
            txtPrice.Text = (course.Price?.ToString("N0") + "₽" ?? "0₽");
            txtDuration.Text = (course.Long?.ToString() ?? "0") + " часов";
            txtStartDate.Text = course.StartDate?.ToString("dd.MM.yyyy") ?? "Не указана";
        }

        private void Seats_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Пересчитываем общую стоимость при изменении количества мест
            if (cbCourse.SelectedItem is Сourse selectedCourse &&
                int.TryParse(txtSeats.Text.Replace("Кол-во мест: ", ""), out int seats) &&
                selectedCourse.Price.HasValue)
            {
                _bid.TotalPrice = seats * selectedCourse.Price.Value;
                txtTotalPrice.Text = "Общая стоимость: " + _bid.TotalPrice.Value.ToString("N0") + "₽";
            }
        }
    }
}

