using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject InventoryMenu;
    [SerializeField] private TextMeshProUGUI ItemName;
    [SerializeField] private TextMeshProUGUI ItemDescription;
    [SerializeField] private List<Item> items;
    [SerializeField] private InventoryPreviewer previewer;
    [SerializeField] private Button UseButton;

    public static InventoryManager Instance { get; private set; }
    public List<Inventory> inventory;

    private int currentIndex = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            inventory = new List<Inventory>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UseButton.onClick.AddListener(OnUseButtonClicked);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            InventoryMenu.SetActive(!InventoryMenu.activeSelf);

            if (InventoryMenu.activeSelf)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                if (EventSystem.current != null)
                {
                    EventSystem.current.SetSelectedGameObject(null);
                }

                if (items.Count > 0)
                {
                    currentIndex = 0;
                    ShowCurrentItem();
                }
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                if (EventSystem.current != null)
                {
                    EventSystem.current.SetSelectedGameObject(null);
                }
            }
        }

        if (InventoryMenu.activeSelf && items.Count > 1)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");

            if (scroll > 0f)
            {
                NextItem();
            }
            else if (scroll < 0f)
            {
                PreviousItem();
            }
        }
    }

    private void ShowCurrentItem()
    {
        if (items.Count == 0 || currentIndex < 0 || currentIndex >= items.Count)
            return;

        if (items[currentIndex].itemType.Equals(ItemType.Other))
        {
            UseButton.gameObject.SetActive(false);
        }
        else
        {
            UseButton.gameObject.SetActive(true);
        }

        Item currentItem = items[currentIndex];

        ItemName.text = currentItem.itemName;
        ItemDescription.text = currentItem.itemDescription;
        previewer.ShowItem(currentItem.itemPrefab);
    }

    private void hideCurrentItem()
    {
        ItemName.text = "";
        ItemDescription.text = "";
        previewer.HideItem();
    }

    private void NextItem()
    {
        if (items.Count == 0) return;

        currentIndex = (currentIndex + 1) % items.Count;
        ShowCurrentItem();
    }

    private void PreviousItem()
    {
        if (items.Count == 0) return;

        currentIndex = (currentIndex - 1 + items.Count) % items.Count;
        ShowCurrentItem();
    }

    private void OnUseButtonClicked()
    {
        if (items.Count == 0 || currentIndex < 0 || currentIndex >= items.Count)
            return;

        if (items[currentIndex].itemType.Equals(ItemType.RecoverSanity))
        {
            SanityManager sanityManager = FindObjectOfType<SanityManager>();
            sanityManager.RecoverSanity(items[currentIndex].Values);

            items.RemoveAt(currentIndex);
            hideCurrentItem();

            if (currentIndex >= items.Count)
                currentIndex = items.Count - 1;

            ShowCurrentItem();
        }
    }

    public void AddItem(Item item)
    {
        if (item == null || items.Contains(item))
        {
            return;
        }

        inventory.Add(new Inventory(item));
        items.Add(item);
    }
}