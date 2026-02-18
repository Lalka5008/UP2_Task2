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
using System.Xml.Linq;
using WpfApp2.Data;
using WpfApp2.Models;
using Microsoft.EntityFrameworkCore;

namespace WpfApp2
{
    /// <summary>
    /// Логика взаимодействия для CreateBidWindow.xaml
    /// </summary>
    public partial class CreateBidWindow : Window
    {
        private Сourse _selectedCourse;
        public CreateBidWindow()
        {
            InitializeComponent();
            LoadData();
        }
        private void LoadData()
        {
            using var db = new Task4Context(); // Замените на ваш контекст

            // Загружаем пользователей
            cbClient.ItemsSource = db.Users
                .Include(u => u.Role)
                .Include(u => u.Company)
                .ToList();

            // Загружаем курсы с доступными местами
            cbCourse.ItemsSource = db.Сourses
                .Include(c => c.Direction)
                .Include(c => c.Teacher)
                .Where(c => c.Free > 0)
                .ToList();
        }

        private void CheckSeats()
        {
            if (_selectedCourse == null) return;

            if (!int.TryParse(txtSeats.Text, out int seats) || seats < 1)
            {
                txtError.Text = "Введите число больше 0";
                return;
            }

            if (seats > _selectedCourse.Free)
            {
                txtError.Text = $"Недостаточно мест! Осталось: {_selectedCourse.Free}";
            }
            else
            {
                txtError.Text = "";

                // Обновляем расчет стоимости
                UpdateTotalPrice();
            }
        }

        private void UpdateTotalPrice()
        {
            if (_selectedCourse == null || !_selectedCourse.Price.HasValue) return;

            if (int.TryParse(txtSeats.Text, out int seats) && seats > 0)
            {
                int totalPrice = seats * _selectedCourse.Price.Value;
                txtTotalPrice.Text = $"Общая стоимость: {totalPrice:N0}₽";
            }
        }

        private void Course_Changed(object sender, SelectionChangedEventArgs e)
        {
            _selectedCourse = cbCourse.SelectedItem as Сourse;
            if (_selectedCourse == null) return;

            // Обновляем информацию о курсе
            txtCourseInfo.Text = _selectedCourse.СourseName;
            txtDirection.Text = $"Направление: {_selectedCourse.Direction?.DirectionName}";
            txtDuration.Text = $"Продолжительность: {_selectedCourse.Long} часов";
            txtPrice.Text = $"Цена за место: {_selectedCourse.Price:N0}₽";
            txtTeacherInfo.Text = $"Преподаватель: {_selectedCourse.Teacher?.TeacherType}, " +
                                $"Вместимость: {_selectedCourse.Teacher?.Capacity}";
            txtStartDate.Text = $"Дата начала: {_selectedCourse.StartDate?.ToString("dd.MM.yyyy")}";
            txtFreeSeats.Text = $"Свободно мест: {_selectedCourse.Free}";

            CheckSeats();
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            // Проверка выбора клиента
            if (cbClient.SelectedItem == null)
            {
                txtError.Text = "Выберите клиента";
                return;
            }

            // Проверка выбора курса
            if (cbCourse.SelectedItem == null)
            {
                txtError.Text = "Выберите курс";
                return;
            }

            // Проверка количества мест
            if (!int.TryParse(txtSeats.Text, out int seats) || seats < 1)
            {
                txtError.Text = "Введите корректное количество мест";
                return;
            }

            using var db = new Task4Context(); // Замените на ваш контекст

            // Получаем свежие данные о курсе
            var course = db.Сourses
                .FirstOrDefault(c => c.СourseId == _selectedCourse.СourseId);

            if (course == null)
            {
                txtError.Text = "Курс не найден";
                return;
            }

            // Проверка свободных мест
            if (seats > course.Free)
            {
                txtError.Text = $"Недостаточно мест! Осталось: {course.Free}";
                return;
            }

            try
            {
                // Получаем выбранного пользователя
                var selectedUser = (User)cbClient.SelectedItem;

                // Создание заявки
                var newBid = new Bid
                {
                    UserId = selectedUser.UserId,
                    СourseId = course.СourseId,
                    BidData = DateOnly.FromDateTime(DateTime.Now),
                    BidStatusId = 1, // По умолчанию "Новая"
                    Seats = seats,
                    TotalPrice = seats * (course.Price ?? 0),
                    Comment = txtComment.Text
                };

                db.Bids.Add(newBid);

                // Обновление свободных мест
                course.Free -= seats;

                db.SaveChanges();

                MessageBox.Show("Заявка успешно создана!", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                txtError.Text = $"Ошибка при создании заявки: {ex.Message}";
            }
        }

        private void Seats_Changed(object sender, TextChangedEventArgs e)
        {
            CheckSeats();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

