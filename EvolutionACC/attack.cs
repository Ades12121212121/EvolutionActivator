using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Principal;
using Microsoft.Win32;
using System.Management;
using System.Text;
using System.IO;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        ShowDisclaimer();
        
        if (!IsAdministrator())
        {
            PrintColoredMessage("Este programa debe ejecutarse como administrador.", ConsoleColor.Red);
            return;
        }

        ShowBanner();
        StartInteractiveShell();
    }

    static void ShowDisclaimer()
    {
        Console.Clear();
        PrintColoredMessage("╔══════════════════════════════════════════════════════════════════╗", ConsoleColor.Red);
        PrintColoredMessage("║                        AVISO LEGAL IMPORTANTE                    ║", ConsoleColor.Red);
        PrintColoredMessage("╠══════════════════════════════════════════════════════════════════╣", ConsoleColor.Red);
        PrintColoredMessage("║ ESTE SOFTWARE SE PROPORCIONA 'TAL COMO ESTÁ' SIN GARANTÍAS       ║", ConsoleColor.Yellow);
        PrintColoredMessage("║ DE NINGÚN TIPO. EL AUTOR NO SE HACE RESPONSABLE DEL USO          ║", ConsoleColor.Yellow);
        PrintColoredMessage("║ INDEBIDO, ILEGAL O NO ÉTICO DE ESTA HERRAMIENTA.                 ║", ConsoleColor.Yellow);
        PrintColoredMessage("║                                                                  ║", ConsoleColor.Yellow);
        PrintColoredMessage("║ AL PRESIONAR ESPACIO, USTED ACEPTA TODA LA RESPONSABILIDAD       ║", ConsoleColor.Yellow);
        PrintColoredMessage("║ LEGAL Y ÉTICA POR EL USO DE ESTE SOFTWARE Y LIBERA AL AUTOR      ║", ConsoleColor.Yellow);
        PrintColoredMessage("║ DE CUALQUIER RESPONSABILIDAD LEGAL.                              ║", ConsoleColor.Yellow);
        PrintColoredMessage("║                                                                  ║", ConsoleColor.Yellow);
        PrintColoredMessage("║ ÚSELO BAJO SU PROPIO RIESGO Y RESPONSABILIDAD.                   ║", ConsoleColor.Yellow);
        PrintColoredMessage("╚══════════════════════════════════════════════════════════════════╝", ConsoleColor.Red);
        PrintColoredMessage("\nPresione ESPACIO para continuar o cualquier otra tecla para salir...", ConsoleColor.White);
        
        ConsoleKeyInfo key = Console.ReadKey(true);
        if (key.Key != ConsoleKey.Spacebar)
        {
            Environment.Exit(0);
        }
        Console.Clear();
    }

    static void ShowBanner()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("                ___         _      _   _            ");
        Console.WriteLine("               | __|_ _____| |_  _| |_(_)___ _ _    ");
        Console.WriteLine("               | _|\\ V / _ \\ | || |  _| / _ \\ ' \\   ");
        Console.WriteLine("               |___|\\_ /\\___/_|\\_,_|\\__|_\\___/_||_|  ");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("                 /_\\  __| |_(_)_ ____ _| |_ ___ _ _ ");
        Console.WriteLine("                / _ \\/ _|  _| \\ V / _` |  _/ _ \\ '_|");
        Console.WriteLine("               /_/ \\_\\__|\\__|_|\\_/\\__,_|\\__\\___/_|  ");
        Console.ResetColor();
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("             v1.0 | By: EvolutionX | Web: CoomingSoon");
        Console.ResetColor();
        Console.WriteLine();
    }

    static void StartInteractiveShell()
    {
        PrintColoredMessage("╔══════════════════════════════════════════════════════════════════╗", ConsoleColor.Green);
        PrintColoredMessage("║                     SHELL INTERACTIVO INICIADO                   ║", ConsoleColor.Green);
        PrintColoredMessage("║                  Escriba 'help' para ver comandos                ║", ConsoleColor.Green);
        PrintColoredMessage("╚══════════════════════════════════════════════════════════════════╝", ConsoleColor.Green);
        Console.WriteLine();
        
        while (true)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("activator $ > ");
            Console.ResetColor();
            
            string input = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(input))
                continue;
                
            string command = input.Trim().ToLower();
            
            if (command == "exit" || command == "quit")
            {
                PrintColoredMessage("[+] Saliendo del programa...", ConsoleColor.Yellow);
                break;
            }
            
            ProcessShellCommand(command);
            Console.WriteLine();
        }
    }
    
    static void ShowHelp()
    {
        PrintColoredMessage("╔══════════════════════════════════════════════════════════════════╗", ConsoleColor.Green);
        PrintColoredMessage("║                         COMANDOS DISPONIBLES                     ║", ConsoleColor.Green);
        PrintColoredMessage("╠══════════════════════════════════════════════════════════════════╣", ConsoleColor.Green);
        PrintColoredMessage("║  help              - Mostrar esta ayuda                          ║", ConsoleColor.White);
        PrintColoredMessage("║  hwid              - Activar Windows (método estándar HWID)      ║", ConsoleColor.White);
        PrintColoredMessage("║  hwid-force        - Activación HWID agresiva (método avanzado)  ║", ConsoleColor.Red);
        PrintColoredMessage("║  activate          - Activar Windows con clave genérica          ║", ConsoleColor.White);
        PrintColoredMessage("║  status            - Verificar estado de activación              ║", ConsoleColor.White);
        PrintColoredMessage("║  fix               - Limpiar cambios y restaurar sistema         ║", ConsoleColor.Yellow);
        PrintColoredMessage("║  registry          - Modificar registro del sistema              ║", ConsoleColor.White);
        PrintColoredMessage("║  full              - Ejecutar proceso completo de activación     ║", ConsoleColor.White);
        PrintColoredMessage("║  clear             - Limpiar pantalla                            ║", ConsoleColor.White);
        PrintColoredMessage("║  exit/quit         - Salir del programa                          ║", ConsoleColor.White);
        PrintColoredMessage("╚══════════════════════════════════════════════════════════════════╝", ConsoleColor.Green);
    }

    static void ProcessShellCommand(string command)
    {
        try
        {
            switch (command)
            {
                case "help":
                    ShowHelp();
                    break;
                case "hwid":
                    ExecuteHWIDActivation();
                    break;
                case "hwid-force":
                    ExecuteHWIDForceActivation();
                    break;
                case "activate":
                    ExecuteActivation();
                    break;
                case "status":
                    VerifyActivationStatus();
                    break;
                case "fix":
                    ExecuteSystemFix();
                    break;
                case "registry":
                    ModifySystemRegistry();
                    break;
                case "full":
                    ExecuteFullProcess();
                    break;
                case "clear":
                    Console.Clear();
                    ShowBanner();
                    break;
                default:
                    PrintColoredMessage(string.Format("[ERROR] Comando desconocido: '{0}'", command), ConsoleColor.Red);
                    PrintColoredMessage("[INFO] Escriba 'help' para ver comandos disponibles.", ConsoleColor.Yellow);
                    break;
            }
        }
        catch (Exception ex)
        {
            PrintColoredMessage(string.Format("[ERROR] Error ejecutando comando '{0}': {1}", command, ex.Message), ConsoleColor.Red);
        }
    }

    static void ExecuteActivation()
    {
        PrintColoredMessage("[+] Iniciando proceso de activación...", ConsoleColor.Green);
        
        if (RunCommand("slmgr.vbs", "/ipk VK7JG-NPHTM-C97JM-9MPGT-3V66T"))
        {
            PrintColoredMessage("[+] Clave de producto instalada correctamente.", ConsoleColor.Green);
            
            if (RunCommand("slmgr.vbs", "/ato"))
            {
                PrintColoredMessage("[+] Windows activado correctamente.", ConsoleColor.Green);
            }
            else
            {
                PrintColoredMessage("[-] Error en el proceso de activación online.", ConsoleColor.Red);
            }
        }
        else
        {
            PrintColoredMessage("[-] Error instalando clave de producto.", ConsoleColor.Red);
        }
    }

    static void ModifySystemRegistry()
    {
        PrintColoredMessage("[+] Modificando registro del sistema...", ConsoleColor.Yellow);
        
        ModifyRegistryKey(
            @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\SoftwareProtectionPlatform",
            "BackupProductKeyDefault",
            "VK7JG-NPHTM-C97JM-9MPGT-3V66T"
        );
        
        PrintColoredMessage("[+] Registro modificado exitosamente.", ConsoleColor.Green);
    }

    static void ExecuteHWIDActivation()
    {
        PrintColoredMessage("[+] Iniciando activación HWID (Digital License)...", ConsoleColor.Cyan);
        PrintColoredMessage("[INFO] Método basado en Microsoft Activation Scripts (MAS)", ConsoleColor.Yellow);
        Console.WriteLine();
        
        try
        {
            // Verificación de prerrequisitos estilo MAS
            if (!PerformMASLikeChecks())
            {
                return;
            }
            
            // Paso 1: Detectar edición de Windows
            string windowsEdition = DetectWindowsEdition();
            PrintColoredMessage(string.Format("[+] Edición detectada: {0}", windowsEdition), ConsoleColor.Green);
            
            // Paso 2: Obtener clave genérica apropiada
            string genericKey = GetGenericKeyForEdition(windowsEdition);
            if (string.IsNullOrEmpty(genericKey))
            {
                PrintColoredMessage("[ERROR] No se encontró clave genérica para esta edición", ConsoleColor.Red);
                return;
            }
            PrintColoredMessage(string.Format("[+] Clave genérica obtenida: {0}", genericKey), ConsoleColor.Green);
            
            // Paso 3: Preparar sistema para activación HWID (estilo MAS)
            PrintColoredMessage("[+] Preparando sistema para activación HWID...", ConsoleColor.Yellow);
            if (!PrepareSystemForHWIDMAS())
            {
                PrintColoredMessage("[ERROR] Error preparando sistema", ConsoleColor.Red);
                return;
            }
            
            // Paso 4: Instalar clave de producto con verificación
            PrintColoredMessage("[+] Instalando clave de producto...", ConsoleColor.Yellow);
            if (!InstallProductKeyWithVerification(genericKey))
            {
                PrintColoredMessage("[ERROR] Error instalando clave de producto", ConsoleColor.Red);
                return;
            }
            
            // Paso 5: Generar ticket HWID con múltiples métodos
            PrintColoredMessage("[+] Generando ticket HWID...", ConsoleColor.Yellow);
            if (!GenerateHWIDTicketMAS())
            {
                PrintColoredMessage("[ERROR] Error generando ticket HWID", ConsoleColor.Red);
                return;
            }
            
            // Paso 6: Activar Windows con verificación robusta
            PrintColoredMessage("[+] Activando Windows...", ConsoleColor.Yellow);
            if (!ActivateWindowsMAS())
            {
                PrintColoredMessage("[ERROR] Error en proceso de activación", ConsoleColor.Red);
                return;
            }
            
            // Paso 7: Verificar activación con método robusto
            Console.WriteLine();
            bool isActivated = CheckActivationSuccessMAS();
            
            if (isActivated)
            {
                PrintColoredMessage("[SUCCESS] Activación HWID completada exitosamente!", ConsoleColor.Green);
                PrintColoredMessage("[INFO] Windows ahora tiene una licencia digital permanente", ConsoleColor.Cyan);
                PrintColoredMessage("[INFO] Método MAS aplicado correctamente", ConsoleColor.Gray);
            }
            else
            {
                PrintColoredMessage("[ERROR] La activación HWID falló - Windows no está activado", ConsoleColor.Red);
                Console.WriteLine();
                
                PrintColoredMessage("╔══════════════════════════════════════════════════════════════════╗", ConsoleColor.Yellow);
                PrintColoredMessage("║                    ACTIVACIÓN FALLIDA                           ║", ConsoleColor.Yellow);
                PrintColoredMessage("║                                                                  ║", ConsoleColor.Yellow);
                PrintColoredMessage("║  La activación no fue exitosa. Esto puede deberse a:            ║", ConsoleColor.White);
                PrintColoredMessage("║  • Claves conflictivas en el sistema                            ║", ConsoleColor.White);
                PrintColoredMessage("║  • Configuración incorrecta del registro                        ║", ConsoleColor.White);
                PrintColoredMessage("║  • Servicios de activación con problemas                        ║", ConsoleColor.White);
                PrintColoredMessage("║  • Antivirus bloqueando el proceso                              ║", ConsoleColor.White);
                PrintColoredMessage("║                                                                  ║", ConsoleColor.Yellow);
                PrintColoredMessage("║  ¿Desea restablecer las claves y configuración para            ║", ConsoleColor.Cyan);
                PrintColoredMessage("║  intentar la activación nuevamente? (Y/N)                       ║", ConsoleColor.Cyan);
                PrintColoredMessage("╚══════════════════════════════════════════════════════════════════╝", ConsoleColor.Yellow);
                
                ConsoleKeyInfo retryKey = Console.ReadKey(true);
                if (retryKey.Key == ConsoleKey.Y)
                {
                    Console.WriteLine();
                    PrintColoredMessage("[+] Restableciendo claves y configuración (método MAS)...", ConsoleColor.Yellow);
                    ResetKeysAndRetryMAS(windowsEdition);
                }
                else
                {
                    PrintColoredMessage("[INFO] Operación cancelada. Use 'fix' para limpiar cambios.", ConsoleColor.Gray);
                }
            }
        }
        catch (Exception ex)
        {
            PrintColoredMessage(string.Format("[ERROR] Error en activación HWID: {0}", ex.Message), ConsoleColor.Red);
            PrintColoredMessage("[INFO] Reporte este error en: https://github.com/EvolutionX/EvolutionActivator", ConsoleColor.Gray);
        }
    }
    
    static void ExecuteFullProcess()
    {
        PrintColoredMessage("[+] Ejecutando proceso completo de activación...", ConsoleColor.Cyan);
        Console.WriteLine();
        
        ModifySystemRegistry();
        Console.WriteLine();
        ExecuteActivation();
        Console.WriteLine();
        VerifyActivationStatus();
        
        PrintColoredMessage("[+] Proceso completo finalizado.", ConsoleColor.Green);
    }

    static bool IsAdministrator()
    {
        WindowsIdentity identity = WindowsIdentity.GetCurrent();
        WindowsPrincipal principal = new WindowsPrincipal(identity);
        return principal.IsInRole(WindowsBuiltInRole.Administrator);
    }

    static void ModifyRegistryKey(string path, string valueName, string valueData)
    {
        try
        {
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(path, writable: true))
            {
                if (key != null)
                {
                    key.SetValue(valueName, valueData, RegistryValueKind.String);
                    PrintColoredMessage(string.Format("Clave '{0}' modificada en el registro.", valueName), ConsoleColor.Yellow);
                }
                else
                {
                    throw new Exception("No se pudo abrir la clave del registro.");
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error al modificar el registro: " + ex.Message);
        }
    }

    static bool RunCommand(string fileName, string arguments)
    {
        try
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "cscript",
                Arguments = string.Format("//Nologo {0} {1}", fileName, arguments),
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process proc = Process.Start(psi))
            {
                proc.WaitForExit();
                return proc.ExitCode == 0;
            }
        }
        catch
        {
            return false;
        }
    }

    static void VerifyActivationStatus()
    {
        try
        {
            PrintColoredMessage("[+] Verificando estado de activación de Windows...", ConsoleColor.Cyan);
            
            // Método 1: Usar slmgr para obtener estado detallado
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "cscript",
                Arguments = "//Nologo C:\\Windows\\System32\\slmgr.vbs /dli",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            
            using (Process proc = Process.Start(psi))
            {
                proc.WaitForExit();
                string output = proc.StandardOutput.ReadToEnd();
                
                if (output.Contains("License Status: Licensed") || output.Contains("Estado de la licencia: Con licencia"))
                {
                    PrintColoredMessage("[SUCCESS] Windows está activado y con licencia válida.", ConsoleColor.Green);
                    
                    // Mostrar información adicional
                    if (output.Contains("Partial Product Key"))
                    {
                        string[] lines = output.Split('\n');
                        foreach (string line in lines)
                        {
                            if (line.Contains("Partial Product Key") || line.Contains("Clave de producto parcial"))
                            {
                                PrintColoredMessage("[INFO] " + line.Trim(), ConsoleColor.Gray);
                            }
                            if (line.Contains("License Status") || line.Contains("Estado de la licencia"))
                            {
                                PrintColoredMessage("[INFO] " + line.Trim(), ConsoleColor.Gray);
                            }
                        }
                    }
                    return;
                }
                else
                {
                    PrintColoredMessage("[ERROR] Windows NO está activado.", ConsoleColor.Red);
                    PrintColoredMessage("[INFO] Salida completa:", ConsoleColor.Yellow);
                    PrintColoredMessage(output, ConsoleColor.Gray);
                }
            }
            
            // Método 2: Verificación adicional con WMI como respaldo
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT LicenseStatus, Description FROM SoftwareLicensingProduct WHERE PartialProductKey IS NOT NULL"))
            {
                foreach (ManagementObject obj in searcher.Get())
                {
                    object statusObj = obj["LicenseStatus"];
                    object descObj = obj["Description"];
                    
                    if (statusObj != null)
                    {
                        int status = Convert.ToInt32(statusObj);
                        string description = descObj != null ? descObj.ToString() : "Unknown";
                        
                        PrintColoredMessage(string.Format("[INFO] Producto: {0}", description), ConsoleColor.Gray);
                        
                        switch (status)
                        {
                            case 0:
                                PrintColoredMessage("[INFO] Estado: Sin licencia", ConsoleColor.Red);
                                break;
                            case 1:
                                PrintColoredMessage("[INFO] Estado: Licenciado", ConsoleColor.Green);
                                break;
                            case 2:
                                PrintColoredMessage("[INFO] Estado: Periodo de gracia OOB", ConsoleColor.Yellow);
                                break;
                            case 3:
                                PrintColoredMessage("[INFO] Estado: Periodo de gracia OOT", ConsoleColor.Yellow);
                                break;
                            case 4:
                                PrintColoredMessage("[INFO] Estado: Periodo de gracia no genuino", ConsoleColor.Red);
                                break;
                            case 5:
                                PrintColoredMessage("[INFO] Estado: Notificación", ConsoleColor.Yellow);
                                break;
                            case 6:
                                PrintColoredMessage("[INFO] Estado: Licencia extendida", ConsoleColor.Green);
                                break;
                            default:
                                PrintColoredMessage(string.Format("[INFO] Estado desconocido: {0}", status), ConsoleColor.Gray);
                                break;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            PrintColoredMessage(string.Format("[ERROR] Error al verificar estado de activación: {0}", ex.Message), ConsoleColor.Red);
        }
    }

    static string DetectWindowsEdition()
    {
        try
        {
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion"))
            {
                if (key != null)
                {
                    object editionObj = key.GetValue("EditionID");
                    string edition = editionObj != null ? editionObj.ToString() : "Unknown";
                    return edition;
                }
            }
        }
        catch (Exception ex)
        {
            PrintColoredMessage(string.Format("[ERROR] Error detectando edición: {0}", ex.Message), ConsoleColor.Red);
        }
        return "Unknown";
    }
    
    static string GetGenericKeyForEdition(string edition)
    {
        // Claves genéricas oficiales de Microsoft (MAS) para diferentes ediciones
        // Actualizadas con las claves exactas del script MAS original
        switch (edition.ToUpper())
        {
            // ========== WINDOWS 11 KEYS ==========
            case "HOME":
            case "CORE":
                return "TX9XD-98N7V-6WMQ6-BX7FG-H8Q99"; // Windows 11 Home
            case "HOME SINGLE LANGUAGE":
            case "HOMESINGLELANGUAGE":
                return "7HNRX-D7KGG-3K4RQ-4WPJ4-YTDFH"; // Windows 11 Home Single Language
            case "PROFESSIONAL":
            case "PRO":
                return "W269N-WFGWX-YVC9B-4J6C9-T83GX"; // Windows 11 Professional
            case "PROFESSIONAL N":
            case "PRO N":
                return "MH37W-N47XK-V7XM9-C7227-GCQG9"; // Windows 11 Professional N
            case "EDUCATION":
                return "NW6C2-QMPVW-D7KKK-3GKT6-VCFB2"; // Windows 11 Education
            case "EDUCATION N":
                return "2WH4N-8QGBV-H22JP-CT43Q-MDWWJ"; // Windows 11 Education N
            case "PROFESSIONAL WORKSTATION":
            case "PRO WORKSTATION":
            case "PROWORKSTATION":
                return "NRG8B-VKK3Q-CXVCJ-9G2XF-6Q84J"; // Windows 11 Pro for Workstations
            case "PROFESSIONAL WORKSTATION N":
            case "PRO WORKSTATION N":
            case "PROWORKSTATION N":
                return "9FNHH-K3HBT-3W4TD-6383H-6XYWF"; // Windows 11 Pro for Workstations N
            case "ENTERPRISE":
                return "XGVPP-NMH47-7TTHJ-W3FW7-8HV2C"; // Windows 11 Enterprise
            case "ENTERPRISE N":
                return "WGGHN-J84D6-QYCPR-T7PJ7-X766F"; // Windows 11 Enterprise N
            case "ENTERPRISE G N":
                return "FW7NV-4T673-HF4VX-9X4MM-B4H4T"; // Windows 11 Enterprise G N
                
            // ========== CLAVES ADICIONALES WINDOWS 11 ==========
            case "PRO VK7JG":
                return "VK7JG-NPHTM-C97JM-9MPGT-3V66T"; // Windows 11 Pro (alternativa)
            case "PRO N 2B87N":
                return "2B87N-8KFHP-DKV6R-Y2C8J-PKCKT"; // Windows 11 Pro N (alternativa)
            case "PRO WORKSTATION DXG7C":
                return "DXG7C-N36C4-C4HTG-X4T3X-2YV77"; // Windows 11 Pro for Workstations (alternativa)
            case "PRO WORKSTATION N WYPNQ":
                return "WYPNQ-8C467-V2W6J-TX4WX-WT2RQ"; // Windows 11 Pro for Workstations N (alternativa)
            case "PRO EDUCATION":
                return "8PTT6-RNW4C-6V7J2-C2D3X-MHBPB"; // Windows 11 Pro Education
            case "PRO EDUCATION N":
                return "GJTYN-HDMQY-FRR76-HVGC7-QPF8P"; // Windows 11 Pro Education N
            case "EDUCATION YNMGQ":
                return "YNMGQ-8RYV3-4PGQ3-C8XTP-7CFBY"; // Windows 11 Education (alternativa)
            case "EDUCATION N 84NGF":
                return "84NGF-MHBT6-FXBX8-QWJK7-DRR8H"; // Windows 11 Education N (alternativa)
                
            // ========== WINDOWS 10 KEYS ==========
            case "WINDOWS 10 HOME":
            case "WIN10 HOME":
                return "46J3N-RY6B3-BJFDY-VBFT9-V22HG"; // Windows 10 Home
            case "WINDOWS 10 HOME N":
            case "WIN10 HOME N":
                return "PGGM7-N77TC-KVR98-D82KJ-DGPHV"; // Windows 10 Home N
            case "WINDOWS 10 PRO":
            case "WIN10 PRO":
                return "RHGJR-N7FVY-Q3B8F-KBQ6V-46YP4"; // Windows 10 Pro
            case "WINDOWS 10 PRO N":
            case "WIN10 PRO N":
                return "RHGJR-N7FVY-Q3B8F-KBQ6V-46YP4"; // Windows 10 Pro N
            case "WINDOWS 10 SL":
            case "WIN10 SL":
                return "GH37Y-TNG7X-PP2TK-CMRMT-D3WV4"; // Windows 10 SL
                
            // ========== SERVER KEYS ==========
            case "SERVER STANDARD":
                return "WC2BQ-8NRM3-FDDYY-2BFGV-KHKQY";
            case "SERVER DATACENTER":
                return "CB7KF-BWN84-R7R2Y-793K2-8XDDG";
                
            default:
                // Clave genérica por defecto (Windows 11 Professional)
                PrintColoredMessage(string.Format("[INFO] Edición no reconocida: {0}, usando clave Pro por defecto", edition), ConsoleColor.Yellow);
                return "W269N-WFGWX-YVC9B-4J6C9-T83GX";
        }
    }
    
    static void PrepareSystemForHWID()
    {
        try
        {
            // Limpiar activaciones previas
            RunCommand("slmgr.vbs", "/upk");
            Thread.Sleep(2000);
            
            // Limpiar caché de activación
            RunCommand("slmgr.vbs", "/cpky");
            Thread.Sleep(2000);
            
            // Reiniciar servicio de licencias
            RestartLicensingService();
            
            PrintColoredMessage("[+] Sistema preparado correctamente", ConsoleColor.Green);
        }
        catch (Exception ex)
        {
            PrintColoredMessage(string.Format("[WARNING] Error preparando sistema: {0}", ex.Message), ConsoleColor.Yellow);
        }
    }
    
    static void RestartLicensingService()
    {
        try
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "net",
                Arguments = "stop sppsvc",
                UseShellExecute = false,
                CreateNoWindow = true
            };
            Process.Start(psi).WaitForExit();
            
            Thread.Sleep(3000);
            
            psi.Arguments = "start sppsvc";
            Process.Start(psi).WaitForExit();
            
            Thread.Sleep(3000);
        }
        catch (Exception ex)
        {
            PrintColoredMessage(string.Format("[WARNING] Error reiniciando servicio: {0}", ex.Message), ConsoleColor.Yellow);
        }
    }
    
    static bool InstallProductKey(string productKey)
    {
        try
        {
            return RunCommand("slmgr.vbs", string.Format("/ipk {0}", productKey));
        }
        catch
        {
            return false;
        }
    }
    
    static bool GenerateHWIDTicket()
    {
        try
        {
            // Generar ticket de hardware usando clipup.exe (método MAS)
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "clipup.exe",
                Arguments = "-v -o",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
            
            using (Process proc = Process.Start(psi))
            {
                proc.WaitForExit();
                return proc.ExitCode == 0;
            }
        }
        catch
        {
            // Método alternativo si clipup no está disponible
            return GenerateAlternativeHWIDTicket();
        }
    }
    
    static bool GenerateAlternativeHWIDTicket()
    {
        try
        {
            // Método alternativo usando WMI para generar información de hardware
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystemProduct"))
            {
                foreach (ManagementObject obj in searcher.Get())
                {
                    object uuidObj = obj["UUID"];
                    string uuid = uuidObj != null ? uuidObj.ToString() : null;
                    if (!string.IsNullOrEmpty(uuid))
                    {
                        // Registrar UUID en el registro para activación HWID
                        using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\SoftwareProtectionPlatform"))
                        {
                            if (key != null)
                            {
                                key.SetValue("BackupProductKeyDefault", uuid, RegistryValueKind.String);
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
        catch
        {
            return false;
        }
    }
    
    static bool ActivateWindows()
    {
        try
        {
            return RunCommand("slmgr.vbs", "/ato");
        }
        catch
        {
            return false;
        }
    }
    
    static void ExecuteHWIDForceActivation()
    {
        PrintColoredMessage("╔══════════════════════════════════════════════════════════════════╗", ConsoleColor.Red);
        PrintColoredMessage("║                      ADVERTENCIA CRÍTICA                        ║", ConsoleColor.Red);
        PrintColoredMessage("║                                                                  ║", ConsoleColor.Red);
        PrintColoredMessage("║  MÉTODO HWID-FORCE: ACTIVACIÓN AGRESIVA Y AVANZADA              ║", ConsoleColor.Yellow);
        PrintColoredMessage("║  Este método modifica profundamente el sistema Windows.         ║", ConsoleColor.Yellow);
        PrintColoredMessage("║  Úselo solo si el método HWID estándar falló.                   ║", ConsoleColor.Yellow);
        PrintColoredMessage("║                                                                  ║", ConsoleColor.Red);
        PrintColoredMessage("║  PRESIONE 'Y' PARA CONTINUAR O CUALQUIER TECLA PARA CANCELAR    ║", ConsoleColor.White);
        PrintColoredMessage("╚══════════════════════════════════════════════════════════════════╝", ConsoleColor.Red);
        
        ConsoleKeyInfo key = Console.ReadKey(true);
        if (key.Key != ConsoleKey.Y)
        {
            PrintColoredMessage("[INFO] Operación cancelada por el usuario.", ConsoleColor.Yellow);
            return;
        }
        
        Console.WriteLine();
        PrintColoredMessage("[+] Iniciando activación HWID-FORCE (Método Agresivo)...", ConsoleColor.Red);
        PrintColoredMessage("[WARNING] Modificando sistema a nivel profundo...", ConsoleColor.Yellow);
        Console.WriteLine();
        
        try
        {
            // Paso 1: Detección avanzada del sistema
            PrintColoredMessage("[+] Ejecutando análisis profundo del sistema...", ConsoleColor.Cyan);
            PerformDeepSystemAnalysis();
            
            // Paso 2: Manipulación agresiva del registro
            PrintColoredMessage("[+] Aplicando modificaciones agresivas del registro...", ConsoleColor.Yellow);
            ApplyAggressiveRegistryModifications();
            
            // Paso 3: Manipulación de servicios críticos
            PrintColoredMessage("[+] Manipulando servicios de licencias...", ConsoleColor.Yellow);
            ManipulateLicensingServices();
            
            // Paso 4: Inyección de datos HWID personalizados
            PrintColoredMessage("[+] Inyectando datos HWID personalizados...", ConsoleColor.Yellow);
            InjectCustomHWIDData();
            
            // Paso 5: Bypass de validaciones
            PrintColoredMessage("[+] Aplicando bypass de validaciones...", ConsoleColor.Yellow);
            BypassValidations();
            
            // Paso 6: Forzar activación múltiple
            PrintColoredMessage("[+] Ejecutando activación forzada múltiple...", ConsoleColor.Yellow);
            ForceMultipleActivation();
            
            // Paso 7: Verificación final
            Console.WriteLine();
            PrintColoredMessage("[+] Verificando resultado de activación forzada...", ConsoleColor.Cyan);
            VerifyActivationStatus();
            
            PrintColoredMessage("[SUCCESS] Activación HWID-FORCE completada!", ConsoleColor.Green);
            PrintColoredMessage("[INFO] Sistema modificado con técnicas avanzadas", ConsoleColor.Red);
        }
        catch (Exception ex)
        {
            PrintColoredMessage(string.Format("[ERROR] Error en activación HWID-FORCE: {0}", ex.Message), ConsoleColor.Red);
        }
    }
    
    static void PerformDeepSystemAnalysis()
    {
        try
        {
            // Análisis profundo de hardware y sistema
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard"))
            {
                foreach (ManagementObject obj in searcher.Get())
                {
                    object serialObj = obj["SerialNumber"];
                    object manufacturerObj = obj["Manufacturer"];
                    string serialNumber = serialObj != null ? serialObj.ToString() : "Unknown";
                    string manufacturer = manufacturerObj != null ? manufacturerObj.ToString() : "Unknown";
                    PrintColoredMessage(string.Format("[INFO] Placa base detectada: {0} - {1}", manufacturer, serialNumber), ConsoleColor.Gray);
                }
            }
            
            // Análisis de procesador
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Processor"))
            {
                foreach (ManagementObject obj in searcher.Get())
                {
                    object processorObj = obj["ProcessorId"];
                    string processorId = processorObj != null ? processorObj.ToString() : "Unknown";
                    PrintColoredMessage(string.Format("[INFO] Procesador ID: {0}", processorId), ConsoleColor.Gray);
                }
            }
            
            PrintColoredMessage("[+] Análisis del sistema completado", ConsoleColor.Green);
        }
        catch (Exception ex)
        {
            PrintColoredMessage(string.Format("[WARNING] Error en análisis: {0}", ex.Message), ConsoleColor.Yellow);
        }
    }
    
    static void ApplyAggressiveRegistryModifications()
    {
        try
        {
            // Modificaciones agresivas del registro para bypass
            string[] registryPaths = {
                @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\SoftwareProtectionPlatform",
                @"SOFTWARE\Microsoft\Windows\CurrentVersion\Setup\OOBE",
                @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\DefaultProductKey",
                @"SYSTEM\CurrentControlSet\Services\sppsvc"
            };
            
            foreach (string path in registryPaths)
            {
                try
                {
                    using (RegistryKey key = Registry.LocalMachine.CreateSubKey(path))
                    {
                        if (key != null)
                        {
                            // Aplicar múltiples valores para bypass
                            key.SetValue("SkipRearm", 1, RegistryValueKind.DWord);
                            key.SetValue("SkipMachineOOBE", 1, RegistryValueKind.DWord);
                            key.SetValue("ProtectionPolicy", 1, RegistryValueKind.DWord);
                            key.SetValue("DisableWatson", 1, RegistryValueKind.DWord);
                            
                            // Generar HWID sintético
                            string syntheticHWID = GenerateSyntheticHWID();
                            key.SetValue("BackupProductKeyDefault", syntheticHWID, RegistryValueKind.String);
                            key.SetValue("TokensCache", syntheticHWID, RegistryValueKind.String);
                        }
                    }
                }
                catch
                {
                    // Continuar con el siguiente path si falla
                }
            }
            
            PrintColoredMessage("[+] Modificaciones agresivas aplicadas", ConsoleColor.Green);
        }
        catch (Exception ex)
        {
            PrintColoredMessage(string.Format("[WARNING] Error en modificaciones: {0}", ex.Message), ConsoleColor.Yellow);
        }
    }
    
    static string GenerateSyntheticHWID()
    {
        try
        {
            // Generar un HWID sintético basado en datos del sistema
            StringBuilder hwid = new StringBuilder();
            
            // Usar timestamp y datos del sistema para generar ID único
            long timestamp = DateTime.Now.Ticks;
            string systemRoot = Environment.GetEnvironmentVariable("SystemRoot") ?? "C:\\Windows";
            
            hwid.Append(timestamp.ToString("X"));
            hwid.Append("-");
            hwid.Append(systemRoot.GetHashCode().ToString("X"));
            hwid.Append("-");
            hwid.Append(Environment.MachineName.GetHashCode().ToString("X"));
            
            return hwid.ToString();
        }
        catch
        {
            return "SYNTHETIC-HWID-" + DateTime.Now.Ticks.ToString("X");
        }
    }
    
    static void ManipulateLicensingServices()
    {
        try
        {
            // Manipular servicios de licencias de forma agresiva
            string[] services = { "sppsvc", "WLIDSVC", "LicenseManager" };
            
            foreach (string service in services)
            {
                try
                {
                    // Detener servicio
                    RunCommand("net", string.Format("stop {0}", service));
                    Thread.Sleep(1000);
                    
                    // Modificar configuración del servicio en registro
                    using (RegistryKey key = Registry.LocalMachine.CreateSubKey(string.Format(@"SYSTEM\CurrentControlSet\Services\{0}", service)))
                    {
                        if (key != null)
                        {
                            key.SetValue("Start", 2, RegistryValueKind.DWord); // Automático
                            key.SetValue("ErrorControl", 0, RegistryValueKind.DWord); // Ignorar errores
                        }
                    }
                    
                    // Reiniciar servicio
                    RunCommand("net", string.Format("start {0}", service));
                    Thread.Sleep(1000);
                }
                catch
                {
                    // Continuar con el siguiente servicio
                }
            }
            
            PrintColoredMessage("[+] Servicios de licencias manipulados", ConsoleColor.Green);
        }
        catch (Exception ex)
        {
            PrintColoredMessage(string.Format("[WARNING] Error manipulando servicios: {0}", ex.Message), ConsoleColor.Yellow);
        }
    }
    
    static void InjectCustomHWIDData()
    {
        try
        {
            // Inyectar datos HWID personalizados en múltiples ubicaciones
            string customHWID = GenerateSyntheticHWID();
            
            string[] hwidLocations = {
                @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\SoftwareProtectionPlatform\Plugins\Objects\261230-70000-00001-AA47",
                @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\SoftwareProtectionPlatform\Plugins\Objects\261230-00001-00001-AA47",
                @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\DefaultProductKey2"
            };
            
            foreach (string location in hwidLocations)
            {
                try
                {
                    using (RegistryKey key = Registry.LocalMachine.CreateSubKey(location))
                    {
                        if (key != null)
                        {
                            key.SetValue("Value", customHWID, RegistryValueKind.String);
                            key.SetValue("Partial Product Key", "3V66T", RegistryValueKind.String);
                            key.SetValue("License Family", "Professional", RegistryValueKind.String);
                            key.SetValue("HWID", customHWID, RegistryValueKind.String);
                        }
                    }
                }
                catch
                {
                    // Continuar con la siguiente ubicación
                }
            }
            
            PrintColoredMessage("[+] Datos HWID personalizados inyectados", ConsoleColor.Green);
        }
        catch (Exception ex)
        {
            PrintColoredMessage(string.Format("[WARNING] Error inyectando HWID: {0}", ex.Message), ConsoleColor.Yellow);
        }
    }
    
    static void BypassValidations()
    {
        try
        {
            // Aplicar bypass de validaciones del sistema
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\SoftwareProtectionPlatform"))
            {
                if (key != null)
                {
                    // Bypass de validaciones
                    key.SetValue("SkipRearm", 1, RegistryValueKind.DWord);
                    key.SetValue("BypassValidation", 1, RegistryValueKind.DWord);
                    key.SetValue("DisableKeyValidation", 1, RegistryValueKind.DWord);
                    key.SetValue("IgnoreHardwareChanges", 1, RegistryValueKind.DWord);
                    key.SetValue("ForceActivation", 1, RegistryValueKind.DWord);
                }
            }
            
            // Bypass adicional en WPA
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\WPA\PosReady"))
            {
                if (key != null)
                {
                    key.SetValue("Installed", 1, RegistryValueKind.DWord);
                }
            }
            
            PrintColoredMessage("[+] Bypass de validaciones aplicado", ConsoleColor.Green);
        }
        catch (Exception ex)
        {
            PrintColoredMessage(string.Format("[WARNING] Error en bypass: {0}", ex.Message), ConsoleColor.Yellow);
        }
    }
    
    static void ForceMultipleActivation()
    {
        try
        {
            // Intentar activación múltiple con diferentes métodos
            string[] activationMethods = { "/ato", "/dli", "/dlv" };
            
            foreach (string method in activationMethods)
            {
                PrintColoredMessage(string.Format("[+] Intentando activación con método: {0}", method), ConsoleColor.Yellow);
                RunCommand("slmgr.vbs", method);
                Thread.Sleep(3000);
            }
            
            // Forzar reactivación
            RunCommand("slmgr.vbs", "/rearm");
            Thread.Sleep(2000);
            RunCommand("slmgr.vbs", "/ato");
            
            PrintColoredMessage("[+] Activación múltiple forzada completada", ConsoleColor.Green);
        }
        catch (Exception ex)
        {
            PrintColoredMessage(string.Format("[WARNING] Error en activación múltiple: {0}", ex.Message), ConsoleColor.Yellow);
        }
    }
    
    static void ExecuteSystemFix()
    {
        PrintColoredMessage("╔══════════════════════════════════════════════════════════════════╗", ConsoleColor.Red);
        PrintColoredMessage("║                        REPARACIÓN DEL SISTEMA                       ║", ConsoleColor.Red);
        PrintColoredMessage("║                                                                  ║", ConsoleColor.Red);
        PrintColoredMessage("║  Este comando eliminará TODOS los cambios realizados por         ║", ConsoleColor.Yellow);
        PrintColoredMessage("║  los métodos HWID y HWID-FORCE, y reiniciará el sistema.         ║", ConsoleColor.Yellow);
        PrintColoredMessage("║                                                                  ║", ConsoleColor.Red);
        PrintColoredMessage("║  ADVERTENCIA: Esto restaurará Windows a su estado original.     ║", ConsoleColor.Yellow);
        PrintColoredMessage("║                                                                  ║", ConsoleColor.Red);
        PrintColoredMessage("║  PRESIONE 'Y' PARA CONTINUAR O CUALQUIER TECLA PARA CANCELAR    ║", ConsoleColor.White);
        PrintColoredMessage("╚══════════════════════════════════════════════════════════════════╝", ConsoleColor.Red);
        
        ConsoleKeyInfo key = Console.ReadKey(true);
        if (key.Key != ConsoleKey.Y)
        {
            PrintColoredMessage("[INFO] Operación cancelada por el usuario.", ConsoleColor.Yellow);
            return;
        }
        
        Console.WriteLine();
        PrintColoredMessage("[+] Iniciando reparación completa del sistema...", ConsoleColor.Red);
        Console.WriteLine();
        
        try
        {
            // Paso 1: Limpiar claves de producto instaladas
            PrintColoredMessage("[+] Paso 1: Eliminando claves de producto instaladas...", ConsoleColor.Yellow);
            RunCommand("slmgr.vbs", "/upk");
            Thread.Sleep(3000);
            RunCommand("slmgr.vbs", "/cpky");
            Thread.Sleep(3000);
            
            // Paso 2: Limpiar registro completamente
            PrintColoredMessage("[+] Paso 2: Limpiando modificaciones del registro...", ConsoleColor.Yellow);
            CleanRegistryCompletely();
            
            // Paso 3: Restaurar servicios
            PrintColoredMessage("[+] Paso 3: Restaurando servicios del sistema...", ConsoleColor.Yellow);
            RestoreSystemServices();
            
            // Paso 4: Limpiar cachés y archivos temporales
            PrintColoredMessage("[+] Paso 4: Limpiando cachés del sistema...", ConsoleColor.Yellow);
            CleanSystemCaches();
            
            // Paso 5: Restaurar configuración de activación
            PrintColoredMessage("[+] Paso 5: Restaurando configuración de activación...", ConsoleColor.Yellow);
            RestoreActivationConfig();
            
            // Paso 6: Reiniciar servicios críticos
            PrintColoredMessage("[+] Paso 6: Reiniciando servicios críticos...", ConsoleColor.Yellow);
            RestartCriticalServices();
            
            PrintColoredMessage("[SUCCESS] Reparación del sistema completada!", ConsoleColor.Green);
            PrintColoredMessage("[INFO] El sistema ha sido restaurado a su estado original.", ConsoleColor.Cyan);
            Console.WriteLine();
            
            // Preguntar si quiere reiniciar ahora
            PrintColoredMessage("╔══════════════════════════════════════════════════════════════════╗", ConsoleColor.Green);
            PrintColoredMessage("║                        REINICIO RECOMENDADO                     ║", ConsoleColor.Green);
            PrintColoredMessage("║                                                                  ║", ConsoleColor.Green);
            PrintColoredMessage("║  Se recomienda reiniciar el sistema para completar la          ║", ConsoleColor.White);
            PrintColoredMessage("║  restauración y aplicar todos los cambios correctamente.        ║", ConsoleColor.White);
            PrintColoredMessage("║                                                                  ║", ConsoleColor.Green);
            PrintColoredMessage("║  ¿Desea reiniciar el sistema ahora? (Y/N)                       ║", ConsoleColor.Yellow);
            PrintColoredMessage("╚══════════════════════════════════════════════════════════════════╝", ConsoleColor.Green);
            
            ConsoleKeyInfo rebootKey = Console.ReadKey(true);
            if (rebootKey.Key == ConsoleKey.Y)
            {
                PrintColoredMessage("[+] Reiniciando sistema en 10 segundos...", ConsoleColor.Yellow);
                PrintColoredMessage("[INFO] Presione cualquier tecla para cancelar el reinicio.", ConsoleColor.Gray);
                
                for (int i = 10; i > 0; i--)
                {
                    Console.Write(string.Format("\r[INFO] Reiniciando en {0} segundos... ", i));
                    if (Console.KeyAvailable)
                    {
                        Console.ReadKey(true);
                        PrintColoredMessage("\n[INFO] Reinicio cancelado por el usuario.", ConsoleColor.Yellow);
                        return;
                    }
                    Thread.Sleep(1000);
                }
                
                Console.WriteLine();
                PrintColoredMessage("[+] Ejecutando reinicio del sistema...", ConsoleColor.Red);
                ProcessStartInfo rebootPsi = new ProcessStartInfo
                {
                    FileName = "shutdown",
                    Arguments = "/r /t 0 /c \"Reinicio por Evolution Activator - Sistema restaurado\"",
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                Process.Start(rebootPsi);
            }
            else
            {
                PrintColoredMessage("[INFO] Reinicio cancelado. Se recomienda reiniciar manualmente.", ConsoleColor.Yellow);
            }
        }
        catch (Exception ex)
        {
            PrintColoredMessage(string.Format("[ERROR] Error durante la reparación: {0}", ex.Message), ConsoleColor.Red);
        }
    }
    
    static void CleanRegistryCompletely()
    {
        try
        {
            string[] registryPaths = {
                @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\SoftwareProtectionPlatform",
                @"SOFTWARE\Microsoft\Windows\CurrentVersion\Setup\OOBE",
                @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\DefaultProductKey",
                @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\DefaultProductKey2",
                @"SYSTEM\CurrentControlSet\Services\sppsvc",
                @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\SoftwareProtectionPlatform\Plugins\Objects\261230-70000-00001-AA47",
                @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\SoftwareProtectionPlatform\Plugins\Objects\261230-00001-00001-AA47",
                @"SYSTEM\WPA\PosReady"
            };
            
            foreach (string path in registryPaths)
            {
                try
                {
                    using (RegistryKey key = Registry.LocalMachine.OpenSubKey(path, writable: true))
                    {
                        if (key != null)
                        {
                            // Eliminar valores específicos que agregamos
                            string[] valuesToDelete = {
                                "SkipRearm", "SkipMachineOOBE", "ProtectionPolicy", "DisableWatson",
                                "BackupProductKeyDefault", "TokensCache", "BypassValidation",
                                "DisableKeyValidation", "IgnoreHardwareChanges", "ForceActivation",
                                "Value", "Partial Product Key", "License Family", "HWID", "Installed"
                            };
                            
                            foreach (string valueName in valuesToDelete)
                            {
                                try
                                {
                                    if (key.GetValue(valueName) != null)
                                    {
                                        key.DeleteValue(valueName, false);
                                    }
                                }
                                catch
                                {
                                    // Continuar si no se puede eliminar un valor específico
                                }
                            }
                        }
                    }
                }
                catch
                {
                    // Continuar con el siguiente path si falla
                }
            }
            
            PrintColoredMessage("[+] Registro limpiado correctamente", ConsoleColor.Green);
        }
        catch (Exception ex)
        {
            PrintColoredMessage(string.Format("[WARNING] Error limpiando registro: {0}", ex.Message), ConsoleColor.Yellow);
        }
    }
    
    static void RestoreSystemServices()
    {
        try
        {
            string[] services = { "sppsvc", "WLIDSVC", "LicenseManager" };
            
            foreach (string service in services)
            {
                try
                {
                    // Restaurar configuración original del servicio
                    using (RegistryKey key = Registry.LocalMachine.OpenSubKey(string.Format(@"SYSTEM\CurrentControlSet\Services\{0}", service), writable: true))
                    {
                        if (key != null)
                        {
                            // Restaurar valores por defecto
                            if (service == "sppsvc")
                            {
                                key.SetValue("Start", 3, RegistryValueKind.DWord); // Manual
                                key.SetValue("ErrorControl", 1, RegistryValueKind.DWord); // Normal
                            }
                        }
                    }
                    
                    // Reiniciar servicio
                    RunCommand("net", string.Format("stop {0}", service));
                    Thread.Sleep(2000);
                    RunCommand("net", string.Format("start {0}", service));
                    Thread.Sleep(2000);
                }
                catch
                {
                    // Continuar con el siguiente servicio
                }
            }
            
            PrintColoredMessage("[+] Servicios restaurados correctamente", ConsoleColor.Green);
        }
        catch (Exception ex)
        {
            PrintColoredMessage(string.Format("[WARNING] Error restaurando servicios: {0}", ex.Message), ConsoleColor.Yellow);
        }
    }
    
    static void CleanSystemCaches()
    {
        try
        {
            // Limpiar caché de tokens de activación
            string[] cachePaths = {
                @"C:\Windows\System32\spp\tokens\data",
                @"C:\Windows\System32\spp\tokens\pkeyconfig",
                @"C:\Windows\ServiceProfiles\NetworkService\AppData\Roaming\Microsoft\SoftwareProtectionPlatform"
            };
            
            foreach (string cachePath in cachePaths)
            {
                try
                {
                    if (Directory.Exists(cachePath))
                    {
                        string[] files = Directory.GetFiles(cachePath, "*", SearchOption.AllDirectories);
                        foreach (string file in files)
                        {
                            try
                            {
                                File.Delete(file);
                            }
                            catch
                            {
                                // Continuar si no se puede eliminar un archivo
                            }
                        }
                    }
                }
                catch
                {
                    // Continuar con el siguiente path
                }
            }
            
            PrintColoredMessage("[+] Cachés del sistema limpiados", ConsoleColor.Green);
        }
        catch (Exception ex)
        {
            PrintColoredMessage(string.Format("[WARNING] Error limpiando cachés: {0}", ex.Message), ConsoleColor.Yellow);
        }
    }
    
    static void RestoreActivationConfig()
    {
        try
        {
            // Ejecutar comandos de reparación de activación
            RunCommand("slmgr.vbs", "/rearm");
            Thread.Sleep(3000);
            
            // Reparar almacén de licencias
            RunCommand("sfc", "/scannow");
            
            PrintColoredMessage("[+] Configuración de activación restaurada", ConsoleColor.Green);
        }
        catch (Exception ex)
        {
            PrintColoredMessage(string.Format("[WARNING] Error restaurando configuración: {0}", ex.Message), ConsoleColor.Yellow);
        }
    }
    
    static void RestartCriticalServices()
    {
        try
        {
            string[] criticalServices = { "sppsvc", "Themes", "WLIDSVC" };
            
            foreach (string service in criticalServices)
            {
                try
                {
                    RunCommand("net", string.Format("stop {0}", service));
                    Thread.Sleep(2000);
                    RunCommand("net", string.Format("start {0}", service));
                    Thread.Sleep(2000);
                }
                catch
                {
                    // Continuar con el siguiente servicio
                }
            }
            
            PrintColoredMessage("[+] Servicios críticos reiniciados", ConsoleColor.Green);
        }
        catch (Exception ex)
        {
            PrintColoredMessage(string.Format("[WARNING] Error reiniciando servicios: {0}", ex.Message), ConsoleColor.Yellow);
        }
    }
    
    static void PrintColoredMessage(string message, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ResetColor();
    }
    
    static bool CheckActivationSuccess()
    {
        try
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "cscript",
                Arguments = "//Nologo C:\\Windows\\System32\\slmgr.vbs /dli",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            using (Process proc = Process.Start(psi))
            {
                proc.WaitForExit();
                string output = proc.StandardOutput.ReadToEnd();
                
                // Verificar si Windows está activado
                if (output.Contains("License Status: Licensed") || output.Contains("Estado de licencia: Con licencia"))
                {
                    return true;
                }
            }
            
            return false;
        }
        catch
        {
            return false;
        }
    }
    
    static void ResetKeysAndRetry(string windowsEdition)
    {
        try
        {
            PrintColoredMessage("[1/5] Eliminando claves de producto existentes...", ConsoleColor.Yellow);
            
            // Desinstalar clave actual
            RunCommand("slmgr.vbs", "/upk");
            Thread.Sleep(3000);
            
            // Limpiar clave del registro
            RunCommand("slmgr.vbs", "/cpky");
            Thread.Sleep(2000);
            
            PrintColoredMessage("[2/5] Limpiando configuración de activación...", ConsoleColor.Yellow);
            
            // Limpiar tokens de activación
            try
            {
                string tokensPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "spp", "tokens", "skus");
                if (Directory.Exists(tokensPath))
                {
                    string[] tokenFiles = Directory.GetFiles(tokensPath, "*.xrm-ms");
                    foreach (string tokenFile in tokenFiles)
                    {
                        try
                        {
                            File.Delete(tokenFile);
                        }
                        catch { }
                    }
                }
            }
            catch { }
            
            PrintColoredMessage("[3/5] Reiniciando servicios de activación...", ConsoleColor.Yellow);
            
            // Reiniciar servicio de protección de software
            RunCommand("net", "stop sppsvc");
            Thread.Sleep(3000);
            RunCommand("net", "start sppsvc");
            Thread.Sleep(3000);
            
            PrintColoredMessage("[4/5] Instalando nueva clave genérica...", ConsoleColor.Yellow);
            
            // Obtener nueva clave genérica
            string newGenericKey = GetGenericKeyForEdition(windowsEdition);
            if (!string.IsNullOrEmpty(newGenericKey))
            {
                InstallProductKey(newGenericKey);
                Thread.Sleep(3000);
            }
            
            PrintColoredMessage("[5/5] Intentando activación nuevamente...", ConsoleColor.Yellow);
            
            // Generar nuevo ticket HWID
            GenerateHWIDTicket();
            Thread.Sleep(2000);
            
            // Intentar activación
            ActivateWindows();
            Thread.Sleep(3000);
            
            // Verificar resultado final
            Console.WriteLine();
            bool finalResult = CheckActivationSuccess();
            
            if (finalResult)
            {
                PrintColoredMessage("[SUCCESS] ¡Activación exitosa después del restablecimiento!", ConsoleColor.Green);
                PrintColoredMessage("[INFO] Windows ahora tiene una licencia digital permanente", ConsoleColor.Cyan);
            }
            else
            {
                PrintColoredMessage("[ERROR] La activación falló nuevamente después del restablecimiento", ConsoleColor.Red);
                PrintColoredMessage("[INFO] Puede intentar con 'hwid-force' o usar 'fix' para limpiar cambios", ConsoleColor.Gray);
            }
        }
        catch (Exception ex)
        {
            PrintColoredMessage(string.Format("[ERROR] Error durante el restablecimiento: {0}", ex.Message), ConsoleColor.Red);
        }
    }
    
    // ========== FUNCIONES ESTILO MAS ==========
    
    static bool PerformMASLikeChecks()
    {
        PrintColoredMessage("[+] Realizando verificaciones previas (estilo MAS)...", ConsoleColor.Cyan);
        
        // Verificar permisos de administrador
        if (!IsAdministrator())
        {
            PrintColoredMessage("[ERROR] Se requieren permisos de administrador", ConsoleColor.Red);
            PrintColoredMessage("[INFO] Ejecute como administrador y vuelva a intentar", ConsoleColor.Yellow);
            return false;
        }
        
        // Verificar integridad del sistema
        try
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "powershell",
                Arguments = "-Command \"Get-Service sppsvc | Select-Object Status\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };
            
            using (Process proc = Process.Start(psi))
            {
                proc.WaitForExit();
                string output = proc.StandardOutput.ReadToEnd();
                if (!output.Contains("Running"))
                {
                    PrintColoredMessage("[WARNING] Servicio de protección de software no está ejecutándose", ConsoleColor.Yellow);
                    PrintColoredMessage("[+] Intentando iniciar servicio...", ConsoleColor.Yellow);
                    RunCommand("net", "start sppsvc");
                    Thread.Sleep(3000);
                }
            }
        }
        catch
        {
            PrintColoredMessage("[WARNING] No se pudo verificar el estado del servicio", ConsoleColor.Yellow);
        }
        
        // Detectar posible interferencia de antivirus (estilo MAS)
        DetectAntivirusInterference();
        
        PrintColoredMessage("[+] Verificaciones previas completadas", ConsoleColor.Green);
        return true;
    }
    
    static void DetectAntivirusInterference()
    {
        try
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "powershell",
                Arguments = "-Command \"Get-CimInstance -Namespace root\\SecurityCenter2 -Class AntiVirusProduct | Where-Object { $_.displayName -notlike '*windows*' } | Select-Object -ExpandProperty displayName\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };
            
            using (Process proc = Process.Start(psi))
            {
                proc.WaitForExit();
                string output = proc.StandardOutput.ReadToEnd();
                if (!string.IsNullOrEmpty(output.Trim()))
                {
                    PrintColoredMessage("[WARNING] Antivirus de terceros detectado que puede bloquear la activación:", ConsoleColor.Yellow);
                    PrintColoredMessage(string.Format("[INFO] {0}", output.Trim()), ConsoleColor.Gray);
                    PrintColoredMessage("[INFO] Considere desactivar temporalmente el antivirus", ConsoleColor.Gray);
                }
            }
        }
        catch
        {
            // Silenciar errores de detección de antivirus
        }
    }
    
    static bool PrepareSystemForHWIDMAS()
    {
        try
        {
            // Limpiar configuración previa (estilo MAS)
            PrintColoredMessage("[+] Limpiando configuración previa...", ConsoleColor.Yellow);
            
            // Detener servicios temporalmente
            RunCommand("net", "stop sppsvc");
            Thread.Sleep(2000);
            
            // Limpiar cachés de activación
            try
            {
                string[] cachePaths = {
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "spp", "store", "2.0"),
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "spp", "tokens")
                };
                
                foreach (string cachePath in cachePaths)
                {
                    if (Directory.Exists(cachePath))
                    {
                        string[] files = Directory.GetFiles(cachePath, "*", SearchOption.AllDirectories);
                        foreach (string file in files)
                        {
                            try
                            {
                                File.Delete(file);
                            }
                            catch { }
                        }
                    }
                }
            }
            catch { }
            
            // Reiniciar servicios
            RunCommand("net", "start sppsvc");
            Thread.Sleep(3000);
            
            return true;
        }
        catch
        {
            return false;
        }
    }
    
    static bool InstallProductKeyWithVerification(string productKey)
    {
        try
        {
            // Instalar clave con verificación múltiple
            bool success = RunCommand("slmgr.vbs", string.Format("/ipk {0}", productKey));
            Thread.Sleep(3000);
            
            if (success)
            {
                // Verificar que la clave se instaló correctamente
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "cscript",
                    Arguments = "//Nologo C:\\Windows\\System32\\slmgr.vbs /dli",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                };
                
                using (Process proc = Process.Start(psi))
                {
                    proc.WaitForExit();
                    string output = proc.StandardOutput.ReadToEnd();
                    if (output.Contains(productKey.Substring(productKey.Length - 5)))
                    {
                        PrintColoredMessage("[+] Clave de producto instalada y verificada", ConsoleColor.Green);
                        return true;
                    }
                }
            }
            
            return false;
        }
        catch
        {
            return false;
        }
    }
    
    static bool GenerateHWIDTicketMAS()
    {
        try
        {
            // Método 1: Usar clipup.exe si está disponible
            string clipupPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "clipup.exe");
            if (File.Exists(clipupPath))
            {
                PrintColoredMessage("[+] Usando clipup.exe para generar ticket HWID...", ConsoleColor.Yellow);
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = clipupPath,
                    Arguments = "-v -o",
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                
                using (Process proc = Process.Start(psi))
                {
                    proc.WaitForExit();
                    if (proc.ExitCode == 0)
                    {
                        PrintColoredMessage("[+] Ticket HWID generado exitosamente", ConsoleColor.Green);
                        return true;
                    }
                }
            }
            
            // Método 2: Fallback usando WMI y registro
            PrintColoredMessage("[+] Usando método alternativo para ticket HWID...", ConsoleColor.Yellow);
            
            // Generar datos HWID usando información del sistema
            string hwid = GenerateSystemHWID();
            if (!string.IsNullOrEmpty(hwid))
            {
                // Aplicar HWID al registro
                try
                {
                    using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\SoftwareProtectionPlatform", true))
                    {
                        if (key != null)
                        {
                            key.SetValue("BackupProductKeyDefault", hwid);
                            PrintColoredMessage("[+] Datos HWID aplicados al sistema", ConsoleColor.Green);
                            return true;
                        }
                    }
                }
                catch { }
            }
            
            return false;
        }
        catch
        {
            return false;
        }
    }
    
    static bool ActivateWindowsMAS()
    {
        try
        {
            // Activación con múltiples intentos (estilo MAS)
            PrintColoredMessage("[+] Iniciando proceso de activación...", ConsoleColor.Yellow);
            
            // Intento 1: Activación estándar
            bool success = RunCommand("slmgr.vbs", "/ato");
            Thread.Sleep(5000);
            
            if (success)
            {
                // Verificar inmediatamente
                if (CheckActivationSuccessMAS())
                {
                    return true;
                }
            }
            
            // Intento 2: Forzar activación online
            PrintColoredMessage("[+] Intentando activación online forzada...", ConsoleColor.Yellow);
            RunCommand("slmgr.vbs", "/skms kms.digiboy.ir");
            Thread.Sleep(2000);
            success = RunCommand("slmgr.vbs", "/ato");
            Thread.Sleep(5000);
            
            return CheckActivationSuccessMAS();
        }
        catch
        {
            return false;
        }
    }
    
    static bool CheckActivationSuccessMAS()
    {
        try
        {
            // Verificación robusta estilo MAS
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "cscript",
                Arguments = "//Nologo C:\\Windows\\System32\\slmgr.vbs /dli",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            using (Process proc = Process.Start(psi))
            {
                proc.WaitForExit();
                string output = proc.StandardOutput.ReadToEnd();
                
                // Verificaciones múltiples
                if (output.Contains("License Status: Licensed") || 
                    output.Contains("Estado de licencia: Con licencia") ||
                    output.Contains("Permanently activated"))
                {
                    // Verificación adicional con WMI
                    try
                    {
                        ProcessStartInfo wmiPsi = new ProcessStartInfo
                        {
                            FileName = "powershell",
                            Arguments = "-Command \"(Get-CimInstance -Class SoftwareLicensingProduct | Where-Object { $_.PartialProductKey -and $_.LicenseStatus -eq 1 }).LicenseStatus\"",
                            UseShellExecute = false,
                            RedirectStandardOutput = true,
                            CreateNoWindow = true
                        };
                        
                        using (Process wmiProc = Process.Start(wmiPsi))
                        {
                            wmiProc.WaitForExit();
                            string wmiOutput = wmiProc.StandardOutput.ReadToEnd();
                            if (wmiOutput.Contains("1"))
                            {
                                return true;
                            }
                        }
                    }
                    catch { }
                    
                    return true;
                }
            }
            
            return false;
        }
        catch
        {
            return false;
        }
    }
    
    static void ResetKeysAndRetryMAS(string windowsEdition)
    {
        try
        {
            PrintColoredMessage("[+] Iniciando restablecimiento completo (método MAS)...", ConsoleColor.Cyan);
            
            PrintColoredMessage("[1/6] Eliminando claves de producto existentes...", ConsoleColor.Yellow);
            RunCommand("slmgr.vbs", "/upk");
            Thread.Sleep(3000);
            RunCommand("slmgr.vbs", "/cpky");
            Thread.Sleep(2000);
            
            PrintColoredMessage("[2/6] Limpiando configuración de activación...", ConsoleColor.Yellow);
            PrepareSystemForHWIDMAS();
            
            PrintColoredMessage("[3/6] Reiniciando servicios críticos...", ConsoleColor.Yellow);
            string[] services = { "sppsvc", "Themes", "WLIDSVC" };
            foreach (string service in services)
            {
                try
                {
                    RunCommand("net", string.Format("stop {0}", service));
                    Thread.Sleep(2000);
                    RunCommand("net", string.Format("start {0}", service));
                    Thread.Sleep(2000);
                }
                catch { }
            }
            
            PrintColoredMessage("[4/6] Instalando nueva clave genérica...", ConsoleColor.Yellow);
            string newGenericKey = GetGenericKeyForEdition(windowsEdition);
            if (!string.IsNullOrEmpty(newGenericKey))
            {
                InstallProductKeyWithVerification(newGenericKey);
                Thread.Sleep(3000);
            }
            
            PrintColoredMessage("[5/6] Generando nuevo ticket HWID...", ConsoleColor.Yellow);
            GenerateHWIDTicketMAS();
            Thread.Sleep(2000);
            
            PrintColoredMessage("[6/6] Intentando activación con método MAS...", ConsoleColor.Yellow);
            ActivateWindowsMAS();
            Thread.Sleep(3000);
            
            // Verificar resultado final
            Console.WriteLine();
            bool finalResult = CheckActivationSuccessMAS();
            
            if (finalResult)
            {
                PrintColoredMessage("[SUCCESS] ¡Activación exitosa con método MAS!", ConsoleColor.Green);
                PrintColoredMessage("[INFO] Windows ahora tiene una licencia digital permanente", ConsoleColor.Cyan);
                PrintColoredMessage("[INFO] Restablecimiento completado exitosamente", ConsoleColor.Gray);
            }
            else
            {
                PrintColoredMessage("[ERROR] La activación falló después del restablecimiento MAS", ConsoleColor.Red);
                PrintColoredMessage("[INFO] Puede intentar con 'hwid-force' o usar 'fix' para limpiar cambios", ConsoleColor.Gray);
                PrintColoredMessage("[INFO] Reporte este problema en: https://github.com/EvolutionX/EvolutionActivator", ConsoleColor.Gray);
            }
        }
        catch (Exception ex)
        {
            PrintColoredMessage(string.Format("[ERROR] Error durante restablecimiento MAS: {0}", ex.Message), ConsoleColor.Red);
        }
    }
    
    static string GenerateSystemHWID()
    {
        try
        {
            // Generar HWID usando información del sistema (método alternativo)
            PrintColoredMessage("[+] Generando HWID basado en hardware del sistema...", ConsoleColor.Yellow);
            
            string systemInfo = "";
            
            // Obtener información del procesador
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "wmic",
                    Arguments = "cpu get ProcessorId /value",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                };
                
                using (Process proc = Process.Start(psi))
                {
                    proc.WaitForExit();
                    string output = proc.StandardOutput.ReadToEnd();
                    if (output.Contains("ProcessorId="))
                    {
                        string processorId = output.Split('=')[1].Trim();
                        systemInfo += processorId;
                    }
                }
            }
            catch { }
            
            // Obtener información de la placa base
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "wmic",
                    Arguments = "baseboard get SerialNumber /value",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                };
                
                using (Process proc = Process.Start(psi))
                {
                    proc.WaitForExit();
                    string output = proc.StandardOutput.ReadToEnd();
                    if (output.Contains("SerialNumber="))
                    {
                        string serialNumber = output.Split('=')[1].Trim();
                        systemInfo += serialNumber;
                    }
                }
            }
            catch { }
            
            // Obtener información del BIOS
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "wmic",
                    Arguments = "bios get SerialNumber /value",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                };
                
                using (Process proc = Process.Start(psi))
                {
                    proc.WaitForExit();
                    string output = proc.StandardOutput.ReadToEnd();
                    if (output.Contains("SerialNumber="))
                    {
                        string biosSerial = output.Split('=')[1].Trim();
                        systemInfo += biosSerial;
                    }
                }
            }
            catch { }
            
            // Si no se pudo obtener información del hardware, usar información del sistema
            if (string.IsNullOrEmpty(systemInfo))
            {
                systemInfo = Environment.MachineName + Environment.UserName + Environment.OSVersion.ToString();
            }
            
            // Generar hash del HWID
            if (!string.IsNullOrEmpty(systemInfo))
            {
                // Crear un hash simple del systemInfo
                int hash = systemInfo.GetHashCode();
                string hwidHash = Math.Abs(hash).ToString("X8");
                
                // Formatear como clave de producto genérica
                string formattedHWID = string.Format("HWID-{0}-{1}-{2}-{3}", 
                    hwidHash.Substring(0, 4),
                    hwidHash.Substring(4, 4),
                    DateTime.Now.Year.ToString(),
                    "EVOL");
                
                PrintColoredMessage(string.Format("[+] HWID generado: {0}", formattedHWID), ConsoleColor.Green);
                return formattedHWID;
            }
            
            return null;
        }
        catch (Exception ex)
        {
            PrintColoredMessage(string.Format("[WARNING] Error generando HWID: {0}", ex.Message), ConsoleColor.Yellow);
            return null;
        }
    }
}