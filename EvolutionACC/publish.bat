@echo off
title Evolution Activator - Auto Compiler
color 0A

echo.
echo ================================================================
echo                 AUTO COMPILER - EVOLUTION ACTIVATOR                
echo                         Build System v1.0                       
echo ================================================================
echo.

:: Verificar si existe el archivo fuente
if not exist "attack.cs" (
    echo [ERROR] No se encontro el archivo attack.cs
    echo [INFO] Asegurate de que attack.cs este en el mismo directorio
    pause
    exit /b 1
)

echo [+] Archivo fuente encontrado: attack.cs
echo [+] Iniciando proceso de compilacion...
echo.

:: Buscar el compilador de C# (.NET Framework)
set "csc_path="
for /f "tokens=*" %%i in ('where csc.exe 2^>nul') do set "csc_path=%%i"

if "%csc_path%"=="" (
    echo [INFO] Buscando compilador de .NET Framework...
    
    :: Buscar en ubicaciones comunes de .NET Framework
    if exist "%WINDIR%\Microsoft.NET\Framework64\v4.0.30319\csc.exe" (
        set "csc_path=%WINDIR%\Microsoft.NET\Framework64\v4.0.30319\csc.exe"
    ) else if exist "%WINDIR%\Microsoft.NET\Framework\v4.0.30319\csc.exe" (
        set "csc_path=%WINDIR%\Microsoft.NET\Framework\v4.0.30319\csc.exe"
    ) else (
        echo [ERROR] No se encontro el compilador de C#
        echo [INFO] Instala .NET Framework Developer Pack o Visual Studio
        pause
        exit /b 1
    )
)

echo [+] Compilador encontrado: %csc_path%
echo.

:: Compilar el archivo
echo [+] Compilando attack.cs...
"%csc_path%" /target:exe /out:attack.exe /optimize+ /platform:anycpu attack.cs

if %errorlevel% equ 0 (
    echo.
    echo [SUCCESS] Compilacion exitosa!
    echo [+] Archivo generado: attack.exe
    
    :: Mostrar informacion del archivo
    if exist "attack.exe" (
        echo [+] Tamano del archivo: 
        for %%A in (attack.exe) do echo     %%~zA bytes
        echo [+] Fecha de creacion: 
        for %%A in (attack.exe) do echo     %%~tA
    )
    
    echo.
    echo ================================================================
    echo                        COMPILACION EXITOSA                       
    echo                                                                  
    echo   El archivo attack.exe ha sido generado correctamente.           
    echo   Puedes ejecutarlo con los siguientes comandos:                  
    echo                                                                  
    echo   attack.exe help      - Mostrar ayuda                            
    echo   attack.exe activate  - Activar Windows                          
    echo   attack.exe status    - Verificar estado                         
    echo   attack.exe full      - Proceso completo                         
    echo ================================================================
    
) else (
    echo.
    echo [ERROR] Error durante la compilacion
    echo [INFO] Revisa el codigo fuente para errores de sintaxis
)

echo.
echo [INFO] Presiona cualquier tecla para salir...
pause >nul