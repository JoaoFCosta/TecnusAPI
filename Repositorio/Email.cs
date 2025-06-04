using System.Net.Mail;
using System.Net;
using TecnusAPI.Repositorio.Interfaces;

namespace TecnusAPI.Repositorio
{
    public class Email : IEmail
    {
        private readonly IConfiguration _configuration;

        public Email(IConfiguration configuration)
        {

            var inMemorySettings = new Dictionary<string, string> {
                {"SMTP:UserName", "mathenrique2004@gmail.com"}, // Seu e-mail do Gmail
                {"SMTP:Name", "Recuperação de Senha"},
                {"SMTP:Host", "smtp.gmail.com"}, // Servidor SMTP do Gmail
                {"SMTP:Senha", "frnp lvsm teym crss"}, // Senha do Gmail ou senha de aplicativo
                {"SMTP:Porta", "587"}, // Porta do Gmail
                {"SMTP:EnableSSL", "true"},
                {"SMTP:UseDefaultCredentials", "false"}
            };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
        }

        public bool Enviar(string email, string assunto, string mensagem)
        {
            try
            {
                string host = _configuration.GetValue<string>("SMTP:Host");
                string nome = _configuration.GetValue<string>("SMTP:Name");
                string username = _configuration.GetValue<string>("SMTP:UserName");
                string senha = _configuration.GetValue<string>("SMTP:Senha");
                int porta = _configuration.GetValue<int>("SMTP:Porta");
                bool enableSsl = _configuration.GetValue<bool>("SMTP:EnableSSL");

                MailMessage mail = new MailMessage()
                {
                    From = new MailAddress(username, nome)
                };

                mail.To.Add(email);
                mail.Subject = assunto;
                mail.Body = mensagem;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;

                using (SmtpClient smtp = new SmtpClient(host, porta))
                {
                    smtp.Credentials = new NetworkCredential(username, senha);
                    smtp.EnableSsl = enableSsl;

                    smtp.Send(mail);
                    return true;
                }
            }
            catch (System.Exception ex)
            {
                // Log da exceção (pode ser substituído por um logger real)
                Console.WriteLine($"Erro ao enviar e-mail: {ex.Message}");
                return false;
            }
        }
    }
}
