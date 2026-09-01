
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour
{
    public List<GameObject> inventory;

    public int currentInventorySlot = 0;

    PlayerInput playerInput;

    void Start()
    {
        playerInput = GetComponentInParent<PlayerScript>().playerCTRLS;

        if (playerInput != null)
        {
            playerInput.actions["SwitchWeapon"].performed += SwitchWeapons;
        }

        intializeInventory();

    }


    void intializeInventory()
    {
        if (inventory.Count == 0)
        { return; }

        for(int i = 0; i < inventory.Count; i ++)
        {
            inventory[i].SetActive(i == 0);
        }
    }


    void SwitchWeapons(InputAction.CallbackContext context)
    {
        if (inventory.Count <= 1)
        { 
            return; 
        }

        if (context.ReadValue<float>() < 0)
        {
            currentInventorySlot -= 1;

        }
        else  if(context.ReadValue<float>() > 0) 
        {
            currentInventorySlot += 1;

        }

        currentInventorySlot = Math.Clamp(currentInventorySlot, 0, (inventory.Count - 1));


        for (int i = 0; i < inventory.Count; i++)
        {
            inventory[i].SetActive(i == currentInventorySlot);
        }
    }


}