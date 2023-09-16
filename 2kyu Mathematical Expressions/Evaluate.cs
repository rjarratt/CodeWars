namespace Evaluation;

using System;
using System.Text;

public class Evaluate
{
    public string eval(string expression)
    {

        string result;
        try
        {
            IEnumerator<Token> tokenEnumerator = Tokenizer.ReadTokens(expression).GetEnumerator();
            double expressionValue = EvaluateExpression(tokenEnumerator);
            if (double.IsInfinity(expressionValue))
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

    private static double EvaluateExpression(IEnumerator<Token> tokenEnumerator)
    {
        double result = 0;
        while (tokenEnumerator.MoveNext())
        {
            Token? currentToken = tokenEnumerator.Current;

            switch (currentToken.TokenType)
            {
                case TokenType.Number:
                    result = double.Parse(currentToken.Symbol);
                    break;
                case TokenType.PlusOperator:
                    result += EvaluateExpression(tokenEnumerator);
                    break;
                case TokenType.MinusOperator:
                    result -= EvaluateExpression(tokenEnumerator);
                    break;
                case TokenType.MultiplyOperator:
                    result *= EvaluateExpression(tokenEnumerator);
                    break;
                case TokenType.DivideOperator:
                    result/= EvaluateExpression(tokenEnumerator);
                    break;
                case TokenType.PowerOperator:
                    result = Math.Pow(result, EvaluateExpression(tokenEnumerator));
                    break;
            }
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
                    switch (currentChar)
                    {
                        case '+':
                            {
                                yield return new Token(TokenType.PlusOperator, string.Empty);
                                break;
                            }
                        case '-':
                            {
                                yield return new Token(TokenType.MinusOperator, string.Empty);
                                break;
                            }
                        case '*':
                            {
                                yield return new Token(TokenType.MultiplyOperator, string.Empty);
                                break;
                            }
                        case '/':
                            {
                                yield return new Token(TokenType.DivideOperator, string.Empty);
                                break;
                            }
                        case '&':
                            {
                                yield return new Token(TokenType.PowerOperator, string.Empty);
                                break;
                            }
                        case '(':
                            {
                                yield return new Token(TokenType.LeftParenthesis, string.Empty);
                                break;
                            }
                        case ')':
                            {
                                yield return new Token(TokenType.RightParenthesis, string.Empty);
                                break;
                            }
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
                if (char.IsDigit(currentChar))
                {
                    currentSymbol.Append(currentChar);
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
