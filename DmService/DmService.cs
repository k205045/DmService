using Dm;
using System.Runtime.InteropServices;


public class DmService
{
    public int re = -1;
    public object intX = -1;
    public object intY = -1;
    public int X => (int)intX;
    public int Y => (int)intY;
    public dmsoft Dm;
    private int _width;
    private int _height;
    public DmService(int Width = 960, int Height = 540, bool showError = false, string? path = null, string? dict = null)
    {

        Dm = new dmsoft();

        SetPath(path);
        SetDict(dict);
        if (showError == false)
        {
            Dm.SetShowErrorMsg(0);
        }
        _height = Height;
        _width = Width;
    }

    /// <summary>
    /// 綁定視窗
    /// </summary>
    /// <param name="hwnd"></param>
    /// <returns></returns>
    public bool BindWindow(int hwnd)
    {
        return Dm.BindWindow(hwnd, "gdi", "windows", "windows", 0) == 1;
    }
    /// <summary>
    /// 設定資源路徑
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>

    public int FindWindow(string lpClassName, string lpWindowName)
    {
        return WindowHelper.FindWindow(lpClassName,lpWindowName);
    }
    public int FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpClassName, string lpWindowName)
    {
        return WindowHelper.FindWindowEx(hwndParent, hwndChildAfter, lpClassName,lpWindowName);
    }
    public bool SetPath(string? path)
    {
        if (string.IsNullOrEmpty(path))
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            path = baseDirectory + "\\Resources";
        }
        Console.WriteLine("SetPath: " + path);
        return Dm.SetPath(path) == 1;
    }
    /// <summary>
    /// 設定字典
    /// </summary>
    /// <param name="dict"></param>
    public void SetDict(string? dict)
    {
        if (string.IsNullOrEmpty(dict))
        {
            dict = "dm_soft";
        }

        Dm.SetDict(0, dict + ".txt");
    }
    #region 其他方法
    /// <summary>
    /// 是否卡死
    /// </summary>
    /// <param name="sec"></param>
    /// <returns></returns>
    public bool IsDisplayDead(int sec = 5)
    {
        return Dm.IsDisplayDead(0, 0, _width, _height, sec) == 1;
    }
    /// <summary>
    /// 截圖，如果大於limit則不截圖
    /// </summary>
    /// <param name="bmp"></param>
    /// <param name="limit"></param>
    public void Capture(string bmp, int limit = 100)
    {
        int index = 0;
        var currentFile = bmp + ".bmp";
        while (File.Exists(Path.Combine(basePath, currentFile)))
        {
            index++;
            currentFile = $"{bmp}{index}.bmp";
        }
        if (index > limit)
        {
            Console.WriteLine($"圖片超過{limit}張");
            return;
        }
        Dm.Capture(0, 0, _width, _height, currentFile);
    }

    /// <summary>
    /// 取色
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public string GetColor(int x, int y)
    {
        return Dm.GetColor(x, y);
    }
    #endregion

    #region 圖片文字
    public bool FindStrB(string str, string colors, double sim = 0.7)
    {
        return Dm.FindStr(0, 0, _width, _height, str, colors, sim, out intX, out intY) >= 0;
    }
    public bool FindStrB(int x1, int y1, int x2, int y2, string str, string colors, double sim = 0.7)
    {
        return Dm.FindStr(x1, y1, x2, y2, str, colors, sim, out intX, out intY) >= 0;
    }
    public int FindPic(string bmps, double sim = 0.7, bool traversal = false)
    {
        var bmp = ProcessBmpString(bmps, traversal);

        return Dm.FindPic(0, 0, _width, _height, bmp, "000000", sim, 0, out intX, out intY);
    }
    public bool FindPicB(string bmps, bool click = false, double sim = 0.7, bool traversal = false)
    {
        var bmp = ProcessBmpString(bmps, traversal);
        var re = Dm.FindPic(0, 0, _width, _height, bmp, "000000", sim, 0, out intX, out intY) >= 0 ? true : false;
        if (re && click)
        {
            MCS();
        }
        return re;
    }
    public bool FindPicB(int x1, int y1, int x2, int y2, string bmps, bool click = false, double sim = 0.7, bool traversal = false)
    {
        var bmp = ProcessBmpString(bmps, traversal);
        var re = Dm.FindPic(x1, y1, x2, y2, bmp, "000000", sim, 0, out intX, out intY) >= 0 ? true : false;
        if (re && click)
        {
            MCS();
        }
        return re;
    }

    public int FindPic(int x1, int y1, int x2, int y2, string bmps, double sim = 0.7, bool traversal = false)
    {
        var bmp = ProcessBmpString(bmps, traversal);
        return Dm.FindPic(x1, y1, x2, y2, bmp, "000000", sim, 0, out intX, out intY);
    }
    public bool FindPicR(string bmps, bool click = false, int time = 10, double sim = 0.7, bool traversal = false)
    {
        var bmp = ProcessBmpString(bmps, traversal);

        var tmptime = 0;
        while (true)
        {
            if (Dm.FindPic(0, 0, _width, _height, bmp, "000000", sim, 0, out intX, out intY) >= 0)
            {
                if (click)
                {
                    MCS();
                }
                return false;
            }
            tmptime++;
            if (tmptime > time)
            {
                return true;
            }

            Thread.Sleep(1000);

        }
    }

    public string ProcessBmpString(string? bmps, bool traversal)
    {
        if (string.IsNullOrEmpty(bmps))
        {
            Console.WriteLine("IsNullOrEmpty");
            throw new Exception(bmps + " IsNullOrEmpty");
        }

        if (traversal && bmps.Contains("|"))
        {
            Console.WriteLine(bmps + " 圖片判斷錯誤: 多張遍歷");
            traversal = false;
        }

        if (bmps.Contains(".bmp"))
        {
            bmps.Replace(".bmp", "");
            Console.WriteLine(bmps + "包含bmp");
        }

        string? bmp;
        if (traversal)
        {
            bmp = Traversal(bmps);
        }
        else if (bmps.Contains("|"))
        {
            var tmp = bmps.Split("|");
            for (int i = 0; i < tmp.Length; i++)
            {
                tmp[i] += ".bmp";
            }

            bmp = string.Join('|', tmp);
        }
        else
        {
            bmp = (bmps + ".bmp");
        }
        return bmp;
    }
    private static string basePath = AppDomain.CurrentDomain.BaseDirectory + "Resources";
    private string Traversal(string baseFilename)
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

    #endregion

    #region 滑鼠
    /// <summary>
    /// 長按滑鼠
    /// </summary>
    /// <param name="intX"></param>
    /// <param name="intY"></param>
    /// <param name="sec"></param>
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
    public void MCSEx(int intXEx, int intYEx, int sec = 2)
    {
        Dm.MoveTo(X + intXEx + GetRandomNumberMove(), Y + intYEx + GetRandomNumberMove());
        Dm.LeftClick();
        Thread.Sleep(sec * 1000);
    }
    public void MCS()
    {
        Dm.MoveTo(X + GetRandomNumberMove(), Y + GetRandomNumberMove());
        Dm.LeftClick();
        Thread.Sleep(2000);
    }
    public void MCS(int sec)
    {
        Dm.MoveTo(X + GetRandomNumberMove(), Y + GetRandomNumberMove());
        Dm.LeftClick();
        Thread.Sleep(sec * 1000);
    }
    public void MCS(double sec)
    {
        Dm.MoveTo(X + GetRandomNumberMove(), Y + GetRandomNumberMove());
        Dm.LeftClick();
        Thread.Sleep((int)(sec * 1000));
    }

    private Random random = new Random();
    private int GetRandomNumberMove()
    {
        return random.Next(0, 6); // 返回0到5的隨機整數
    }
    #endregion

    /// <summary>
    /// Win32Api，原生方法比Dm還快
    /// </summary>
    public static class WindowHelper
    {
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
        
    }

}
