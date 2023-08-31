TicTacToe
============================

TicTacToe(井字棋)游戏Demo

包含了一个用以初步尝试实现游戏逻辑的C++文件和用Unity实现的项目文件。

C++文件位于根目录 TicTacToeDemo.cpp，其中初步尝试用minimax算法解决AI人机对战。

Unity项目文件位于根目录 UnityProject\TicTacToe文件夹中，所用引擎版本为2021.3.21f1c1。

***

项目文件目录
---------------------------

* 预制体目录Assets\Resources\Prefabs
  
  存放了两种棋子，棋盘，棋盘格子的预制体，UI界面预制体在Assets\Resources\Prefabs\UI下，UI用UIPanelInfo管理路径，UIManager加载。

* 材质目录 Assets\Resources\Materials
  
  包含了两种棋子，棋盘的材质，简单的颜色改变用以区分。

* 场景目录 Assets\Scenes
  
  Demo仅包含一个场景，没有复杂场景切换。

* 字体目录 Assets\Font
  
  游戏UI用了TMP组件处理文本，导入了第三方字体用以显示中文。

* 代码目录 Assets\Scripts
  
  除了UI逻辑，其他主要逻辑没有做文件夹区分
  
  - GameManager.cs | TicTacToeManager.cs
    
    游戏管理及主要逻辑管理，单例，GameManager管理游戏生命周期，TicTacToeManager处理游戏主要逻辑，包含棋盘创建，AI逻辑，落子逻辑。

  - Board.cs
    
    棋盘model，管理棋盘数据结构和逻辑，用二维数组保存棋盘格子状态，包含判断格子归属，设置格子归属，清空格子，清空棋盘，判断胜利等方法。

  - Tile.cs

    格子model，管理格子数据结构和逻辑，拥有私有变量PlayerType用来设置格子归属，row和col设置格子索引

  - TileController.cs
    
    附加在格子预制体上的脚本，创建delegate，用raycast实现点击格子事件通知TicTacToeManager，传参为格子GameObject

  - GameType.cs | PlayerType.cs
    
    两个枚举定义当前玩家和当前游戏模式

  - Scripts\UI\UIManager.cs
    
    UI单例管理类，负责全局UI管理，解析UIPanelInfo.json，从路径中提取UI面板预制体，用栈管理panel。

  - Scripts\UI\UIPanelInfo.cs
    
    继承ISerializationCallbackReceiver解析UIPanelInfo.json。

  - Scripts\UI\UIPanelType.cs
    
    枚举UI页面

  - Scripts\UI\Panel

    UI页面逻辑，BasePanel基类，处理UI页面包括进入，退出，点击按钮的逻辑。