using Common.Models;
using DataAcceess.IRepository;
using Services.EmailService;
using Services.IService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service
{
    public class AdminService : IAdminService
    {
        public readonly IGenericRepository<Person> _genericRepositroy;
        public readonly IEmailSender _emailSender;

        public AdminService(IGenericRepository<Person> genericRepositroy, IEmailSender emailSender)
        {
            _genericRepositroy = genericRepositroy;
            _emailSender = emailSender;
        }

        public async Task AcceptAccount(int idPerson)
        {
            await Verification(Enums.VerificationStatus.Active, idPerson);
        }

        public async Task DeniedAccount(int idPerson)
        {
            await Verification(Enums.VerificationStatus.Denied, idPerson);
        }

        private async Task Verification(Enums.VerificationStatus status, int idPerson)
        {
            Person person = await _genericRepositroy.GetByObject(idPerson);
            if (person == null)
            {
                throw new KeyNotFoundException("User does not exists.");
            }
            if (person.PersonType == Enums.PersonType.Customer || person.PersonType == Enums.PersonType.Admin || person.PersonType == Enums.PersonType.None)
            {
                throw new Exception("Seller only can be verified.");
            }

            person.Verification = status;
            person.DecisionMade = true;
            await _genericRepositroy.Update(person);
            await _genericRepositroy.Save();

            await SendMail(person.Email, status);
        }

        private async Task SendMail(string email, Enums.VerificationStatus status)
        {
            StringBuilder sb = new StringBuilder();
            string username = email.Split('@')[0];

            if (email == null)
            {
                throw new ArgumentNullException("Email does not exist.");
            }
            if (email == "")
            {
                throw new FormatException("Email does not exist.");
            }

            sb.AppendLine($"Hi {username},\n");
            sb.AppendLine($"The status of your verification is: {status.ToString().ToUpper()}.");
            sb.AppendLine("\nRegards");

            var message = new Message(
                new string[] { email },
                "Verification status",
                sb.ToString()
            );

            await _emailSender.SendEmailAsync(message);
        }
    }
}