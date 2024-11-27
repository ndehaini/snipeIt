# Instructions  
#### Windows  
#Download DotNet SDK   
#ensure latest which is net8.0 (lts) 
https://dotnet.microsoft.com/download  

#Add Playwright Package  
dotnet add package Microsoft.Playwright

#Build  
dotnet build

#After a successfull build, a browser install script is left in the bin folder. Run it for Browser Installation. You only need to do it once  
#Windows powershell  
powershell ./bin/Debug/net8.0/playwright.ps1 install  

#run program  
dotnet run

#Options:  
#If you want to get all of the debug info then uncomment the first line in Program.cs  //#define DEBUG_MODE  
#If you want to run in headless mode then set the boolean [run_headless] to "true" on line 10 in Program.cs  

#### Linux
##Beware  
#Not ideal to run on linux  
#.net8 and Playwright are fragmented on ubuntu(24) where I tested this program in a headless setup.  I needed to install powershell in my wsl to get it working.  
#if you want run on linux then:  

#Download .net 8 SDK on your distro  
#Install Powershell  
#add Playwright package  
dotnet add package Microsoft.Playwright  

#Build the code  
dotnet build  

#Set run_headless to true in program.cs

#Run playwright.ps1  
sudo pwsh ./bin/Debug/net8.0/playwright.ps1 install-deps  

#Run Program  
dotnet run  

