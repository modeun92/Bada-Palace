﻿using Assets.Scripts.Global;
using Assets.Scripts.Item;
using Assets.Scripts.Maze;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public MazeConstructor MazeGenerator;
    public InventoryManager Inventory;
    public int Level = 1;
    public int Stage = 1;
    // Start is called before the first frame update
    void Start()
    {
        Level = ConfigManager.Prog.CurrentProgress.Level;
        Stage = ConfigManager.Prog.CurrentProgress.Stage;
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void GoToNextStage()
    {
        ConfigManager.Prog.IncreaseStage();
        Level = ConfigManager.Prog.CurrentProgress.Level;
        Stage = ConfigManager.Prog.CurrentProgress.Stage;
    }
    public void PlayMazeGame()
    {
        MazeGenerator.Generate(Level, Stage);
        //depending on level ramdomly giving the items as a gift
    }
    private void AwardPrize()
    {
        int earn = (int)(Level * 2.5f);
        Inventory.AddGold(earn);
        TryAddItem(ItemType.STARFISH, (int)(Level * 1.5f));
        //
        Inventory.AddGold(50);
        TryAddItem(ItemType.STARFISH, 5);
        TryAddItem(ItemType.SHELL, 10);
        TryAddItem(ItemType.FISH, 2);
        TryAddItem(ItemType.SPEED_POTION, 1);
        TryAddItem(ItemType.INVISIBLE_POTION, 3);
        //
    }
    private void TryAddItem(ItemType aItemType)
    {
        Item lItem = ItemList.GetItem(aItemType);
        if (lItem != null)
        {
            Inventory.AddItem(lItem);
        }
    }
    private void TryAddItem(ItemType aItemType, int aAmount)
    {
        Item lItem = ItemList.GetItem(aItemType, aAmount);
        if (lItem != null)
        {
            Inventory.AddItem(lItem);
        }
    }
}
