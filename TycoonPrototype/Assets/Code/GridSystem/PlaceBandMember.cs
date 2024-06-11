using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class PlaceBandMember : MonoBehaviour
{
    public static PlaceBandMember currentInstance;

    public bool placingBandMember = false;
    public GameObject currentBandMember;
    private Vector3 prevPos;

    private Vector3 currentTilePos;
    private GameObject stageUI;
    private GameObject image;
    private Stage tempStage;
    private TilemapCollider2D tempStageCol;
    private GridLayout gridLayout;

    private void Start()
    {
        currentInstance = this;
        gridLayout = BuildingSystem.currentInstance.gridLayout;
        stageUI = StageBuilder.currentInstance.StageUI;
    }

    private void Update()
    {

        if(!placingBandMember)
        {
            return;
        }

        currentTilePos = BuildingSystem.currentInstance.mouseCellPos;
        currentBandMember.transform.localPosition = gridLayout.CellToLocalInterpolated(currentTilePos);

        if (Input.GetMouseButtonDown(0))
        {
            if (tempStage.isMouseOverStage)
            {
                PlaceMember();
            } else
            {
                Debug.Log("Band Member outside stage bounds");
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if(CheckForDelete(currentBandMember))
            {
                tempStage.tilemap.color = Color.white;
                Destroy(currentBandMember);
                currentBandMember = null;
                prevPos = Vector3Int.zero;
                tempStageCol = null;
                tempStage = null;
                placingBandMember = false;

                return;
            }

            currentBandMember.transform.localPosition = prevPos;
            PlaceMember();
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            RotateBandMember();
        }

    }

    public void SelectMember(GameObject bandMember)
    {
        placingBandMember = true;

        tempStage = StageBuilder.currentInstance.currentActiveStageUI;
        tempStageCol = tempStage.GetComponent<TilemapCollider2D>();
        tempStage.tilemap.color = Color.yellow;
        stageUI.SetActive(false);

        CheckBandMemberAvailability(bandMember);

        stageUI.SetActive(false);

        if(currentBandMember == null)
        {
            currentBandMember = Instantiate(bandMember, new Vector3(currentTilePos.x, currentTilePos.y, currentTilePos.z), Quaternion.identity);
            currentBandMember.name = bandMember.name.Replace("(Clone)", "").Trim();
        }
        image = currentBandMember.gameObject.transform.Find("Image").gameObject;
    }

    private void CheckBandMemberAvailability(GameObject bandMember)
    {
        switch(bandMember.name)
        {
            case "Alex":
                if (tempStage.alexObject != null)
                    prevPos = tempStage.alexObject.transform.localPosition;
                    currentBandMember = tempStage.alexObject;
                break;
            case "Rockelle":
                if (tempStage.rockelleObject != null)
                    prevPos = tempStage.rockelleObject.transform.localPosition;
                    currentBandMember = tempStage.rockelleObject;
                break;
            case "Lexie":
                if (tempStage.lexieObject != null)
                    prevPos = tempStage.lexieObject.transform.localPosition;
                    currentBandMember = tempStage.lexieObject;
                break;
            case "Picu":
                if (tempStage.picuObject != null)
                    prevPos = tempStage.picuObject.transform.localPosition;
                    currentBandMember = tempStage.picuObject;
                break;
        }
    }

    private bool CheckForDelete(GameObject bandMember)
    {
        switch (bandMember.name)
        {
            case "Alex":
                if (tempStage.alexObject == null)
                {
                    return true;
                }
                    return false;
                

            case "Rockelle":
                if (tempStage.rockelleObject == null)
                {
                    return true;
                }

                    return false;
                

            case "Lexie":
                if (tempStage.lexieObject == null)
                {
                    return true;
                }

                    return false;
                

            case "Picu":
                if (tempStage.picuObject == null)
                {
                    return true;
                }

                    return false;
                
        }
        return false;
    }


    private void PlaceMember()
    {
        AssignBandMemberInStage();
        stageUI.SetActive(true);
        CameraController.instance.cameraActive = false;
        tempStage.tilemap.color = Color.white;
        image = null;
        currentBandMember = null;
        prevPos = Vector3Int.zero;
        tempStageCol = null;
        tempStage = null;
        placingBandMember = false;
    }

    private void AssignBandMemberInStage()
    {
        switch (currentBandMember.name)
        {
            case "Alex":
                if (tempStage.alexObject  == null)
                   tempStage.alexObject = currentBandMember;
                break;
            case "Rockelle":
                if (tempStage.rockelleObject == null)
                    tempStage.rockelleObject = currentBandMember;
                break;
            case "Lexie":
                if (tempStage.lexieObject == null)
                    tempStage.lexieObject = currentBandMember;
                break;
            case "Picu":
                if (tempStage.picuObject == null)
                    tempStage.picuObject = currentBandMember;
                break;
        }
    }

    private void RotateBandMember()
    {
        if(image != null)
        {
            Vector2 scale = image.transform.localScale;
            scale.x *= -1;

            image.transform.localScale = scale;
        }
    }
}
