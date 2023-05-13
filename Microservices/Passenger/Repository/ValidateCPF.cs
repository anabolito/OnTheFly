using System.ComponentModel.DataAnnotations;

namespace PassengerAPI.Service
{
    public static class ValidateDocument
    {
        #region[ValidateDoc]
        public static bool ValidateCPF(string cpf)
        {
            if (!ValidateCPFIfFormatIsCorrect(cpf))
            {
                var cpfToVerify = FormatCPF(cpf);
                cpfToVerify = cpfToVerify.Remove(9, 2);
                int aux = 0;
                for (int i = 0, j = 10; i <= 8; i++, j--)
                    aux += (cpfToVerify[i] - '0') * j;
                var firstDigit = aux * 10 % 11;
                if (firstDigit == 10)
                    firstDigit = 0;
                cpfToVerify += firstDigit;
                aux = 0;
                for (int i = 0, j = 11; i <= 9; i++, j--)
                    aux += (cpfToVerify[i] - '0') * j;
                var secondDigit = aux * 10 % 11;
                if (secondDigit == 10)
                    secondDigit = 0;
                cpfToVerify += secondDigit;
                if (FormatCPF(cpf) == cpfToVerify)
                {
                    return true;
                }
            }
            return false;
        }
        public static string FormatCPF(string rawCPF)
        {
            string cpf = rawCPF.Trim().Replace(".", "").Replace("-", "");
            return cpf;
        }
        public static bool ValidateCPFIfFormatIsCorrect(string cpf)
        {
            if (int.TryParse(cpf, out _))
            {
                return true;
            }
            if (FormatCPF(cpf).Length != 11)
                return true;
            string aux = "11111111111";
            for (int i = 0; i < 10; i++)
            {
                if (float.Parse(aux) * i == float.Parse(FormatCPF(cpf)))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}
