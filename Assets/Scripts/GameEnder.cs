using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnder : MonoBehaviour
{
    public void EndGame()
    {
        if (ItemHand.starfishList != null)
        {
            foreach (var starfishObj in ItemHand.starfishList)
            {
                Destroy(starfishObj);
            }
            ItemHand.starfishList.Clear();
        }
    }
}
