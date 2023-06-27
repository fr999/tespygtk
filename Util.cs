using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using GTranslate.Translators;
using System.Threading.Tasks;
using Gtk;

using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

//./NekoParadise.sh --lint

//./renpy.sh NekoParadise-0.16-pc-standard-compressed/ translate ital

//./NekoParadise.sh '' translate ital


namespace tespygtk
{
    public class Album
    {
        public string Name {get;set;}
        public TextIter Start {get;set;}
        public TextIter End {get; set;}
        public Album(string _name, TextIter _start, TextIter _end)
        {
            Name = _name;
            Start = _start;
            End = _end;
        }
    }    

public class UnRen2
    {
        string Path;
        string unren_copy;

        string unren_name;

        string process_name;

        string message;
        public List<string> list = new List<string>();
        public string pythonLocations {get; set;}
        public string pythonArg {get; set;}

        private MessageDialog Dialog = null;
        private ProgressBar Bar = null;
        
        public UnRen2(string _path, MessageDialog _dialog, ProgressBar _bar)
        {
            this.Path = _path;
            this.Dialog = _dialog;
            this.Bar = _bar;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                unren_name = "unren.bat";
                process_name = "cmd";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
            unren_name = "unren.command";
            process_name = "/bin/bash";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) {
            Console.WriteLine("MacOS");
            unren_name = "unren.command";
            //????// 
            process_name = "/bin/bash";
            }
        }   
        public async void UnRen()
        {
            
            unren_copy = System.IO.Path.Combine(this.Path, unren_name);
            try 
            {
                //File.Copy("py/unren_rpa.rpy", unren_rpa);
                File.Copy($"py/{unren_name}", unren_copy);
            }
              catch (IOException ex)
            {
                Dialog.Text = $"IOException: {ex.Message}\t\nFichier Supprimée";
                File.Delete(unren_copy);
                Dialog.Run();
                Dialog.Destroy();
                return;
            }

            //pythonLocations = GetPythonPath(this.Path);
            //if (pythonLocations == string.Empty) 
            //{
                //list.Add($"IOException: Impossible de trouver Python");
               // Text.Buffer.Text = $"IOException: Impossible de trouver Python\n{this.Path}";
                //File.Delete(unren_rpa);
               // return;
            //}            
            //pythonArg = string.Format("{0} {1}", unren_rpa, this.Path);

            
            //Task.Run(() => Connection()).ContinueWith(t => { 
            //Text.Buffer.Text = "";
            //Text.Buffer.Text = string.Join("\t", list);
            //Console.WriteLine(string.Join("\t", list));
            //File.Delete(unren_rpa);
            //}); 
           
            var result = await Task.Run(() => Connection());
            Dialog.Text = message;
            Dialog.Run();
            Dialog.Destroy(); 
            File.Delete(unren_copy);           
        }

        public bool Connection()
        {
            ProcessStartInfo start = new ProcessStartInfo()
            {
                ArgumentList = {$"./{unren_name}", this.Path}
            };
            //start.FileName = this.pythonLocations;
            start.FileName = process_name;
            start.WorkingDirectory = this.Path;
            start.WindowStyle = ProcessWindowStyle.Normal;
            //Console.WriteLine(targetPath);
            //start.Arguments = this.pythonArg;
            start.UseShellExecute = true;
            //start.RedirectStandardOutput = true;
            //start.RedirectStandardError = true;

            try
            {
            Process process = Process.Start(start);
            bool inProcess = true;
            //Text.Buffer.Text = "Patience...";
            Bar.Fraction = 0.0;
            while(inProcess)
            {
                
                if (Bar.Fraction >= 1.0)
                {
                    Bar.Fraction = 0.0;
                }
                else
                {
                    Bar.Fraction += 0.1;
                    System.Threading.Thread.Sleep(200);

                }

                if (process.HasExited)
                {
                    Bar.Fraction = 0.0;
                    inProcess = false;
                }
            }

            //string stderr = process.StandardError.ReadToEnd();
            //string stresult = process.StandardOutput.ReadToEnd();
            //Console.WriteLine(stresult);
            //list.Add(stresult);
            //list.Add(stderr);
            //Text.Buffer.Text = "";
            //Text.Buffer.Text = stresult;
            message = "Tache Terminée";
            }
            catch (Exception e)
            {
            //Dialog.Text = $"Erreur: {e.Message}";
            message = $"Erreur: {e.Message}";
            Console.WriteLine("eerrururu");
            //File.Delete(unren_copy);
            //Dialog.Run();
            //Dialog.Destroy();

            }
            return false;
        }


