

using UnityEngine;

public class cardToHand : MonoBehaviour
{
    public GameObject Hand;
    public GameObject HandCard;

    void Start()
    {
        if (Hand == null)
        {
            
            Hand = GameObject.Find("Hand");
        }

        if (HandCard != null)
        {
            
            HandCard.transform.SetParent(Hand.transform);
            HandCard.transform.localScale = Vector3.one;
        }
    }

    void Update()
    {
        if (HandCard != null)
        {
            HandCard.transform.position = new Vector3(transform.position.x, transform.position.y, -48);
            HandCard.transform.eulerAngles = new Vector3(25, 0, 0);
        }
    }
}