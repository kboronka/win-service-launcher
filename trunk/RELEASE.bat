:SoftwareRequired
	:: Microsoft.NET v2.0						http://www.filehippo.com/download_dotnet_framework_2/
	:: Microsoft.NET v3.5						http://www.filehippo.com/download_dotnet_framework_3/
	:: TortoiseSVN (+command line tools)		http://www.filehippo.com/download_tortoisesvn/
	:: 7zip 32bit								http://www.filehippo.com/download_7zip_32/
	:: 7zip 64bit								http://www.filehippo.com/download_7-zip_64/
	:: SharpDevelop v3.2.1.6466					http://sourceforge.net/projects/sharpdevelop/files/SharpDevelop%203.x/3.2/SharpDevelop_3.2.1.6466_Setup.msi/download

:DownloadLink
	:: GoogleCode://code.google.com/p/sar-tool/downloads/list
	:: SourceForge: http://sourceforge.net/projects/sartool/files/

:BuildEnvironment
	@echo off
	pushd "%~dp0"
	set SOLUTION=WinServiceLauncher.sln
	set REPO=https://win-service-launcher.googlecode.com/svn
	set CONFIG=Release
	set BASEPATH=%~dp0

:Paths
	set SAR="lib\sar\sar.exe"
	set ZIP="%PROGRAMFILES%\7-Zip\7zG.exe" a -tzip

	
:Build
	echo "VERSION.MAJOR.MINOR.BUILD".
	set /p VERSION="> "

	svn cleanup
	svn update

	%SAR% -f.d  ".\WinServiceLauncherInstaller\bin\%CONFIG%\*.*" /q /svn
	%SAR% -f.bsd "\WinServiceLauncher\*.cs" "Kevin Boronka"
	%SAR% -f.bsd "\WinServiceLauncherInstaller\*.cs" "Kevin Boronka"
	%SAR% -f.bsd "\WinServiceLauncherSetup\*.cs" "Kevin Boronka"
	%SAR% -f.bsd "\WinServiceLauncherTester\*.cs" "Kevin Boronka"
	
	%SAR% -assy.ver "\WinServiceLauncher\AssemblyInfo.*" %VERSION%
	%SAR% -assy.ver "\WinServiceLauncherInstaller\AssemblyInfo.*" %VERSION%
	%SAR% -assy.ver "\WinServiceLauncherSetup\AssemblyInfo.*" %VERSION%
	%SAR% -assy.ver "\WinServiceLauncherTester\AssemblyInfo.*" %VERSION%

	echo building binaries
	%SAR% -b.net 4.0 %SOLUTION% /p:Configuration=%CONFIG% /p:Platform=\"x86\"
	if errorlevel 1 goto BuildFailed

	
:BuildComplete
	%ZIP% "WinServiceLauncher v%VERSION%.zip" .\WinServiceLauncherInstaller\bin\%CONFIG%\*.*
	
	svn commit -m "version %VERSION%"
	svn copy %REPO%/trunk %REPO%/tags/%VERSION% -m "version %VERSION% release"
	svn update

	echo build completed
	popd
	exit /b 0


:BuildFailed
	echo build failed
	pause
	popd
	exit /b 1	
