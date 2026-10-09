//using System.Diagnostics.CodeAnalysis;
//using System.Reflection.Metadata;

using System.Globalization;

namespace Backend;

public static class ExpressionEvaluator
{
    public static double Evalute(string infix) => EvalutePostfix(ToPostfix(Tokenize(infix)));

    // Splits "12+3.5" into ["12", "+", "3.5"]
    private static List<string> Tokenize(string infix)
    {
        var tokens = new List<string>();
        var number = string.Empty;

        foreach (var item in infix)
        {
            if (char.IsDigit(item) || item == '.')
            {
                number += item;
            }
            else
            {
                if (number != string.Empty)
                {
                    tokens.Add(number);
                    number = string.Empty;
                }

                if (item != ' ')
                {
                    tokens.Add(item.ToString());
                }
            }
        }

        if (number != string.Empty)
        {
            tokens.Add(number);
        }

        return tokens;
    }

    private static List<string> ToPostfix(List<string> tokens)
    {
        var postfix = new List<string>();
        var stack = new Stack<string>();

        foreach (var token in tokens)
        {
            if (!IsOperator(token))
            {
                postfix.Add(token);
            }
            else if (token == ")")
            {
                while (stack.Count > 0 && stack.Peek() != "(")
                {
                    postfix.Add(stack.Pop());
                }

                if (stack.Count == 0)
                {
                    throw new Exception("Invalid expression.");
                }

                stack.Pop();
            }
            else
            {
                while (stack.Count > 0 && PriorityInfix(token) <= PriorityStack(stack.Peek()))
                {
                    postfix.Add(stack.Pop());
                }

                stack.Push(token);
            }
        }

        while (stack.Count > 0)
        {
            var op = stack.Pop();
            if (op == "(")
            {
                throw new Exception("Invalid expression.");
            }
            postfix.Add(op);
        }

        return postfix;
    }

    private static int PriorityStack(string op) => op switch
    {
        "^" => 3,
        "*" => 2,
        "/" => 2,
        "+" => 1,
        "-" => 1,
        "(" => 0,
        _ => throw new Exception("Invalid expression."),
    };

    private static int PriorityInfix(string op) => op switch
    {
        "^" => 4,
        "*" => 2,
        "/" => 2,
        "+" => 1,
        "-" => 1,
        "(" => 5,
        _ => throw new Exception("Invalid expression."),
    };

    private static bool IsOperator(string token) =>
        token is "^" or "*" or "/" or "+" or "-" or "(" or ")";

    private static double EvalutePostfix(List<string> postfix)
    {
        var stack = new Stack<double>();

        foreach (var token in postfix)
        {
            if (IsOperator(token))
            {
                var ope2 = stack.Pop();
                var ope1 = stack.Pop();
                stack.Push(Calculate(ope1, ope2, token));
            }
            else
            {
                stack.Push(double.Parse(token, CultureInfo.InvariantCulture));
            }
        }

        return stack.Pop();
    }

    private static double Calculate(double ope1, double ope2, string item) => item switch
    {
        "*" => ope1 * ope2,
        "/" => ope1 / ope2,
        "+" => ope1 + ope2,
        "-" => ope1 - ope2,
        "^" => Math.Pow(ope1, ope2),
        _ => throw new Exception("Invalid expression."),
    };
}