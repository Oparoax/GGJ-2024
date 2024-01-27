using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] public int numberOfBallons = 0;

    public UnityEvent<PlayerInventory> OnBalloonCollected;

    public void BalloonCollected()
    {
        numberOfBallons++;
        OnBalloonCollected.Invoke(this);
    }
}
