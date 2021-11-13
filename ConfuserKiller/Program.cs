using ConfuserKiller.Protections;
using dnlib.DotNet;
using dnlib.DotNet.Writer;
using dnlib.IO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ConfuserKiller {
    class Program {
        #region other stuff
        public static void antitamper() {
            bool? flag = AntiTamper.IsTampered(module);
            bool flag2 = true;
            bool flag3 = (flag.GetValueOrDefault() == flag2) & (flag != null);
            bool flag4 = flag3;
            bool flag5 = flag4;
            if (flag5) {
                CustomWriteLine("[$] Anti-Tamper detected", ConsoleColor.Yellow);
                IImageStream imageStream = module.MetaData.PEImage.CreateFullStream();
                byte[] rawbytes = imageStream.ReadBytes((int)imageStream.Length);
                try {
                  module = AntiTamper.UnAntiTamper(module, rawbytes);
                    CustomWriteLine("[$] Anti-Tamper removed succesfully", ConsoleColor.Green);
                }
                catch {
                    CustomWriteLine("[$] Failed to remove Anti-Tamper", ConsoleColor.Red);
                }
            }
        }
        public static void Staticpacker() {
            bool flag = Packer.IsPacked(module);
            bool flag2 = flag;
            bool flag3 = flag2;
            if (flag3) {
                CustomWriteLine("[$] Static compressor detected", ConsoleColor.Yellow);
                try {
                    StaticPacker.Run(module);
                    CustomWriteLine("[$] Compressor removed succesfully", ConsoleColor.Green);
                }
                catch {
                    CustomWriteLine("[$] Failed to remove Compressor", ConsoleColor.Red);
                }
           
              module.EntryPoint = module.ResolveToken(StaticPacker.epToken) as MethodDef;
            }
        }
        public static bool veryVerbose = false;

       
        public static string Asmpath;

 
        public static ModuleDefMD module;

       
        public static Assembly asm;

  
        private Assembly assembly;

       
        private static string path = null;

      
        public string DirectoryName = "";

       
        public static int MathsAmount;

        public static void CustomWriteLine(string text, ConsoleColor color) {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
        }
        public static void ExceptionWriteLine(Exception ex) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[" + DateTime.Now + "] " + ex.Message);
        }
        public static void ExceptionWriteLine(Exception ex, string message) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[" + DateTime.Now + "] " + ex.Message.Replace(".", "") + ": " + message);
        }
        #endregion
        static void Main(string[] args) {
       
            string ascii = @"


 ▄████▄   ▒█████   ███▄    █   █████▒█    ██   ██████ ▓█████  ██▀███      ██ ▄█▀ ██▓ ██▓     ██▓    ▓█████  ██▀███  
▒██▀ ▀█  ▒██▒  ██▒ ██ ▀█   █ ▓██   ▒ ██  ▓██▒▒██    ▒ ▓█   ▀ ▓██ ▒ ██▒    ██▄█▒ ▓██▒▓██▒    ▓██▒    ▓█   ▀ ▓██ ▒ ██▒
▒▓█    ▄ ▒██░  ██▒▓██  ▀█ ██▒▒████ ░▓██  ▒██░░ ▓██▄   ▒███   ▓██ ░▄█ ▒   ▓███▄░ ▒██▒▒██░    ▒██░    ▒███   ▓██ ░▄█ ▒
▒▓▓▄ ▄██▒▒██   ██░▓██▒  ▐▌██▒░▓█▒  ░▓▓█  ░██░  ▒   ██▒▒▓█  ▄ ▒██▀▀█▄     ▓██ █▄ ░██░▒██░    ▒██░    ▒▓█  ▄ ▒██▀▀█▄  
▒ ▓███▀ ░░ ████▓▒░▒██░   ▓██░░▒█░   ▒▒█████▓ ▒██████▒▒░▒████▒░██▓ ▒██▒   ▒██▒ █▄░██░░██████▒░██████▒░▒████▒░██▓ ▒██▒
░ ░▒ ▒  ░░ ▒░▒░▒░ ░ ▒░   ▒ ▒  ▒ ░   ░▒▓▒ ▒ ▒ ▒ ▒▓▒ ▒ ░░░ ▒░ ░░ ▒▓ ░▒▓░   ▒ ▒▒ ▓▒░▓  ░ ▒░▓  ░░ ▒░▓  ░░░ ▒░ ░░ ▒▓ ░▒▓░
  ░  ▒     ░ ▒ ▒░ ░ ░░   ░ ▒░ ░     ░░▒░ ░ ░ ░ ░▒  ░ ░ ░ ░  ░  ░▒ ░ ▒░   ░ ░▒ ▒░ ▒ ░░ ░ ▒  ░░ ░ ▒  ░ ░ ░  ░  ░▒ ░ ▒░
░        ░ ░ ░ ▒     ░   ░ ░  ░ ░    ░░░ ░ ░ ░  ░  ░     ░     ░░   ░    ░ ░░ ░  ▒ ░  ░ ░     ░ ░      ░     ░░   ░ 
░ ░          ░ ░           ░           ░           ░     ░  ░   ░        ░  ░    ░      ░  ░    ░  ░   ░  ░   ░     
░                                                                                                                   

";

            Console.WindowWidth = 130;
            Console.Title = "Confuser Killer v1.0.0";
     
            CustomWriteLine(ascii, ConsoleColor.Red);
            CustomWriteLine("Do not forget Fork & Star!: https://github.com/MrReverse/ConfuserKiller", ConsoleColor.White);
            string filepath = "nul";
            try {
                filepath = args[0];
               module = ModuleDefMD.Load(filepath);
                asm = Assembly.LoadFrom(filepath);
                Asmpath = filepath;
            }
            catch (Exception ex) {
                ExceptionWriteLine(ex, "Cannot load file...");
                Console.ReadKey();
            }
            bool flag = File.Exists(filepath);
            if (!flag) {
                FileNotFoundException exx = new FileNotFoundException();
                ExceptionWriteLine(exx, "Cannot find file...: " +filepath);
                Console.ReadKey();
            }
            CustomWriteLine("[$] Loaded file succesfully: " + filepath, ConsoleColor.Green);
           
                CustomWriteLine("[$] Removing attributes...", ConsoleColor.Yellow);
            CustomWriteLine("[$] Removed Attributes: " + AttributeRemover.start(module), ConsoleColor.Green);
            try {
        antitamper();
            }
            catch {
                CustomWriteLine("[$] Failed to remove Anti-Tamper", ConsoleColor.Red);
            }
            try{
               Staticpacker();
            }

            catch {
                CustomWriteLine("[$] Failed to remove Static Packer", ConsoleColor.Red);
            }
            try {

                CustomWriteLine("[$] Removing Anti-Debug", ConsoleColor.Yellow);
                antidebugger.Run(module);
                CustomWriteLine("[$] Anti-Debug call removed from module", ConsoleColor.Green);
            }
            catch (Exception) {
                CustomWriteLine("[$] Failed to remove Anti-Debug", ConsoleColor.Red);
            }
            try {
                CustomWriteLine("[$] Cleaning control-flow cases", ConsoleColor.Yellow);
                ControlFlowRun.cleaner(module);
                CustomWriteLine("[$] Clean succeded", ConsoleColor.Green);
            }
            catch (Exception) {
                CustomWriteLine("[$] Failed to clean control-flow", ConsoleColor.Red);
            }
            	try
			{
                CustomWriteLine("[$] Fixing Proxy calls", ConsoleColor.Yellow);
                int num = ReferenceProxy.ProxyFixer(module);
                CustomWriteLine("[$] Proxy calls fixed: " + num, ConsoleColor.Green);
            }
			catch (Exception)
			{
                CustomWriteLine("[$] Failed to fix Proxy calls", ConsoleColor.Red);
            }
            try {
                CustomWriteLine("[$] Removing static strings", ConsoleColor.Yellow);
                int num2 = StaticStrings.Run(module);
                CustomWriteLine("[$] Removed static strings: " + num2, ConsoleColor.Green);
            }
            catch (Exception) {
                CustomWriteLine("[$] Failed to decrypt static strings", ConsoleColor.Red);
            }
            try {
                CustomWriteLine("[$] Removing Anti-Dump", ConsoleColor.Yellow);
                antidumper.Run(module);
                CustomWriteLine("[$] Anti-Dump removed from module", ConsoleColor.Green);
            }
            catch (Exception) {
                CustomWriteLine("[$] Failed to remove Anti-Dump", ConsoleColor.Red);
            }
            try {
                CustomWriteLine("[$] Decrypting Resources", ConsoleColor.Yellow);
                ResourcesDeobfuscator.Deobfuscate(module);
            }
            catch (Exception) {
                CustomWriteLine("[$] Failed to decrypt resources", ConsoleColor.Red);
            }
            string text3 = Path.GetDirectoryName(filepath);
            bool flag17 = !text3.EndsWith("\\");
            bool flag18 = flag17;
            bool flag19 = flag18;
            bool flag20 = flag19;
            if (flag20) {
                text3 += "\\";
            }
            string filename = string.Format("{0}{1}_Killed{2}", text3, Path.GetFileNameWithoutExtension(filepath), Path.GetExtension(filepath));
            ModuleWriterOptions moduleWriterOptions = new ModuleWriterOptions(module);
            moduleWriterOptions.MetaDataOptions.Flags |= MetaDataFlags.PreserveAll;
            moduleWriterOptions.Logger = DummyLogger.NoThrowInstance;
            NativeModuleWriterOptions nativeModuleWriterOptions = new NativeModuleWriterOptions(module);
            nativeModuleWriterOptions.MetaDataOptions.Flags |= MetaDataFlags.PreserveAll;
            nativeModuleWriterOptions.Logger = DummyLogger.NoThrowInstance;
            bool isILOnly = module.IsILOnly;
            bool flag21 = isILOnly;
            bool flag22 = flag21;
            if (flag22) {
                module.Write(filename, moduleWriterOptions);
            }
            else {
                module.NativeWrite(filename, nativeModuleWriterOptions);
            }
            CustomWriteLine("[$] ConfuserEx is Killed succesfully, press any key to contiune", ConsoleColor.White);
            Console.ReadKey();
        }
    }
}
