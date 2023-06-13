using System;
using System.Collections.Generic;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;
using System.Text.RegularExpressions;



//peux etre utile
//GLib.Timeout.Add(100, onTimeout);

// bool changed = false;

// txtEditor.Buffer.Changed += new EventHandler(onChangeEvent); 

// public void onChangeEvent(object sender,EventArgs e)   {
//   changed = true;
// }

namespace tespygtk

{
    class MainWindow : Window
    {

        //public static int counter;
        //public static IDictionary<int, string> listtag;

        //public static List<int> listtag;
        //public static Tuple<string, int, int>[] listtag = null;

        List<Album> listOfAlbums = new List<Album>();

	    private Edit textView = null;


        public static Dictionary<string, string> filedict = null;

        [UI] private Dialog _poperror = null;

        [UI] private ListBox _listerror = null;
        [UI] private Button _btnopenfolder = null;

        [UI] private MenuItem _btnrpa = null;

        [UI] private MenuItem _btnunren = null;

        [UI] private Button _btnextract = null;

        [UI] private Button _btntranslate = null;

        [UI] private Button _btncompile = null;



        [UI] private Button _btnbarsauv = null;


        [UI] private Button _btntabaccueil = null;
        [UI] private Button _btntabfile = null;
        [UI] private Button _btntabtexte = null;
        [UI] private Button _btntabsauv = null;

        [UI] private Entry _entrymaxcara = null;
        [UI] private Button _btnselectcara = null;

        [UI] private ComboBox _combosearch = null;
        [UI] private SearchEntry _searchentry = null;

        [UI] private Entry _replaceentry = null;

        [UI] private Entry _entryerrortotal = null;

        [UI] private Entry _entrytotal = null;
        [UI] private Entry _entryselect = null;
        [UI] private Entry _entrytag = null;


        

        [UI] private Button _btnnext = null;
        [UI] private Button _btnonereplace = null;
        [UI] private Button _btnallreplace = null;
        [UI] private Button _btnerror = null;

        [UI] private Button _btnundo = null;
        [UI] private Button _btnredo = null;


        [UI] private FontButton _btnfont = null;


        [UI] private TreeView _treeview = null;

        [UI] private TreeView _treeviewselect = null;


        [UI] private TextView _textviewsource = null;
        
        [UI] private TextView _textviewtexte = null;

        [UI] private TextView _textviewsauv = null;


        [UI] private Label _txtfolder = null;
        [UI] private Label _txtnbfolder = null;
        [UI] private Label _txtselfile = null;

        [UI] private Notebook _notebook = null;


        [UI] private InfoBar _bottomnav = null;

        [UI] private InfoBar _topnav = null;

        [UI] private InfoBar _infotexte = null;

        //[UI] private TreeSelection _treeselection = null;

        [UI] private CellRendererText _colsource = null;
        [UI] private CellRendererText _coltexte = null;
        [UI] private CellRendererText _colsauv = null;

        [UI] private CellRendererToggle _togglesource = null;
        [UI] private CellRendererToggle _toggletexte = null;
        [UI] private CellRendererToggle _togglesauv = null;

        [UI] private ProgressBar _progress  = null;

        [UI] private ToggleButton _toggleerror = null;

        [UI] private ToggleButton _toggleparenthese = null;

        [UI] private ToggleButton _togglequote = null;

        private int _counter;

        Stack<string> undoStack;
		Stack<string> redoStack;

        bool editchange = false;

        private Gtk.ListStore store = new Gtk.ListStore (typeof(string), typeof(string), typeof(string));
        
        private Gtk.ListStore fileslist = new Gtk.ListStore (typeof(bool), typeof(string), typeof(bool), typeof(string), typeof(bool), typeof(string));
        //typeof(Gdk.Pixbuf)

        string Box_Bracket;

        public MainWindow() : this(new Builder("MainWindow.glade")) { }

