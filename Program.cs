// See https://aka.ms/new-console-template for more information
using System;
using System.Threading.Tasks;
using Microsoft.Playwright;

class Program
{
    const string snipe_url  = "https://demo.snipeitapp.com/login";
    const string asset_name = "X7 Apple macbook Pro 13";
    const string asset_tag  = "MBP13-X588";
    const string serial_no  = "XA235219JI";


    private static IPage? page;      // Type for the page
    private static IBrowser? browser; // Type for the browser
    private static void cw(string msg)
    {
        Console.WriteLine(msg);
    }

    private static async Task Init()
    {
        var playwright = await Playwright.CreateAsync();   // Playwright is of type Playwright
        browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false, SlowMo = 1000 });  // Browser is of type IBrowser
        // Create a new page
        page = await browser.NewPageAsync();  // Page is of type IPage
    }

    private static async Task Login()
    {
        if((page == null) || (browser == null))
            return;

        cw("Logging in. go to url");
        await page.GotoAsync(snipe_url);
        cw("fill user name");
        await page.FillAsync("input[name='username']", "admin");
        cw("fill password");
        await page.FillAsync("input[name='password']", "password");
        cw("Submit");
        await page.ClickAsync(".btn.btn-primary.btn-block");
    }

    private static async Task CreateAsset()
    {
        if((page == null) || (browser == null))
            return;

        cw("Get hardware route");
        await page.WaitForSelectorAsync("a[href='https://demo.snipeitapp.com/hardware']");

        cw("Go to hardware route");
        await page.ClickAsync("a[href='https://demo.snipeitapp.com/hardware']");
        cw("wait asset record");
        await page.WaitForSelectorAsync("a[href='https://demo.snipeitapp.com/hardware/create'].btn.btn-primary.pull-right", new PageWaitForSelectorOptions { State = WaitForSelectorState.Visible });
        cw("Create asset record");
        await page.Locator("a[href='https://demo.snipeitapp.com/hardware/create'].btn.btn-primary.pull-right").ClickAsync();        

        cw("Add asset");         
        cw("Add Company");         
        await page.Locator("#select2-company_select-container").ClickAsync();
        cw("wait for dropdown");
        await page.WaitForSelectorAsync(".select2-dropdown .select2-results", new PageWaitForSelectorOptions { Timeout = 5000 });
        cw("select company");
        await page.Locator(".select2-dropdown .select2-results ").Nth(0).ClickAsync();
        cw("Add Asset tag and serial");         
        await page.FillAsync("input[name='asset_tags[1]']", asset_tag);
        await page.FillAsync("input[name='serials[1]']", serial_no);
        cw("Add model ID");
        cw("Open Modal Form");
        await page.Locator("a[href='https://demo.snipeitapp.com/modals/model']").ClickAsync();
        cw("Wait for the first input to be visible");
        await page.Locator("input#modal-name").WaitForAsync(new() { State = WaitForSelectorState.Visible });
        cw("Add an asset name [arbitrary value]");
        await page.Locator("input#modal-name").FillAsync("Apple Macbook Pro");

        cw("Click 'Category dropdown");
        await page.Locator("div.col-md-8.col-xs-12.required").ClickAsync();        
        cw("Wait for the dropdown");
        await page.WaitForSelectorAsync("ul.select2-results__options",  new PageWaitForSelectorOptions { Timeout = 5000 });
        cw("Select 'Laptop' from dropdown");
        await page.Locator("ul.select2-results__options li").GetByText("Laptop", new() { Exact = true }).ClickAsync();
        cw("Click Manufacturer");
        await page.Locator("span.select2-selection[aria-labelledby='select2-modal-manufacturer_id-container']").ClickAsync();        
        cw("Wait for manufacturer dropdown");
        await page.WaitForSelectorAsync("ul.select2-results__options", new PageWaitForSelectorOptions { Timeout = 5000 });
        cw("Select Apple from dropdown");
        await page.Locator("ul.select2-results__options li").GetByText("Apple", new() { Exact = true }).ClickAsync();
        cw("Add Model no");
        await page.Locator("input#modal-model_number").FillAsync(asset_name);
        cw("Save the New model");
        await page.Locator("button#modal-save").ClickAsync();

        cw("Change status to ready to deploy");
        await page.ClickAsync("#select2-status_select_id-container");
        await page.WaitForSelectorAsync(".select2-results__option",new PageWaitForSelectorOptions { Timeout = 5000 });  // Ensure options are visible
        await page.ClickAsync(".select2-results__option:has-text('Ready to Deploy')");
        cw("wait for user button");
        await page.WaitForSelectorAsync("#assignto_selector", new() { State = WaitForSelectorState.Visible });
        cw("Wait for user");
        await page.ClickAsync("#assignto_selector .btn input[value='user'] + i"); // Select the User option        
        cw("assign user to asset");
        await page.ClickAsync("#assigned_user");
        cw("wait for dropdown");
        await page.WaitForSelectorAsync(".select2-dropdown .select2-results",new PageWaitForSelectorOptions { Timeout = 5000 });
        cw("Select an item");
        await page.Locator(".select2-dropdown .select2-results ").Nth(0).ClickAsync();
        cw("submit new asset record");
        await page.Locator("button.btn.btn-primary.pull-right[style*='margin-left:5px']").ClickAsync();

        Console.WriteLine("Successfully Created a new Asset");
    }

    private static async Task Verify()
    {
        if((page == null) || (browser == null))
            return;
        cw("Verifying asset");
        cw("Go to the hardware route with a search parameter for a unique asset tag");
        string url  = string.Format("https://demo.snipeitapp.com/hardware?search={0}", asset_tag);
        await page.GotoAsync(url);

        cw("Wait for network to idle");
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);  // Wait for network to idle
        cw("Wait for asset tag: " + asset_tag);
        string tag_val  = string.Format("a:has-text('{0}')", asset_tag);
        await page.WaitForSelectorAsync(tag_val, new() { State = WaitForSelectorState.Visible });

        cw("Check that it exists");
        var assetExists = await page.IsVisibleAsync(tag_val);


        if (!assetExists)
        {
            Console.WriteLine("Error: asset Not found.");
            return;
        }

        Console.WriteLine("Asset Found.");

        // Navigate to asset page
        cw("Get asset name");
       await page.ClickAsync(tag_val);

        cw("Go to history");
        await page.ClickAsync("a[href='#history']");

        var historyVisible = await page.IsVisibleAsync("text=Created asset");

        var tableLocator = page.Locator("#assetHistory");
        var rowsLocator = tableLocator.Locator("tbody tr");
        var rowCount = await rowsLocator.CountAsync();
        Console.WriteLine("No of Rows in History = {0}", rowCount);
        for(int i = 0; i < rowCount; i++)
        {
            var row = rowsLocator.Nth(i);
            var date = await row.Locator("td:nth-child(2)").TextContentAsync();
            var user = await row.Locator("td:nth-child(3)").TextContentAsync();
            var action = await row.Locator("td:nth-child(4)").TextContentAsync();
            var assetLink = await row.Locator("td:nth-child(5) a").TextContentAsync();
            var assetTag = await row.Locator("td:nth-child(5) a").GetAttributeAsync("href");
            var userName = await row.Locator("td:nth-child(6)").TextContentAsync();
            var checkoutStatus = await row.Locator("td:nth-child(7)").TextContentAsync();
            Console.WriteLine($"Row {i + 1}:");
            Console.WriteLine($"Date: {date}");
            Console.WriteLine($"User: {user}");
            Console.WriteLine($"Action: {action}");
            Console.WriteLine($"Asset: {assetLink}");
            Console.WriteLine($"Asset Tag: {assetTag}");
            Console.WriteLine($"User Name: {userName}");
            Console.WriteLine($"Checkout Status: {checkoutStatus}");
            Console.WriteLine("----------------------------");
        }
        Console.WriteLine((rowCount > 0)
            ? "History validation successful."
            : "History validation failed.");        
    }

    public static async Task Main(string[] args)
    {
        cw("Hello World");
        await Init();    
        await Login();  

        await CreateAsset();
        await Verify();
        if(browser != null)
            await browser.CloseAsync();
    }
}
