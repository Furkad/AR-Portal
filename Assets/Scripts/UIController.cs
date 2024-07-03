using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private GameObject inventoryPanel;
    private RectTransform inventoryPanelRect;
    [SerializeField]
    private RectTransform arrowRect;

    [SerializeField] 
    private GameObject hintPanel;
    [SerializeField]
    private TMP_Text hintText;

    [SerializeField]
    private float shownYPos;
    [SerializeField]
    private float hiddenYPos;

    private bool isInventoryPanelOn = true;

    private TextAsset hintsFile;
    private List<string> hintString = new List<string>();
    private int currentHint = -1;

    public bool isPaused = false;

    private void Awake()
    {
        inventoryPanelRect = inventoryPanel.GetComponent<RectTransform>();
        Locale currentLocale = LocalizationSettings.SelectedLocale;
        if (currentLocale == LocalizationSettings.AvailableLocales.GetLocale("ru"))
            hintsFile = Resources.Load("Hints/HintTextsRus") as TextAsset;
        else
            hintsFile = Resources.Load("Hints/HintTextsEng") as TextAsset;
        hintString = hintsFile.text.Split('\n').ToList();
        HintPanelUpdate(0);
    }

    public void HideShowInventoryPanel()
    {
        if (isInventoryPanelOn)
        {
            inventoryPanelRect.anchoredPosition = new Vector2(0f, hiddenYPos);
            arrowRect.Rotate(0f, 0f, 180f);
            isInventoryPanelOn = false;
        }
        else
        {
            inventoryPanelRect.anchoredPosition = new Vector2(0f, shownYPos);
            arrowRect.Rotate(0f, 0f, -180f);
            isInventoryPanelOn = true;
        }
    }

    public void HintPanelUpdate(int stringNumber)
    {
        hintText.text = hintString[stringNumber];
        currentHint = stringNumber;
    }

    public void HintPanelUpdate(string locale)
    {
        if (locale == "ru")
            hintsFile = Resources.Load("Hints/HintTextsRus") as TextAsset;
        else
            hintsFile = Resources.Load("Hints/HintTextsEng") as TextAsset;
        hintString = hintsFile.text.Split('\n').ToList();
        hintText.text = hintString[currentHint];
    }

    public void HideShowPanel(GameObject panelGO)
    {
        panelGO.SetActive(panelGO.activeSelf ? false : true);
        if (panelGO.name == "PausePanel")
            isPaused = panelGO.activeSelf;
    }
}
