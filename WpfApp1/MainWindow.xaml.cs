using Microsoft.Web.WebView2.Core;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly WebView2Controller Controller = new WebView2Controller();

        public MainWindow()
        {
            InitializeComponent();

            Task.Run(async () =>
            {
                var op = new CoreWebView2EnvironmentOptions("--disable-web-security");
                var env = await CoreWebView2Environment.CreateAsync(null, null, op);
                await Controller.GetWebView2().EnsureCoreWebView2Async(env);
            }).ContinueWith(_ =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Assembly assembly = Assembly.GetExecutingAssembly();
                    this.WebViewPanel.Children.Add(this.Controller.GetWebView2());
                    var url = string.Concat("file:///", System.IO.Path.Combine(System.IO.Path.GetDirectoryName(assembly.Location), "Views", "index.html"));
                    this.Controller.Navigate(url);
                });
            });
        }
    }
}
