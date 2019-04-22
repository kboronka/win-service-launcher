:BuildEnvironment
	@echo off
	pushd "%~dp0"
	set SOLUTION=WinServiceLauncher.sln
	set CONFIG=Release
	set BASEPATH=%~dp0

:Paths
	set SAR="libs\sar-tool\sar.exe"
	set ZIP="%PROGRAMFILES%\7-Zip\7zG.exe" a -tzip
	
:Build
	echo "VERSION.MAJOR.MINOR.BUILD".
	set /p VERSION="> "

	call UpdateExternals.bat
	
	svn cleanup
	svn update

	%SAR% -f.bsd \WinServiceLauncher\*.cs "Kevin Boronka"
	%SAR% -assy.ver \sar\AssemblyInfo.* %VERSION%

	%SAR% -f.del WinServiceLauncher\bin\%CONFIG%\*.* /q /svn
	
	echo building binaries
	%SAR% -b.net 3.5 %SOLUTION% /p:Configuration=%CONFIG% /p:Platform=\"x86\"
	if errorlevel 1 goto BuildFailed	

:BuildComplete
	copy WinServiceLauncher\bin\%CONFIG%\*.exe release\*.exe
	copy WinServiceLauncher\bin\%CONFIG%\*.dll release\*.dll
	copy WinServiceLauncher\bin\%CONFIG%\*.pdb release\*.pdb
	copy WinServiceLauncher\WinServiceLauncher.example.xml release\*.xml
	copy LICENSE release\LICENSE
	
	%ZIP% "WinServiceLauncher %VERSION%.zip" .\release\*.*
	
	echo build completed
	popd
	exit /b 0

:BuildFailed
	echo build failed
	pause
	popd
	exit /b 1
