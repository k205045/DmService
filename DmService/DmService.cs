using Dm;
using System.Runtime.InteropServices;


public class DmService
{
    public DmService(bool showError = false)
    {
        Dm = new dmsoft();
        SetPath();
        SetDict();
        if (showError == false)
        {
            Dm.SetShowErrorMsg(0);
        }

    }
    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

    public const uint WM_CLOSE = 0x0010;

    [DllImport("User32.dll", EntryPoint = "FindWindow")]
    public extern static int FindWindow(string lpClassName, string lpWindowName);

    [DllImport("User32.dll", EntryPoint = "FindWindowEx")]
    public static extern int FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpClassName, string lpWindowName);
    [DllImport("shell32.dll")]
    public static extern IntPtr ShellExecute(IntPtr hwnd,  //窗口句柄
       string lpOperation,   //指定要進行的操作
       string lpFile,        //要執行的程序、要瀏覽的文件夾或者網址
       string lpParameters,  //若lpFile參數是一個可執行程序，則此參數指定命令行參數
       string lpDirectory,   //指定默認目錄
       int nShowCmd          //若lpFile參數是一個可執行程序，則此參數指定程序窗口的初始顯示方式(參考如下枚舉)
     );
    //[DllImport("ole32.dll")]
    //private static extern int CoInitialize(IntPtr pvReserved);

    public int re;
    //public int tmpTime = 0;
    public object intX;
    public object intY;
    public int X => (int)intX;
    public int Y => (int)intY;

    //public string colection;
    public dmsoft Dm;

    //private bool waitIcon = true;

    public void Register()
    {

        // 使用 Type.GetTypeFromProgID 方法獲取 COM 物件的類型
        //Type dmType = Type.GetTypeFromProgID("dm.dmsoft");

        // 使用 dynamic 關鍵字創建 COM 物件的實例
        //dynamic dm = Activator.CreateInstance(dmType);

        //dm = new Dm.dmsoft();
        //int result = CoInitialize(IntPtr.Zero);
        //dm = new Dm.dmsoft();
        //re = Dm.SetPath(path);
        //dm.SetDict(0, "dm_soft部隊.txt");
    }
    public bool IsDisplayDead(int sec = 5)
    {
        return Dm.IsDisplayDead(0, 0, 960, 540, sec) == 1 ? true : false;
    }

    public void Close(string windowName)
    {
        // 用窗口的标题来找到窗口句柄（HWND）
        IntPtr hWnd = FindWindow("LDPlayerMainFrame", windowName);

        if (hWnd != IntPtr.Zero)
        {
            //Console.WriteLine("Window found, closing it.");

            // 发送一个 WM_CLOSE 消息来关闭窗口
            SendMessage(hWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
        }
    }

    public void SetPath(string path)
    {
        //var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        Dm.SetPath(path);
    }
    public void SetPath()
    {
        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        var path = baseDirectory + "\\Resources";
        Console.WriteLine("path " + path);
        Dm.SetPath(path);
    }
    public void SetDict(string dict = "dm_soft")
    {
        Dm.SetDict(0, dict + ".txt");
    }



    public string GetColor(int x, int y)
    {
        return Dm.GetColor(x, y);
        //dm.ger
    }

    public bool FindStrB(string str, string colors, double sim = 0.7)
    {
        return Dm.FindStr(0, 0, 960, 540, str, colors, sim, out intX, out intY) >= 0 ? true : false;
    }
    public bool FindStrB(int x1, int y1, int x2, int y2, string str, string colors, double sim = 0.7)
    {
        return Dm.FindStr(x1, y1, x2, y2, str, colors, sim, out intX, out intY) >= 0 ? true : false;
    }
    public int FindPic(string mybmps, double sim = 0.7, bool traversal = false)
    {
        var bmp = ProcessBmpString(mybmps, traversal);

        return Dm.FindPic(0, 0, 960, 540, bmp, "000000", sim, 0, out intX, out intY);
    }
    public bool FindPicB(string mybmps, bool click = false, double sim = 0.7, bool traversal = false)
    {
        var bmp = ProcessBmpString(mybmps, traversal);
        var re = Dm.FindPic(0, 0, 960, 540, bmp, "000000", sim, 0, out intX, out intY) >= 0 ? true : false;
        if (re && click)
        {
            MCS();
        }
        return re;
    }
    public bool FindPicB(int x1, int y1, int x2, int y2, string mybmps, bool click = false, double sim = 0.7, bool traversal = false)
    {
        var bmp = ProcessBmpString(mybmps, traversal);
        var re = Dm.FindPic(x1, y1, x2, y2, bmp, "000000", sim, 0, out intX, out intY) >= 0 ? true : false;
        if (re && click)
        {
            MCS();
        }
        return re;
    }

    public int FindPic(int x1, int y1, int x2, int y2, string mybmps, double sim = 0.7, bool traversal = false)
    {
        var bmp = ProcessBmpString(mybmps, traversal);
        return Dm.FindPic(x1, y1, x2, y2, bmp, "000000", sim, 0, out intX, out intY);
    }
    public bool FindPicR(string mybmps, bool click = false, int time = 10, double sim = 0.7, bool traversal = false)
    {
        var bmp = ProcessBmpString(mybmps, traversal);

        var tmptime = 0;
        while (true)
        {
            if (Dm.FindPic(0, 0, 960, 540, bmp, "000000", sim, 0, out intX, out intY) >= 0)
            {
                if (click)
                {
                    MCS();
                }
                return false;
            }

            Thread.Sleep(1000);
            tmptime++;
            if (tmptime > time)
            {
                return true;
            }
        }
    }

    public string ProcessBmpString(string? mybmps, bool traversal)
    {
        if (traversal && mybmps.Contains("|"))
        {
            throw new Exception(mybmps + " 圖片判斷錯誤: 多張遍歷");
        }
        if (string.IsNullOrEmpty(mybmps))
        {
            throw new Exception(mybmps + " IsNullOrEmpty");
        }

        if (mybmps.Contains(".bmp"))
        {
            mybmps.Replace(".bmp", "");
            Console.WriteLine(mybmps + "包含bmp");
        }

        string? bmp;
        if (traversal)
        {
            bmp = Traversal(mybmps);
        }
        else if (mybmps.Contains("|"))
        {
            var tmp = mybmps.Split("|");
            for (int i = 0; i < tmp.Length; i++)
            {
                tmp[i] += ".bmp";
            }

            bmp = string.Join('|', tmp);
        }
        else
        {
            bmp = (mybmps + ".bmp");
        }
        return bmp;
    }




    private Random random = new Random();

    private int GetRandomNumberMove()
    {
        return random.Next(0, 6); // 返回0到5的隨機整數
    }
    public void MDSU(int intX, int intY, int sec = 2)
    {
        Dm.MoveTo(intX + GetRandomNumberMove(), intY + GetRandomNumberMove());
        Dm.LeftDown();
        Thread.Sleep(sec * 1000);
        Dm.LeftUp();
    }
    public void MCS(int intX, int intY)
    {
        Dm.MoveTo(intX + GetRandomNumberMove(), intY + GetRandomNumberMove());
        Dm.LeftClick();
        Thread.Sleep(2000);
    }

    public void MCS(int intX, int intY, int sec = 2)
    {
        Dm.MoveTo(intX + GetRandomNumberMove(), intY + GetRandomNumberMove());
        Dm.LeftClick();
        Thread.Sleep(sec * 1000);
    }
    public void MCS(int intX, int intY, double sec = 2.0)
    {
        Dm.MoveTo(intX + GetRandomNumberMove(), intY + GetRandomNumberMove());
        Dm.LeftClick();
        Thread.Sleep((int)(sec * 1000));
    }
    public void MCSEx(int intXex, int intYex, int sec = 2)
    {
        //Thread.Sleep(sec2 * 1000);
        Dm.MoveTo(X + intXex + GetRandomNumberMove(), Y + intYex + GetRandomNumberMove());
        Dm.LeftClick();
        //Task.Delay(sec * 1000).Wait();
        Thread.Sleep(sec * 1000);
    }
    public void MCS()
    {
        Dm.MoveTo(X + GetRandomNumberMove(), Y + GetRandomNumberMove());
        Dm.LeftClick();
        //Task.Delay(2000).Wait();
        Thread.Sleep(2000);
    }
    public void MCS(int sec)
    {
        Dm.MoveTo(X + GetRandomNumberMove(), Y + GetRandomNumberMove());
        Dm.LeftClick();
        //Task.Delay(2000).Wait();
        Thread.Sleep(sec * 1000);
    }
    public void MCS(double sec)
    {
        Dm.MoveTo(X + GetRandomNumberMove(), Y + GetRandomNumberMove());
        Dm.LeftClick();
        //Task.Delay(2000).Wait();
        Thread.Sleep((int)(sec * 1000));
    }
    private static string basePath = AppDomain.CurrentDomain.BaseDirectory + "Resources";
    private static string Traversal(string baseFilename)
    {
        List<string> filenames = new List<string>();
        //string basePath = AppDomain.CurrentDomain.BaseDirectory;

        int index = 0;
        string currentFile = baseFilename + ".bmp";
        while (File.Exists(Path.Combine(basePath, currentFile)))
        {
            filenames.Add(currentFile);
            index++;
            currentFile = $"{baseFilename}{index}.bmp";
        }

        return string.Join("|", filenames);
    }



    //static void Shuffle<T>(IList<T> list)
    //{
    //    Random random = new Random();

    //    for (int i = list.Count - 1; i > 0; i--)
    //    {
    //        int j = random.Next(i + 1);
    //        T temp = list[i];
    //        list[i] = list[j];
    //        list[j] = temp;
    //    }
    //}


    protected string RandomChoice(string s)
    {
        var random = new Random();
        int index = random.Next(s.Length);
        return s[index].ToString();
    }

}
