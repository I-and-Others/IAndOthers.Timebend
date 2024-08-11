using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatModel : MonoBehaviour
{
    public string CharacterClass {  get; private set; }
    public int Health { get; private set; }
    public int Attack { get; private set; }
    public int Defense { get; private set; }
    public float MovementSpeed { get; private set; }

    public CharacterStatModel(string characterClass, int health, int attack, int defense, float movementSpeed)
    {
        CharacterClass = characterClass;
        Health = health;
        Attack = attack;
        Defense = defense;
        MovementSpeed = movementSpeed;
    }
}
