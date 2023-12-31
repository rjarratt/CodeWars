﻿namespace Evaluation;

using System;
using System.Collections.Generic;
using System.Text;

public class Evaluate
{
    private const int plusMinusPrecedence = 0;
    private const int multiplyDividePrecedence = 1;
    private const int powerPrecedence = 3;
    private const int unaryPrecedence = 2;

    public string eval(string expression)
    {

        string result;
        try
        {
            IEnumerator<Token> tokenEnumerator = Tokenizer.ReadTokens(expression).GetEnumerator();
            bool moreTokens = true;
            double expressionValue = EvaluateExpression(tokenEnumerator, 0, ref moreTokens);
            if (moreTokens || double.IsInfinity(expressionValue) || double.IsNaN(expressionValue))
            {
                result = "ERROR";
            }
            else
            {
                result = expressionValue.ToString();
            }
        }
        catch
        {
            result = "ERROR";
        }

        return result;
    }

    private static double EvaluateExpression(IEnumerator<Token> tokenEnumerator, int precedenceLevel, ref bool moreTokens)
    {
        double result = 0;
        int nextPrecedence = -1;
        moreTokens = tokenEnumerator.MoveNext();
        bool endOfExpression = false;
        bool isUnary = true;
        while (moreTokens && !endOfExpression)
        {
            Token? currentToken = tokenEnumerator.Current;

            switch (currentToken.TokenType)
            {
                case TokenType.Number:
                    result = double.Parse(currentToken.Symbol);
                    moreTokens = tokenEnumerator.MoveNext();
                    break;

                case TokenType.PlusOperator:
                    nextPrecedence = isUnary ? unaryPrecedence : plusMinusPrecedence;
                    if (precedenceLevel <= nextPrecedence)
                    {
                        result += EvaluateExpression(tokenEnumerator, nextPrecedence, ref moreTokens);
                    }

                    break;

                case TokenType.MinusOperator:
                    nextPrecedence = isUnary? unaryPrecedence : plusMinusPrecedence;
                    if (precedenceLevel <= nextPrecedence)
                    {
                        result -= EvaluateExpression(tokenEnumerator, nextPrecedence, ref moreTokens);
                    }

                    break;

                case TokenType.MultiplyOperator:
                    nextPrecedence = multiplyDividePrecedence;
                    if (precedenceLevel <= nextPrecedence)
                    {
                        result *= EvaluateExpression(tokenEnumerator, nextPrecedence, ref moreTokens);
                    }

                    break;

                case TokenType.DivideOperator:
                    nextPrecedence = multiplyDividePrecedence;
                    if (precedenceLevel <= nextPrecedence)
                    {
                        result /= EvaluateExpression(tokenEnumerator, nextPrecedence, ref moreTokens);
                    }

                    break;

                case TokenType.PowerOperator:
                    nextPrecedence = powerPrecedence;
                    if (precedenceLevel <= nextPrecedence)
                    {
                        result = Math.Pow(result, EvaluateExpression(tokenEnumerator, nextPrecedence, ref moreTokens));
                        nextPrecedence = precedenceLevel;
                    }

                    break;

                case TokenType.LeftParenthesis:
                    result = EvaluateExpression(tokenEnumerator, plusMinusPrecedence, ref moreTokens);
                    if (tokenEnumerator.Current.TokenType != TokenType.RightParenthesis)
                    {
                        throw new InvalidOperationException("Mismatched parentheses");
                    }

                    moreTokens = tokenEnumerator.MoveNext();
                    break;

                case TokenType.RightParenthesis:
                    endOfExpression = true;
                    break;

                case TokenType.Identifier:
                    string function = currentToken.Symbol;
                    if (!tokenEnumerator.MoveNext())
                    {
                        throw new InvalidOperationException("Missing argument");
                    }

                    double argument = EvaluateExpression(tokenEnumerator, plusMinusPrecedence, ref moreTokens);
                    if (tokenEnumerator.Current.TokenType != TokenType.RightParenthesis)
                    {
                        throw new InvalidOperationException("Closing parenthesis on function call is missing");
                    }

                    moreTokens = tokenEnumerator.MoveNext();

                    switch (function)
                    {
                        case "log":
                            result = Math.Log10(argument);
                            break;

                        case "ln":
                            result = Math.Log(argument);
                            break;

                        case "exp":
                            result = Math.Exp(argument);
                            break;

                        case "sqrt":
                            result = Math.Sqrt(argument);
                            break;

                        case "abs":
                            result = Math.Abs(argument);
                            break;

                        case "atan":
                            result = Math.Atan(argument);
                            break;

                        case "acos":
                            result = Math.Acos(argument);
                            break;

                        case "asin":
                            result = Math.Asin(argument);
                            break;

                        case "sinh":
                            result = Math.Sinh(argument);
                            break;

                        case "cosh":
                            result = Math.Cosh(argument);
                            break;

                        case "tanh":
                            result = Math.Tanh(argument);
                            break;

                        case "tan":
                            result = Math.Tan(argument);
                            break;

                        case "sin":
                            result = Math.Sin(argument);
                            break;

                        case "cos":
                            result = Math.Cos(argument);
                            break;

                        default:
                            throw new InvalidOperationException("Unknown function");
                    }

                    break;

                default:
                    throw new InvalidOperationException("Unsupported token");
            }

            if (nextPrecedence >=0 && precedenceLevel > nextPrecedence)
            {
                endOfExpression = true;
            }

            isUnary = false;
        }

        return result;
    }

