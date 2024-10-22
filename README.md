# 大漠插件類別庫

## 簡介

這是一份關於將大漠插件改寫成更易於使用的類別庫的文件。大漠插件是一個用於Windows平台的鍵盤、滑鼠以及圖色控制的插件。本類別庫的目的是為了便於C#使用者使用這個插件，透過將其核心功能封裝成更加友好的接口。

## 功能特點

- **易於使用**：對大漠插件的主要功能進行了封裝，使其更易於在不同的應用程式中集成和使用。

## 大漠安裝指南

1. 解壓縮專案中的 `dm.zip` 檔案。
2. 以系統管理員身分執行解壓縮後的 `.bat` 檔案。

## 使用方法

若要在您的.NET專案中使用`DmService` DLL，大漠3.1233須將專案設定成x86，然後依照下列步驟進行：

**方法一：加入參考**

1. **將`DmService`專案加入至您的解決方案**：
   - 在Visual Studio中，右鍵點擊解決方案名稱，選擇「新增」->「現有專案…」。
   - 瀏覽至`DmService`專案檔案（通常是`.csproj`或`.vbproj`檔案），然後點擊「開啟」。

2. **為您的專案新增對`DmService`的參考**：
   - 在解決方案資源管理器中，右鍵點擊您想使用`DmService`的專案名稱。
   - 選擇「新增」->「參考…」，在彈出的對話框中選擇「專案」分頁。
   - 於列表中勾選`DmService`專案，然後點擊「確定」。

3. **在您的專案中使用`DmService`**：
   - 在您的程式碼檔案中，新增對`DmService`命名空間的引用：
     ```csharp
     using DmService; // 請替換為實際的命名空間。
     ```
   - 您可以透過繼承`DmService`來擴展功能：
     ```csharp
     public class MyDmService : DmService {
       // 新增或覆寫功能。
     }
     ```
   - 或者，您可以建立一個`DmService`的實例：
     ```csharp
     var dmService = new DmService();
     // 使用dmService實例。
     ```

請依照您實際的命名空間和類別名稱調整上述代碼範例。


**方法二：使用DLL**
   - 將本專案建置成DLL檔案，然後在你的應用程式中直接引用這個DLL。

# 初始化
```csharp
DmService? dm = null;
try
{
    //初始化dm時，需直接輸入視窗大小、圖片路徑、字典路徑、是否顯示錯誤訊息、
    dm = new DmService(1920,1080);
}
catch (Exception e)
{
    //通常是被防毒刪除或沒有註冊
    Console.WriteLine("dm初始化失敗");
    Console.ReadKey();
}


int hwnd = dm.FindWindow("lpClassName", "lpWindowName"); //找窗口句炳
dm.BindWindow(hwnd); //輸入句柄並綁定
```
# 找圖
- 所有圖片副檔名都得是bmp，故找圖不需要再寫.bmp
## 一般找圖
```csharp
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
```
## 持續找圖
```csharp
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
```

### 如何解決找不到Dm插件的問題

如果在使用過程中你的程式無法找到Dm插件，請按照以下步驟操作：

1. 在你的解決方案中，選擇「專案」>「加入」>「COM參考」。
2. 在出現的對話框中搜索「dm」。
3. 找到對應的項目後打勾，然後點擊「確定」。

這樣應該能夠解決找不到Dm插件的問題。

# 喜歡的話幫我按個星星，感謝
