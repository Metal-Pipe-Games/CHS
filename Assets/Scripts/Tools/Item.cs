using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Item : MonoBehaviour
{
    public abstract ItemType Type { get; }
    public bool pickedUp = false;

    public Transform Player;
    public LayerMask LayerMask;
    public float maxDistance = 1;
    public GameObject item;

    public UnityEvent onPickup;

    public enum ItemType
    {
        None,
        Key,
        Interactable
    }

    public bool Pickup()
    {
        pickedUp = true;
        onPickup.Invoke();
        return true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if(Vector3.Distance(transform.position,Player.position) < maxDistance)
            {
                Pickup();
            }
            /*
            RaycastHit look;
            if (Physics.Raycast(Player.position, transform.position - Player.position, out look, maxDistance, LayerMask.value))
            {
                if (look.collider.CompareTag("Item"))
                {
                    Pickup();
                }
            }/**/
        }
    }

    public void DestroyItem(float destroyAfter)
    {
        Destroy(item, destroyAfter);
    }
}