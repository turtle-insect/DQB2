![DL Count](https://img.shields.io/github/downloads/turtle-insect/DQB2/total.svg)

# 概要
Switch & STEAM ドラゴンクエストビルダーズ2のセーブデータ編集Tool

# Portal
http://www.dragonquest.jp/builders2/

# ソフト
■Switch  
https://ec.nintendo.com/JP/ja/titles/70010000002603  
■STEAM  
https://store.steampowered.com/app/1072420/  
■XBox、PC  
https://www.microsoft.com/ja-jp/p/b1e651fe-2ee9-4bd1-85ce-49fa3f9ca6bd/9pfd00czj35v  
■Play Station  
https://www.jp.playstation.com/games/dragon-quest-builders-2-ps4/


# 実行に必要
* Windows マシン
* [.NET 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)の導入
* セーブデータの吸い出し
* セーブデータの書き戻し

# Build環境
* Windows 10(64bit)
* Visual Studio 2022

# セーブデータの構成
* AUTOxx.BIN
  * オートセーブ
* CMNDAT.BIN
  * ゲーム全体に紐づく設定を保持
* SCSHDAT.BIN
  * フォトのデータ
* STGDATxx.BIN
  * 各ステージに紐づく設定を保持
    * 01：からっぽ島
    * 02：モンゾーラ島
    * 03：オッカムル島
    * 04：ムーンブルク島
    * 05：破壊天体シドー
    * 09：ツリル島
    * 10：監獄島
    * 12：かいたく島1
    * 13：かいたく島2
    * 16：かいたく島3

# 編集時の手順
* saveDataを吸い出す
* CMNDAT.exe
  * 『CMNDAT.BIN』をToolで読み込む
* SCSHDAT.exe
  * 『SCSHDAT.BIN』をToolで読み込む
* STGDAT.exe
  * 『STGDATxx.BIN』をToolで読み込む
* 任意の編集を行う
* saveDataを書き戻す

# Special Thanks
* https://docs.google.com/spreadsheets/d/1w8Njk2d0DolMqHw4ZycqsAR4asmEpn0By7m_QpUUArs
* [TaoistYang](https://gbatemp.net/threads/dragon-quest-builders-2-save-editor-pc-steam.558947/#post-9504144)

# LINKDATA edit
## external Tool
* g1t_tools_0.3.zip
* [gimp](https://www.gimp.org/) 2.10.30

## Process
* start LINKDATA.exe
* load LINKDATA.IDX
* Export index = 2659
* change tab idxzrc
* load 02659.idxzrc
* unpack
* rename 02659.unpack to 02659.g1t
* start g1t_tools's qg1t_tool.exe
* load 02659.g1t
* Extract
* start gimp
* load 02659_0.dxt5
* eny edit
* export DDS
* start g1t_tools's qg1t_tool.exe
* Replace (browser)
* select export's DDS
* start LINKDATA.exe
* change tab pack
* load 02659.g1t
* split size = 2097152(=0x200000)
* change tab IDX
* Import index = 2659

ex)  
Item's message resource  
index = 90(jp), 91(us)  
  
Item's Inventory & Bag Icon resource  
index = 795  
