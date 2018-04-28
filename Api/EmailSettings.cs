using System;
using Microsoft.AspNetCore.NodeServices;
namespace MomitKiller.Api
{
    public class EmailSettings
    {
        public string SmtpServer
        {
            get;
            set;
        }

        public int Port
        {
            get;
            set;
        }

        public string Username
        {
            get;
            set;
        }

        public string Passwd
        {
            get;
            set;
        }

        public string FromEmail
        {
            get;
            set;
        }

        public string ToEmail
        {
            get;
            set;
        }

        public EmailSettings()
        {
            
        }
    }
}
