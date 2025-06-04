namespace TecnusAPI.Utils
{
    public class ValidatorCpfCnpj
    {
        public static bool ValidarCpfCnpj(string CPF_CNPJ_Logista)
        {
            // Remove caracteres não numéricos
            CPF_CNPJ_Logista = new string(CPF_CNPJ_Logista.Where(char.IsDigit).ToArray());

            // Verifica se é CPF
            if (CPF_CNPJ_Logista.Length == 11)
            {
                return ValidarCpf(CPF_CNPJ_Logista);
            }
            // Verifica se é CNPJ
            else if (CPF_CNPJ_Logista.Length == 14)
            {
                return ValidarCnpj(CPF_CNPJ_Logista);
            }
            // Se não for nem CPF nem CNPJ
            else
            {
                return false;
            }
        }

        public static bool ValidarCpf(string CPF_CNPJ_Logista)
        {
            // Verifica se o CPF tem 11 dígitos
            if (CPF_CNPJ_Logista.Length != 11)
            {
                return false;
            }

            // Verifica se todos os dígitos são iguais (CPF inválido)
            if (CPF_CNPJ_Logista.Distinct().Count() == 1)
            {
                return false;
            }

            // Cálculo dos dígitos verificadores
            int[] multiplicadoresPrimeiroDigito = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicadoresSegundoDigito = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string cpfSemDigitos = CPF_CNPJ_Logista.Substring(0, 9);
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
            return CPF_CNPJ_Logista.EndsWith(digitoVerificador1.ToString() + digitoVerificador2.ToString());
        }

        public static bool ValidarCnpj(string CPF_CNPJ_Logista)
        {
            // Verifica se o CNPJ tem 14 dígitos
            if (CPF_CNPJ_Logista.Length != 14)
            {
                return false;
            }

            // Verifica se todos os dígitos são iguais (CNPJ inválido)
            if (CPF_CNPJ_Logista.Distinct().Count() == 1)
            {
                return false;
            }

            // Cálculo dos dígitos verificadores
            int[] multiplicadoresPrimeiroDigito = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicadoresSegundoDigito = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            string cnpjSemDigitos = CPF_CNPJ_Logista.Substring(0, 12);
            int soma = 0;

            for (int i = 0; i < 12; i++)
            {
                soma += int.Parse(cnpjSemDigitos[i].ToString()) * multiplicadoresPrimeiroDigito[i];
            }

            int resto = soma % 11;
            int digitoVerificador1 = resto < 2 ? 0 : 11 - resto;

            cnpjSemDigitos += digitoVerificador1;
            soma = 0;

            for (int i = 0; i < 13; i++)
            {
                soma += int.Parse(cnpjSemDigitos[i].ToString()) * multiplicadoresSegundoDigito[i];
            }

            resto = soma % 11;
            int digitoVerificador2 = resto < 2 ? 0 : 11 - resto;

            // Verifica se os dígitos verificadores estão corretos
            return CPF_CNPJ_Logista.EndsWith(digitoVerificador1.ToString() + digitoVerificador2.ToString());
        }
    }
}
