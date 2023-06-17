using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum Direction { Left, Right }

public class InventoryRenderer : MonoBehaviour
{
    private InventoryCell[] cells;
    private bool showCells = false;
    private float cellShowTimer = 0;

    void Start()
    {
        cells = GetComponentsInChildren<InventoryCell>();
    }

    void Update()
    {
        DoRender();
    }

    void DoRender()
    {
        if (showCells)
        {
            if (cellShowTimer > 0)
                cellShowTimer -= Time.deltaTime * 100;
            else
            {
                var hiddenCells = cells.Where(c => !c.GetIsVisible());

                if (hiddenCells.Count() > 1)
                {
                    var randomCell = hiddenCells.ToArray()[Random.Range(0, hiddenCells.Count() - 1)];
                    randomCell.Show();
                    cellShowTimer = 2;
                }
                else
                    showCells = false;
            }
        }
    }

    public void Show()
    {
        showCells = true;
    }

    public void Hide()
    {
        foreach (InventoryCell cell in cells)
            cell.Hide();

        showCells = false;
    }
}
