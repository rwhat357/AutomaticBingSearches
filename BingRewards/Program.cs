using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

namespace BingRewards
{
    class Program
    {
        private static string _defaultUsername;
        private static string _defaultPassword;
        private static int _numberOfSearches;
        private static FirefoxDriver _driver;
        private static WebDriverWait _wait;
        private static IJavaScriptExecutor _jse;
        private static string _bingUrl;

        static void Main(string[] args)
        {
            Initialize();

            GoToBing();
            SignIntoMicrosoft();
            DoSearches();

            CleanUp();
        }

        private static void Initialize()
        {
            _defaultUsername = ConfigurationManager.AppSettings["defaultUsername"];
            _defaultPassword = ConfigurationManager.AppSettings["defaultPassword"];

            _numberOfSearches = Convert.ToInt32(ConfigurationManager.AppSettings["numberOfSearches"]);
            _driver = new FirefoxDriver();
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(value: 10));
            _jse = (IJavaScriptExecutor)_driver;
            _bingUrl = "http://www.bing.com";
        }

        private static void DoSearches()
        {
            for (var i = 0; i < _numberOfSearches; i++)
            {
                // If search is made before username is visible, that login will 
                SafelyFindElement(By.Id("id_n"));
                var datetime = DateTime.Now.ToString("h:mm:s tt");
                var search  = SafelyFindElement(By.Id("sb_form_q"));
                search.Clear();
                search.SendKeys(datetime);
                search.SendKeys(Keys.Enter);
            }
        }

        private static void GoToBing()
        {
            _driver.Navigate().GoToUrl(_bingUrl);
            var signin = SafelyFindElement(By.Id("id_s"));
            signin.Click();

            var connect = SafelyFindElement(By.ClassName("id_link_text"));
            connect.Click();
        }

        private static IWebElement SafelyFindElement(By by)
        {
            return _wait.Until(ExpectedConditions.ElementIsVisible(by));
        }

        private static void SignIntoMicrosoft()
        {
            var user = SafelyFindElement(By.TagName("input"));
            var pass = SafelyFindElement(By.Id("i0118"));
            user.SendKeys(_defaultUsername);
            pass.SendKeys(_defaultPassword);
            
            var submit = SafelyFindElement(By.Id("idSIButton9"));
            submit.SendKeys(Keys.Enter);
        }

        private static void CleanUp()
        {
            _driver.Quit();
        }
    }
}
