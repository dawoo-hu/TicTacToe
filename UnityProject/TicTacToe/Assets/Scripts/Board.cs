using Unity.VisualScripting;
using UnityEngine;

public class Board
{
    private Tile[,] tiles = new Tile[3, 3];

    public Board()
    {
        //构造初始化格子信息
        tiles = new Tile[3, 3];
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                tiles[i, j] = new Tile();
            }
        }
    }

    public void SetTile(int row, int col)
    {
        tiles[row, col].SetTileInfo(row, col);
    }

    public PlayerType GetTileOwner(int row, int col)
    {
        return tiles[row, col].CheckOwner();
    }

    public void SetTileOwner(int row, int col, PlayerType playerType)
    {
        tiles[row, col].MakeMove(playerType);
    }

    public void ClearTile(int row, int col)
    {
        tiles[row, col].MakeMove(PlayerType.None);
    }

    public bool isEmpty(int row, int col)
    {
        return tiles[row, col].CheckOwner() == PlayerType.None;
    }

    public bool IsFull()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (isEmpty(i, j))
                {
                    return false;
                }
            }
        }
        return true;
    }

    public bool CheckWin(PlayerType playerType)
    {
        //判断游戏结束
        for (int i = 0; i < 3; ++i) 
        {
            if (tiles[i, 0].CheckOwner() == playerType && tiles[i, 1].CheckOwner() == playerType && tiles[i, 2].CheckOwner() == playerType)
                return true;
            if (tiles[0, i].CheckOwner() == playerType && tiles[1, i].CheckOwner() == playerType && tiles[2, i].CheckOwner() == playerType)
                return true;
        }
        if (tiles[0, 0].CheckOwner() == playerType && tiles[1, 1].CheckOwner() == playerType && tiles[2, 2].CheckOwner() == playerType)
            return true;
        if (tiles[0, 2].CheckOwner() == playerType && tiles[1, 1].CheckOwner() == playerType && tiles[2, 0].CheckOwner() == playerType)
            return true;
        return false;
    }

    public void ClearBoard()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                ClearTile(i, j);
            }
        }
    }

}