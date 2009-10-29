@echo off
SET DOTNET2PATH="%windir%\microsoft.net\framework\v2.0.50727"
%DOTNET2PATH%\REGASM /u "%~dp0prabir.vscocor.dll"
del "%~dp0prabir.vscocor.tlb"
pause