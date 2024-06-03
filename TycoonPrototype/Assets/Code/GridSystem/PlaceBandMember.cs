using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class PlaceBandMember : MonoBehaviour
{
    public static PlaceBandMember currentInstance;

    public bool placingBandMember = false;
    public GameObject currentBandMember;

    private Vector3 currentTilePos;
    private GameObject stageUI;
    private Stage tempStage;
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
    }

    public void SelectMember(GameObject bandMember)
    {
        placingBandMember = true;

        tempStage = StageBuilder.currentInstance.currentActiveStageUI;
        tempStage.tilemap.color = Color.yellow;
        stageUI.SetActive(false);

        CheckBandMemberAvailability(bandMember);

        tempStage.ClearStageUI();

        if(currentBandMember == null)
        {
            currentBandMember = Instantiate(bandMember, new Vector3(currentTilePos.x, currentTilePos.y, currentTilePos.z), Quaternion.identity);
            currentBandMember.name = bandMember.name.Replace("(Clone)", "").Trim();
        }
    }

    private void CheckBandMemberAvailability(GameObject bandMember)
    {
        switch(bandMember.name)
        {
            case "Alex":
                if (tempStage.alexObject != null)
                    currentBandMember = tempStage.alexObject;
                break;
            case "Rockelle":
                if (tempStage.rockelleObject != null)
                    currentBandMember = tempStage.rockelleObject;
                break;
            case "Lexie":
                if (tempStage.lexieObject != null)
                    currentBandMember = tempStage.lexieObject;
                break;
            case "Picu":
                if (tempStage.picuObject != null)
                    currentBandMember = tempStage.picuObject;
                break;
        }
    }


    private void PlaceMember()
    {

        currentBandMember.transform.localPosition = gridLayout.CellToLocalInterpolated(currentTilePos);
        AssignBandMemberInStage();
        tempStage.FillStageUI();
        tempStage.tilemap.color = Color.white;
        currentBandMember = null;
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
}