        private string GetPythonPath(string path) {
            
            string archi = string.Empty;

            if (IntPtr.Size == 4)
            {
                archi = "i686";
            }
            else 
            {
                archi = "x86";
            }
            string os = string.Empty;
            string python = string.Empty;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                os = "windows";
                python = "python.exe";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
            os = "linux";
            python = "python";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) {
            Console.WriteLine("MacOS");
            os = "macos";
            //????// 
            python = "python";
            }

            string[] allFiles = Directory.GetDirectories(path+ "/lib/", "*"+os+"**"+archi+"*");
            foreach(string file in allFiles)
            {
                string targetPath = System.IO.Path.Combine(file, python);
                if (File.Exists(targetPath))
                {
                return targetPath;
                }
                else
                {
                    return string.Empty;
                }
                
            }
            return string.Empty;
        }

    }

public class Newlangue
    {
        string Path;

        string Name;
        string Langue;
        string unren_name;

        string process_name;

        string message;

        private ProgressBar Bar = null;
        
        public Newlangue(string _path, string _langue, ProgressBar _bar)
        {
            this.Path = System.IO.Path.GetDirectoryName(_path);
            this.Name = System.IO.Path.GetFileName(_path);
            this.Langue = _langue;
            this.Bar = _bar;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                process_name = "cmd";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
            process_name = "/bin/bash";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) {
            Console.WriteLine("MacOS");
            //????// 
            process_name = "/bin/bash";
            }
        }   
        public async void Language()
        {           
            var result = await Task.Run(() => Connection());
            MessageDialog md = new MessageDialog (null, 
            DialogFlags.DestroyWithParent, MessageType.Warning, 
            ButtonsType.Ok, message);
            md.Run();
            md.Destroy(); 
        }

        public bool Connection()
        {
            ProcessStartInfo start = new ProcessStartInfo()
            {
                //./NekoParadise.sh '' translate ital
                ArgumentList = {$"./{this.Name}", "", "translate", {this.Langue}}
            };
            //start.FileName = this.pythonLocations;
            start.FileName = process_name;
            start.WorkingDirectory = this.Path;
            start.WindowStyle = ProcessWindowStyle.Normal;
            start.UseShellExecute = false;
            //start.RedirectStandardOutput = true;
            start.RedirectStandardError = true;

            try
            {
            Process process = Process.Start(start);
            bool inProcess = true;
            Bar.Fraction = 0.0;
            while(inProcess)
            {
                
                if (Bar.Fraction >= 1.0)
                {
                    Bar.Fraction = 0.0;
                }
                else
                {
                    Bar.Fraction += 0.1;
                    System.Threading.Thread.Sleep(200);

                }

                if (process.HasExited)
                {
                    Bar.Fraction = 0.0;
                    inProcess = false;
                }
            }

            string stderr = process.StandardError.ReadToEnd();
            //string stresult = process.StandardOutput.ReadToEnd();
            //Console.WriteLine(stresult);
            message = $"Tache Terminée {stderr}";
            }
            catch (Exception e)
            {
            message = $"Erreur: {e.Message}";
            }
            return false;
        }

    }


