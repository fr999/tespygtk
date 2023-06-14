using System;
using Gtk;

namespace tespygtk
{
    class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Application.Init();
            ApplyTheme();


            var app = new Application("org.tespygtk", GLib.ApplicationFlags.None);
            app.Register(GLib.Cancellable.Current);

            var win = new MainWindow();
            app.AddWindow(win);

            win.Show();
            Application.Run();
        }

        public static void ApplyTheme() {
            // Load the Theme
            Gtk.CssProvider css_provider = new Gtk.CssProvider();
            //css_provider.LoadFromPath("themes/DeLorean-3.14/gtk-3.0/gtk.css")
            css_provider.LoadFromPath("custom.css");
            Gtk.StyleContext.AddProviderForScreen(Gdk.Screen.Default, css_provider, 800);
        }



    }
}
