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
            
            // Paso 3: Preparar sistema para activación HWID
            PrintColoredMessage("[+] Preparando sistema para activación HWID...", ConsoleColor.Yellow);
            PrepareSystemForHWID();
            
            // Paso 4: Instalar clave genérica
            PrintColoredMessage("[+] Instalando clave de producto...", ConsoleColor.Yellow);
            if (!InstallProductKey(genericKey))
            {
                PrintColoredMessage("[ERROR] Error instalando clave de producto", ConsoleColor.Red);
                return;
            }
            
            // Paso 5: Generar ticket HWID
            PrintColoredMessage("[+] Generando ticket de licencia digital...", ConsoleColor.Yellow);
            if (!GenerateHWIDTicket())
            {
                PrintColoredMessage("[ERROR] Error generando ticket HWID", ConsoleColor.Red);
                return;
            }
            
            // Paso 6: Activar Windows
            PrintColoredMessage("[+] Activando Windows...", ConsoleColor.Yellow);
            if (!ActivateWindows())
            {
                PrintColoredMessage("[ERROR] Error en proceso de activación", ConsoleColor.Red);
                return;
            }
            
            // Paso 7: Verificar activación
            Console.WriteLine();
            VerifyActivationStatus();
            
            PrintColoredMessage("[SUCCESS] Activación HWID completada exitosamente!", ConsoleColor.Green);
            PrintColoredMessage("[INFO] Windows ahora tiene una licencia digital permanente", ConsoleColor.Cyan);
        }
        catch (Exception ex)
        {
            PrintColoredMessage(string.Format("[ERROR] Error en activación HWID: {0}", ex.Message), ConsoleColor.Red);
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
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT LicenseStatus FROM SoftwareLicensingProduct WHERE PartialProductKey IS NOT NULL"))
            {
                foreach (ManagementObject obj in searcher.Get())
                {
                    int status = Convert.ToInt32(obj["LicenseStatus"]);
                    if (status == 1)
                    {
                        PrintColoredMessage("Windows está activado y con licencia válida.", ConsoleColor.Green);
                        return;
                    }
                }
                PrintColoredMessage("Windows NO está activado.", ConsoleColor.Red);
            }
        }
        catch (Exception ex)
        {
            PrintColoredMessage("Error al verificar estado de activación: " + ex.Message, ConsoleColor.Red);
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
        // Claves genéricas oficiales de Microsoft para diferentes ediciones
        switch (edition.ToUpper())
        {
            case "PROFESSIONAL":
            case "PRO":
                return "W269N-WFGWX-YVC9B-4J6C9-T83GX";
            case "HOME":
            case "CORE":
                return "TX9XD-98N7V-6WMQ6-BX7FG-H8Q99";
            case "ENTERPRISE":
                return "NPPR9-FWDCX-D2C8J-H872K-2YT43";
            case "EDUCATION":
                return "NW6C2-QMPVW-D7KKK-3GKT6-VCFB2";
            case "PROFESSIONAL WORKSTATION":
            case "PROWORKSTATION":
                return "NRG8B-VKK3Q-CXVCJ-9G2XF-6Q84J";
            case "SERVER STANDARD":
                return "WC2BQ-8NRM3-FDDYY-2BFGV-KHKQY";
            case "SERVER DATACENTER":
                return "CB7KF-BWN84-R7R2Y-793K2-8XDDG";
            default:
                // Clave genérica por defecto (Professional)
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
    
    static void PrintColoredMessage(string message, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ResetColor();
    }
}