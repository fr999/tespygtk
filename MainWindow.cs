using System;
using System.Collections.Generic;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Threading;


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

        public static Dictionary<string, string> filedict = null;

        [UI] private Dialog _poperror = null;

        [UI] private ListBox _listerror = null;
        [UI] private Button _btnopenfolder = null;

        //menu top
        [UI] private MenuItem _menuitemopen = null;
        [UI] private MenuItem _menuitemquit = null;
        [UI] private MenuItem _btnrpa = null;
        [UI] private MenuItem _toolgeneratelanguage = null;
        [UI] private MenuItem _toolforcelanguage = null;
        [UI] private MenuItem _toollint = null;

        [UI] private MenuItem _menuitemabout = null;


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

        [UI] private Button _btnnext = null;
        [UI] private Button _btnonereplace = null;
        [UI] private Button _btnallreplace = null;
        [UI] private Button _btnerror = null;

        [UI] private Button _btnundo = null;
        [UI] private Button _btnredo = null;


        [UI] private FontButton _btnfont = null;


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

        [UI] private ProgressBar _progress  = null;

        [UI] private ToggleButton _toggleerror = null;

        [UI] private ToggleButton _toggleparenthese = null;

        [UI] private ToggleButton _togglequote = null;

        //button translate bar
        [UI] private Button _btnbartranslate = null;
        [UI] private ComboBox _combointranslate = null;
        [UI] private ComboBox _comboouttranslate = null;
        [UI] private ComboBox _combotranslate = null;

        Stack<string> undoStack;
		Stack<string> redoStack;

        bool editchange = false;
        
        private Gtk.ListStore fileslist = new Gtk.ListStore (typeof(string), typeof(string), typeof(bool), typeof(string), typeof (string), typeof(bool), typeof(string), typeof(string), typeof(string));
        //typeof(Gdk.Pixbuf)

        string Box_Bracket;

        public MainWindow() : this(new Builder("MainWindow.glade")) { }

        private MainWindow(Builder builder) : base(builder.GetRawOwnedObject("MainWindow"))
        {
            builder.Autoconnect(this);

            DeleteEvent += Window_DeleteEvent;
            _menuitemquit.Activated += Window_DeleteEvent2;

            
            _poperror.DeleteEvent += poperror_DeleteEvent;
            _poperror.WindowStateEvent += poperror_Activate;
            _toggleerror.Clicked += _togglepopup_Clicked;
            _toggleparenthese .Clicked += _togglepopup_Clicked;
            _togglequote.Clicked += _togglepopup_Clicked;


            //_button1.Clicked += Button1_Clicked;
            _menuitemopen.Activated += btnopenfolder_Clicked;
            _btnopenfolder.Clicked += btnopenfolder_Clicked;

            _btntabaccueil.Clicked += btntabaccueil_Clicked;
            _btntabfile.Clicked += btntabaccueil_Clicked;
            _btntabtexte .Clicked += btntabaccueil_Clicked;
            _btntabsauv.Clicked += btntabaccueil_Clicked;
    
            _treeviewselect.RowActivated += homeselection_RowActivated;


            _listerror.ListRowActivated += listerror_ListRowActivated;

            //_btnfont.FontSet += btnfont_FontSet;


            _btnextract.Clicked += btnextract_Clicked;
            _btntranslate.Clicked += btntranslate_Clicked;
            _btncompile.Clicked += btncompile_Clicked;

            _btnrpa.Activated += btnrpa_Clicked;
            _toolgeneratelanguage.Activated += toolgeneratelanguage_Clicked;
            _toollint.Activated += toollint_Clicked;
            _toolforcelanguage.Activated += toolforcelanguage_Clicked;

            _menuitemabout.Activated += menuitemabout_Clicked;

            _btnbarsauv.Clicked += btnbarsauv_Clicked;
            _btnselectcara.Clicked += btnselectcara_Cliked;


            _btnnext.Clicked += btnnext_Clicked;
            _btnonereplace.Clicked += btnonereplace_Clicked;
            _btnallreplace.Clicked += btnallreplace_Clicked;
            _btnundo.Clicked += btnundo_Clicked;
            _btnredo.Clicked += btnredo_Clicked;

            _btnerror.Clicked += btnerror_Cliked;

            _btnbartranslate.Clicked += btnbartranslate_Clicked;

            //_textviewsource.Buffer.InsertText += textviewsource_Changed;

            _textviewsource.Buffer.UserActionBegun += OnUserActionBegun;
            _textviewtexte.Buffer.UserActionBegun += OnUserActionBegun;
            _textviewsauv.Buffer.UserActionBegun += OnUserActionBegun;

            _textviewsource.Buffer.MarkSet += OnActionSelect;
            _textviewtexte.Buffer.MarkSet += OnActionSelect;
            _textviewsauv.Buffer.MarkSet += OnActionSelect;

            //_searchentry.Changed += btnsearch_Clicked;
            //_searchentry.KeyPressEvent += EntryKeyPressEvent;

            //_searchentry.Activated += new EventHandler (btnsearch_Clicked);
            _searchentry.Activated += btnsearch_Clicked;


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

        private void Window_DeleteEvent2(object sender, EventArgs a)
        {
            Application.Quit();
        }

        private void menuitemabout_Clicked(object sender, EventArgs a)
        {
            Gtk.AboutDialog dialog = new Gtk.AboutDialog();

            //Gdk.Pixbuf logo = new Gdk.Pixbuf("assets/logo.jpg");

            dialog.Icon = null;
            dialog.IconName = null;
            //dialog.Logo = logo;
            dialog.LogoIconName = "auth-sim-missing-symbolic";
            dialog.ProgramName = "TesPyGtk";
            dialog.Version = "0.0.4";
            dialog.Comments = "Gestion des fichiers de traductions Renpy";
            dialog.Authors = new string[] {"A_Furbyz"};
            dialog.Copyright = "F999 Team";
            dialog.License = "GPLv3";
            dialog.Website = "https://github.com/fr999/tespygtk";
            dialog.WebsiteLabel = "Github F999";
            dialog.Run();
            dialog.Destroy();
        }

        private void OnActionSelect(object sender, EventArgs a)
        {
            try {
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
            } catch {
                return;
            }
    
//            _entryselect
        }

        private async void btnbartranslate_Clicked(object sender, EventArgs a)
        {
            TreeIter news, newsin, newsout;
            TextIter start, end;

            var names = Lookup();
            var buffer = names.Item1.Buffer;

            //var TreeModel = _comboouttranslate.Model;
            _combotranslate.GetActiveIter(out news);
            String translate = (String) _combotranslate.Model.GetValue (news, 0);

            _combointranslate.GetActiveIter(out newsin);
            String intranslate = (String) _combointranslate.Model.GetValue(newsin, 1);
            //Console.WriteLine(intranslate);

            _comboouttranslate.GetActiveIter(out newsout);
            String outtranslate = (String) _comboouttranslate.Model.GetValue(newsout, 1);
            //Console.WriteLine(outtranslate);
            
            string result = string.Empty;

            if (buffer.GetSelectionBounds(out start, out end))
            {
                string text = buffer.GetText(start, end, true);
                result = await Util.Translate(translate, intranslate, outtranslate, text);
            
                if (result != string.Empty)
                {
                buffer.DeleteSelection(true, true);
                buffer.InsertAtCursor(result);
                //buffer.PlaceCursor(end);
                }
                else
                {
                    return;
                }
            
            }
            //Console.WriteLine(result);
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
                    case 3:
                    //@"goto line"

                    TextIter startline = names.Item1.Buffer.GetIterAtLine(Int32.Parse(text)-1);
                    TextIter endline =  names.Item1.Buffer.GetIterAtLine(Int32.Parse(text));

       
                    names.Item1.Buffer.ApplyTag(names.Item3, startline, endline);
                    names.Item1.ScrollToIter(endline, 0, false, 0, 0);

                
                break;

                default:
                break;
            }

        }

        void rechercheRegex()
        {
    
            var names = Lookup();
            
            TextBuffer buffer = names.Item1.Buffer;

            TextIter start = buffer.StartIter;
            TextIter end = buffer.EndIter;

            buffer.RemoveTag(names.Item3, start, end);
            buffer.RemoveTag(names.Item4, start, end);
            listOfAlbums.Clear();
            var children = _listerror.Children;           
            foreach (Gtk.Widget element in children)
                _listerror.Remove(element);
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
            Gtk.TextIter start;
            Gtk.TextIter end;

            var names = Lookup();
            int max = 1000;
            try 
            {
            max = int.Parse(_entrymaxcara.Text);
            }
            catch
            {
                return;
            }

            names.Item1.Buffer.GetSelectionBounds(out start, out end);
            if(start.EndsLine()) 
            {
                start.ForwardChars(1);
            }

            names.Item1.GrabFocus();
            end = names.Item1.Buffer.GetIterAtOffset(end.Offset + max);
            end.BackwardChars(end.LineOffset);
            end.BackwardChars(1);

            string text = names.Item1.Buffer.GetText(start, end, true);
            names.Item1.Buffer.SelectRange(start, end);
            names.Item1.ScrollToIter(end, 0, false, 0, 0);

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
            if(msg_sure()){
                return;
            }

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

            if (System.IO.File.Exists(filedict["file_fr"]))
            {

                MessageDialog md = new MessageDialog (
                    this, 
                    DialogFlags.DestroyWithParent, 
                    MessageType.Warning, 
                    ButtonsType.OkCancel, 
                    String.Format("Le fichier {0} existe\nVoulez-vous l'ecraser.",  filedict["filename_fr"]));
                Gtk.ResponseType res = (Gtk.ResponseType)md.Run();
                md.Destroy();

            if (res == ResponseType.Cancel)
            {
                md.Destroy();
                return;
            }
            }

            System.IO.StreamWriter output = new System.IO.StreamWriter(filedict["file_fr"]);

            var dictionary = System.IO.File.ReadAllLines(filedict["file"]);

            //pourcent = math.floor(cnt*1000/num_lines)
            int total = dictionary.Length;

            int cnt=1;

            MessageDialog md2 = new MessageDialog (this, 
            DialogFlags.DestroyWithParent, MessageType.Info, 
            ButtonsType.Ok,"");

            try
            {
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
                    md2.Text = String.Format("Fichier Créer avec Succée {0}.",  filedict["filename_fr"]);

            }
            catch(Exception exp)  
            {  
                //Console.Write(exp.Message);
                md2.Text = String.Format("Erreur {0}.",  exp.Message);

            } 

            md2.Run();
            md2.Destroy();

            treeselection_write(true);

        }

        private void btncompile_Clicked(object sender, EventArgs a)
        {

            if (System.IO.File.Exists(filedict["file_old"]))
            {

                MessageDialog md = new MessageDialog (
                    this, 
                    DialogFlags.DestroyWithParent, 
                    MessageType.Warning, 
                    ButtonsType.OkCancel, 
                    String.Format("Le fichier {0} existe\nVoulez-vous l'ecraser.",  filedict["filename_old"]));
                Gtk.ResponseType res = (Gtk.ResponseType)md.Run();
                md.Destroy();

            if (res == ResponseType.Cancel)
            {
                md.Destroy();
                return;
            }
            else 
            {
                System.IO.File.Delete(filedict["file_old"]);
            }
            }
            //copie
            try 
            {
                System.IO.File.Copy(filedict["file"], filedict["file_old"], true);

            }
            catch(Exception exp)
            {
                MessageDialog md1 = new MessageDialog (this, 
                DialogFlags.DestroyWithParent, MessageType.Error, 
                ButtonsType.Ok, String.Format("Erreur Impossible de créer {0}.{1}",  filedict["file_old"], exp.Message));
                md1.Run();
                md1.Destroy();
                return;
            }

            var dictionary = System.IO.File.ReadAllLines(filedict["file"]);
            //string[] texte_dictionary = System.IO.File.ReadAllLines(filedict["file_fr"]);
            //pourcent = math.floor(cnt*1000/num_lines)
            int total = dictionary.Length;

            int cnt=1;

            MessageDialog md2 = new MessageDialog (this, 
            DialogFlags.DestroyWithParent, MessageType.Info, 
            ButtonsType.Ok,"");

            try 
            {

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

                md2.Text = String.Format("Fichier Créer avec Succé et Sauvegardé {0}/{1}.",  filedict["filename"], filedict["filename_old"]);

            }
            catch(Exception exp)  
            {  
                //Console.Write(exp.Message);
                md2.Text = String.Format("Erreur {0}.", exp.Message);
  
            } 

            _progress.Fraction = 0;

            md2.Run();
            md2.Destroy();

            treeselection_write(true);
            
        }

        private void toolforcelanguage_Clicked(object sender, EventArgs a)
        {
            string folder = string.Empty;
            string message = string.Empty;

            Gtk.FileChooserDialog filechooser = new Gtk.FileChooserDialog("Sélectionner le dossier tl", null,
            FileChooserAction.SelectFolder,
            "Cancel",ResponseType.Cancel,
            "Open",ResponseType.Accept);
            //filechooser.Run();
            Label winlabel = new Label("Sélectionner le dossier langue à forcer ex:game/tl/french");
            filechooser.ContentArea.PackStart (winlabel, true, false, 10);
            filechooser.ShowAll();
            Gtk.ResponseType dialog = (Gtk.ResponseType)filechooser.Run();
            
            if (dialog == ResponseType.Accept) 
            {
                folder = filechooser.CurrentFolder;
            }
            else
            {
                filechooser.Destroy();
                return;
            }
            filechooser.Destroy();
            if (folder != string.Empty)
            {
                try 
                {
                    string name = System.IO.Path.GetFileName(folder);
                    string newfile = System.IO.Path.Join(folder, $"_{name}.rpy");
                    string someText = $"define config.language = \"{name}\"\ninit offset = 100";
                    System.IO.StreamWriter output = new System.IO.StreamWriter(newfile);
                    output.Write(someText);
                    output.Close();
                    message = $"Fichier créer avec succée: {newfile}";
                } 
                catch(Exception e)
                {
                    //Console.WriteLine(e.Message);
                    message = e.Message;
                }

                MessageDialog md = new MessageDialog (this, 
                DialogFlags.DestroyWithParent, MessageType.Info, 
                ButtonsType.Ok, message);
                md.Run();
                md.Destroy();
            }

        }

        private async void toollint_Clicked(object sender, EventArgs a)
        {
            string folder = string.Empty;

            Gtk.FileChooserDialog filechooser = new Gtk.FileChooserDialog("Sélectionner le fichier executable du jeux", null,
            FileChooserAction.Open,
            "Cancel",ResponseType.Cancel,
            "Open",ResponseType.Accept);
            //filechooser.Run();
            Label winlabel = new Label("Sélectionner l'executable pour votre system: .sh Linux / .exe Windows");
            filechooser.ContentArea.PackStart (winlabel, true, false, 10);
            filechooser.ShowAll();
            FileFilter filter = new FileFilter();
            filter.AddPattern("*.exe");
            filter.AddPattern("*.sh");
            filechooser.AddFilter(filter);
            Gtk.ResponseType dialog = (Gtk.ResponseType)filechooser.Run();
            
            if (dialog == ResponseType.Accept) 
            {
                folder = filechooser.Filename;
            }
            else
            {
                filechooser.Destroy();
                return;
            }
            filechooser.Destroy();


            if (folder != string.Empty)
            {
                Newlint test = new Newlint(folder, _progress);            
                //test.Lint();
                var result = await test.Lint();

                string lintfile = System.IO.Path.Join(System.IO.Path.GetDirectoryName(folder), "log.txt");
                if (System.IO.File.Exists(lintfile))
                {
                    
                    Dialog md = new Dialog ();
                    md.SetSizeRequest(600, 400);

                    ScrolledWindow scroll = new ScrolledWindow();
                    TextView viewtexte = new TextView();
                    viewtexte.LeftMargin = 10;
                    viewtexte.RightMargin = 10;
                    viewtexte.TopMargin = 10;
                    viewtexte.BottomMargin = 10;

                    System.IO.StreamReader stream = new System.IO.StreamReader(lintfile);
				    viewtexte.Buffer.Text = stream.ReadToEnd();
                    scroll.Add(viewtexte);
                    md.ContentArea.PackStart (scroll, true, true, 10);
                    md.ShowAll();
                    //Gtk.ResponseType res = (Gtk.ResponseType)md.Run();
                    //md.Destroy();
                }
                
            }

        }

        private void toolgeneratelanguage_Clicked(object sender, EventArgs a)
        {
            string folder = string.Empty;
            string newlangue = string.Empty;

            Gtk.FileChooserDialog filechooser = new Gtk.FileChooserDialog("Sélectionner le fichier executable du jeux", null,
            FileChooserAction.Open,
            "Cancel",ResponseType.Cancel,
            "Open",ResponseType.Accept);
            //filechooser.Run();
            Label winlabel = new Label("Sélectionner l'executable du jeux pour votre system: .sh Linux / .exe Windows");
            filechooser.ContentArea.PackStart (winlabel, true, false, 10);
            filechooser.ShowAll();
            FileFilter filter = new FileFilter();
            filter.AddPattern("*.exe");
            filter.AddPattern("*.sh");
            filechooser.AddFilter(filter);
            Gtk.ResponseType dialog = (Gtk.ResponseType)filechooser.Run();
            
            if (dialog == ResponseType.Accept) 
            {
                folder = filechooser.Filename;
            }
            else
            {
                filechooser.Destroy();
                return;
            }
            filechooser.Destroy();


            if (folder != string.Empty)
            {
                MessageDialog md = new MessageDialog (this, 
                DialogFlags.DestroyWithParent, MessageType.Warning, 
                ButtonsType.Ok, "Langue de la traduction ex:french/english");

                Entry entrylangue = new Entry("french");
                md.ContentArea.PackStart (entrylangue, true, false, 10);
                md.ShowAll();
                Gtk.ResponseType res = (Gtk.ResponseType)md.Run();
                if (res == ResponseType.Ok) 
                {
                    newlangue = entrylangue.Text;
                }
                md.Destroy();
                if (newlangue != string.Empty)
                {
                    Newlangue test = new Newlangue(folder, newlangue, _progress);            
                    test.Language();
                    //Console.WriteLine("passssss");
                }
            }

        }

        private void btnrpa_Clicked(object sender, EventArgs a)
        {
            Gtk.FileChooserDialog filechooser = new Gtk.FileChooserDialog("Sélectionner le dossier racine du jeux", this,
            FileChooserAction.SelectFolder,
            "Cancel",ResponseType.Cancel,
            "Open",ResponseType.Accept);
            //filechooser.Run();
            Label winlabel = new Label("Sélectionner le dossier racine du jeux");
            filechooser.ContentArea.PackStart (winlabel, true, false, 10);
            filechooser.ShowAll();
            Gtk.ResponseType dialog = (Gtk.ResponseType)filechooser.Run();
            
            string folder = string.Empty;

            if (dialog == ResponseType.Accept) 
            {
                folder = filechooser.CurrentFolder;
            }
            else
            {
                filechooser.Destroy();
                return;
            }
            filechooser.Destroy();


            if (folder != string.Empty)
            {
                MessageDialog md = new MessageDialog (this, 
                DialogFlags.DestroyWithParent, MessageType.Warning, 
                ButtonsType.Ok, "");
                //Gtk.ResponseType res = (Gtk.ResponseType)md.Run();
                //md.Destroy();

                _progress.Fraction = 0;
                UnRen2 test = new UnRen2(folder, md, _progress);
                //Thread t = new Thread(new ThreadStart(test.UnRen));
                //test.UnRen();            
                test.UnRen();

                //Task task = test.UnRen();
                //await Task.Run(() => test.UnRen()).ContinueWith(t => taskend = false);                        
                
                //listerror = test.list;
            }

        }        

        private void btnopenfolder_Clicked(object sender, EventArgs a)
        {

            Gtk.FileChooserDialog filechooser = new Gtk.FileChooserDialog("Sélectionner le dossier langue", this,
            FileChooserAction.SelectFolder,
            "Cancel",ResponseType.Cancel,
            "Open",ResponseType.Accept);
            Label winlabel = new Label("Sélectionner le dossier langue ex:game/tl/french");
            filechooser.ContentArea.PackStart (winlabel, true, false, 10);
            filechooser.ShowAll();

            if (filechooser.Run() == (int)ResponseType.Accept) 
            {
                //efface listore
                fileslist.Clear();
                /////System.IO.Directory.CreateDirectory(filechooser.CurrentFolder);
                string[] files = System.IO.Directory.GetFiles(filechooser.CurrentFolder, "*.rpy", System.IO.SearchOption.AllDirectories);
                Array.Sort(files);

                _txtfolder.Text = filechooser.CurrentFolder;
                _txtnbfolder.Text = string.Format($"Nbs de fichiers: {files.Length.ToString()}");


                foreach (string file in files)
                {
                    string fileName = System.IO.Path.GetFileName(file);
                    string targetPath = System.IO.Path.Combine(filechooser.CurrentFolder, fileName);
                    //long length = new System.IO.FileInfo(file).Length;

                    string lenght = Util.GetFileSize(file);

                    listfile(file, TreeIter.Zero);
                    
                }

            }


            filechooser.Destroy();
            //Console.WriteLine(this._btnopenfolder[]file);
                    
        }

        private void listfile(string file, TreeIter iter)
        {
            filedict = Util.getDictFile(file);
            string lenght = Util.GetFileSize(file);
            bool check_fr = false;
            bool check_old = false;
            string file_fr = string.Empty;
            string file_old = string.Empty;
            string color1 = null;
            string color2 = null;

            // if (System.IO.File.Exists(filedict["file"]))
            //     {

            //     }
            if (System.IO.File.Exists(filedict["file_fr"]))
                {
                    check_fr = true;
                    file_fr = filedict["filename_fr"];
                    color1 = "LightGreen";
                }

            if (System.IO.File.Exists(filedict["file_old"]))
                {
                    check_old = true;
                    file_old = filedict["filename_old"];
                    color2 = "LightGreen";
                    //LightCoral
                }
            if (iter.Equals(TreeIter.Zero))
            {
            fileslist.AppendValues(new object[] {lenght, filedict["filename"], check_fr, file_fr, color1, check_old, file_old, color2, file });
            }
            else
            {
            fileslist.SetValues(iter, new object[] {lenght, filedict["filename"], check_fr, file_fr, color1, check_old, file_old, color2, file });
            }
            //fileslist.SetValues(iter, )
            _treeviewselect.Model = fileslist;
            fileslist.SetSortColumnId(1, Gtk.SortType.Ascending);

            
        }

        private void homeselection_RowActivated(object sender, RowActivatedArgs args)
        {
            if(msg_sure()){
                return;
            }
            
            //select
            var model = _treeviewselect.Model;
            Gtk.TreeIter selected;
            _treeviewselect.Selection.GetSelected(out selected);
            var value2 = model.GetValue(selected, 8);

            filedict = Util.getDictFile(value2.ToString());

            treeselection_write(false);       
        }


        public bool msg_sure() 
        {
            if (editchange) {
                MessageDialog md = new MessageDialog (this, 
                DialogFlags.DestroyWithParent, MessageType.Warning, 
                ButtonsType.OkCancel, "ATTENTION : Modification non Enregistrée \nSi vous continuez les modifications ne seront pas pris en compte. (Continuer ?)");
                //md.Run();
                Gtk.ResponseType res = (Gtk.ResponseType)md.Run();
                md.Destroy();

                if (res == ResponseType.Ok)
                {
                    _infotexte.Visible = false;
                    editchange = false;
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return false;
        }  

    	private void OnWidgetDrawn (object o, DrawnArgs args) {


        TextView textView = o as TextView;      
        Cairo.Context cr = args.Cr;

        //var parent = textView.Parent as ScrolledWindow;

      
    	/* Draw text */
		Gdk.CairoHelper.SetSourceRgba(cr, StyleContext.GetColor (StateFlags.Link));
        Pango.Layout textLayout = new Pango.Layout(textView.PangoContext);
        //textLayout.Alignment = Pango.Alignment.Right;
        textLayout.FontDescription = Pango.FontDescription.FromString (_btnfont.FontName);
    	int infoCount = textView.Buffer.LineCount;
        //taille fenetre
        int minVisibleY = textView.VisibleRect.Top;
   		int maxVisibleY = textView.VisibleRect.Bottom;

        //visible start et end iters
   		TextIter startIter, endIter;
   		int lineTop;
    	textView.GetLineAtY(out startIter, minVisibleY, out lineTop);
    	textView.GetLineAtY(out endIter, maxVisibleY, out lineTop);
        int startLine = startIter.Line;
        int endLine = endIter.Line;

        List<string> listnum = new List<string>();


    	for (int i = startLine ; i <= endLine ; i++) {
            listnum.Add(i.ToString());
		}

        textLayout.SetText(String.Join("\n", listnum));
        cr.MoveTo(5, 10);
        int textLayoutWidth, textLayoutHeight;
        textLayout.GetPixelSize(out textLayoutWidth, out textLayoutHeight);
        

        Pango.CairoHelper.ShowLayout(cr, textLayout); 

        /* text view's left margin */
        textView.SetBorderWindowSize(TextWindowType.Left, textLayoutWidth + 10);

		//cr.GetTarget().Dispose();
		//cr.Dispose();
        cr.Stroke();

    }
        private void treeselection_write(bool update)
        {

            //new Gdk.Pixbuf("assets/report.png")
            if (System.IO.File.Exists(filedict["file"]))
                {                    
                    System.IO.StreamReader stream = new System.IO.StreamReader(filedict["file"]);
				    _textviewsource.Buffer.Text = stream.ReadToEnd();
                    _textviewsource.Editable = true;
                    _textviewsource.Sensitive = true;

                    //_textviewsource.ModifyFont(Pango.FontDescription.FromString ("Comic Sans MS 40"));
                    _textviewsource.ModifyFont(Pango.FontDescription.FromString (_btnfont.FontName));
                
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

                    System.IO.StreamReader stream = new System.IO.StreamReader(filedict["file_fr"]);
				    _textviewtexte.Buffer.Text = stream.ReadToEnd();
                    _textviewtexte.Editable = true;
                    _textviewtexte.Sensitive = true;
                    _textviewtexte.ModifyFont(Pango.FontDescription.FromString (_btnfont.FontName));


                }
            else 
                {

                    _textviewtexte.Buffer.Text = "";
                    _textviewtexte.Editable = false;
                    _textviewtexte.Sensitive = false;
                }
            if (System.IO.File.Exists(filedict["file_old"]))
                {

                     System.IO.StreamReader stream = new System.IO.StreamReader(filedict["file_old"]);
				    _textviewsauv.Buffer.Text = stream.ReadToEnd();
                    _textviewsauv.Editable = true;
                    _textviewsauv.Sensitive = true;
                    _textviewsauv.ModifyFont(Pango.FontDescription.FromString (_btnfont.FontName));

                        
                }
            else
                {
                _textviewsauv.Buffer.Text = "";
                _textviewsauv.Editable = false;
                _textviewsauv.Sensitive = false;
                }

            if (update)
            {
            //select
            var model = _treeviewselect.Model;
            Gtk.TreeIter selected;
            _treeviewselect.Selection.GetSelected(out selected);
            var value2 = model.GetValue(selected, 8);
            listfile(value2.ToString(), selected);
            }
                    
        }

        
    }
}
