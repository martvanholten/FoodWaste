﻿namespace Domain.Models
{
    public class MailData
    {
        public string EmailToId { get; set; } = null!;
        public string EmailToName { get; set; } = null!;
        public string EmailSubject { get; set; } = null!;
        public string EmailBody { get; set; } = null!;
    }
}
