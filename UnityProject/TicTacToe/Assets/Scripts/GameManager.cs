using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    public static GameManager Instance { get; private set; }
    
    private PlayerType playerType = PlayerType.None;
    private GameType gameType = GameType.None;
    
    private GameObject Board;
    private GameObject currentInstance;
    

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

    private void Start()
    {
        UIManager uiManager = UIManager.GetInstance();
        uiManager.PushPanel(UIPanelType.LoginPanel);
        
        Board = Resources.Load<GameObject>("Prefabs/Plane");

    }
    
    // public void StartGame()
    // {
    //     Debug.Log("Game started!");
    // }
    //
    // public void EndGame()
    // {
    //     Debug.Log("Game ended!");
    // }

    public GameType GetGameType()
    {
        return gameType;
    }

    public PlayerType GetPlayerType()
    {
        return playerType;
    }

    public void SetPlayerType(PlayerType playerType)
    {
        this.playerType = playerType;
    }

    public void ChoosePvpMode()
    {
        gameType = GameType.Pvp;
        playerType = PlayerType.Player1;
        currentInstance = Instantiate(Board);
    }
    
    public void ChoosePveMode()
    {
        gameType = GameType.Pve;
        playerType = PlayerType.Player1;
        currentInstance = Instantiate(Board);
    }
    
    public void QuitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }


    public void EndBoard()
    {
        Destroy(currentInstance);
        gameType = GameType.None;
        playerType = PlayerType.None;
    }
    public void ReloadGame()
    {
        TicTacToeManager.Instance.Reload();
    }

}
