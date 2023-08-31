using UnityEngine;

public class LoginPanel:BasePanel
{
    private CanvasGroup canvasGroup;
    private void Start()
    {
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
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
    public void OnPvpBtnClicked()
    {
        GameManager.Instance.ChoosePvpMode();
        UIManager.GetInstance().PopPanel();
    }
    
    public void OnPveBtnClicked()
    {
        GameManager.Instance.ChoosePveMode();
        UIManager.GetInstance().PopPanel();
    }
    
    public void OnExitBtnClicked()
    {
        GameManager.Instance.QuitGame();
    }
}