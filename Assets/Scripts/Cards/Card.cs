using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New card", menuName = "Card")]
public class Card : ScriptableObject
{
    public new string name;
    public string description;

    public Sprite sprite;

    public int manaCost;
    public int attack;
    public int health;

    List<Effect> effects;
}
