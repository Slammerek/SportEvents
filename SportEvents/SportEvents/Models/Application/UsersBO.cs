﻿using SportEvents.Controllers.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportEvents.Models.Application
{
    public class UsersBO
    {
        private DataContext db = new DataContext();


        public List<User> ListOfUsers()
        {
            return db.Users.ToList();
        }

        public bool RegisterUser(User user)
        {
            user.RegistrationTime = DateTime.Now;
            user.Password = UtilityMethods.CalculateHashMd5(user.Password);
            user.PasswordComparison = UtilityMethods.CalculateHashMd5(user.PasswordComparison);
            db.Users.Add(user);
            db.SaveChanges();

            string smtpUserName = "sportevents1@seznam.cz";
            string smtpPassword = "777003862";
            string smtpHost = "smtp.seznam.cz";
            int smtpPort = 25;
            string emailTo = user.Email;
            string subject = string.Format("Potvrzeni registrace");
            string body = string.Format("Děkujeme Vám za Vaši registraci <b>{0}</b>:)<br/><br/>Váš ERASMUS team", user.FirstName);

            EmailService service = new EmailService();

            bool kq = service.Send(smtpUserName, smtpPassword, smtpHost, smtpPort, emailTo, subject, body);

            return kq;

        }

        public User GetUser(Login login)
        {
            User user = db.GetUserByEmail(login.Email);

            return user;
        }


        internal bool IsEmailInDatabase(string email)
        {
            if (db.IsEmailInDatabase(email))
            {
                return true;
            }
            return false;

        }

        internal bool IsUserRegistered(string email, string loginPassword)
        {

            if (db.IsEmailInDatabase(email) && UtilityMethods.
                ComparePasswords(UtilityMethods.CalculateHashMd5(loginPassword), db.GetHashedPassword(email)))
            {
                return true;
            }
            return false;
        }
    }
}