        private MainWindow(Builder builder) : base(builder.GetRawOwnedObject("MainWindow"))
        {
            builder.Autoconnect(this);

            DeleteEvent += Window_DeleteEvent;
            _poperror.DeleteEvent += poperror_DeleteEvent;
            _poperror.WindowStateEvent += poperror_Activate;
            _toggleerror.Clicked += _togglepopup_Clicked;
            _toggleparenthese .Clicked += _togglepopup_Clicked;
            _togglequote.Clicked += _togglepopup_Clicked;


            //_button1.Clicked += Button1_Clicked;
            _btnopenfolder.Clicked += btnopenfolder_Clicked;

            _btntabaccueil.Clicked += btntabaccueil_Clicked;
            _btntabfile.Clicked += btntabaccueil_Clicked;
            _btntabtexte .Clicked += btntabaccueil_Clicked;
            _btntabsauv.Clicked += btntabaccueil_Clicked;
    
            _treeview.RowActivated += treeselection_RowActivated;


            _listerror.ListRowActivated += listerror_ListRowActivated;

            //_btnfont.FontSet += btnfont_FontSet;


            _btnextract.Clicked += btnextract_Clicked;
            _btntranslate.Clicked += btntranslate_Clicked;
            _btncompile.Clicked += btncompile_Clicked;

            _btnrpa.Activated += _btnrpa_Clicked;

            _btnbarsauv.Clicked += btnbarsauv_Clicked;
            _btnselectcara.Clicked += btnselectcara_Cliked;


            _btnnext.Clicked += btnnext_Clicked;
            _btnonereplace.Clicked += btnonereplace_Clicked;
            _btnallreplace.Clicked += btnallreplace_Clicked;
            _btnundo.Clicked += btnundo_Clicked;
            _btnredo.Clicked += btnredo_Clicked;

            _btnerror.Clicked += btnerror_Cliked;

            //_textviewsource.Buffer.InsertText += textviewsource_Changed;

            _textviewsource.Buffer.UserActionBegun += OnUserActionBegun;
            _textviewtexte.Buffer.UserActionBegun += OnUserActionBegun;
            _textviewsauv.Buffer.UserActionBegun += OnUserActionBegun;

            _textviewsource.Buffer.MarkSet += OnActionSelect;
            _textviewtexte.Buffer.MarkSet += OnActionSelect;
            _textviewsauv.Buffer.MarkSet += OnActionSelect;

            //_searchentry.Changed += btnsearch_Clicked;
            //_searchentry.KeyPressEvent += EntryKeyPressEvent;

            _searchentry.Activated += new EventHandler (btnsearch_Clicked);


            //defaul page
            _notebook.CurrentPage = 0;
            _btntabaccueil.Relief = ReliefStyle.Half;
            //_reveal.RevealChild = true;
            _topnav.Visible = false;
            _bottomnav.Visible = false;

            _toggleerror.Sensitive = false;
             
            //_panedsearch.Position = -10;
            //textView = new Edit(_textviewsource);
            Util.addTabs(_textviewsource, _textviewtexte, _textviewsauv);         

            _btnfont.FontSet += (o, args) =>
            {
                _textviewsource.ModifyFont(Pango.FontDescription.FromString (_btnfont.FontName));

                _textviewtexte.ModifyFont(Pango.FontDescription.FromString (_btnfont.FontName));

                _textviewsauv.ModifyFont(Pango.FontDescription.FromString (_btnfont.FontName));

            };


			undoStack = new Stack<string> ();
			redoStack = new Stack<string> ();
            undoStack.Clear ();
			redoStack.Clear ();

        //majuscule ou chiffre
        var Box_Brackets = new List<string>();
        Box_Brackets.Add(@"\[[^\]]*[A-Z]+[^\]]*\]");
        //ouvert et pas fermer
        Box_Brackets.Add(@"^.*?\[(?!.*?\])[^\]].*?$");
        //fermer pas ouvert
        Box_Brackets.Add(@"^[^[\r\n]*\].*?$");


        //majuscule ou chiffre
        //var Brace_Brackets = new List<string>();
        Box_Brackets.Add(@"\{[^\}]*[A-Z]+[^\}]*\}");
        //ouvert et pas fermer
        Box_Brackets.Add(@"^.*?\{(?!.*?\})[^\}].*?$");
        //#fermer pas ouvert
        Box_Brackets.Add(@"^[^{\r\n]*\}.*?$");

        //string.Format("@\"{0}\"", text);
        Box_Brackets.Add("@{(/?!(/.*?)/?(a|alpha|alt|b|color|cps|font|i|image|k|noalt|outlinecolor|plain|s|size|space|u|vspace|#|done|fast|nw|p|w|clear)(=.*?)/?})|\".+?\".+?\"");

        Box_Bracket = string.Join("|", Box_Brackets.ToArray());

        //_btnfont.FontSet  += new System.EventHandler(this.OnFontbutton1FontSet);          

        }


        private void Window_DeleteEvent(object sender, DeleteEventArgs a)
        {
            Application.Quit();
        }

        private void OnActionSelect(object sender, EventArgs a)
        {
            var names = Lookup();

            TextIter start, end;
            if (names.Item1.Buffer.GetSelectionBounds(out start, out end))
            {
            int numtexte = names.Item1.Buffer.GetText(start, end, true).Length;
            _entryselect.Text = numtexte.ToString();
            }
            else 
            {
            _entryselect.Text = "0";
            }
    
//            _entryselect
        }


