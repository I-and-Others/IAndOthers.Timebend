using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{

    public CharacterStatModel characterStats;
    private IDisplayStats statsDisplay;

    [SerializeField] private string className;
    [SerializeField] private int Health;
    [SerializeField] private int Attack;
    [SerializeField] private int Defence;
    [SerializeField] private float MovementSpeed;

    public bool isSelected = false;
    

    // Start is called before the first frame update
    void Start()
    {
        statsDisplay = GetComponent<IDisplayStats>();
        if (statsDisplay == null ) { Debug.LogError("Stats did not implemented or not found!");  }
        characterStats = new CharacterStatModel(className,Health, Attack, Defence,MovementSpeed);
      
    }

    private void OnMouseEnter()
    {
        statsDisplay.Display(characterStats);
    }

    private void OnMouseExit()
    {
        if(!isSelected) statsDisplay.Hide();
                    
    }

    private void OnMouseDown()
    {
        isSelected = true;
        statsDisplay.Display(characterStats);
    }

    public void Deselect()
    {
        isSelected = false;
        statsDisplay.Hide();
    }
}
