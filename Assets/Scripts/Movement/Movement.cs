using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Character character;

    private Vector3 targetPosition;

    private bool isMoving = false;

    private void Update()
    {
        if (Input.GetMouseButtonDown(1)) 
        {

            if (character != null && character.isSelected)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    
                    targetPosition = hit.point;
                    isMoving = true;
                }
            }
        }

        if (isMoving)
        {
            MoveCharacter();
        }
    }

    void MoveCharacter()
    {
        
        character.transform.position = Vector3.MoveTowards(character.transform.position, targetPosition, character.characterStats.MovementSpeed * Time.deltaTime);
        
        if (Vector3.Distance(character.transform.position, targetPosition) < 0.1f)
        {
            isMoving = false;
            character.Deselect();
        }
    }
}
