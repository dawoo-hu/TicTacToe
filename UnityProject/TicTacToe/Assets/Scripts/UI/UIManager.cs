using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    private Transform canvasTransform;

    private Transform CanvasTransform
    {
        get
        {
            if (canvasTransform == null)
            {
                canvasTransform = GameObject.Find("Canvas").transform;
            }

            return canvasTransform;
        }
    }

    private Dictionary<UIPanelType, string> panelPathDicr;

    private Dictionary<UIPanelType, BasePanel> panelDict; 

    private Stack<BasePanel> panelStacks;
    
    public void PushPanel(UIPanelType panelType)
    {
        if (panelStacks == null)
        {
            panelStacks = new Stack<BasePanel>();
        }

        //判断一下栈里面是否有页面
        if (panelStacks.Count > 0)
        {
            BasePanel topPanel = panelStacks.Peek();
            topPanel.OnPause();
        }

        BasePanel panel = GetPanel(panelType);
        panel.OnEnter();
        panelStacks.Push(panel);
    }

    public void PopPanel()
    {
        if (panelStacks == null)
        {
            panelStacks = new Stack<BasePanel>();
        }

        if (panelStacks.Count <= 0)
        {
            return;
        }

        BasePanel topPanel = panelStacks.Pop();
        topPanel.OnExit();
        if (panelStacks.Count <= 0) return;
        BasePanel topPanel2 = panelStacks.Peek();
        topPanel2.OnResume();
    }
    
    public BasePanel GetPanel(UIPanelType panelType)
    {
        if (panelDict == null)
        {
            panelDict = new Dictionary<UIPanelType, BasePanel>();
        }

        BasePanel panel;
        panelDict.TryGetValue(panelType, out panel);//TODO


        if (panel == null)
        {
            string path;
            panelPathDicr.TryGetValue(panelType, out path);

            var pan = Resources.Load(path);
            GameObject instPanel = GameObject.Instantiate(pan) as GameObject; 
            instPanel.transform.SetParent(CanvasTransform, false); 
            panelDict.Add(panelType, instPanel.GetComponent<BasePanel>());
            return instPanel.GetComponent<BasePanel>();
        }
        else
        {
            return panel;
        }
    }


    private UIManager()
    {
        ParseUIPanelTypeJson();
    }

    //单例化
    private static UIManager instance;

    public static UIManager GetInstance()
    {
        if (instance == null)
        {
            instance = new UIManager();
        }

        return instance;
    }

    [Serializable]
    class UIPanelTypeJson
    {
        public List<UIPanelInfo> infoList;
    }


    //解析Json文件
    private void ParseUIPanelTypeJson()
    {
        panelPathDicr = new Dictionary<UIPanelType, string>();
        TextAsset ta = Resources.Load<TextAsset>("Prefabs/UI/UIPanelInfo");

        UIPanelTypeJson
            jsonObject =
                JsonUtility.FromJson<UIPanelTypeJson>(ta
                    .text);

        foreach (UIPanelInfo info in jsonObject.infoList) 
        {
            panelPathDicr.Add(info.panelType, info.path); 
        }
    }
}