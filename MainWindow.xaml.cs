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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Snake
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Dictionary<GridValue, ImageSource> gridValToImage = new() // Словарь - Значение сетки=Изображение
        {
            { GridValue.Empty, Images.Empty},
            { GridValue.Snake, Images.Body},
            {GridValue.Food, Images.Food }
        };

        private readonly Dictionary<Direction, int> dirToRotaion = new() // Словарь для вращение картинки головы змейки
        {
            {Direction.Up, 0},
            {Direction.Right, 90},
            {Direction.Down, 180},
            {Direction.Left, 270}
        };

        private readonly int rows = 15, cols = 15; // Переменные для строк и стоблцов 
        private readonly Image[,] gridImages; // Массив 2D-изображении для управление
        private GameState gameState; // Обьект Состояние Игры
        private bool gameRunning; // Булевая переменная 

        public MainWindow()
        {
            InitializeComponent();
            gridImages = SetupGrid(); // Вызов метода для возвращение массива и изображение сетки
            gameState = new GameState(rows, cols); // Вызов метода Состояние игры
        }

        private async Task RunGame()
        {
            Draw(); // Вызов отрисовки сетки 
            await ShowCountDown(); // Вызываем обратный отчет до начала игры
            OverlayText.Visibility = Visibility.Hidden; // Скрывает Оверлей 
            await GameLoop(); // Вызов Игрового цикла (как она будет работать)
            await ShowGameOver(); // После Смерти змейки, вызываем обратно игру
            gameState = new GameState(rows, cols); // новое игровое состояние
        }

        private async void Windows_PreviewKeyDown(object sender, KeyEventArgs e) // Событие предварительного просмотра нажатие кнопки
        {
            if (OverlayText.Visibility == Visibility.Visible) // Если видно наложение 
            {
                e.Handled = true; // предотвращяет нажания окна(клавиши)
            }

            if (!gameRunning) // если игра еще не запущена 
            {
                gameRunning = true; // Вызываем игру 
                await RunGame(); // Ждем запуск игры
                gameRunning = false; // Обратно в false
            }
        }

        private void Windows_KeyDown(object sender, KeyEventArgs e) // Управление игрой 
        {
            if (gameState.GameOver) // Если Игра закончилась
            {
                return;
            }
            switch (e.Key)
            {
                case Key.D:                                     // Если пользователь вводит эти кнопки
                case Key.Right:                                 // То
                    gameState.ChangeDirection(Direction.Right); // Меняем следующее направление на право
                    break;
                case Key.A:
                case Key.Left:
                    gameState.ChangeDirection(Direction.Left);
                    break;
                case Key.W:
                case Key.Up:
                    gameState.ChangeDirection(Direction.Up);
                    break;
                case Key.S:
                case Key.Down:
                    gameState.ChangeDirection(Direction.Down);
                    break;
            }
        }


        private async Task GameLoop() // Игровой цикл (как она будет работать)
        {
            while (!gameState.GameOver) // Работает пока игра не закончится 
            {
                await Task.Delay(80); // Скорость змейки (Задержка на 0,08 сек)
                gameState.Move(); // Вызов Метод перемещение 
                Draw(); // Рисуем новое состояние игры
            }
        }



        private Image[,] SetupGrid() // Добавляет элементы управление изображения в игровую сетку и вернет их в 2Д массив
        {
            Image[,] images = new Image[rows, cols]; // Создание 2Д массива
            GameGrid.Rows = rows; // Устанавливаем кол-во строк и столбцов
            GameGrid.Columns = cols;

            for (int r = 0; r < rows; r++) // Перебираем все позиции
            {
                for (int c = 0; c < cols; c++)
                {
                    Image image = new Image // Для каждого создаем новое изображение
                    {

                        Source = Images.Empty, // Его источник - пустой ресурс изображение
                        RenderTransformOrigin = new Point(0.5, 0.5) // начало преоброзавание рендеринга 0.5
                    };

                    images[r, c] = image; // Сохраняем это изображение в массиве
                    GameGrid.Children.Add(image); //добавляем его как элемент игровой сетки
                }
            }

            return images; // Возвращяем массив изображении
        }






        private void Draw() // Вызов отрисовки сетки 
        {
            DrawGrid(); // Вызов Обновление сетки
            DrawsnakeHead(); // Вызов Головы Змейки 
            ScoreText.Text = $"SCORE {gameState.Score}"; // Устанавливаем текст Счетчика
        }

        private void DrawGrid() // Обновляеть изображение каждой сетки в массиве
        {
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    GridValue gridVal = gameState.Grid[r, c]; // Получаем значение сетки в текущий момент
                    gridImages[r, c].Source = gridValToImage[gridVal]; // Установка источника для изображение с помощью словаря
                    gridImages[r, c].RenderTransform = Transform.Identity;
                }
            }
        }

        private void DrawsnakeHead() // Рисовка Головы Змеи
        {
            Position headpos = gameState.HeadPosition(); // Получаем положение головы змеи
            Image image = gridImages[headpos.Row, headpos.Col]; // Получаем Изображение для этой позиции
            image.Source = Images.Head; // Устанавливаем его источник

            int rotation = dirToRotaion[gameState.Dir];  // Получаем кол-во градусов головы
            image.RenderTransform = new RotateTransform(rotation); // Поворачиваем голову 
        }

        private async Task DrawDeadSnake() // Рисовка Смерти Змеи
        {
            List<Position> positions = new List<Position>(gameState.SnakePositions()); // Создание списка со всеми позициями змеи

            for (int i = 0; i < positions.Count; i++) // От хвоста до головы
            {
                Position pos = positions[i];
                ImageSource source = (i == 0) ? Images.DeadHead : Images.DeadBody; // Определение изображение для этой позиции
                gridImages[pos.Row, pos.Col].Source = source; // Установка изображение в текущую позицию
                await Task.Delay(50); // скорость Замены тела на мертвое
            }
        }

        private async Task ShowCountDown() // Обратный отчет перед запуском игры
        {
            for (int i = 3; i >= 1; i--)
            {
                OverlayText.Text = i.ToString();  // Пишем с задержкой в 0,5 сек
                await Task.Delay(500);
            }
        }

        private async Task ShowGameOver() // 
        {
            await DrawDeadSnake();
            await Task.Delay(1000); // начинается через 1 секунду
            OverlayText.Visibility = Visibility.Visible; // делаем оверлей опять видимым
            OverlayText.Text = "НАЖМИТЕ ЛЮБУЮ КНОПКУ ДЛЯ СТАРТА ИГРЫ"; // Выводим текст
        }

    }
}