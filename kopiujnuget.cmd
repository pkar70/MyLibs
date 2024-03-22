@ECHO OFF
ECHO Czy skompilowales wszystkie (ARM,x86,x64,Any)?
PAUSE

SET newestNuget=NO
FOR %%f in (*nupkg) DO SET newestNuget=%%f

IF %newestNuget%==NO GOTO lNoUwp

ECHO kopiuje %newestNuget%
REM nuget add %newestNuget% -source H:\Home\PIOTR\VStudio\PrywatneNugety
COPY %newestNuget% H:\Home\PIOTR\VStudio\PrywatneNugety

GOTO :EOF

:lNoUWP
CD bin

CD Release
IF %1.==deb. CD ..\Debug 
IF %1.==/d. CD ..\Debug 

FOR %%f in (*.nupkg) DO SET newestNuget=%%f

SET newestNuget=%newestNuget:.symbols.nupkg=.nupkg%

ECHO kopiuje %newestNuget%
COPY %newestNuget% H:\Home\PIOTR\VStudio\PrywatneNugety

CD ..
CD ..
