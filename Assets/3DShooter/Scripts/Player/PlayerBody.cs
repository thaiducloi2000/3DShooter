using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBody : MonoBehaviour
{
    private GameObject player; // -> IPlayer

    public GameObject Player
    {
        get
        {
            if (player == null)
            {
                player = transform.parent.root.GetComponent<GameObject>();
            }
            return player;
        }
    }
}
