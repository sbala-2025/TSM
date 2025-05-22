@echo off
echo Publishing TSM application to IIS...

rem Set variables
set SOURCE_DIR=D:\AI Projects\TSM\IISDeployment\API\publish
set PUBLISH_DIR=D:\PublishedApp
set APP_POOL_NAME=MyAppPool
set SITE_NAME=TSM
set APP_PORT=8080

echo Creating directory %PUBLISH_DIR% if it doesn't exist...
if not exist "%PUBLISH_DIR%" mkdir "%PUBLISH_DIR%"

echo Copying application files to publish directory...
xcopy /E /Y /I "%SOURCE_DIR%\*" "%PUBLISH_DIR%"

echo Creating IIS App Pool if it doesn't exist...
%windir%\system32\inetsrv\appcmd list apppool /name:"%APP_POOL_NAME%" >nul 2>&1
if %ERRORLEVEL% neq 0 (
    echo Creating App Pool %APP_POOL_NAME%...
    %windir%\system32\inetsrv\appcmd add apppool /name:"%APP_POOL_NAME%" /managedRuntimeVersion:v4.0 /managedPipelineMode:Integrated
    %windir%\system32\inetsrv\appcmd set apppool /apppool.name:"%APP_POOL_NAME%" /processModel.identityType:ApplicationPoolIdentity
) else (
    echo App Pool %APP_POOL_NAME% already exists.
)

echo Creating IIS Site if it doesn't exist...
%windir%\system32\inetsrv\appcmd list site /name:"%SITE_NAME%" >nul 2>&1
if %ERRORLEVEL% neq 0 (
    echo Creating Site %SITE_NAME% on port %APP_PORT%...
    %windir%\system32\inetsrv\appcmd add site /name:"%SITE_NAME%" /physicalPath:"%PUBLISH_DIR%" /bindings:http/*:%APP_PORT%:
) else (
    echo Site %SITE_NAME% already exists. Updating settings...
    %windir%\system32\inetsrv\appcmd set site /site.name:"%SITE_NAME%" /physicalPath:"%PUBLISH_DIR%" /bindings:http/*:%APP_PORT%:
)

echo Assigning App Pool to Site...
%windir%\system32\inetsrv\appcmd set app "%SITE_NAME%/" /applicationPool:"%APP_POOL_NAME%"

echo Setting folder permissions...
icacls "%PUBLISH_DIR%" /grant "IIS_IUSRS:(OI)(CI)(RX)" /T
icacls "%PUBLISH_DIR%" /grant "%APP_POOL_NAME%:(OI)(CI)(M)" /T
icacls "%PUBLISH_DIR%" /grant "NETWORK SERVICE:(OI)(CI)(RX)" /T

echo Check if web.config exists...
if not exist "%PUBLISH_DIR%\web.config" (
    echo Creating default web.config...
    echo ^<?xml version="1.0" encoding="UTF-8"?^> > "%PUBLISH_DIR%\web.config"
    echo ^<configuration^> >> "%PUBLISH_DIR%\web.config"
    echo   ^<system.webServer^> >> "%PUBLISH_DIR%\web.config"
    echo     ^<handlers^> >> "%PUBLISH_DIR%\web.config"
    echo       ^<add name="aspNetCore" path="*" verb="*" modules="AspNetCoreModuleV2" resourceType="Unspecified" /^> >> "%PUBLISH_DIR%\web.config"
    echo     ^</handlers^> >> "%PUBLISH_DIR%\web.config"
    echo     ^<aspNetCore processPath="dotnet" arguments=".\TSM.dll" stdoutLogEnabled="false" stdoutLogFile=".\logs\stdout" hostingModel="inprocess" /^> >> "%PUBLISH_DIR%\web.config"
    echo   ^</system.webServer^> >> "%PUBLISH_DIR%\web.config"
    echo ^</configuration^> >> "%PUBLISH_DIR%\web.config"
)

echo Verifying required modules...
%windir%\system32\dism.exe /online /get-featureinfo /featurename:IIS-ASPNET45 | find "State : Enabled" >nul
if %ERRORLEVEL% neq 0 (
    echo Installing ASP.NET features...
    %windir%\system32\dism.exe /online /enable-feature /featurename:IIS-ASPNET45 /all
)

echo Checking for AspNetCoreModule...
%windir%\system32\inetsrv\appcmd list module /name:AspNetCoreModuleV2 >nul 2>&1
if %ERRORLEVEL% neq 0 (
    echo AspNetCoreModuleV2 might not be installed.
    echo Please install the .NET Core Hosting Bundle from:
    echo https://dotnet.microsoft.com/download/dotnet-core
    echo.
    echo Press any key to open the download page in your browser...
    pause >nul
    start https://dotnet.microsoft.com/download/dotnet-core
    echo After installing the hosting bundle, please restart this script.
    echo Press any key to continue anyway...
    pause >nul
)

echo Starting App Pool and Site...
%windir%\system32\inetsrv\appcmd start apppool /apppool.name:"%APP_POOL_NAME%"
%windir%\system32\inetsrv\appcmd start site /site.name:"%SITE_NAME%"

echo Recycling App Pool to apply changes...
%windir%\system32\inetsrv\appcmd recycle apppool /apppool.name:"%APP_POOL_NAME%"

echo TSM application published successfully to IIS.
echo Site URL: http://localhost:%APP_PORT%/
echo.
echo If you still encounter HTTP 500.19 errors, verify that:
echo 1. The correct web.config file exists and has proper permissions
echo 2. All required IIS modules are installed (AspNetCoreModuleV2)
echo 3. The application DLL names in web.config match your actual DLLs

pause 