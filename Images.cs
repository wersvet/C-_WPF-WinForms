/*
namespace Snake
{
    public static class Images
    {
        public readonly static ImageSource Empty = LoadImage("Empty.png"); // Статические переменные для каждого из ресурсов изображение
        public readonly static ImageSource Body = LoadImage("Body.png");
        public readonly static ImageSource Head = LoadImage("Head.png");
       // public readonly static ImageSource Food = LoadImage("Food.png");
        public readonly static ImageSource food1 = LoadImage("food1.jpg");
        public readonly static ImageSource food2rightup = LoadImage("food2rightup.jpg");
        public readonly static ImageSource food3leftdown = LoadImage("food3leftdown.jpg");
        public readonly static ImageSource food4rightdown = LoadImage("food4rightdown.jpg");
        public readonly static ImageSource DeadBody = LoadImage("DeadBody.png");
        public readonly static ImageSource DeadHead = LoadImage("DeadHead.png");

        private static ImageSource LoadImage(string fileName) // Загружает фото с заданный именем файла 
        {
            return new BitmapImage(new Uri($"Assets/{fileName}", UriKind.Relative)); // возвращяет его в качестве источника изображение

        }
    }
}

*/
using System;
using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Snake
{
    public static class Images
    {
        public readonly static ImageSource Empty = LoadImage("Empty.png"); // Статические переменные для каждого из ресурсов изображение
        public readonly static ImageSource Body = LoadImage("Body.png");
        public readonly static ImageSource Head = LoadImage("Head.png");
        public readonly static ImageSource Food = LoadImage("Food.png");
        public readonly static ImageSource DeadBody = LoadImage("DeadBody.png");
        public readonly static ImageSource DeadHead = LoadImage("DeadHead.png");

        private static ImageSource LoadImage(string fileName) // Загружает фото с заданный именем файла 
        {
            return new BitmapImage(new Uri($"Assets/{fileName}", UriKind.Relative)); // возвращяет его в качестве источника изображение

        }
    }
}