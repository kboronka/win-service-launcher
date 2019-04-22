# win-service-launcher
runs as a service and launches non-service applications

## dev environment
[Microsoft.NET v3.5](https://dotnet.microsoft.com/download/thank-you/net35-sp1)
[7zip](https://www.7-zip.org/download.html)
[SharpDevelop](http://www.icsharpcode.net/OpenSource/SD/Download/Default.aspx#SharpDevelop5x)

## how to install


## setting up 'launchers'
### types of launcher schedules
#### OnInterval
Launches process at given intervals
Parameters: `name, working-path, command, arguments, interval (ms)`

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
Launches application process when the service is being started, then continuously monitors the process to make sure it is always running.
Parameters: `name, working-path, command, arguments, processName`

### environment variables
Sets custom environment variables for the process being launched 
parameters: `variable, value`

### Example XML configuration file
```xml
<?xml version="1.0" encoding="utf-8"?>
<WinServiceLauncher version="0.0.0.42">
	<Launcher name="Node.JS Web App" working-path="c:\my-node-app\" command="C:\Program Files\nodejs\npm.cmd" arguments="start">
		<Environment variable="PORT" value="8080" />
		<Environment variable="DATABASE" value="mongodb://localhost:27017/dbname" />
		<OnStartup/>
	</Launcher>
</WinServiceLauncher>
```

