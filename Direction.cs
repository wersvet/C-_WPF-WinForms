using System;
using System.Collections.Generic;

namespace Snake
{
    public class Direction // (Направление змейки)
    {
        // Создаем 4 направления для змейки: (Строка, Стоблец)
        public readonly static Direction Left = new Direction(0, -1);
        public readonly static Direction Right = new Direction(0, 1);
        public readonly static Direction Up = new Direction(-1, 0);
        public readonly static Direction Down = new Direction(1, 0);

        public int RowOffset { get; } // Создаем два смещение сетки:
        public int ColOffset { get; } //(Строки - Row (horizontal ), столбцов - Colums (vertical)) 

        private Direction(int rowOffset, int colOffset)  // В функции устанавливаем их.
        {
            RowOffset = rowOffset;
            ColOffset = colOffset;
        }

        public Direction Opposite() // Для проерки направлении в противоположную часть
        {
            return new Direction(-RowOffset, -ColOffset);
        }


        // Создать операцию -Перезапись equals и хэш кода Чтобы Класс (Direction) 
        // направление можно было использовать == Ctrl + "." (dot)

        public override bool Equals(object obj)
        {
            return obj is Direction direction &&
                   RowOffset == direction.RowOffset &&
                   ColOffset == direction.ColOffset;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(RowOffset, ColOffset);
        }

        public static bool operator ==(Direction left, Direction right)
        {
            return EqualityComparer<Direction>.Default.Equals(left, right);
        }

        public static bool operator !=(Direction left, Direction right)
        {
            return !(left == right);
        }
    }
}