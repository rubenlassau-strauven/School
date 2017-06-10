using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StringCalculatorKata
{
    public class StringCalculator
    {
        List<string> numbersList;
        public int Add(string numbers)
        {
            if (!numbers.Equals(String.Empty))
            {
                
                if (numbers.StartsWith("//"))
                {
                    ExtractNumbers(RemoveDelimiterDeclaration(numbers), new string[] { GetDelimiter(numbers) });
                    return Calculation();
                }
                else if (numbers.Contains(',') || numbers.Contains('\n'))
                {
                    ExtractNumbers(numbers, new string[] { ",", "\n" });
                    return Calculation();
                }
                else
                {
                    numbersList = new List<string>() { numbers };
                    return CheckAndConvert(numbers);
                }
            }
            return 0;
        }

        private string RemoveDelimiterDeclaration(string numbers)
        {
            return numbers.Split('\n')[1];
        }

        private string GetDelimiter(string numbers)
        {      
            return numbers[2].ToString();
        }

        private int Calculation()
        {
            int result = 0;
            foreach (string number in numbersList)
            {
                result += CheckAndConvert(number);
            }
            return result;
        }

        private void ExtractNumbers(string numbers, string[] delimiters)
        {
            numbersList = numbers.Split(delimiters, StringSplitOptions.None).ToList();
        }

        private int CheckAndConvert(string number)
        {
            try
            {
                int parsedNumber = Convert.ToInt32(number);
                if (parsedNumber < 0)
                    throw new ArgumentException($"Negatives not allowed: {FindAllNegatives()}");
                return parsedNumber;

            } catch (FormatException)
            {
                throw new ArgumentException($"Invalid number {ToErrorString(number)}");
            }
        }

        private string FindAllNegatives()
        {
            StringBuilder output = new StringBuilder();
            foreach (string number in numbersList)
            {
                if (number.StartsWith("-"))
                    output.Append(number+" ");
            }
            return output.ToString().Trim();

        }

        private string ToErrorString(string number)
        {
            number = Regex.Replace(number, "\n", "");
            return number;
        }
    }
}