        private void poperror_DeleteEvent(object sender, DeleteEventArgs a)
        {
            var children = _listerror.Children;
            foreach (Gtk.Widget element in children)
                _listerror.Remove(element);

            _poperror.Hide ();
			a.RetVal = true;
            //_listerror.Destroy();
            //_poperror.Hide();
            //_poperror.Destroy();
        }

        private void _togglepopup_Clicked(object sender, EventArgs a)
        {

            ToggleButton btn = sender as ToggleButton;

            _toggleerror.Sensitive = true;
            _toggleparenthese.Sensitive = true;
            _togglequote.Sensitive = true;

            btn.Sensitive = false;

            rechercheRegex();
            
        }

        private void poperror_Activate(object sender, WindowStateEventArgs a)
        {
		    if (a.Event.ChangedMask == Gdk.WindowState.Focused) {
                if (editchange)
                {
                rechercheRegex();
                editchange = false;
                }
            }
                        
        }


        private void textviewsource_Changed(object sender, InsertTextArgs a)
        {
  
        }

        private void OnUserActionBegun(object sender, EventArgs args)
		{
            var names = Lookup();
			undoStack.Push(names.Item1.Buffer.Text);

            _infotexte.Visible = true;
            editchange = true;
      
		}

        private void btnundo_Clicked(object sender, EventArgs args)
		{
            var names = Lookup();

			redoStack.Push(names.Item1.Buffer.Text);
			if (undoStack.Count>0)	names.Item1.Buffer.Text = undoStack.Pop ();
		}

        private void btnredo_Clicked(object sender, EventArgs args)
		{
            var names = Lookup();

			undoStack.Push(names.Item1.Buffer.Text);
			if (redoStack.Count>0)	names.Item1.Buffer.Text = redoStack.Pop ();
		}

        private void btnerror_Cliked(object sender, EventArgs a)
        {

            //_panedsearch.Position = 700;
            rechercheRegex();

            // foreach(var tag in listOfAlbums)
            // {  
            //     ListBoxRow row = new ListBoxRow() { Name = tag.Name};
            //     row.Add(new Label(tag.Name));
            //     _listerror.Add(row);
            // }

            //_listerror.RowActivated += (sender, e) => command.Invoke((ListBox)sender, e);
            //_listerror.Show();
            //_listerror.Visible = true;
            _poperror.Modal = false;
     
            _poperror.ShowAll();
            //_poperror.Run();
            //_poperror.Hide();
               
        }

        private void listerror_ListRowActivated(object sender,  ListRowActivatedArgs a)
        {

            var names = Lookup();

            //Console.WriteLine(test);
            TextIter start, end;
            names.Item1.Buffer.GetSelectionBounds(out start, out end);

            int bint = _listerror.SelectedRow.Index;
            var select = listOfAlbums[bint];
    

            names.Item1.Buffer.SelectRange(select.Start, select.End);
            names.Item1.ScrollToIter(select.End, 0, false, 0, 0);
            
        }


        private void btnallreplace_Clicked(object sender, EventArgs a)
        {
            var names = Lookup();
            TextBuffer buffer = names.Item1.Buffer;
            Gtk.TextIter start;
            Gtk.TextIter end;

            string replace = _replaceentry.Text;

            recherche();
            int total = listOfAlbums.Count;

            for(int i = 0; i < total; i++)
            {
                recherche();
                foreach(var tag in listOfAlbums)
                {  
                    buffer.SelectRange(tag.Start, tag.End);
                    if (buffer.GetSelectionBounds(out start, out end))
                    {
                    buffer.DeleteSelection(true, true);
                    buffer.InsertAtCursor(replace);
                    buffer.PlaceCursor(tag.End);
                    break;
                    }
                }
            }

            MessageDialog md = new MessageDialog (this, 
            DialogFlags.DestroyWithParent, MessageType.Error, 
            ButtonsType.Ok, String.Format("{0} Mots remplacer.",  total));
            md.Run();
            md.Destroy();


        }
        private void btnonereplace_Clicked(object sender, EventArgs a)
        {
            var names = Lookup();
            string replace = _replaceentry.Text;

            //Edit edit = new Edit(names.Item1);
            //edit.ReplaceSelection(_replaceentry.Text);
            TextBuffer buffer = names.Item1.Buffer;

            Gtk.TextIter start;
            Gtk.TextIter end;

            if (buffer.GetSelectionBounds(out start, out end))
            {
            //buffer.BeginUserAction();
            buffer.DeleteSelection(true, true);
            buffer.InsertAtCursor(replace);
            buffer.PlaceCursor(end);
            //buffer.EndUserAction();
            }


        }

