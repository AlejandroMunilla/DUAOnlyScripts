// Copyright (c) Pixel Crushers. All rights reserved.

using UnityEngine;
using System.Collections;


namespace PixelCrushers.DialogueSystem.SequencerCommands
{

    /// <summary>
    /// </summary>
    [AddComponentMenu("")] // Hide from menu.
    public class SequencerCommandAddInventory : SequencerCommand
    {
        GameController gc;
        private string nameItem;
        private bool loaded = false;
        
   

        public void Start()
        {
            Debug.Log("Start");
            if (loaded == false)
            {
                gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
            }
            nameItem = GetParameter(0);
            Debug.Log(nameItem);

            gc.inventory.Add(nameItem);
        }
    }
}

