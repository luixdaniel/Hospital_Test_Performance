using System;
using System.Net;
using System.Net.Mail;
using Hospital_Test_Performance.Database;
using Hospital_Test_Performance.Models;

namespace Hospital_Test_Performance.Utils
{
    /// <summary>
    /// Servicio para enviar correos y registrar los intentos en memoria.
    /// Usa Mailtrap (sandbox) para pruebas seguras de envío de correo.
    /// </summary>
    public class EmailService
    {
        private readonly DatabaseContent _db;

        public EmailService(DatabaseContent db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        /// <summary>
        /// Envía un correo y lo registra en el historial.
        /// </summary>
        public bool SendEmail(string to, string subject, string body, string from = "noreply@hospital.com", int? appointmentId = null)
        {
            var record = new EmailRecord
            {
                Id = _db.EmailHistory.Count > 0 ? _db.EmailHistory[^1].Id + 1 : 1,
                Timestamp = DateTime.UtcNow,
                OriginalTo = to,
                From = from,
                Subject = subject,
                Body = body,
                AppointmentId = appointmentId
            };

            try
            {
                // ✅ Configuración SMTP para Mailtrap
                var smtpHost = "sandbox.smtp.mailtrap.io";
                var smtpPort = 2525;
                var smtpUser = "a1874d4929064b";   // tu usuario Mailtrap
                var smtpPass = "18b314c471ec38";         // tu contraseña Mailtrap (coloca el valor completo real)

                using var client = new SmtpClient(smtpHost, smtpPort)
                {
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    EnableSsl = true, // Mailtrap soporta STARTTLS
                    Credentials = new NetworkCredential(smtpUser, smtpPass)
                };

                // 📩 Correo de prueba
                from = "noreply@hospital.com"; 
                record.FinalTo = to;

                using var mail = new MailMessage(from, record.FinalTo, subject, body);
                client.Send(mail);

                record.Sent = true;
                _db.EmailHistory.Add(record);
                return true;
            }
            catch (Exception ex)
            {
                record.Sent = false;
                record.ErrorMessage = ex.Message;
                record.ErrorDetail = ex.ToString();
                _db.EmailHistory.Add(record);
                return false;
            }
        }

        /// <summary>
        /// Envía un correo de prueba.
        /// </summary>
        public bool SendTestEmail(string to)
        {
            return SendEmail(to, "Correo de prueba - Hospital System", "Este es un correo de prueba enviado desde Mailtrap Sandbox.");
        }
    }
}