        private void btnnext_Clicked(object sender, EventArgs a)
        {
            //this.ActivelyWaitFor( 1000 );
            var names = Lookup();

            //int pos = 0;
            TextBuffer buffer = names.Item1.Buffer;

            TextIter startIter, endIter;

            endIter =  buffer.StartIter;

            if (buffer.GetSelectionBounds (out startIter, out endIter))
            {
                startIter = endIter;    
            } 
            else 
            {
                startIter = buffer.GetIterAtOffset(buffer.CursorPosition);
                if (startIter.Offset == buffer.CharCount)
                {
                    startIter = buffer.StartIter;
                }
            }

            //TextIter start = buffer.StartIter;
            TextIter end;

            while (startIter.ForwardToTagToggle(null))
            {

                if (startIter.StartsTag(null))
                {
                    end = startIter;
                    end.ForwardToTagToggle(null);
                    names.Item1.Buffer.SelectRange(startIter, end);
                    //Console.WriteLine(string.Format("start {0} end {1}", startIter.Offset, end.Offset));
                    startIter = end;
                    names.Item1.ScrollToIter(startIter, 0, false, 0, 0);
                    break;
                }
    
            }
                //start.EndsTag
                //names.Item1.Buffer.SelectRange(start, end);


			//end = start;
            //end.ForwardToTagToggle(cstag);
            //string test = names.Item1.Buffer.GetText(start,end, false);
            //names.Item1.Buffer.SelectRange(start, end);
            //Console.WriteLine(test);

        }

        private void btntranslate_Clicked(object sender, EventArgs a)
        {

            if (System.IO.File.Exists(filedict["file_fr"]))
            {
                _btntabtexte.Relief = ReliefStyle.Half;
                _notebook.CurrentPage = 2;
                _topnav.Visible = true;
                _bottomnav.Visible = true;
            }
            else 
            {

                MessageDialog md = new MessageDialog (this, 
                DialogFlags.DestroyWithParent, MessageType.Error, 
                ButtonsType.Ok, String.Format("Impossible d'ouvrir {0}.",  filedict["file_fr"]));
                md.Run();
                md.Destroy();

            }

        }

        (TextView, string, string, string) Lookup() // tuple return type
        {
            if (_notebook.CurrentPage == 1)
            {
            return (_textviewsource, filedict["file"], "tag_search_source", "tag_error_source");
            }
            if (_notebook.CurrentPage == 2)
            {
            return (_textviewtexte, filedict["file_fr"], "tag_search_texte", "tag_error_texte");
            }
            if (_notebook.CurrentPage == 3)
            {
            return (_textviewsauv, filedict["file_old"], "tag_search_sauv", "tag_error_sauv");
            }
            return (null, null, null, null);
        }

        private void btnsearch_Clicked(object sender, EventArgs a)
        {
            recherche();
        }


        void recherche()
        {
    
            var names = Lookup();
            
            TextIter startiter, enditer;

            TextBuffer buffer = names.Item1.Buffer;

            TextIter start = buffer.StartIter;
            TextIter end = buffer.EndIter;

            buffer.RemoveTag(names.Item3, start, end);
            buffer.RemoveTag(names.Item4, start, end);
            listOfAlbums.Clear();

            int combo = _combosearch.Active;            
            string text = _searchentry.Text;

            switch (combo)
            {
                case 0: case 1:

                    TextSearchFlags flag = TextSearchFlags.TextOnly|TextSearchFlags.CaseInsensitive;

                    if (combo == 1)
                    {
                        flag = TextSearchFlags.TextOnly;
                    }

                    
                    while (start.ForwardSearch(text, flag, out startiter, out enditer, end))
                    {

                            buffer.ApplyTag(names.Item3, startiter, enditer);
                            Album example = new Album("test", startiter, enditer);
                            listOfAlbums.Add(example);
                            //start.ForwardLine();
                            start = enditer;
                    }
                break;

                case 2:
                    //@"{(.+?)}"
                    //string regtext = string.Format("@\"{0}\"", text);
                    foreach (Match match in Regex.Matches(names.Item1.Buffer.Text, text))
                    {
                        //char char out = match.ValueSpan;
                        int regexstart  =   match.Index;
                        int regexend = regexstart + match.Length;

                        TextIter startnew = names.Item1.Buffer.GetIterAtOffset(regexstart);
                        TextIter endnew = names.Item1.Buffer.GetIterAtOffset(regexend);
       
                        names.Item1.Buffer.ApplyTag(names.Item3, startnew, endnew);
                        Album example = new Album("test", startnew, endnew);
                        listOfAlbums.Add(example);
                    }
                break;

                default:
                break;
            }

        }

