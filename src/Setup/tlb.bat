FOR /D %%I IN (%0) DO CD /d %%~dpI
%windir%\Microsoft.NET\Framework\v2.0.50727\regasm.exe /tlb Prabir.Cocor.VisualStudio.dll
"c:\Program Files (x86)\Windows Installer XML v3.5\bin\heat" file Prabir.Cocor.VisualStudio.dll --gg -dr INSTALLDIR\bin -o cocor_vs_dll.wxs
"c:\Program Files (x86)\Windows Installer XML v3.5\bin\heat" file Prabir.Cocor.VisualStudio.tlb --gg -dr INSTALLDIR\bin -o cocor_vs_tlb.wxs
pause