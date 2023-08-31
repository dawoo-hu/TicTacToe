using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OverPanel:BasePanel
{
    private CanvasGroup canvasGroup;
    private GameObject winnerText; //处理胜利玩家信息
    private void Start()
    {
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
        
        if (winnerText == null)
        {
            winnerText = GameObject.Find("TxtWinner");
        }
        
        winnerText.GetComponent<TMP_Text>().text = " ";


    }
    public override void OnExit()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }
    public void OnClosePanel()
    {
        UIManager.GetInstance().PopPanel();
    }
    
    public override void OnEnter()
    {
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
        
        Vector3 temp = transform.localPosition;
        // temp.x = 600;
        transform.localPosition = temp;
    }
    public override void OnPause()
    {
        canvasGroup.blocksRaycasts = false;
    }
    public override void OnResume()
    {
        canvasGroup.blocksRaycasts = true;
    }
    public void OnAgainBtnClicked()
    {
        Debug.Log("再来一局");
        GameManager.Instance.ReloadGame();
        UIManager.GetInstance().PopPanel();
    }
    
    public void OnMenuBtnClicked()
    {
        Debug.Log("回到主菜单");
        GameManager.Instance.EndBoard();
        UIManager.GetInstance().PopPanel();
        UIManager.GetInstance().PushPanel(UIPanelType.LoginPanel);
    }

    public void OnExitBtnClicked()
    {
        GameManager.Instance.QuitGame();
    }
}