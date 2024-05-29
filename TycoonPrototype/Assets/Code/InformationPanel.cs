using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InformationPanel : MonoBehaviour
{
    public GameObject hoverPanel;
    public Text nameText;
    public Text capacityText;

    public float hoverOffsetX;
    public float hoverOffsetY;
    public LayerMask buildingLayer;
    public static InformationPanel instance;

    private bool isHoverOn;

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
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, buildingLayer);

        // 检查是否击中了物体
        if (hit.collider != null && hit.collider.CompareTag("Building") && hit.collider.GetComponent<Building>().Placed)
        {
            //Debug.Log("hit");
            ShowHoverPanel(hit.collider.GetComponent<Building>());
        }
        else
        {
            //Debug.Log("not hit");
            HideHoverPanel();
        }
    }


    public void ShowHoverPanel(Building hoveredBuilding)
    {
        isHoverOn = true;
        hoverPanel.SetActive(true);

        nameText.text = hoveredBuilding.name;
        capacityText.text = ("[" + hoveredBuilding.capacityCount + "/" + hoveredBuilding.capacityMax.ToString() + "]");
    }

    public void HideHoverPanel()
    {
        isHoverOn = false;
        hoverPanel.SetActive(false);
    }
}