        void rechercheRegex()
        {
    
            var names = Lookup();
            
            TextIter startiter, enditer;

            TextBuffer buffer = names.Item1.Buffer;

            TextIter start = buffer.StartIter;
            TextIter end = buffer.EndIter;

            buffer.RemoveTag(names.Item3, start, end);
            buffer.RemoveTag(names.Item4, start, end);
            listOfAlbums.Clear();
            var children = _listerror.Children;           
            foreach (Gtk.Widget element in children)
                _listerror.Remove(element);

            int combo = _combosearch.Active;            
            //string text = _searchentry.Text;
            
            string text = @"{(.+?)}";
            if (!_toggleerror.Sensitive)
            {
                text = Box_Bracket;
            }
            else if (!_toggleparenthese.Sensitive)
            {
                text = @"{(.+?)}";
            }
            else if (!_togglequote.Sensitive)
            {
                text = @"\[(.+?)\]";
            }

            //string regtext = string.Format("@\"{0}\"", text);
            foreach (Match match in Regex.Matches(buffer.Text, text))
            {
                //char char out = match.ValueSpan;
                int regexstart  =   match.Index;
                int regexend = regexstart + match.Length;

                TextIter startnew = buffer.GetIterAtOffset(regexstart);
                TextIter endnew = buffer.GetIterAtOffset(regexend);

                string texte = match.Value;

                names.Item1.Buffer.ApplyTag(names.Item4, startnew, endnew);
                Album example = new Album(texte, startnew, endnew);
                listOfAlbums.Add(example);
            }

            foreach(var tag in listOfAlbums)
            {  
                ListBoxRow row = new ListBoxRow() { Name = tag.Name};
                row.Add(new Label(tag.Name));
                _listerror.Add(row);
            }
            _entryerrortotal.Text = listOfAlbums.Count.ToString();
            _listerror.ShowAll();
        }

        private void btnselectcara_Cliked(object sender, EventArgs a)
        {
            //Console.WriteLine(_entrymaxcara.Text);
            Gtk.TextIter start;
            Gtk.TextIter end;

    
           //change cursor position
           //textView.Buffer.PlaceCursor(textView.Buffer.EndIter).
           // try 
            //{
            var names = Lookup();
            string max = _entrymaxcara.Text;

            //var start2 = names.Item1.Buffer.CursorPosition;

            names.Item1.Buffer.GetSelectionBounds(out start, out end);

            names.Item1.GrabFocus();
            //start = names.Item1.Buffer.CursorPosition;
            end = names.Item1.Buffer.GetIterAtOffset(end.Offset + 3000);
            end.BackwardChars(end.LineOffset); 

            //end = end.CharsInLine;
            //end.CharsInLine = 3000;
            string text = names.Item1.Buffer.GetText(start, end, true);

            //TextMark startMark = buffer.CreateMark("start", start, false);
            //TextMark endMark   = buffer.CreateMark("end", end, true);
            //TextIter start3 = buffer.GetIterAtMark(startMark);
            //TextIter end3   = buffer.GetIterAtMark(endMark)
            //names.Item1.Buffer.PlaceCursor(end);
            names.Item1.Buffer.SelectRange(start, end);
            names.Item1.ScrollToIter(end, 0, false, 0, 0);
            

            //Console.WriteLine(text);
            //}
            //catch 
            //{

            //}

        }

        private void btnbarsauv_Clicked(object sender, EventArgs a)
        {

            try {
                var names = Lookup();
                System.IO.StreamWriter output = new System.IO.StreamWriter(names.Item2);
                output.Write(names.Item1.Buffer.Text);
                output.Close();

                _infotexte.Visible = false;
                editchange = false;

                MessageDialog md1 = new MessageDialog (this, 
                DialogFlags.DestroyWithParent, MessageType.Info, 
                ButtonsType.Ok, String.Format("Enregistrer avec succée {0}.", names.Item2));
                md1.Run();
                md1.Destroy();
    
            
            }
            catch {

                MessageDialog md1 = new MessageDialog (this, 
                DialogFlags.DestroyWithParent, MessageType.Error, 
                ButtonsType.Ok, String.Format("Erreur Impossible de sauvegarder {0}.",  filedict["file_fr"]));
                md1.Run();
                md1.Destroy();
                return;

            }


        }
 

