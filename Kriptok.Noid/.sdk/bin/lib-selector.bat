REM Post-Build
REM
REM ..\..\.sdk\bin\lib-selector.bat $(ConfigurationName)

set from=%cd% \..\..\.sdk\bin\%1%
set to=%cd%

 REM echo [Build.log]    > build.log
 REM echo FROM : %from% >> build.log
 REM echo TO   : %to%   >> build.log

copy "%from%\Kriptok.dll" "%to%\Kriptok.dll" /y
copy "%from%\Kriptok.pdb" "%to%\Kriptok.pdb" /y
copy "%from%\Kriptok.xml" "%to%\Kriptok.xml" /y

