using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace P1XCS000086.Services.Interfaces.Models.Automation
{
	public interface IPixivDataModel
	{
		public Task PixivLogin(string mailAddress, string password);
	}
}
