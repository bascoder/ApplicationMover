:: Name:     junction.cmd
:: Purpose:  Create a directory junction
:: Author:   Bascoder
:: Version: 3.0

@ECHO OFF
SETLOCAL ENABLEEXTENSIONS ENABLEDELAYEDEXPANSION

SET arg_count=0
for %%x in (%*) do SET /A arg_count+=1

ECHO This script is created to move directories between different drives/partitions or other folders.
ECHO.

ECHO ****************************************************************************************************************************************************************
ECHO DISCLAIMER: This script is in beta, any form of damage like file loss will not be compensed by the owner of this script!
ECHO.
ECHO ****************************************************************************************************************************************************************
ECHO.

ECHO argument count: %arg_count%

:: assign command line args or read from stdin
IF %arg_count% EQU 2 (
	ECHO %1
	SET source=%~1
	SET destination=%~2
) ELSE (
	SET /p source= What is the source folder?
	ECHO.
	SET /p destination= What is the destination folder?
	ECHO.
)

ECHO Source is: "%source%"
ECHO.
ECHO Destination: "%destination%"
ECHO.

:: copy files to destination and remove original folder
XCOPY /E /V /I /F /Y "%source%" "%destination%"
CMD /C RD /S /Q "%source%"
CMD /C MKLINK /J "%source%" "%destination%"

IF %ERRORLEVEL% EQU 0 (
	ECHO All tasks completed successfully.
	ECHO.
	GOTO :END
) ELSE (
	ECHO One or several errors were found while executing.
	ECHO.
	ECHO Make sure if directory paths were passed correctly.
	ECHO.
	ECHO Terminating with error code 2...
	ECHO.
	EXIT /B 2
)

:END
ENDLOCAL
ECHO ON
@EXIT /B 0
