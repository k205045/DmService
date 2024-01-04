namespace example
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DmService? dm = null;
            try
            {
                //初始化dm時，可直接輸入視窗大小、圖片路徑、字典路徑、是否顯示錯誤訊息、
                dm = new DmService(1920,1080);
            }
            catch (Exception e)
            {
                Console.WriteLine("dm初始化失敗，代表沒有註冊大漠");
                Console.ReadKey();
            }
            

            int hwnd = dm.FindWindow("lpClassName", "lpWindowName"); //找窗口句炳
            dm.BindWindow(hwnd); //輸入句柄並綁定

            //注意，所有圖片都得是bmp，故找圖不需要再寫.bmp

            //一般找圖
            if (dm.FindPicB("圖片名稱"))
            {
                //找到了
                Console.WriteLine("找到 圖片名稱");

                // dm.MCS()為多載方法
                dm.MCS(); // 滑鼠移動至圖片、點擊、休息2秒
                dm.MCS(5); // 滑鼠移動至圖片、點擊、休息5秒

                dm.MCS(100, 100); // 滑鼠移動至100,100、點擊、休息2秒
                dm.MCS(100, 100, 5); // 滑鼠移動至100,100、點擊、休息5秒
            }
            //沒找到往下執行
            Console.WriteLine("沒找到 圖片名稱");


            while (true)
            {
                //隔一秒找一次圖片
                if (dm.FindPicR("圖片1"))
                {
                    //時間內沒找到圖片
                    Console.WriteLine("沒找到 圖片1");

                    //重來
                    continue;
                }
                //找到了往下執行
                Console.WriteLine("找到圖片1，執行下一步");

                //通常執行點擊剛剛找到的圖片
                dm.MCS();

                //隔一秒找一次圖片
                if (dm.FindPicR("圖片2"))
                {
                    //時間內沒找到圖片
                    Console.WriteLine("沒找到 圖片2");

                    //重來
                    continue;
                }
                //找到了往下執行
                Console.WriteLine("找到圖片2，執行下一步");

                //通常執行點擊剛剛找到的圖片
                dm.MCS();

                break;
            }
        }

        /// <summary>
        /// 參數使用方法
        /// </summary>
        public void Func()
        {
            //找圖說明 FindPic可限定範圍 例如 FindPicB(100, 100, 200, 200, "圖片名稱")

            // 參數使用方法
            // 找圖方法的 traversal 設為true時候，會一次尋找"圖片名稱"、"圖片名稱1"、"圖片名稱2"等等
            // 找圖要找多張bmp 以|分開 例如 "圖片名稱|圖片名稱1|圖片名稱2"
            // 因此「traversal」與「bmp的|」使用方法為互斥，只能使用一個

            // sim 設為0.9時候，會尋找相似度90%以上的圖片
            // click 設為true時候，會點擊找到的圖片


            // FindPicR 的 time為尋找時間，單位為秒，預設10秒
        }
    }
}
