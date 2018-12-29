using System;

namespace YEngine
{

    public enum CRType
    {
        Number = 0,
        Scientific = 1
    }

    /// <summary>
    /// Numerical formatter.
    /// Formates number in currency formate and Scientific notation.
    /// </summary>
    public class NumberFormater
    {
        public string Format(Decimal number)
        {
            return FormatNumberString(number.ToString());
        }

        public string Format(double number)
        {
            return FormatNumberString(RemoveDecimals(number));
        }

        public string FormatScientific(double number)
        {
            return (FormatScientific(RemoveDecimals(number)));
        }
        public string FormatScientific(string number)
        {
            string s = number;
            int n = s.Length - 1;
            if (n > 0)
            {
                return s.Substring(0, 1) + "." + (s.Length < 3 ? s.Substring(1, 1) + "0" : s.Substring(1, 2)) + "e" + string.Format("{0:D3}", n);
            }
            else
            {
                return s + ".e" + string.Format("{0:D3}", n);
            }
        }

        public string RemoveDecimals(double number)
        {
            string val = number.ToString().ToLower();
            string[] str;
            string S;
            if (val.Contains("e"))
            {
                str = val.Split(new char[] { '.', 'e', '+' });
                S = str[0] + str[1];
                int N = int.Parse(str[3]) + str[1].Length;
                for (int i = 0; i < N; i++)
                {
                    S += "0";
                }
            }
            else
            {
                str = val.Split(new char[] { '.' });
                if (str.Length > 1)
                {
                    S = str[0] + str[1];
                    int N = str[1].Length;
                    for (int i = 0; i < N; i++)
                    {
                        S += "0";
                    }
                }
                else
                {
                    S = str[0];
                }
            }
            return S;
        }



        private string FormatNumberString(string number)
        {
            if (number.Length < 4)
            {
                return number;
            }

            if (number.Length < 7)
            {
                return FormatThousands(number);
            }

            return FormatGeneral(number);
        }

        private string FormatThousands(string number)
        {
            string leadingNumbers = number.Substring(0, number.Length - 3);
            string decimals = number.Substring(number.Length - 3);

            return CreateNumericalFormat(leadingNumbers, decimals, "K");
        }

        private string CreateNumericalFormat(string leadingNumbers, string decimals, string suffix)
        {
            return String.Format("{0}.{1}{2}", leadingNumbers, decimals, suffix);
        }

        private string FormatGeneral(string number)
        {
            int amountOfLeadingNumbers = (number.Length - 7) % 3 + 1;
            string leadingNumbers = number.Substring(0, amountOfLeadingNumbers);
            string decimals = number.Substring(amountOfLeadingNumbers, 2);

            return CreateNumericalFormat(leadingNumbers, decimals, GetSuffixForNumber(number));
        }

        private string GetSuffixForNumber(string number)
        {
            int numberOfThousands = (number.Length - 1) / 3;

            switch (numberOfThousands)
            {
                case 1:
                    return "K";
                case 2:
                    return "M";
                case 3:
                    return "B";
                case 4:
                    return "T";
                case 5:
                    return "Q";
                default:
                    return GetProceduralSuffix(numberOfThousands - 5);
            }
        }

        private string GetProceduralSuffix(int numberOfThousandsAfterQ)
        {
            if (numberOfThousandsAfterQ < 27)
            {
                return ((char)(numberOfThousandsAfterQ + 96)).ToString();
            }

            int rightChar = (numberOfThousandsAfterQ % 26);
            string right = rightChar == 0 ? "z" : ((char)(rightChar + 96)).ToString();
            string left = ((char)(((numberOfThousandsAfterQ - 1) / 26) + 96)).ToString();
            return left + right;
        }
    }


}