using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity.UI.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cash_Back.Utility
{
	public class EmailSender : IEmailSender
	{
		public async Task SendEmailAsync(string email, string subject, string htmlMessage)
		{
			MailjetClient client = new MailjetClient("d33585d766795406a4ce08211b16f475", "26d85571444a94880cf21ce45fc01578")
			{
				
			};
			MailjetRequest request = new MailjetRequest
			{
				Resource = Send.Resource,
			}
			.Property(Send.FromEmail, "cashback.suppot2021@gmail.com")
			.Property(Send.FromName, "Cash Back APP")
			.Property(Send.Subject, subject)
			.Property(Send.TextPart, htmlMessage)
			.Property(Send.HtmlPart, htmlMessage)
			.Property(Send.Recipients, new JArray {
				new JObject {
				 {"Email", email}
				 }
				});

			MailjetResponse response = await client.PostAsync(request);
		}
	}
}
