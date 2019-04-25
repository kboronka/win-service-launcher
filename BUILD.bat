@echo off
pushd "%~dp0"

:Paths
	set BASEPATH=%~dp0
	set SAR="libs\sar-tool\sar.exe"
	set ZIP="%PROGRAMFILES%\7-Zip\7zG.exe"

:Build
	echo "VERSION.MAJOR.MINOR.BUILD".
	set /p VERSION="> "

	%SAR% -f.bsd \WinServiceLauncher\*.cs "Kevin Boronka"
	%SAR% -assy.ver \WinServiceLauncher\AssemblyInfo.* %VERSION%

	%SAR% -f.del WinServiceLauncher\bin\Release\*.* /q /svn
	
	echo building binaries
	%SAR% -b.net 3.5 WinServiceLauncher.sln /p:Configuration=Release /p:Platform=\"x86\"
	if errorlevel 1 goto BuildFailed	

:BuildComplete
	copy WinServiceLauncher\bin\Release\*.exe dist\*.exe
	copy WinServiceLauncher\bin\Release\*.dll dist\*.dll
	copy WinServiceLauncher\bin\Release\*.pdb dist\*.pdb
	copy WinServiceLauncher\WinServiceLauncher.example.xml dist\WinServiceLauncher.example.xml
	copy LICENSE dist\LICENSE
	
	%ZIP% a -tzip "WinServiceLauncher %VERSION%.zip" .\dist\*.*
	
	echo build completed
	popd
	exit /b 0

:BuildFailed
	echo build failed
	pause
	popd
	exit /b 1
