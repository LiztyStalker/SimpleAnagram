using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class UIText : MonoBehaviour
{
    [SerializeField]
    Image image;

    [SerializeField]
    UnityEngine.UI.GraphicRaycaster gRaycaster;

    [SerializeField]
    GameObject moveObject;

    [SerializeField]
    UnityEngine.UI.Text moveText;

    [SerializeField]
    UITextButton tmpUITextButton;

    [SerializeField]
    Transform batchTransform;

    [SerializeField]
    Text playBtnText;

    [SerializeField]
    Button hintButton;

    List<UITextButton> uiList = new List<UITextButton>();

    bool isSuccess = true;

    AnagramData data;

    void Awake()
    {
        DataManager.GetInstance.initInstance();
    }

    void Start()
    {
        ClearList();

        data = GetData();

        if (data != null)
        {
            foreach(char c in data.question){
                UITextButton txtBtn = Instantiate(tmpUITextButton);
                txtBtn.Initialize(gRaycaster, this, c);
                txtBtn.transform.SetParent(batchTransform);
                txtBtn.transform.localPosition = Vector3.one;
                txtBtn.GetComponent<Image>().color = Color.white;
                txtBtn.gameObject.SetActive(true);
                uiList.Add(txtBtn);
            }
            image.sprite = data.sprite;
        }

        isSuccess = false;


        //uiTextButton = gameObject.GetComponentsInChildren<UITextButton>().ToList<UITextButton>();
        //foreach (UITextButton textbtn in uiTextButton)
        //{
        //    textbtn.SetParent(this);
        //}
    }

    AnagramData GetData()
    {
        if (!isSuccess)
            return data;
        return DataManager.GetInstance.GetData(DataManager.TYPE_LEVEL.EASY);
    }

    void ClearList()
    {

        for (int i = uiList.Count - 1; i >= 0; i--)
        {
            Destroy(uiList[i].gameObject);
        }
        uiList.Clear();
    }

    public void setMove(string text)
    {
        moveText.text = text;
        moveObject.SetActive(true);
    }

    public void resetMove()
    {
        moveText.text = "";
        moveObject.SetActive(false);
        CheckWord();
    }

    public void moveBtn(Vector2 pos)
    {
        moveObject.transform.position = pos;
    }

    public void SortBtn(UITextButton btn, UITextButton searchBtn)
    {
        if (btn != searchBtn)
        {
            //해당 버튼 위치 기억
            //삽입 버튼 위치 기억
            //해당 버튼 위치부터 삽입 버튼 위치까지 땡기기

            //밀기 
            int index = uiList.IndexOf(searchBtn);
            uiList.Remove(btn);
            uiList.Insert(index, btn);

            btn.transform.SetSiblingIndex(index);
            Debug.Log("Index : " + index);
            CheckWord();
        }
    }

   

    void CheckWord()
    {
        string word = "";
        foreach (UITextButton textbtn in uiList)
        {
            word += textbtn.text;
        }

        Debug.Log("Word " + word);

        if (word == data.answer)
        {
            isSuccess = true;
            Debug.Log("Success!");
        }
        ShowButton();
    }


    public void OnPlayClicked()
    {
        Start();
    }

    void ShowButton()
    {
        if (isSuccess)
        {
            playBtnText.text = "Next";
        }
        else
        {
            playBtnText.text = "Retry";
        }
    }

    public void OnHintClicked()
    {
        int index = -1;
        //랜덤 위치가 맞으면 위에서 다시 반복
        while (index < 0)
        {
            //전체 중에서 위치 랜덤 고르기
            index = UnityEngine.Random.Range(0, data.question.Length);
            if (uiList[index].isHold && uiList[index].text == data.answer[index].ToString())
                index = -1;
        }

        Debug.Log("index : " + index);

        //안맞으면 
        //맞는 위치 인덱스 찾기
        foreach(UITextButton txtBtn in uiList){
            if (!txtBtn.isHold && txtBtn.text == data.answer[index].ToString()) 
            {
                //해당 위치에 알파벳 삽입 후 색상 변경
                ChangeBtn(txtBtn, index);
                txtBtn.SetHold();
                break;
            }
        }
    }

    void ChangeBtn(UITextButton btn, int index)
    {
        UITextButton[] tmpList = uiList.ToArray<UITextButton>();

        int tmpIndex = uiList.IndexOf(btn);

        UITextButton tmpBtn = tmpList[index];
        tmpList[index] = btn;
        tmpList[tmpIndex] = tmpBtn;

        Debug.Log("tmpBtn " + tmpBtn.text + " " + tmpList[index].text + " " + tmpList[tmpIndex].text);

        tmpList[index].transform.SetSiblingIndex(index);
        tmpList[tmpIndex].transform.SetSiblingIndex(tmpIndex);

        uiList = tmpList.ToList<UITextButton>();
        CheckWord();
    }

    public void OnExitClicked()
    {
        Application.Quit();
    }
}

