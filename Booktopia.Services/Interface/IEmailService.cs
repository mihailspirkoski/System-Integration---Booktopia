﻿using Booktopia.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Booktopia.Services.Interface
{
    public interface IEmailService
    {
        Task SendEmailAsync(List<EmailMessage> allMails);
    }
}
