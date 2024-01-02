namespace example
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var dm = new DmService();

            int hwnd;//句柄
            hwnd = dm.FindWindow("lpClassName", "lpWindowName"); //找窗口
            dm.BindWindow(hwnd); //輸入句柄並綁定

            //注意，所有圖片都得是bmp，故找圖不需要再寫.bmp

            //如果找到圖片，其他參數可以調整
            if (dm.FindPicB("圖片名稱"))
            {
                //找到了

                //此為多載方法
                dm.MCS(); //滑鼠移動、點擊、休息兩秒

            }
            //沒找到往下執行


            //隔一秒找一次圖片
            if (dm.FindPicR("圖片名稱"))
            {
                //時間內沒找到圖片
            }
            //找到了往下執行


            //參數使用方法
            // traversal 設為true時候，會一次尋找"圖片名稱"、"圖片名稱1"、"圖片名稱2"等等
            // sim 設為0.9時候，會尋找相似度90%以上的圖片
            // click 設為true時候，會點擊找到的圖片
        }
    }
}
