using Unity.VisualScripting;
using UnityEngine;

public class DeathFloor : MonoBehaviour
{
    [SerializeField] public MeshCollider planeObject;
    private void Start()
    {
        if (planeObject == null)
        {
            planeObject = GetComponentInChildren<MeshCollider>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GameObject().GetComponent<PlayerController>();
        if (player != null)
        {
            player.Respawn();
        }
    }
}
