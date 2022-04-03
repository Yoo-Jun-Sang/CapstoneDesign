﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class BE2_UI_BlocksSelectionViewer : MonoBehaviour
{
    public static BE2_UI_BlocksSelectionViewer instance;
    public List<BE2_UI_SelectionPanel> selectionPanelsList;
    [Header("Add Block To Panel")]
    public Transform blockToAddTransform;
    public int panelIndex;
    public bool addBlock = false;

    // v2.4 - bugfix: fixed blocks selection panel not scrolling after block being dragged to ProgrammingEnv
    ScrollRect _scrollRect;

    void Awake()
    {
        instance = this;
        selectionPanelsList = new List<BE2_UI_SelectionPanel>();

        _scrollRect = GetComponent<ScrollRect>();
    }

    void Start()
    {
        selectionPanelsList.AddRange(GetComponentsInChildren<BE2_UI_SelectionPanel>());
    }

    void Update()
    {
        if (addBlock)
        {

            AddBlockToPanel(blockToAddTransform, selectionPanelsList[panelIndex]);
            addBlock = false;
        }
    }

    public void UpdateSelectionPanels()
    {
        selectionPanelsList = new List<BE2_UI_SelectionPanel>();
        selectionPanelsList.AddRange(GetComponentsInChildren<BE2_UI_SelectionPanel>());
    }

    public void AddBlockToPanel(Transform blockTransform, BE2_UI_SelectionPanel selectionPanel)
    {
        Transform blockCopy = Instantiate(blockTransform, Vector3.zero, Quaternion.identity, selectionPanel.transform);
        blockCopy.name = blockCopy.name.Replace("(Clone)", "");

        BE2_BlockUtils.RemoveEngineComponents(blockCopy);
        BE2_BlockUtils.AddSelectionMenuComponents(blockCopy);
        Debug.Log("+ Block added to selection menu");

        GameObject prefabBlock = BE2_BlockUtils.CreatePrefab(blockTransform.GetComponent<I_BE2_Block>());
        blockCopy.GetComponent<BE2_UI_SelectionBlock>().prefabBlock = prefabBlock;
        Debug.Log("+ Block prefab created");
    }

    // v2.4 - bugfix: fixed blocks selection panel not scrolling after block being dragged to ProgrammingEnv.
    //                Changed EnableScroll subscription to pointer up event from BE2_DragSelectionBlock and BE2_DragSelectionVariable to BE2_UI_BlocksSelectionViewer
    void OnEnable()
    {
        BE2_MainEventsManager.Instance.StartListening(BE2EventTypes.OnPointerUpEnd, EnableScroll);
    }

    void OnDisable()
    {
        BE2_MainEventsManager.Instance.StopListening(BE2EventTypes.OnPointerUpEnd, EnableScroll);
    }

    void EnableScroll()
    {
        _scrollRect.enabled = true;
    }
}