public class Newlint
    {
        string Path;

        string Name;
        string unren_name;

        string process_name;

        string message;

        private ProgressBar Bar = null;
        
        public Newlint(string _path, ProgressBar _bar)
        {
            this.Path = System.IO.Path.GetDirectoryName(_path);
            this.Name = System.IO.Path.GetFileName(_path);
            this.Bar = _bar;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                process_name = "cmd";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
            process_name = "/bin/bash";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) {
            Console.WriteLine("MacOS");
            //????// 
            process_name = "/bin/bash";
            }
        }   
        public async Task<bool> Lint()
        {           
            var result = await Task.Run(() => Connection());
            MessageDialog md = new MessageDialog (null, 
            DialogFlags.DestroyWithParent, MessageType.Warning, 
            ButtonsType.Ok, message);
            md.Run();
            md.Destroy();
            return true; 
        }

        public bool Connection()
        {
            ProcessStartInfo start = new ProcessStartInfo()
            {
                //./NekoParadise.sh '' translate ital
                ArgumentList = {$"./{this.Name}", "--lint"}
            };
            //start.FileName = this.pythonLocations;
            start.FileName = process_name;
            start.WorkingDirectory = this.Path;
            start.WindowStyle = ProcessWindowStyle.Normal;
            start.UseShellExecute = false;
            //start.RedirectStandardOutput = true;
            start.RedirectStandardError = true;

            try
            {
            Process process = Process.Start(start);
            bool inProcess = true;
            Bar.Fraction = 0.0;
            while(inProcess)
            {
                
                if (Bar.Fraction >= 1.0)
                {
                    Bar.Fraction = 0.0;
                }
                else
                {
                    Bar.Fraction += 0.1;
                    System.Threading.Thread.Sleep(200);

                }

                if (process.HasExited)
                {
                    Bar.Fraction = 0.0;
                    inProcess = false;
                }
            }

            string stderr = process.StandardError.ReadToEnd();
            //string stresult = process.StandardOutput.ReadToEnd();
            //Console.WriteLine(stresult);
            message = $"Tache Terminée {stderr}";
            }
            catch (Exception e)
            {
            message = $"Erreur: {e.Message}";
            }
            return false;
        }

    }

    public class Edit
    {

        private TextView textView = null;

        private TextTag tag_search_source = new TextTag("tag_search_source");
        public string Name {get;set;}
        public TextIter Start {get;set;}
        public TextIter End {get; set;}
        public Edit(TextView textView)
        {
            this.textView = textView;

            tag_search_source.Background = "#758EA4";
            tag_search_source.Foreground = "white";
            tag_search_source.Weight = Pango.Weight.Bold;

            this.textView.Buffer.TagTable.Add(tag_search_source);

        }
        public TextView TextView {
		get { return textView; }
	    }

        public TextBuffer TextBuffer {
		get { return TextView.Buffer; }
	    }

        public void ReplaceSelection (string replacement) 
        {
            TextBuffer buffer = this.textView.Buffer;
            buffer.BeginUserAction();
            buffer.DeleteSelection(true, true);
            buffer.InsertAtCursor(replacement);
            buffer.EndUserAction();
        }
    }

    class Util
    {        
        //public static Dictionary<string, string> filedict = new Dictionary<string, string>();//this line is added.


        public static async Task<String> Translate(string service, string languagein, string languageout, string text)
        {
            //var translator = new AggregateTranslator();
            //new GoogleTranslator();
            //new GoogleTranslator2();
            //new MicrosoftTranslator(); 
            //new YandexTranslator();
            //new BingTranslator();
            if (languagein == "auto") 
            {
                languagein = null;
            }

            try
            {
                switch (service)
                {  
                    case "Google":
                        var translator = new GoogleTranslator();
                        var aggrtext = await translator.TranslateAsync(text, languageout, languagein);
                        //Console.WriteLine($"Source Language: {aggrtext.SourceLanguage} target {aggrtext.TargetLanguage} service {aggrtext.Service}");
                        return aggrtext.Translation;
                    case "Google2":
                        var translator2 = new GoogleTranslator2();
                        var aggrtext2 = await translator2.TranslateAsync(text, languageout, languagein);
                        //Console.WriteLine($"Source Language: {aggrtext2.SourceLanguage} target {aggrtext2.TargetLanguage} service {aggrtext2.Service}");
                        return aggrtext2.Translation;
                    case "Yandex":
                        var translator3 = new YandexTranslator();
                        var aggrtext3 = await translator3.TranslateAsync(text, languageout, languagein);
                        //Console.WriteLine($"Source Language: {aggrtext3.SourceLanguage} target {aggrtext3.TargetLanguage} service {aggrtext3.Service}");
                        return aggrtext3.Translation;
                    case "Bing":
                        var translator4 = new BingTranslator();
                        var aggrtext4 = await translator4.TranslateAsync(text, languageout, languagein);
                        //Console.WriteLine($"Source Language: {aggrtext4.SourceLanguage} target {aggrtext4.TargetLanguage} service {aggrtext4.Service}");
                        return aggrtext4.Translation;
                    case "Microsoft":
                        var translator5 = new MicrosoftTranslator();
                        var aggrtext5 = await translator5.TranslateAsync(text, languageout, languagein);
                        //Console.WriteLine($"Source Language: {aggrtext5.SourceLanguage} target {aggrtext5.TargetLanguage} service {aggrtext5.Service}");
                        return aggrtext5.Translation;            
                    default:
                        var translator6 = new AggregateTranslator();
                        var aggrtext6 = await translator6.TranslateAsync(text, languageout, languagein);
                        //Console.WriteLine($"Source Language: {aggrtext6.SourceLanguage} target {aggrtext6.TargetLanguage} service {aggrtext6.Service}");
                        return aggrtext6.Translation;
                } 
            }
            
                //var aggrtext = await translator.TranslateAsync(text, languageout);
                //Console.WriteLine($"Translation: {aggrtext.Translation}");
                //Console.WriteLine($"Source Language: {aggrtext.SourceLanguage}");
                //Console.WriteLine($"Target Language: {aggrtext.TargetLanguage}");
                //Console.WriteLine($"Service: {aggrtext.Service}");
                //return aggrtext.Translation;
            catch (Exception e)
            {
                Console.WriteLine($"erreur {e}");
            }
            return string.Empty;
            
        }

        public static Dictionary<string, string> getDictFile (string File)
        {
            string File_fr = File + "_fr";
            string File_old = File + "_old";

            MainWindow.filedict = new Dictionary<string, string>()
            {
	            {"file", File},
	            {"file_fr", File_fr},
	            {"file_old", File_old},
                {"filename", System.IO.Path.GetFileName(File)},
                {"filename_fr", System.IO.Path.GetFileName(File_fr)},
	            {"filename_old", System.IO.Path.GetFileName(File_old)}
            };

            return MainWindow.filedict;
        }

        public static string GetFileSize(string FilePath)
        {
            if (System.IO.File.Exists(FilePath))
            {
                long fileSizeibBytes = new System.IO.FileInfo(FilePath).Length;
                long fileSizeibKbs = fileSizeibBytes / 1024;
                string texte = fileSizeibKbs.ToString() + " Ko";
                return texte;
            }
            return "0 Ko";
        }
        public static bool Rpa(string FilePath)
        {
            System.IO.Directory.CreateDirectory(FilePath);
                //string[] files = System.IO.Directory.GetFiles(filechooser.CurrentFolder);
            string[] files = System.IO.Directory.GetFiles(FilePath, "*.rpa", System.IO.SearchOption.AllDirectories);
            Array.Sort(files);

            foreach (string file in files)
            {
                string fileName = System.IO.Path.GetFileName(file);
                string targetPath = System.IO.Path.Combine(FilePath, fileName);
                //long length = new System.IO.FileInfo(file).Length;

                //string lenght = Util.rpafile(file);

                //System.IO.File.Move(file, targetPath);
                //Console.WriteLine(length);

                    
            }
            return true;
        }

    public static void addTabs(TextView source, TextView texte, TextView sauv) 
    {
            //TAG
            //orange DC3122
            //bleau 758EA4
            //violet A33E50
            //rose B13C52

            TextTag tag_search_source = addtab("tag_search_source", "#758EA4", "white");
            TextTag tag_search_texte = addtab("tag_search_texte", "#758EA4", "white");
            TextTag tag_search_sauv = addtab("tag_search_sauv", "#758EA4", "white");

            source.Buffer.TagTable.Add(tag_search_source);
            texte.Buffer.TagTable.Add(tag_search_texte);
            sauv.Buffer.TagTable.Add(tag_search_sauv);

            TextTag tag_error_source = addtab("tag_error_source", "#DC3122", "white");
            TextTag tag_error_texte = addtab("tag_error_texte", "#DC3122", "white");
            TextTag tag_error_sauv = addtab("tag_error_sauv", "#DC3122", "white");

            source.Buffer.TagTable.Add(tag_error_source);
            texte.Buffer.TagTable.Add(tag_error_texte);
            sauv.Buffer.TagTable.Add(tag_error_sauv);            
                     
            // var searchTag = new TextTag ("search");
            // searchTag.Background = "#758EA4";
            // searchTag.Foreground = "white";
            // searchTag.Weight = Pango.Weight.Bold;

            // _textviewsource.Buffer.TagTable.Add(errorTag);
            // _textviewtexte.Buffer.TagTable.Add(errorTag);
            // _textviewsauv.Buffer.TagTable.Add(errorTag);

    
    }

    private static TextTag addtab(string name, string background, string foreground)
    {
        var Tag = new TextTag (name);
        Tag.Background = background;
        Tag.Foreground = foreground;
        Tag.Weight = Pango.Weight.Bold;
        return Tag;
    }

    public static string ParseRpyLine(string line)
        {

            line = line.Trim();
            if (line.Length <= 6) 
            {
                return string.Empty;
            }


            if (line[0] == '#' || line.Substring(0, 3) == "old")
            {
                return string.Empty;
            }

            // try
            // {
            // var m1 = Regex.Matches(line, @"""(.*?)""");
            // //Console.WriteLine(m1[0].Value.ToString().Trim('"'));
            // line = m1[0].Value.ToString().Trim('"');
            // }
            // catch
            // {
            //     return string.Empty;
            // }
                        bool fQuotesFound = false;
            int left = 0;
            int right = line.Length;

            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == '"')
                {
                    fQuotesFound = true;
                    if (left == 0)
                    {
                        left = i + 1;
                    }
                    else
                    {
                        if (i == 0 || line[i - 1] != '\\')
                        {
                            right = i;
                            break;
                        }
                    }
                }
            }

            if (!fQuotesFound)
                return string.Empty;

            line = line.Substring(left, right - left);

            return line;

        }

    }
}