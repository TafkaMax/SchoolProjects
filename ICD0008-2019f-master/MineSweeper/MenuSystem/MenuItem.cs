using System;

namespace MenuSystem
{
    public class MenuItem
    {
        private string _title = default!;

        public string Title
        {
            get => _title;
            set => _title = Validate(value, 1, 100, false);
        }


        // reference to an method, which returns a string and takes no parameters
        // func<...parameter types..., return type>
        public Func<string, string> CommandToExecute { get; set; } = default!;
        
        private static string Validate(string item, int minLength, int maxLength, bool toUpper)
        {
            item = item.Trim();
            if (toUpper)
            {
                item = item.ToUpper();
            }
            if (item.Length < minLength  || item.Length > maxLength)
            {
                throw new ArgumentException(
                    $"String is not correct length (" +
                    $"{minLength}-{maxLength})! Got " +
                    $"{item.Length} characters.");
            }

            return item;
        }

        public override string ToString()
        {
            return Title;
        }
    }
}