        private void btntabaccueil_Clicked(object sender, EventArgs a)
        {

            Button btn = sender as Button;

            //reset button
            _btntabaccueil.Relief = ReliefStyle.None;
            _btntabfile.Relief = ReliefStyle.None;
            _btntabtexte.Relief = ReliefStyle.None;
            _btntabsauv.Relief = ReliefStyle.None;
            _topnav.Visible = false;
            _bottomnav.Visible = false;
 

            //int test = _notebook.CurrentPage;
            
            if (btn == _btntabaccueil)
            {
                btn.Relief = ReliefStyle.Half;
                //btn.UseUnderline = true;
                _notebook.CurrentPage = 0;
            }
            if (btn == _btntabfile)
            {
                btn.Relief = ReliefStyle.Half;
                _notebook.CurrentPage = 1;
                _topnav.Visible = true;
                _bottomnav.Visible = true;
                _entrytotal.Text = _textviewsource.Buffer.CharCount.ToString();
            }
            if (btn == _btntabtexte)
            {
                btn.Relief = ReliefStyle.Half;
                _notebook.CurrentPage = 2;
                _topnav.Visible = true;
                _bottomnav.Visible = true;
                _entrytotal.Text = _textviewtexte.Buffer.CharCount.ToString();
            }
            if (btn == _btntabsauv)
            {
                btn.Relief = ReliefStyle.Half;
                _notebook.CurrentPage = 3;
                _topnav.Visible = true;
                _bottomnav.Visible = true;
                _entrytotal.Text = _textviewsauv.Buffer.CharCount.ToString();

            }


             //_toggle.Mode = false;

            //Console.WriteLine(test);
        }

        private void btnextract_Clicked(object sender, EventArgs a)
        {

            System.IO.StreamWriter output = new System.IO.StreamWriter(filedict["file_fr"]);

            var dictionary = System.IO.File.ReadAllLines(filedict["file"]);

            //pourcent = math.floor(cnt*1000/num_lines)
            int total = dictionary.Length;

            int cnt=1;
            foreach (string line in dictionary)
                {
                    string newline = Util.ParseRpyLine(line);
                    double calc = cnt/total;
                    _progress.Fraction = Math.Floor(calc);
                    cnt++;
                    if (newline != string.Empty)
                    {
                        output.WriteLine(newline);
                        //Console.WriteLine(line);
                    }

                }
                _progress.Fraction = 0;
                output.Close();

                MessageDialog md = new MessageDialog (this, 
                DialogFlags.DestroyWithParent, MessageType.Info, 
                ButtonsType.Ok,"");

            if (System.IO.File.Exists(filedict["file_fr"]))
                {
                    md.Text = String.Format("Fichier Créer avec Succée {0}.",  filedict["filename_fr"]);
        
                }
                else 
                {
                    md.Text = String.Format("Erreur Impossible de trouver {0}.",  filedict["filename_fr"]);

                }

                md.Run();
                md.Destroy();

                treeselection_Update();

        }

        private void btncompile_Clicked(object sender, EventArgs a)
        {
            //copie
            try 
            {
                System.IO.File.Copy(filedict["file"], filedict["file_old"]);

            }
            catch 
            {
                MessageDialog md1 = new MessageDialog (this, 
                DialogFlags.DestroyWithParent, MessageType.Error, 
                ButtonsType.Ok, String.Format("Erreur Impossible de créer {0}.",  filedict["file_old"]));
                md1.Run();
                md1.Destroy();
                return;
            }

            var dictionary = System.IO.File.ReadAllLines(filedict["file"]);
            //string[] texte_dictionary = System.IO.File.ReadAllLines(filedict["file_fr"]);
            //pourcent = math.floor(cnt*1000/num_lines)
            int total = dictionary.Length;

            int cnt=1;

            System.IO.StreamReader reader = new System.IO.StreamReader(filedict["file_fr"]);

            using (System.IO.StreamWriter output = new System.IO.StreamWriter(filedict["file"]))
            {
                foreach (string line in dictionary)
                {
                    string newline = Util.ParseRpyLine(line);
                    double calc = cnt/total;
                    _progress.Fraction = Math.Floor(calc);
                    cnt++;
                    if (newline != string.Empty)
                    {
                        var replaceline = line.Replace(newline, reader.ReadLine());
                        output.WriteLine(replaceline);

                    }
                    else 
                    {
                        output.WriteLine(line);

                    }

                }
            }

            //System.IO.File.WriteAllLines(filedict["file"], dictionary);


                _progress.Fraction = 0;

                MessageDialog md = new MessageDialog (this, 
                DialogFlags.DestroyWithParent, MessageType.Info, 
                ButtonsType.Ok,"");

            if (System.IO.File.Exists(filedict["file_old"]))
                {
                    md.Text = String.Format("Fichier Créer avec Succé et Sauvegardé {0}/{1}.",  filedict["filename"], filedict["filename_old"]);
        
                }
                else 
                {
                    md.Text = String.Format("Erreur Impossible de trouver {0}.",  filedict["filename_old"]);

                }

                md.Run();
                md.Destroy();

                treeselection_Update();
            
        }

