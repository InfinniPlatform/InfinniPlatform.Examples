using System;

namespace Infinni.Northwind.Queues
{
    /// <summary>
    /// Пример тела сообщения.
    /// </summary>
    public class ExampleMessage
    {
        public ExampleMessage(int someInt, string someText, DateTime someDateTime)
        {
            SomeInt = someInt;
            SomeText = someText;
            SomeDateTime = someDateTime;
        }

        public int SomeInt { get; set; }

        public string SomeText { get; set; }

        public DateTime SomeDateTime { get; set; }

        //Для красивого вывода в консоль.
        public override string ToString()
        {
            return $"{SomeInt}, {SomeText}, {SomeDateTime:s}";
        }
    }
}