#Download DotNet 
#ensure latest which is net8.0
https://dotnet.microsoft.com/download

#Add Package
dotnet add package Microsoft.Playwright

#Build 
dotnet build

#Get Browser Installation
#Windows powershell
powershell ./bin/Debug/net8.0/playwright.ps1 install

#Linux bash
bash ./bin/Debug/net8.0/playwright.sh install
