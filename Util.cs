using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Gtk;

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

                System.IO.File.Move(file, targetPath);
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