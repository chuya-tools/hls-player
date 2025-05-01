using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.Web.WebView2.Core; // 必要な名前空間を追加
using Microsoft.Web.WebView2.WinForms;
using Microsoft.Web.WebView2.Wpf;
using WebView2 = Microsoft.Web.WebView2.Wpf.WebView2;  // 必要な名前空間を追加

namespace WpfApp1
{
    public class WebView2Controller
    {
        private readonly WebView2 webView2 = new WebView2(); // WebView2 のインスタンスを作成
        public WebView2Controller()
        {
            Task.Run(async () =>
            {
                var op = new CoreWebView2EnvironmentOptions("--disable-web-security");
                var env = await CoreWebView2Environment.CreateAsync(null, null, op);
                await webView2.EnsureCoreWebView2Async(env);
            });
            this.webView2.CoreWebView2InitializationCompleted += this.CoreWebView2Initialization;
        }

        private void CoreWebView2Initialization(object sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            if (e.IsSuccess)
            {
                Debug.WriteLine("初期化成功");

                // CORS違反を仮想ホスト名マッピングで回避する
                Assembly assembly = Assembly.GetExecutingAssembly();
                // アプリケーション内部のフォルダ
                webView2.CoreWebView2.SetVirtualHostNameToFolderMapping("app-folder", System.IO.Path.GetDirectoryName(assembly.Location), CoreWebView2HostResourceAccessKind.Allow);
                // Dドライブ
                webView2.CoreWebView2.SetVirtualHostNameToFolderMapping("storage", "D:\\videos", CoreWebView2HostResourceAccessKind.Allow);

                // JavaScriptからC#メソッドを実行できるようにする
                webView2.CoreWebView2.AddHostObjectToScript("class", new JsFuncs("https://storage/bunny/video.m3u8"));
            }
            else
            {
                Debug.Fail("CoreWebView2の初期化に失敗しました。");
            }
            Debug.WriteLine("CoreWebView2InitializationCompleted");
        }

        public async void Navigate(string uri)
        {
            if (this.webView2.CoreWebView2 is null)
                await this.webView2.EnsureCoreWebView2Async();
            this.webView2.CoreWebView2.Navigate(uri);

            Debug.WriteLine("Navigate");
        }

        public WebView2 GetWebView2()
        {
            return this.webView2;
        }
    }

    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ComVisible(true)]
    public class JsFuncs
    {
        public string VideoSourcePath = string.Empty;

        public JsFuncs(string videoSourcePath)
        {
            VideoSourcePath = videoSourcePath;
        }
        public string GetVideoSourcePath()
        {
            return "https://storage/bunny/video.m3u8";
        }
    }
}
