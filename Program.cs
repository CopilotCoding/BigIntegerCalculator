using System;
using System.Numerics;

namespace Calculator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the calculator!");

            while (true)
            {
                string equation = GetEquationFromUser();
                BigInteger result = EvaluateEquation(equation);

                Console.WriteLine("Result: " + result);

                Console.WriteLine("Do you want to continue? (y/n)");
                string answer = Console.ReadLine();

                if (answer.ToLower() != "y")
                {
                    break;
                }
            }

            Console.WriteLine("Goodbye!");
        }

        static string GetEquationFromUser()
        {
            Console.WriteLine("Enter an equation:");
            return Console.ReadLine();
        }

        static BigInteger EvaluateEquation(string equation)
        {
            EquationParser parser = new EquationParser(equation);
            return parser.Parse();
        }

        class EquationParser
        {
            private readonly string equation;
            private int index;

            public EquationParser(string equation)
            {
                this.equation = equation;
                this.index = 0;
            }

            public BigInteger Parse()
            {
                BigInteger result = ParseTerm();

                while (index < equation.Length && "+-".Contains(equation[index]))
                {
                    char operation = equation[index];
                    index++;

                    BigInteger number = ParseTerm();

                    result = PerformOperation(result, operation.ToString(), number);
                }

                return result;
            }

            private BigInteger ParseTerm()
            {
                BigInteger result = ParseFactor();

                while (index < equation.Length && "*/%^".Contains(equation[index]))
                {
                    char operation = equation[index];
                    index++;

                    BigInteger number = ParseFactor();

                    result = PerformOperation(result, operation.ToString(), number);
                }

                return result;
            }

            private BigInteger ParseFactor()
            {
                if (char.IsDigit(equation[index]))
                {
                    string number = "";

                    while (index < equation.Length && char.IsDigit(equation[index]))
                    {
                        number += equation[index];
                        index++;
                    }

                    return BigInteger.Parse(number);
                }
                else if (equation[index] == '(')
                {
                    index++;

                    BigInteger result = Parse();

                    if (equation[index] != ')')
                    {
                        throw new ArgumentException("Invalid equation: " + equation);
                    }

                    index++;

                    return result;
                }
                else if (equation[index] == '-')
                {
                    index++;

                    BigInteger number = ParseFactor();

                    return -number;
                }
                else
                {
                    throw new ArgumentException("Invalid equation: " + equation);
                }
            }
        }

        static BigInteger PerformOperation(BigInteger number1, string operation, BigInteger number2)
        {
            if (operation == "+")
            {
                return number1 + number2;
            }
            else if (operation == "-")
            {
                return number1 - number2;
            }
            else if (operation == "*")
            {
                return number1 * number2;
            }
            else if (operation == "/")
            {
                return number1 / number2;
            }
            else if (operation == "%")
            {
                return number1 % number2;
            }
            else if (operation == "^")
            {
                return BigInteger.Pow(number1, (int)number2);
            }
            else
            {
                throw new ArgumentException("Invalid operation: " + operation);
            }
        }
    }
}
