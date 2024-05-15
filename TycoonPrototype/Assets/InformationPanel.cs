using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InformationPanel : MonoBehaviour
{
    public GameObject hoverPanel;

    public float hoverOffsetX;
    public float hoverOffsetY;
    public LayerMask layerMask;


    private bool isHoverOn;

    public static InformationPanel instance;


    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isHoverOn)
        {
            hoverPanel.transform.position = Input.mousePosition + new Vector3(hoverOffsetX, hoverOffsetY, 0);
        }
    }

    private void FixedUpdate()
    {
        // 获取鼠标位置并转换为世界坐标
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // 执行2D射线检测，使用指定的层
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, layerMask);

        // 检查是否击中了物体
        if (hit.collider != null)
        {
            Debug.Log("hit");
            ShowHoverPanel();
        }
        else
        {
            Debug.Log("not hit");
            HideHoverPanel();
        }
    }


    public void ShowHoverPanel()
    {
        isHoverOn = true;
        hoverPanel.SetActive(true);

    }

    public void HideHoverPanel()
    {
        isHoverOn = false;
        hoverPanel.SetActive(false);
    }
}
