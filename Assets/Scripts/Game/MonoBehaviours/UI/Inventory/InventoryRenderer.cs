using System.Linq;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class InventoryRenderer : Singleton<InventoryRenderer>
{

    private bool _initialized = false;

    private InventoryCell[] _allCells, _inventoryCells, _fittingCells;
    private Transform _textContainer;
    private bool _showCells = false;
    private float _cellShowTimer = 0;

    void Start()
    {
        VerifyInitialize();
        Hide();
    }

    void VerifyInitialize()
    {
        if (_initialized)
            return;

        _textContainer = transform.Find("text");

        InitCells();
        _initialized = true;
    }

    void InitCells()
    {
        _allCells = GetComponentsInChildren<InventoryCell>();

        var invCellsTfm = transform.Find("inventoryCells");
        _inventoryCells = invCellsTfm?.GetComponentsInChildren<InventoryCell>() ?? new InventoryCell[0];
        var fittingCellsTfm = transform.Find("fittingCells");
        _fittingCells = fittingCellsTfm?.GetComponentsInChildren<InventoryCell>() ?? new InventoryCell[0];
    }

    void Update()
    {
        DoRender();
    }

    void DoRender()
    {
        if (_showCells)
        {
            if (_cellShowTimer > 0)
                _cellShowTimer -= Time.deltaTime * 100;
            else
            {
                var _inventory = InventoryManager.SelectedInventory;
                var hiddenCells = _allCells.Where(c => !c.GetIsVisible());

                // TODO only show the number of cells for the max items e.g. if 5 max items, show only 5 cells
                var inventoryCellsToUse = _inventoryCells.Select(c => c.Id < _inventory.MaxItems);
                var fittingCellsToUse = _fittingCells.Select(c => c.Id < _inventory.MaxFittings);

                if (hiddenCells.Count() > 1)
                {
                    var randomCell = hiddenCells.ToArray()[Random.Range(0, hiddenCells.Count() - 1)];
                    randomCell.Show();
                    _cellShowTimer = 2;
                }
                else
                    _showCells = false;
            }
        }
    }

    public static void Show()
    {
        Instance._textContainer?.gameObject.SetActive(true);
        Instance._showCells = true;
    }

    public static void Hide()
    {
        foreach (InventoryCell cell in Instance._allCells)
            cell.Hide();

        Instance._textContainer?.gameObject.SetActive(false);
        Instance._showCells = false;
    }

    public static void UpdateUI()
    {
        Instance.VerifyInitialize();

        var _inventory = InventoryManager.SelectedInventory.Required();

        for (int i = 0; i < Instance._allCells.Count(); i++)
        {
            var cell = Instance._allCells[i].Required();
            cell.SetItem(null);
        }

        var _inventoryCells = Instance._inventoryCells.Required();

        for (int i = 0; i < _inventory.Items.Count(); i++)
        {
            var cell = _inventoryCells.FirstOrDefault(c => c.Id == i).Required();

            if (cell == null)
                throw new NullReferenceException("No cell found for id " + i);

            var item = _inventory.Items.ElementAt(i);
            cell.SetItem(item);
        }

        var _fittingCells = Instance._fittingCells;

        for (int i = 0; i < _inventory.Fittings.Count(); i++)
        {
            var cell = _fittingCells.FirstOrDefault(c => c.Id == i);

            if (cell == null)
                throw new NullReferenceException("No cell found for id " + i);

            var item = _inventory.Fittings.ElementAt(i);
            cell.SetItem(item);
        }


    }
}
