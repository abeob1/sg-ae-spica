@ECHO OFF

REM The following directory is for .NET 4.0
set DOTNETFX2=%SystemRoot%\Microsoft.NET\Framework\v4.0.30319
set PATH=%PATH%;%DOTNETFX2%

echo UnInstalling Email Triggering Win Service...
echo ---------------------------------------------------
C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil /u E:\Abeo-Projects\Abeo_Projects_WorkArea\SPICA\BPSYNC\AE_SPICA_WS01.exe

echo ---------------------------------------------------
pause
echo Done.