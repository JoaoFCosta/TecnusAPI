namespace TecnusAPI.Utils
{
    public class CpfValidator
    {
        public static bool ValidarCpfCliente(string CPF_Usuario)
        {
            // Remove caracteres não numéricos
            CPF_Usuario = new string(CPF_Usuario.Where(char.IsDigit).ToArray());

            // Verifica se o CPF tem 11 dígitos
            if (CPF_Usuario.Length != 11)
            {
                return false;
            }

            // Verifica se todos os dígitos são iguais (CPF inválido)
            if (CPF_Usuario.Distinct().Count() == 1)
            {
                return false;
            }

            // Cálculo dos dígitos verificadores
            int[] multiplicadoresPrimeiroDigito = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicadoresSegundoDigito = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string cpfSemDigitos = CPF_Usuario.Substring(0, 9);
            int soma = 0;

            for (int i = 0; i < 9; i++)
            {
                soma += int.Parse(cpfSemDigitos[i].ToString()) * multiplicadoresPrimeiroDigito[i];
            }

            int resto = soma % 11;
            int digitoVerificador1 = resto < 2 ? 0 : 11 - resto;

            cpfSemDigitos += digitoVerificador1;
            soma = 0;

            for (int i = 0; i < 10; i++)
            {
                soma += int.Parse(cpfSemDigitos[i].ToString()) * multiplicadoresSegundoDigito[i];
            }

            resto = soma % 11;
            int digitoVerificador2 = resto < 2 ? 0 : 11 - resto;

            // Verifica se os dígitos verificadores estão corretos
            return CPF_Usuario.EndsWith(digitoVerificador1.ToString() + digitoVerificador2.ToString());
        }
    }
}