        private void _btnrpa_Clicked(object sender, EventArgs a)
        {
            Gtk.FileChooserDialog filechooser = new Gtk.FileChooserDialog("Choose the file to open", this,
            FileChooserAction.SelectFolder,
            "Cancel",ResponseType.Cancel,
            "Open",ResponseType.Accept);

            if (filechooser.Run() == (int)ResponseType.Accept) 
            {
                Util.Rpa(filechooser.CurrentFolder);

            }
            filechooser.Destroy();

        }
        

        private void btnopenfolder_Clicked(object sender, EventArgs a)
        {

            Gtk.FileChooserDialog filechooser = new Gtk.FileChooserDialog("Choose the file to open", this,
            FileChooserAction.SelectFolder,
            "Cancel",ResponseType.Cancel,
            "Open",ResponseType.Accept);

            if (filechooser.Run() == (int)ResponseType.Accept) 
            {
                //efface listore
                store.Clear();

                //System.IO.FileStream file = System.IO.File.OpenRead(filechooser.CurrentFolder);
                //System.IO.Directory.SetCurrentDirectory(filechooser.CurrentFolder);
                //string folder = Path.GetDirectoryName( file );
                System.IO.Directory.CreateDirectory(filechooser.CurrentFolder);
                //string[] files = System.IO.Directory.GetFiles(filechooser.CurrentFolder);
                string[] files = System.IO.Directory.GetFiles(filechooser.CurrentFolder, "*.rpy", System.IO.SearchOption.AllDirectories);
                Array.Sort(files);

                _txtfolder.Text = filechooser.CurrentFolder;
                _txtnbfolder.Text = files.Length.ToString();
		        //string[] directories = System.IO.Directory.GetDirectories(filechooser.CurrentFolder);
                //Gtk.TreeViewColumn pathColumn = new Gtk.TreeViewColumn ();
                //pathColumn.Title = "Path";
                //_treeview.AppendColumn( pathColumn );

                //var pathListStore = new Gtk.ListStore( typeof( string ) );
                //_treeview.Model = liststore7;


                foreach (string file in files)
                {
                    string fileName = System.IO.Path.GetFileName(file);
                    string targetPath = System.IO.Path.Combine(filechooser.CurrentFolder, fileName);
                    //long length = new System.IO.FileInfo(file).Length;

                    string lenght = Util.GetFileSize(file);

                    
                    System.IO.File.Move(file, targetPath);
                    //Console.WriteLine(length);
                    //store.AppendValues(fileName, lenght, file);

                    store.AppendValues(new object[] { fileName, lenght, file });
                    //liststore7.AppendValues(fileName, lenght, file);
                    
                }

                store.SetSortColumnId(0, Gtk.SortType.Ascending);
                _treeview.Model = store;


                //Console.WriteLine(test);
                //file.Close();
            }


            filechooser.Destroy();
            //Console.WriteLine(this._btnopenfolder[]file);
                    
        }

