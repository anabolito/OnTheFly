using System.ComponentModel.DataAnnotations;

namespace PassengerAPI.Service
{
    public static class validateCPF 
    {
        #region[ValidateDoc]
        public static bool ValidateDoc(string cpf)
        {
            int[] numbers = new int[11];
            int sumA = 0, sumB = 0;
            double restA = 0, restB = 0;

            cpf = cpf.Replace(".", "").Replace("-", "");

            if (cpf.Length < 11) return false;

            for (int i = 0; i < 11; i++)
            {
                if (!int.TryParse(cpf[i].ToString(), out numbers[i]))
                    return false;
            }

            if (numbers[0] == numbers[1] && numbers[1] == numbers[2] && numbers[2] == numbers[3] && numbers[3] == numbers[4] &&
                numbers[4] == numbers[5] && numbers[5] == numbers[6] && numbers[6] == numbers[7] && numbers[7] == numbers[8] &&
                numbers[9] == numbers[9] && numbers[10] == numbers[10]) return false;

            for (int i = 0, j = 10; i < 9; i++, j--)
            {
                sumA += numbers[i] * j;
            }

            restA = sumA % 11;

            if (restA < 2)
                restA = 0;
            else
                restA = 11 - restA;


            for (int i = 0, j = 11; i < 10; i++, j--)
            {
                sumB += numbers[i] * j;
            }

            restB = sumB % 11;

            if (restB < 2)
                restB = 0;
            else
                restB += 11 - restB;

            if (restA == numbers[9] && restB == numbers[10])
                return true;
            else
                return false;
        }
        #endregion
    }
}
