using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Ink;

namespace Snake
{
    public class GameState
    {
        public int Rows { get; }
        public int Cols { get; }
        public GridValue[,] Grid { get; }
        public Direction Dir { get; private set; }
        public int Score { get; private set; }
        public bool GameOver { get; private set; }

        private readonly LinkedList<Direction> dirChanges = new LinkedList<Direction>();
        private readonly LinkedList<Position> snakePositions = new LinkedList<Position>();
        private readonly Random random = new Random();


        public GameState(int rows, int cols) // Конструктор который принимает кол-во строк и столбцов в параметр
        {
            Rows = rows;
            Cols = cols;
            Grid = new GridValue[rows, cols];
            Dir = Direction.Right; //Начальное направление

            AddSnake(); // Вызов Появление змейки
            AddFood(); // Вызов Еды в рандомные позиции
        }

        private void AddSnake() // Первоначальное появление змейки
        {
            int r = Rows / 2;
            for (int c = 1; c <= 3; c++)
            {
                Grid[r, c] = GridValue.Snake;  // Установка места спауна
                snakePositions.AddFirst(new Position(r, c));

            }
        }


        private IEnumerable<Position> EmptyPositions() // Возвращает все пустые позиции сетки
        {
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Cols; c++)
                {
                    if (Grid[r, c] == GridValue.Empty) // Если это клетка пустая
                    {
                        yield return new Position(r, c); // То мы возвращяем эту позицию
                    }
                }
            }
        }

        private void AddFood() // Метод добавление еды
        {
            List<Position> empty = new List<Position>(EmptyPositions()); // Список пустых позиции

            if (empty.Count == 0) // Если ты выйграл(пустых мест нету)
            {
                return;
            }
            // for (int i = 0; i <= 3; i++)
            // {
            // RenderTransformOrigin = new Point(0.5, 0.5)
            Position pos = empty[random.Next(empty.Count)];
            // Position pos2 = 
            //  Grid[pos.Row + 1, pos.Col] = GridValue.Food;
            //  Grid[pos.Row + 1, pos.Col + 1] = GridValue.Food;
            //  Grid[pos.Row, pos.Col + 1] = GridValue.Food;
            Grid[pos.Row, pos.Col] = GridValue.Food;
            // }
        }

        public Position HeadPosition() // Проверка позиции головы змейки
        {
            return snakePositions.First.Value; // возвращяем Первую позицию змеи
        }

        public Position TailPosition() // Проверка позиции хвоста змейки
        {
            return snakePositions.Last.Value; // возвращяем Последнюю позицию змеи
        }

        public IEnumerable<Position> SnakePositions() // Возвращяет позицию всей змеи
        {
            return snakePositions;
        }

        private void AddHead(Position pos) // При смерти змейки добовляем метрвую голову.
        {
            snakePositions.AddFirst(pos); // Заменяет Первую голову на мертвую
            Grid[pos.Row, pos.Col] = GridValue.Snake;
        }

        private void RemoveTail() // Для удаление Хвоста
        {
            Position tail = snakePositions.Last.Value; // Получаем текущюю позицию хвоста
            Grid[tail.Row, tail.Col] = GridValue.Empty; // Делаем эту позицию пустой
            snakePositions.RemoveLast(); // Удаляем ее из списка
        }

        private Direction GetLastDirection() // Возвращяет последнее предопределенное направление змеи
        {
            if (dirChanges.Count == 0) // Если сетка(буфер) пустая 
            {
                return Dir; // Врзвращяет текущее направление
            }

            return dirChanges.Last.Value; // В другом случае он возвращяет последнее направление(если идешь вверх, то вниз нельзя || если идешь на право, на лево нельзя )
        }

        private bool CanChangeDirection(Direction newDir) // Проверка можем ли мы двигаться в следующем шаге 
        {
            if (dirChanges.Count == 2) // Если в буфере сохранены два изменение направления
            {
                return false; // буфер заполнен, и вовращяем Ложь
            }

            // Если в буфере есть место
            Direction lastDir = GetLastDirection(); // Получаем последнее направление 
            return newDir != lastDir && newDir != lastDir.Opposite(); // True = если последнее и нове направление не одинаковы, и если они не Противоположны друг другу
        }

        public void ChangeDirection(Direction dir) // Изменение направление змейки
        {
            if (CanChangeDirection(dir)) // Если ты можешь менять направление 
            {
                dirChanges.AddLast(dir); // Меняем его  
            }
        }

        private bool OutsideGrid(Position pos) // Проверка Выхода змеи за сетку 
        {
            return pos.Row < 0 || pos.Row >= Rows || pos.Col < 0 || pos.Col >= Cols;
        }

        private GridValue WillHit(Position newHeadpos) // Проверка на самоурон
        {
            if (OutsideGrid(newHeadpos))  // если новая позиция змеи будет за пределами сетки
            {
                return GridValue.Outside; // 
            }

            // Можно не делать
            if (newHeadpos == TailPosition()) // Если ты чуть не ударил хвост (Голова = Хвост)
            {
                return GridValue.Empty; // Квадрт пустой, игра идет дальше 
            }

            return Grid[newHeadpos.Row, newHeadpos.Col]; // Обычный случай вернет то что хранится в сетке 
        }


        public void Move() // Движение змейки на один щаг
        {
            if (dirChanges.Count > 0) // Проверка есть ли изменение направление в буфере
            {
                Dir = dirChanges.First.Value; // Меняем следующее направление 
                dirChanges.RemoveFirst(); // И Удалим это изменение из буфера
            }

            Position newHeadpos = HeadPosition().Translate(Dir); // Получаем новую позицию головы
            GridValue hit = WillHit(newHeadpos); // Проверка что голова ударится с помощью WillHit

            if (hit == GridValue.Outside || hit == GridValue.Snake) // Если поподание - точка снаружи или тело змейки(смерть) 
            {
                GameOver = true; // Смерть
            }

            else if (hit == GridValue.Empty) // Если змейка идет в пустую точку 
            {
                RemoveTail(); // Удаляем хвост
                AddHead(newHeadpos); // Добовляем голову 

            }

            else if (hit == GridValue.Food) // Если змейка идет в точку с едой 
            {
                AddHead(newHeadpos); // не удаляем хвост, +новая позиция головы 
                Score++; // +1 к счету 
                AddFood(); // Вызов метода с едой
            }


        }
    }
}