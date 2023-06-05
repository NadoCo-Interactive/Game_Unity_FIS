using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
  private RectTransform rectHUDFade;
  private RectTransform rectEquipped;
  private RectTransform rectInventory;

  private bool inventoryIsVisible = false;

  // Start is called before the first frame update
  void Start()
  {
    rectHUDFade = transform.Find("HUDFade")?.GetComponent<RectTransform>();
    rectEquipped = transform.Find("Equipped")?.GetComponent<RectTransform>();
    rectInventory = transform.Find("Inventory")?.GetComponent<RectTransform>();
  }

  // Update is called once per frame
  void Update()
  {
    if (Input.GetKeyDown(KeyCode.Tab))
      inventoryIsVisible = !inventoryIsVisible;

    if (inventoryIsVisible)
    {
      rectHUDFade.gameObject.SetActive(true);
      rectInventory.gameObject.SetActive(true);
      rectEquipped.gameObject.SetActive(false);
    }
    else
    {
      rectHUDFade.gameObject.SetActive(false);
      rectInventory.gameObject.SetActive(false);
      rectEquipped.gameObject.SetActive(true);
    }
  }
}
