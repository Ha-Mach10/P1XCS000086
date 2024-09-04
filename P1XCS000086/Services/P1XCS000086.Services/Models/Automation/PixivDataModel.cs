using P1XCS000086.Services.Interfaces.Models.Automation;
using System;
using System.Collections.Generic;
using System.Text;
using Meowtrix.PixivApi;
using Meowtrix.PixivApi.Json;
using Meowtrix.PixivApi.Models;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Net.Mail;
using System.Diagnostics.CodeAnalysis;
using P1XCS000086.Services.Sql.MySql;
using P1XCS000086.Services.Sql;

namespace P1XCS000086.Services.Models.Automation
{
	public class PixivDataModel : IPixivDataModel
	{
		#region Selenium用
		private const string CPixivUrl = "https://www.pixiv.net/";
		private const string CPixivLoginUrl = "https://accounts.pixiv.net/login?return_to=https%3A%2F%2Fwww.pixiv.net%2F&lang=ja&source=pc&view_type=page";

		private const string COptionHeadless = "--headless";

		private const string CElementNameLogInAnkerLink = "signup-form__submit--login";
		private const string CElementNameMailOrIdInputAndPasswordInput = "sc-bn9ph6-6 cYyjQe";
		private const string CElementLoginSubmitButton = "sc-aXZVg fSnEpf sc-eqUAAy hhGKQA sc-2o1uwj-10 ldVSLT sc-2o1uwj-10 ldVSLT";
		#endregion

		private const string c_databaseName = "account_manager";

        private readonly string ConnStr = SqlConnectionStrings.ConnectionStrings[c_databaseName];


		public PixivDataModel()
		{

		}




		public async Task PixivLogin(string mailAddress, string password)
		{
			// オプションを設定
			ChromeOptions options = new();

			// ヘッドレス（Window非表示）
			// options.AddArguments(COptionHeadless);

			// ドライバの実行
			using (IWebDriver driver = new ChromeDriver(options))
			{
				try
				{
					// Pixivのログインページへ遷移
					await driver.Navigate().GoToUrlAsync(CPixivUrl);

					// デバッグ用
					if (options.Arguments is null)
					{
						driver.Manage().Window.Maximize();
					}

					// タイムアウト設定（5秒）
					driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(60000);

					// #下記# html要素を取得し、操作する

					// 「ログイン」用アンカー要素を取得し、キーを送る
					var ankerLink = driver.FindElement(By.ClassName(CElementNameLogInAnkerLink));
					ankerLink.SendKeys(Keys.Enter);

					var mailAndPasswordInput = driver.FindElements(By.ClassName(CElementNameMailOrIdInputAndPasswordInput));
					var submitButton = driver.FindElement(By.ClassName(CElementLoginSubmitButton));

					if (mailAndPasswordInput.Count == 2)
					{
						mailAndPasswordInput[0].SendKeys(mailAddress);
						mailAndPasswordInput[1].SendKeys(password);
					}

					// 
					submitButton.Click();

					driver.Quit();
				}
				catch (Exception ex)
				{
					driver.Quit();
				}
				finally { driver.Quit(); }
			}			
		}

		public async Task PixivLogin()
		{
			string query = $"SELECT `service_name` FROM `{c_databaseName}`;";

			SqlSelect select = new SqlSelect(ConnStr);

			List<string> contentItems = select.SelectedColumnToList("service_name", query);

		}
	}
}
