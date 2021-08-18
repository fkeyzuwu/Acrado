using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Deck : NetworkBehaviour
{
    private Stack<Card> cardDeck = new Stack<Card>();
    [SerializeField] private Card[] cardArray;

    void Start()
    {
        CreateDeck();
    }

    public void CreateDeck()
    {
        int n = cardArray.Length;
        for (int i = 0; i < n; i++)
        {
            // NextDouble returns a random number between 0 and 1.
            int r = i + (int)(Random.Range(0f, 1f) * (n - i));
            Card card = cardArray[r];
            cardArray[r] = cardArray[i];
            cardArray[i] = card;
        }

        for(int i = 0; i < n; i++)
        {
            cardDeck.Push(cardArray[i]);
        }
    }

    public Stack<Card> CardDeck
    {
        get { return cardDeck; }
        set { cardDeck = value; }
    }

    /*[CustomEditor(typeof(Deck))]
    public class StackPreview : Editor
    {
        public override void OnInspectorGUI()
        {
            // get the target script as TestScript and get the stack from it
            var ts = (Deck)target;
            var stack = ts.deck;

            // some styling for the header, this is optional
            var bold = new GUIStyle();
            bold.fontStyle = FontStyle.Bold;
            GUILayout.Label("Items in my stack", bold);

            // add a label for each item, you can add more properties
            // you can even access components inside each item and display them
            // for example if every item had a sprite we could easily show it 
            foreach (var item in stack)
            {
                GUILayout.Label(item.name);
            }
        }
    }*/
}


