@ECHO OFF
pushd %~dp0

SET msbuild="%windir%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe"

IF '%1'=='' (SET target="Build") ELSE (SET target=%1)
IF '%2'=='' (SET buildConfig="Release") ELSE (SET buildConfig=%2)

%msbuild% .\build.proj /t:%target% /property:Configuration=%buildConfig%
if errorlevel 1 goto BuildFail
goto BuildSuccess

:BuildFail
echo.
echo *** BUILD FAILED ***
goto End

:BuildSuccess
echo.
echo **** BUILD SUCCESSFUL ***
goto end

:End
popd