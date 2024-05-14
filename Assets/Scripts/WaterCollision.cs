using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCollision : MonoBehaviour
{
    MenuController controller;

    private void Start()
    {
        controller = FindObjectOfType<MenuController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            controller.LoadGameOverScene();
    }
}
