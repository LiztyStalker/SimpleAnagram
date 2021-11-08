using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UITextButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {


    [SerializeField]
    UnityEngine.UI.Text btnText;


    GraphicRaycaster gr;

    PointerEventData ped;


    UIText parent;

    bool isDrag = false;

    bool m_isHold = false;

    public string text { get { return btnText.text; } }
    public bool isHold { get { return m_isHold; } }


    public void SetHold()
    {
        m_isHold = true;
        GetComponent<Image>().color = Color.yellow;

    }


    public void Initialize(GraphicRaycaster raycaster, UIText parent, char c)
    {
        this.parent = parent;
        btnText.text = c.ToString() ;
        gr = raycaster;
        ped = new PointerEventData(null);
    }

    
	
	// Update is called once per frame
	void FixedUpdate () {
        if (isDrag)
        {
            MoveButton();
        }
	}

    void MoveButton()
    {
        parent.moveBtn(Input.mousePosition);
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        isDrag = true;
        btnText.gameObject.SetActive(false);
        parent.setMove(btnText.text);
    }



    public void OnPointerUp(PointerEventData eventData)
    {
        isDrag = false;
//        transform.SetParent(parent);

        //패널 레이캐스트
        ped.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        gr.Raycast(ped, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.tag == "Btn")
            {
                UITextButton txtBtn = result.gameObject.GetComponent<UITextButton>();
                parent.SortBtn(this, txtBtn);
                //
                //이동하는 버튼 위치부터 삽입 버튼의 위치까지 앞으로 땡기고
                //삽입버튼을 해당 위치로 이동
                Debug.Log("result : " + result.gameObject.name);
                break;
            }
        }

        btnText.gameObject.SetActive(true);
        parent.resetMove();

        
        
        //        EventSystem.current.currentSelectedGameObject
        //버튼 위치이면 해당 버튼 위치에 삽입
        //버튼 위치가 아니면 돌아가기
    }

   
}
