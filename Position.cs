using System;
using System.Collections.Generic;

namespace Snake
{
    public class Position // (Позиция вещей в сетке)
    {
        public int Row { get; } // Добовляем конструктор принимающий строку и стоблцы 
        public int Col { get; }

        public Position(int row, int col) // (Row, Col) как параметр
        {
            Row = row;
            Col = col;
        }

        public Position Translate(Direction dir) // возвращает позицию, когда перемещяемся на один шаг
        {
            return new Position(Row + dir.RowOffset, Col + dir.ColOffset); // Row/Col + смещение строки/столбца
        }


        // Создать операцию - Перезапись Equals и хэш кода Чтобы Класс (Position) 
        // Позиции можно было использовать == Ctrl + "." (dot)
        public override bool Equals(object obj)
        {
            return obj is Position position &&
                   Row == position.Row &&
                   Col == position.Col;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Row, Col);
        }

        public static bool operator ==(Position left, Position right)
        {
            return EqualityComparer<Position>.Default.Equals(left, right);
        }

        public static bool operator !=(Position left, Position right)
        {
            return !(left == right);
        }
    }
}