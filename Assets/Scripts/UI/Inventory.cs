using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private InventoryCell[] cells;
    private bool showCells = true;
    private float cellShowTimer = 0;

    void Start()
    {
        cells = GetComponentsInChildren<InventoryCell>();
    }

    // Update is called once per frame
    void Update()
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

            Debug.Log("cellShowTimer=" + cellShowTimer);
        }
    }

    private void ShowRandomCell()
    {

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
