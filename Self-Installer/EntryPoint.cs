using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace NoPdaDelay
{
    internal class EntryPoint
    {
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var result = "Unknown error occurred.";
            try
            {
                var fileDialog = new OpenFileDialog
                {
                    Title = "Select Subnautica.exe",
                    Filter = "Game File|Subnautica.exe",
                    Multiselect = false
                };
                Console.WriteLine("INFO: Please select your game executable.");
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    result = CopyPlugin(fileDialog.FileName);
                }
            }
            finally
            {
                Console.WriteLine(result);
                Console.WriteLine("Press enter to close...");
                Console.ReadLine();
            }
        }

        public static string CopyPlugin(string gameExePath)
        {
            var thisAssembly = Assembly.GetExecutingAssembly();
            var srcPath = thisAssembly.Location;
            var gameDir = Path.GetDirectoryName(gameExePath);

            var bepinexPath = Path.Combine(gameDir, "BepInEx");
            if (!Directory.Exists(bepinexPath))
            {
                return "ERROR: Could not find BepInEx installation. Please install BepInEx.";
            }

            var pluginsPath = Path.Combine(bepinexPath, "plugins");
            if (!Directory.Exists(pluginsPath))
            {
                Console.WriteLine("INFO: Could not find plugins path, creating...");
                try
                {
                    Directory.CreateDirectory(pluginsPath);
                }
                catch
                {
                    return "ERROR: Could not create plugins path.";
                }
            }

            var pluginFolder = Path.Combine(pluginsPath, thisAssembly.GetName().Name);
            if (!Directory.Exists(pluginFolder))
            {
                Console.WriteLine("INFO: Creating plugin directory...");
                try
                {
                    Directory.CreateDirectory(pluginFolder);
                }
                catch
                {
                    return "ERROR: Could not create folder for plugin.";
                }
            }

            var pluginPath = Path.Combine(pluginFolder, $"{thisAssembly.GetName().Name}.dll");
            if (File.Exists(pluginPath))
            {
                Console.Write("INFO: Plugin already exists, should it be overwritten? (Y/N) ");
                var option = Console.ReadLine();
                if (!option.Equals("y", StringComparison.InvariantCultureIgnoreCase))
                {
                    return "INFO: Plugin already existed, no action taken.";
                }
            }

            try
            {
                Console.WriteLine("INFO: Copying mod to plugin folder...");
                File.Copy(srcPath, pluginPath, true);
            }
            catch
            {
                return "ERROR: Unable to copy mod to plugin folder.";
            }
            
            return "Installation complete!";
        }
    }
}