        public bool msg_sure() 
        {
            MessageDialog md = new MessageDialog (this, 
            DialogFlags.DestroyWithParent, MessageType.Warning, 
            ButtonsType.OkCancel, "ATTENTION : Modification non Enregistrée \nSi vous continuez les modifications ne seront pas pris en compte. (Continuer ?)");
            //md.Run();
            Gtk.ResponseType res = (Gtk.ResponseType)md.Run();
            md.Destroy();

            if (res == ResponseType.Ok)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void treeselection_RowActivated(object sender, RowActivatedArgs args)
        {
            if (editchange) {

                bool res = msg_sure();
                if (!res){
                    return;
                }
                _infotexte.Visible = false;
                editchange = false;

            }
            //select
            var model = _treeview.Model;
            Gtk.TreeIter selected;
            _treeview.Selection.GetSelected(out selected);
            var value2 = model.GetValue(selected, 2);
            var value = model.GetValue(selected, 0);

            _txtselfile.Text = value2.ToString();

            filedict = Util.getDictFile(value2.ToString());


            treeselection_Update();
        }


        private void treeselection_Update()
        {

            fileslist.Clear();
            
            bool check_source = false;
            bool check_fr = false;
            bool check_old = false;

            //new Gdk.Pixbuf("assets/report.png")
            if (System.IO.File.Exists(filedict["file"]))
                {
                    check_source = true;
                    
                    System.IO.StreamReader stream = new System.IO.StreamReader(filedict["file"]);
				    _textviewsource.Buffer.Text = stream.ReadToEnd();
                    _textviewsource.Editable = true;
                    _textviewsource.Sensitive = true;

                    //_textviewsource.ModifyFont(Pango.FontDescription.FromString ("Comic Sans MS 40"));
                    _textviewsource.ModifyFont(Pango.FontDescription.FromString (_btnfont.FontName));

                    
                    //string fontName = _btnfont.FontName;
                    //Pango.FontDescription fontDescription = Pango.FontDescription.FromString(fontName);

                    //_textviewsource.Add(fontDescription);
                    //_textviewsource.Buffer.Text = "testttt";
                    //sw.Write(_textviewsource.Buffer.Text);
                    //sw.Close();
                    //string text = _textviewsource.Buffer.Text;
                    //int lines = text.Split('\n').Length;
                    //msg = String.Format("This {0}.  The value is {1}.",  lines,  text.Length);

                    

                }
            else
                {

                _textviewsource.Buffer.Text = "";
                _textviewsource.Editable = false;
                _textviewsource.Sensitive = false;
                
                MessageDialog md = new MessageDialog (this, 
                DialogFlags.DestroyWithParent, MessageType.Error, 
                ButtonsType.Close, "Error loading file");
                md.Run();
                md.Destroy();
                return;
                    
                }

            if (System.IO.File.Exists(filedict["file_fr"]))
                {
                    check_fr = true;
                    //coltexte = new Gdk.Color(153, 255, 153);
                    setexiste(true, _toggletexte, _textviewtexte, _coltexte);

                    System.IO.StreamReader stream = new System.IO.StreamReader(filedict["file_fr"]);
				    _textviewtexte.Buffer.Text = stream.ReadToEnd();
                    //_textviewtexte.Editable = true;
                    //_textviewtexte.Sensitive = true;
                    _textviewtexte.ModifyFont(Pango.FontDescription.FromString (_btnfont.FontName));


                }
            else
                {
                    //Console.WriteLine("textee nonono");
                    setexiste(false, _toggletexte, _textviewtexte, _coltexte);                
                    
                }
            if (System.IO.File.Exists(filedict["file_old"]))
                {
                    check_old = true;
                    //colsauv = new Gdk.Color(153, 255, 153);
                    setexiste(true, _togglesauv, _textviewsauv, _colsauv);

                     System.IO.StreamReader stream = new System.IO.StreamReader(filedict["file_old"]);
				    _textviewsauv.Buffer.Text = stream.ReadToEnd();
                    //_textviewsauv.Editable = true;
                    //_textviewsauv.Sensitive = true;
                    _textviewsauv.ModifyFont(Pango.FontDescription.FromString (_btnfont.FontName));

                        
                }
            else
                {
                    //Console.WriteLine("sauv nonono");
                    setexiste(false, _togglesauv, _textviewsauv, _colsauv);   
                }

            //fileslist.AppendValues(true, value.ToString(),true, value.ToString(),true, value.ToString());
            fileslist.AppendValues(new object[] { true, filedict["filename"], check_fr, filedict["filename_fr"], check_old, filedict["filename_old"] });
            
            //bleau
            //Gdk.Color bleucolor = _coltexte.CellBackgroundGdk;
            //bleucolor = new Gdk.Color(153, 255, 255);
            //_coltexte.CellBackgroundGdk = bleucolor;


            _treeviewselect.Model = fileslist;

            //_togglesource.Active = true;
                    
        }


        private void setexiste(bool existe, CellRendererToggle cheched, TextView textview, CellRenderer col)
        {
            Gdk.Color colorred = new Gdk.Color(255, 153, 153);

            //cheched.Active = false;
        
            if (existe)
            {
                colorred = new Gdk.Color(153, 255, 153);
                textview.Editable = true;
                //textview.Sensitive = true;
                //fileslist.SetValue(iter, 0, true);
            }
            else
            {
                colorred = new Gdk.Color(255, 153, 153);
                textview.Buffer.Text = "";
                textview.Editable = false;
                //textview.Sensitive = false;
                cheched.Sensitive = false;

            }

            //cheched.CellBackgroundGdk = colorred;

            col.CellBackgroundGdk = colorred;

        }

        
    }
}
