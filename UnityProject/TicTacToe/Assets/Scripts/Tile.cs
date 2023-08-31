using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    private PlayerType _playerType = PlayerType.None;
    private int row;
    private int col;

    public void SetTileInfo(int row, int col)
    {
        this.row = row;
        this.col = col;
    }

    public void MakeMove(PlayerType playerType)
    {
        _playerType = playerType;
    }

    public PlayerType CheckOwner()
    {
        return _playerType;
    }

    public bool CheckIsMoved()
    {
        if (_playerType != PlayerType.None)
        {
            return true;
        }

        return false;
    }
}
