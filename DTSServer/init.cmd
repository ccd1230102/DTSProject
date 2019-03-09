md packages
cd packages
..\nuget install bootstrap -Version 4.3.1 -source https://api.nuget.org/v3/index.json
..\nuget install Chart.js -Version 2.7.3 -source https://api.nuget.org/v3/index.json
..\nuget install jQuery -Version 3.3.1 -source https://api.nuget.org/v3/index.json
..\nuget install Microsoft.CodeDom.Providers.DotNetCompilerPlatform -Version 2.0.1 -source https://api.nuget.org/v3/index.json
..\nuget install popper.js -Version 1.14.3 -source https://api.nuget.org/v3/index.json
xcopy .\Microsoft.CodeDom.Providers.DotNetCompilerPlatform.2.0.1\tools\RoslynLatest ..\DTSServer\bin\roslyn /y /e /i /q
cd ..\