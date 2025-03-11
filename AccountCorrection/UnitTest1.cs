
using Microsoft.Playwright;


namespace AccountCorrection
{
    public class Tests
    {
        private IPage Page;
        private IBrowserContext Context;
        private IBrowser Browser;

        [SetUp]
        public async Task Setup()
        {
            // Initialize Playwright
            var playwright = await Playwright.CreateAsync();
            Browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });

            // Create a new browser context
            Context = await Browser.NewContextAsync();
        }

        [Test]
        public async Task Login()
        {
            // Create a new page from the context
            Page = await Context.NewPageAsync();

            // Navigate to the login page
            await Page.GotoAsync("https://portal.utopiaindustries.pk/uind/accounts-finance/support-queries/correction-debit-account-view");
            // Perform login actions
            await Write("body > div > div > div > div > form > div:nth-child(5) > input", "mujahid.ali", "Enter Username");
            await Write("body > div > div > div > div > form > div:nth-child(6) > div > input", "Mujahid@123", "Enter Password");
            await Click("body > div > div > div > div > form > button", "Click Login");

            Thread.Sleep(6000);

            await Context.StorageStateAsync(new()
            {
                Path = "C:\\Users\\DELL\\source\\repos\\AccountCorrection\\AccountCorrection\\state.json"
            });
        }



        private static IEnumerable<TestCaseData> TestData()
        {
            string csvFilePath = @"C:\Users\mujahid.akber\Desktop\account.csv";
            var lines = File.ReadAllLines(csvFilePath).Skip(1); // Skip header line

            foreach (var line in lines)
            {
                var values = line.Split(',');
                string code = values[0];
                string account1 = values[1];
                string account2 = values[2];

                yield return new TestCaseData(code, account1, account2);
                    
            }
        }


        [Test]
        [TestCaseSource(nameof(TestData))]
        public async Task Newaccount(string code, string account1, string account2)
        { 
          // Load the previously saved context state from a file
            Context = await Browser.NewContextAsync(new BrowserNewContextOptions
            {
                StorageStatePath = "C:\\Users\\DELL\\source\\repos\\AccountCorrection\\AccountCorrection\\state.json"
            });

            await Write("#cancelVoucherApp > form > div > div:nth-child(2) > div > div > input.form-control", code, "Enter Code");
            await Press("#cancelVoucherApp > form > div > div:nth-child(2) > div > div > input.form-control", "ArrowDown", "Press Arrowdown");
            Thread.Sleep(1000);
            await Press("#cancelVoucherApp > form > div > div:nth-child(2) > div > div > input.form-control", "Tab", "Press Tab");
            await Click("#cancelVoucherApp > form > div > div:nth-child(2) > div > div > div > a:nth-child(1)", "Click Enter");


            await Write("#cancelVoucherApp > form > div > div:nth-child(4) > div > div > input.form-control", account1, "Enter Account1");
            await Press("#cancelVoucherApp > form > div > div:nth-child(4) > div > div > input.form-control", "ArrowDown", "Press Arrowdown");
            Thread.Sleep(1000);
            await Press("#cancelVoucherApp > form > div > div:nth-child(4) > div > div > input.form-control", "Tab", "Press Tab");
            await Click("#cancelVoucherApp > form > div > div:nth-child(4) > div > div > div > a:nth-child(1)", "Click Enter");



            await Write("#cancelVoucherApp > form > div > div:nth-child(5) > div > div > input.form-control", account2, "Enter Account2");
            await Press("#cancelVoucherApp > form > div > div:nth-child(5) > div > div > input.form-control", "ArrowDown", "Press Arrowdown");
            Thread.Sleep(1000);
            await Press("#cancelVoucherApp > form > div > div:nth-child(5) > div > div > input.form-control", "Tab", "Press Tab");
            await Click("#cancelVoucherApp > form > div > div:nth-child(5) > div > div > div > a:nth-child(1)", "Click Enter");


            await Click("#cancelVoucherApp > form > button", "Click Submit");



        }


        public async Task Click(string locator, string detail)
        {
            try
            {
                await Page.ClickAsync(locator);
                //await ExtentReport.TakeScreenshot(Page, Status.Pass, detail);
                await Task.Delay(1000); // Replaces Thread.Sleep with Task.Delay
            }
            catch (Exception ex)
            {
                //await ExtentReport.TakeScreenshot(Page, Status.Fail, "Click Failed" + ex);
                throw; // Re-throw the exception to catch errors
            }
        }

        public async Task Write(string locator, string data, string detail)
        {
            try
            {
                await Page.FillAsync(locator, data);
                //await ExtentReport.TakeScreenshot(Page, Status.Pass, detail);
                await Task.Delay(1000); // Replaces Thread.Sleep with Task.Delay
            }
            catch (Exception ex)
            {
                //await ExtentReport.TakeScreenshot(Page, Status.Fail, "Entry Failed" + ex);
                throw; // Re-throw the exception to catch errors
            }
        }

        public async Task Press(string locator, string data, string detail)
        {
            try
            {
                await Page.PressAsync(locator, data);
                //await ExtentReport.TakeScreenshot(Page, Status.Pass, detail);
                await Task.Delay(1000); // Replaces Thread.Sleep with Task.Delay
            }
            catch (Exception ex)
            {
                //await ExtentReport.TakeScreenshot(Page, Status.Fail, "Entry Failed" + ex);
                throw; // Re-throw the exception to catch errors
            }
        }
    }
}