    private static class Tokenizer
    {
        public static IEnumerable<Token> ReadTokens(string expression)
        {
            IEnumerator<char> characterEnumerator = expression.ToLower().GetEnumerator();

            while (characterEnumerator.MoveNext())
            {
                bool moreCharacters = true;
                char currentChar = characterEnumerator.Current;
                if (char.IsDigit(currentChar))
                {
                    Token token = ParseNumber(characterEnumerator, out moreCharacters, ref currentChar);
                    yield return token;
                }
                else if (char.IsLetter(currentChar))
                {
                    Token token = ParseIdentifier(characterEnumerator, out moreCharacters, ref currentChar);
                    yield return token;
                }

                if (moreCharacters)
                {
                    Token? token = null;
                    switch (currentChar)
                    {
                        case '+':
                            {
                                token = new Token(TokenType.PlusOperator, string.Empty);
                                break;
                            }
                        case '-':
                            {
                                token = new Token(TokenType.MinusOperator, string.Empty);
                                break;
                            }
                        case '*':
                            {
                                token = new Token(TokenType.MultiplyOperator, string.Empty);
                                break;
                            }
                        case '/':
                            {
                                token = new Token(TokenType.DivideOperator, string.Empty);
                                break;
                            }
                        case '&':
                            {
                                token = new Token(TokenType.PowerOperator, string.Empty);
                                break;
                            }
                        case '(':
                            {
                                token = new Token(TokenType.LeftParenthesis, string.Empty);
                                break;
                            }
                        case ')':
                            {
                                token = new Token(TokenType.RightParenthesis, string.Empty);
                                break;
                            }
                    }

                    if (token is not null)
                    {
                        yield return token;
                    }
                }
            }
        }

        private static Token ParseNumber(IEnumerator<char> characterEnumerator, out bool moreCharacters, ref char currentChar)
        {
            StringBuilder currentSymbol = new StringBuilder();
            currentSymbol.Append(currentChar);
            while (moreCharacters = characterEnumerator.MoveNext())
            {
                currentChar = characterEnumerator.Current;
                if (char.IsDigit(currentChar) || currentChar == '.')
                {
                    currentSymbol.Append(currentChar);
                }
                else if (currentChar == 'e')
                {
                    currentSymbol.Append(currentChar);
                    if (moreCharacters = characterEnumerator.MoveNext())
                    {
                        currentChar = characterEnumerator.Current;
                        if (currentChar == '+' || currentChar == '-')
                        {
                            currentSymbol.Append(currentChar);
                            if (moreCharacters = characterEnumerator.MoveNext())
                            {
                                currentChar = characterEnumerator.Current;
                                if (!char.IsDigit(currentChar))
                                {
                                    throw new InvalidOperationException("Invalid exponent");
                                }

                                currentSymbol.Append(currentChar);
                            }
                        }
                        else if (char.IsDigit(currentChar))
                        {
                            currentSymbol.Append(currentChar);
                        }
                        else
                        {
                            throw new InvalidOperationException("Missing exponent");
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException("Missing exponent");
                    }
                }
                else
                {
                    break;
                }
            }

            return new Token(TokenType.Number, currentSymbol.ToString());
        }

        private static Token ParseIdentifier(IEnumerator<char> characterEnumerator, out bool moreCharacters, ref char currentChar)
        {
            StringBuilder currentSymbol = new StringBuilder();
            currentSymbol.Append(currentChar);
            while (moreCharacters = characterEnumerator.MoveNext())
            {
                currentChar = characterEnumerator.Current;
                if (char.IsLetterOrDigit(currentChar))
                {
                    currentSymbol.Append(currentChar);
                }
                else
                {
                    break;
                }
            }

            return new Token(TokenType.Identifier, currentSymbol.ToString());
        }
    }
    private enum TokenType
    {
        Identifier,
        Number,
        PlusOperator,
        MinusOperator,
        MultiplyOperator,
        DivideOperator,
        PowerOperator,
        LeftParenthesis,
        RightParenthesis,
    }

    private record Token(TokenType TokenType, string Symbol);

}
