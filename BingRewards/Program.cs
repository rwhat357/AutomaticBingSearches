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

        static void Main(string[] args)
        {
            Initialize();

            GoToBing();
            SignIntoMicrosoft();
            DoSearches();

            CleanUp();
        }

        private static void CleanUp()
        {
            _driver.Quit();
        }

        private static void DoSearches()
        {
            for (var i = 0; i < _numberOfSearches; i++)
            {
                // If search is made before username is visible, that login will 
                var usernameAvailable = _wait.Until(ExpectedConditions.ElementIsVisible(By.Id("id_n")));
                var datetime = DateTime.Now.ToString("h:mm:s tt");
                var search  = _wait.Until(ExpectedConditions.ElementIsVisible(By.Id("sb_form_q")));
                search.Clear();
                search.SendKeys(datetime);
                search.SendKeys(Keys.Enter);
            }
        }

        private static void Initialize()
        {
            _defaultUsername = ConfigurationManager.AppSettings["defaultUsername"];
            _defaultPassword = ConfigurationManager.AppSettings["defaultPassword"];

            _numberOfSearches = Convert.ToInt32(ConfigurationManager.AppSettings["numberOfSearches"]);
            _driver = new FirefoxDriver();
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(value: 10));
            _jse = (IJavaScriptExecutor)_driver;
        }

        private static void GoToBing()
        {
            _driver.Navigate().GoToUrl("http://www.bing.com/");

            //var signin = _wait.Until(ExpectedConditions.ElementIsVisible(By.Id("id_s")));
            var signin = SafelyFindElement(By.Id("id_s"));
            signin.Click();

            var connect = _wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("id_link_text")));
            connect.Click();
        }

        private static IWebElement SafelyFindElement(By by)
        {
            return _wait.Until(ExpectedConditions.ElementIsVisible(by));
        }

        private static void SignIntoMicrosoft()
        {
            //_driver.Navigate().GoToUrl("https://login.live.com/login.srf?wa=wsignin1.0&rpsnv=12&ct=1468856874&rver=6.7.6631.0&wp=MBI&wreply=https%3a%2f%2fwww.bing.com%2fsecure%2fPassport.aspx%3frequrl%3dhttp%253a%252f%252fwww.bing.com%252f%253fwlexpsignin%253d1&lc=1033&id=264960");

            var user = _wait.Until(ExpectedConditions.ElementIsVisible((By.TagName("input"))));
            var pass = _driver.FindElement(By.Id("i0118"));
            user.SendKeys(_defaultUsername);
            pass.SendKeys(_defaultPassword);
            
            var submit = _wait.Until(ExpectedConditions.ElementIsVisible(By.Id("idSIButton9")));
            submit.SendKeys(Keys.Enter);
        }

    }
}
