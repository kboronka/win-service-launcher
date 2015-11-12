:BuildEnvironment
	@echo off
	pushd "%~dp0"

:Paths
	set uSAR="release\sar.exe"

	%uSAR% -bower