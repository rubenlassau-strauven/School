using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringCalculatorKata
{
    public class StringCalculator
    {
        public static int Add(string numbers)
        {
            string[] delimiters = new string[] { ",", "\n" };
            if (numbers.StartsWith("//"))
            {
                delimiters = new string[] {numbers[2].ToString()};
                numbers = numbers.Substring(4);
            }

            if (String.IsNullOrEmpty(numbers))
                return 0;
            List<String> numbersList = numbers.Split(delimiters, StringSplitOptions.None).ToList();
            int parsedNumber = 0;
            foreach (string number in numbersList)
            {
                if (!int.TryParse(number,out parsedNumber))   
                    throw new ArgumentException("Invalid number "+number);
            }            
            int totaal = 0;
            if (numbersList.Count == 1)
                return Convert.ToInt32(numbers);
            numbersList.ForEach(n => totaal += Convert.ToInt32(n));
            return totaal;
        }
    }
}
