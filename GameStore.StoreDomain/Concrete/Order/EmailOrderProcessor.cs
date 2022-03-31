using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using GameStore.StoreDomain.Abstract;
using GameStore.StoreDomain.Entities;

namespace GameStore.StoreDomain.Concrete.Order
{
	public class EmailOrderProcessor : IOrderProcessor
	{
		private EmailSettings settings;

		public EmailOrderProcessor(EmailSettings settings)
		{
			this.settings = settings;
		}

		public void ProcessOrder(Cart cart, ShippingDetails details)
		{
			using (var smtpClient = new SmtpClient())
			{
				smtpClient.EnableSsl = settings.UseSsl;
				smtpClient.Host = settings.ServerName;
				smtpClient.Port = settings.ServerPort;
				smtpClient.UseDefaultCredentials = false;
				smtpClient.Credentials = new NetworkCredential(settings.Username, settings.Password);

				if (settings.WriteAsFile)
				{
					smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
					smtpClient.PickupDirectoryLocation = settings.FileLocation;
					smtpClient.EnableSsl = false;
				}

				var body = new StringBuilder()
					.AppendLine("Новый заказ обработан\n----")
					.AppendLine("Товары:");

				foreach (var line in cart.Lines)
				{
					body.AppendFormat("{0} x {1} (итого: {2:c}", line.Quantity, line.Game.Name, line.LinePrice);
				}

				body.AppendFormat("Общая стоимость: {0:c}", cart.ComputeTotalValue())
					.AppendLine("---")
					.AppendLine("Доставка:")
					.AppendLine(details.Name)
					.AppendLine(details.Line1)
					.AppendLine(details.Line2 ?? "")
					.AppendLine(details.Line3 ?? "")
					.AppendLine(details.City)
					.AppendLine(details.Country)
					.AppendLine("---")
					.AppendFormat("Подарочная упаковка: {0}",
						details.GiftWrap ? "Да" : "Нет");

				var message = new MailMessage(settings.MailFromAddress, settings.MailToAddress, "Новый заказ отправлен!", body.ToString());

				if (settings.WriteAsFile)
				{
					message.BodyEncoding = Encoding.UTF8;
				}

				smtpClient.Send(message);
			}
		}
	}

	public class EmailSettings
	{
		public string MailToAddress = "ustinhtc@gmail.com";
		public string MailFromAddress = "gamestore@example.com";
		public bool UseSsl = true;
		public string Username = "MySmtpUsername";
		public string Password = "MySmtpPassword";
		public string ServerName = "smtp.example.com";
		public int ServerPort = 587;
		public bool WriteAsFile = true;
		public string FileLocation = @"c:\game_store_emails";
	}
}
