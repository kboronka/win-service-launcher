# win-service-launcher
runs as a service and launches non-service applications

## how to install
1. Deploy the dist folder
2. install service run `WinServiceLauncher.exe install <optional_service_name>` as administrator
3. edit WinServiceLauncher.xml 
3. start the service `WinServiceLauncher.exe start`

## setting up launchers

### schedule types
#### OnInterval
Launches process at given intervals
Parameters: `name, working-path, command, arguments, interval`
interval is in milliseconds

#### OnTimeOfDay
Launches application process at given intervals
Parameters: `name, working-path, command, arguments, time`

#### OnStartup
Launches application process when the service is being started
Parameters: `name, working-path, command, arguments, time`

#### OnShutdown
Launches application process when the service is being shutdown
Parameters: `name, working-path, command, arguments`

#### KeepAlive
make sure it is always running.
Parameters: `name, working-path, command, arguments, processName`

### environment variables
Sets custom environment variables for the process being launched 
parameters: `variable, value`

### Example XML configuration file
```xml
<?xml version="1.0" encoding="utf-8"?>
<WinServiceLauncher version="0.0.0.42">
  <Launcher name="Node.JS Web App" 
            working-path="c:\my-node-app\" 
            command="C:\Program Files\nodejs\npm.cmd" 
            arguments="start">
    <Environment variable="PORT" value="8080" />
    <Environment variable="DATABASE" value="mongodb://localhost:27017/dbname" />
    <OnStartup/>
  </Launcher>
</WinServiceLauncher>
```

## dev environment
- [SharpDevelop](http://www.icsharpcode.net/OpenSource/SD/Download/Default.aspx#SharpDevelop5x)
- [Microsoft.NET v3.5](https://dotnet.microsoft.com/download/thank-you/net35-sp1)
- [7zip](https://www.7-zip.org/download.html)
