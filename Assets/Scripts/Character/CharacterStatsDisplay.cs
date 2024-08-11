using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStatsDisplay : MonoBehaviour , IDisplayStats
{
    [SerializeField]private GameObject uiPanel;
    [SerializeField] private TMP_Text classText;
    [SerializeField] private TMP_Text statsText;

    // Start is called before the first frame update
    private void Start()
    {
        Hide();
    }

    public void Display(CharacterStatModel stats)
    {
        uiPanel.SetActive(true);
        classText.text = stats.CharacterClass;
        statsText.text = $"Health: {stats.Health}\nAttack: {stats.Attack}\nDefense: {stats.Defense}\nMovement Speed: {stats.MovementSpeed}";
    }


    public void Hide()
    {
        uiPanel.SetActive(false);
    }
}
