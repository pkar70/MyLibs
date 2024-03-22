@ECHO OFF
ECHO Czy skompilowales wszystkie (ARM,x86,x64,Any)?
PAUSE

FOR %%f in (*nuspec) DO SET newestNuget=%%f
ECHO popraw version, release info

notepad %newestNuget%

PAUSE

nuget pack -Properties Configuration=Release

FOR %%f in (*nupkg) DO SET newestNuget=%%f

REM nuget add %newestNuget% -source H:\Home\PIOTR\VStudio\PrywatneNugety
COPY %newestNuget% H:\Home\PIOTR\VStudio\PrywatneNugety