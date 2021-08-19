using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[CreateAssetMenu(fileName = "New card", menuName = "Card")]
public class Card : ScriptableObject
{
    public new string name;
    public string description;

    //public Sprite sprite;

    public int manaCost;
    public int attack;
    public int health;
}

public static class CustomReadWriteFunctions
{
    public static void WriteCard(this NetworkWriter writer, Card value)
    {
        writer.WriteString(value.name);
        writer.WriteString(value.description);
        writer.WriteInt(value.manaCost);
        writer.WriteInt(value.attack);
        writer.WriteInt(value.health);
    }

    public static Card ReadCard(this NetworkReader reader)
    {
        Card card = ScriptableObject.CreateInstance<Card>();

        card.name = reader.ReadString();
        card.description = reader.ReadString();
        card.manaCost = reader.ReadInt();
        card.attack = reader.ReadInt();
        card.health = reader.ReadInt();

        return card;
    }
}
