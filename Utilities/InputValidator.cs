using System;
using System.Collections.Generic;
using System.Text;


using System;
using System.Linq;

namespace CourierTrackingSystem.Utilities;

public class InputValidator
{
    public static string ReadRequired(string message)
    {
        while (true)
        {
            Console.Write(message);
            string input = Console.ReadLine() ?? "";

            if (!string.IsNullOrWhiteSpace(input))
                return input.Trim();

            Console.WriteLine("Input cannot be empty!");
        }
    }

    public static string ReadPhoneNumber(string message)
    {
        while (true)
        {
            string phone = ReadRequired(message);

            if (phone.Length == 10 && phone.All(char.IsDigit))
                return phone;

            Console.WriteLine("Enter a valid 10-digit phone number!");
        }
    }

    public static int ReadMenuChoice()
    {
        Console.Write("Enter choice: ");
        string input = Console.ReadLine() ?? "";

        if (int.TryParse(input, out int choice))
            return choice;

        return -1;
    }
}

