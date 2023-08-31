using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicTacToeManager : MonoBehaviour
{
    public static TicTacToeManager Instance { get; private set; }
    public GameObject tilePrefab; // 格子预制体
    private int rows = 3; // 棋盘的行数
    private int columns = 3; // 棋盘的列数
    private float spacing = 1.2f; // 格子之间的间距
    private Vector3[,] ChessPos; //存储格子位置，根据存储的格子位置判断所点击格子是哪一个，减少开销可以写死位置判断

    private GameType curGameType = GameType.None;
    private PlayerType curPlayerType = PlayerType.None;

    public GameObject ChessWhite;
    public GameObject ChessBlack;

    private List<GameObject> ChessList;

    private Board _board;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        _board = new Board();
        ChessPos = new Vector3[3, 3];
        ChessList = new List<GameObject>();
        curGameType = GameManager.Instance.GetGameType();
        curPlayerType = GameManager.Instance.GetPlayerType();

        GenerateChessboard();
        TileController.OnTileClicked += HandleTileClicked;
    }

    void GenerateChessboard()
    {
        // 计算整个棋盘的宽度和长度
        float totalWidth = columns * spacing;
        float totalLength = rows * spacing;

        // 计算左上角格子的起始位置
        Vector3 startOffset = new Vector3(-totalWidth / 2 + spacing / 2, 0, totalLength / 2 - spacing / 2);

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                // 计算每个格子的位置
                Vector3 position = startOffset + new Vector3(col * spacing, 0, -row * spacing);
                GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity);
                tile.transform.parent = gameObject.transform;
                ChessPos[row, col] = position;
                _board.SetTile(row, col);
            }
        }
    }

    void MakeMove(int i, int j)
    {
        // 落子方法
        if (isGameOver())
        {
            Debug.Log("=======================");
            return;
        }
        
        if (_board.isEmpty(i, j))
        {
            _board.SetTileOwner(i, j, curPlayerType);
            Vector3 pos = ChessPos[i, j];

            SetAChess(pos);

            if (CheckWin())
            {
                UIManager.GetInstance().PushPanel(UIPanelType.OverPanel);
            }

            if (_board.IsFull())
            {
                UIManager.GetInstance().PushPanel(UIPanelType.OverPanel);
            }

            if (curGameType == GameType.Pve)
            {
                if (curPlayerType == PlayerType.Player1)
                {
                    curPlayerType = PlayerType.AI;
                    GameManager.Instance.SetPlayerType(PlayerType.AI);
                }
                else
                {
                    curPlayerType = PlayerType.Player1;
                    GameManager.Instance.SetPlayerType(PlayerType.Player1);
                }
            }
            else if (curGameType == GameType.Pvp)
            {
                if (curPlayerType == PlayerType.Player1)
                {
                    curPlayerType = PlayerType.Player2;
                    GameManager.Instance.SetPlayerType(PlayerType.Player2);
                }
                else
                {
                    curPlayerType = PlayerType.Player1;
                    GameManager.Instance.SetPlayerType(PlayerType.Player1);
                }
            }

            if (curGameType == GameType.Pve)
            {
                if (curPlayerType == PlayerType.AI)
                {
                    MakeAIMove();
                }
            }
        }
    }

    void MakeAIMove()
    {
        //AI回合落子
        Tuple<int, int> bestMove = FindBestMove();
        if (bestMove != null)
        {
            MakeMove(bestMove.Item1, bestMove.Item2);
        }
    }

    public List<Tuple<int, int>> GetAvailableMoves()
    {
        //找到空的格子
        List<Tuple<int, int>> moves = new List<Tuple<int, int>>();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (_board.isEmpty(i, j))
                {
                    moves.Add(new Tuple<int, int>(i, j));
                }
            }
        }
        return moves;
    }

    public int Evaluate()
    {
        //当递归过程中判断游戏结束，返回评估值
        if (_board.CheckWin(PlayerType.AI)) return 1;
        if (_board.CheckWin(PlayerType.Player1)) return -1;
        return 0;
    }

    private bool isGameOver()
    {
        //递归过程中判断是否游戏结束
        return (_board.CheckWin(PlayerType.AI) || _board.CheckWin(PlayerType.Player1) || GetAvailableMoves().Count == 0);
    }
    

    public int MiniMax(int depth, bool isMaximizing)
    {
        //miniamax算法，通过递归模拟落子
        if (isGameOver())
        {
            //递归模拟判断游戏结束，返回评估值，终止递归
            return Evaluate();
        }
        
        if (isMaximizing)
        {
            int maxEval = int.MinValue;
            foreach (var move in GetAvailableMoves())
            {
                _board.SetTileOwner(move.Item1, move.Item2, PlayerType.AI);
                maxEval = Math.Max(maxEval, MiniMax(depth + 1, !isMaximizing));
                _board.ClearTile(move.Item1, move.Item2);
            }
            return maxEval;
        }
        else
        {
            int minEval = int.MaxValue;
            foreach (var move in GetAvailableMoves())
            {
                _board.SetTileOwner(move.Item1, move.Item2, PlayerType.Player1);
                minEval = Math.Min(minEval, MiniMax(depth + 1, !isMaximizing));
                _board.ClearTile(move.Item1, move.Item2);
            }
            return minEval;
        }
    }

    public Tuple<int, int> FindBestMove()
    {
        //AI找到最佳格子，初始化一个List，将可落子的格子添加，递归模拟
        int bestEval = int.MinValue;
        Tuple<int, int> bestMove = null;
        foreach (var move in GetAvailableMoves())
        {
            _board.SetTileOwner(move.Item1, move.Item2, PlayerType.AI);
            int moveEval = MiniMax(0, false);
            _board.ClearTile(move.Item1, move.Item2);
            if (moveEval > bestEval)
            {
                bestEval = moveEval;
                bestMove = move;
            }
        }
        return bestMove;
    }

    void SetAChess(Vector3 pos)
    {
        //在棋盘落子，生成棋子实体
        GameObject curChess;
        if (curPlayerType != PlayerType.None)
        {
            if (curPlayerType == PlayerType.Player1)
            {
                curChess = ChessWhite;
            }
            else
            {
                curChess = ChessBlack;
            }

            GameObject chess = Instantiate(curChess, pos, Quaternion.identity);
            ChessList.Add(chess);
            chess.transform.parent = gameObject.transform;
        }
        return;
    }

    void HandleTileClicked(TileClickedData tile)
    {
        //获取被点击格子
        GameObject clickedCube = tile.tile;
        Vector3 tilePos = clickedCube.transform.position;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (ChessPos[i, j].x == tilePos.x && ChessPos[i, j].z == tilePos.z)
                {
                    MakeMove(i, j);
                }
            }
        }
    }

    private bool CheckWin()
    {
        return _board.CheckWin(curPlayerType);
    }

    public void Reload()
    {
        foreach (var chess in ChessList)
        {
            Destroy(chess);
        }
        ChessList.Clear();
        _board.ClearBoard();
        curPlayerType = PlayerType.Player1;
    }
    

    private void OnDestroy()
    {
        TileController.OnTileClicked -= HandleTileClicked;
    }
}