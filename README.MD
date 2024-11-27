#Before you run this program
#Download DotNet   
#ensure latest which is net8.0  
https://dotnet.microsoft.com/download  

#Add Playwright Package  
dotnet add package Microsoft.Playwright

#Build  
dotnet build

#After a successfull build, a browser install script is left in the bin folder. Run it for Browser Installation. You only need to do it once  
#Windows powershell  
powershell ./bin/Debug/net8.0/playwright.ps1 install  

#Linux bash  
bash ./bin/Debug/net8.0/playwright.sh install  

#Options:  
#If you want to get all of the debug info then uncomment the first line in Program.cs  //#define DEBUG_MODE  
#If you want to run in headless mode then set the boolean [run_headless] to "true" on line 10 in Program.cs  
