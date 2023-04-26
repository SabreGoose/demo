using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    private LevelManager levelManager;

    private void Awake() 
    {
        levelManager = FindObjectOfType<LevelManager>();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (levelManager is null)
            return;
        Item item = other.GetComponent<Item>();
        if (item is null)
            return;
        levelManager.CollectItem(item);
        Destroy(item.gameObject);
    }


}
