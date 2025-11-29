using UnityEngine;
using UnityEngine.UI;
using ST07.Player; // Inventory, ItemStack 쓸려고

public class InventoryUIController : MonoBehaviour
{
    [Header("Refs")]
    public Inventory inventory;        // 플레이어 Inventory
    public GameObject inventoryPanel;  // Panel_Inventory
    public Transform slotsParent;      // GridParent (GridLayoutGroup 달린 애)
    public GameObject slotPrefab;      // Slot 프리팹

    public Transform player;   // ✅ 인스펙터에서 Player 드래그해서 넣기

    [Header("Grid Size")]
    public int columns = 7;
    public int rows = 7;

    [Header("Weight UI")]
    public Image weightFill;   // ★ 추가: Filled Image 연결
    public Text weightText;

    // 인벤토리 열림/닫힘 이벤트
    public event System.Action OnInventoryOpened;   // 인벤토리가 열렸을 때
    public event System.Action OnInventoryClosed;   // 인벤토리가 닫혔을 때

    private InventorySlotUI[] slots;

    private void Awake()
    {
        // 7x7 슬롯 자동 생성
        CreateSlots();

        // 시작할 때 패널 꺼두기
        if (inventoryPanel != null)
            inventoryPanel.SetActive(false);
    }

    private void OnEnable()
    {
        if (inventory != null)
            inventory.OnInventoryChanged += RefreshUI;
    }

    private void OnDisable()
    {
        if (inventory != null)
            inventory.OnInventoryChanged -= RefreshUI;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (inventoryPanel == null) return;

            bool show = !inventoryPanel.activeSelf;
            inventoryPanel.SetActive(show);

            if (show)
            {
                RefreshUI();   // 열릴 때 한 번 갱신
                OnInventoryOpened?.Invoke();  // ✅ 인벤토리 열림 이벤트 발생
            }
            else
            {
                OnInventoryClosed?.Invoke();  // ✅ 인벤토리 닫힘 이벤트 발생
            }
        }
    }

    private void CreateSlots()
    {
        int total = columns * rows;
        slots = new InventorySlotUI[total];

        // 플레이어 Transform 찾기 (Player 태그 사용)
        Transform playerTransform = null;
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            playerTransform = playerObj.transform;

        for (int i = 0; i < total; i++)
        {
            GameObject slotObj = Instantiate(slotPrefab, slotsParent);
            InventorySlotUI ui = slotObj.GetComponent<InventorySlotUI>();

            // 인벤토리 & 플레이어 넘겨줌
            ui.Initialize(inventory, playerTransform);

            slots[i] = ui;
            ui.Clear(); // 시작은 빈칸
        }
    }

    private void RefreshUI()
    {
        if (inventory == null || slots == null) return;

        var list = inventory.items;

        // 인벤토리 내용 → 슬롯에 채우기
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < list.Count)
                slots[i].Set(list[i]);
            else
                slots[i].Clear();
        }

        // 무게 표시
        float cur = inventory.CurrentWeight;
        float max = inventory.weightLimitKg;

        // ★ 추가: 게이지 fillAmount
        if (weightFill != null)
        {
            float ratio = (max > 0f) ? cur / max : 0f;
            weightFill.fillAmount = Mathf.Clamp01(ratio);
        }

        //텍스트로 Kg 수치 표시
        if (weightText != null)
        {
            weightText.text = $"{cur:0.0} / {max:0.0} kg";
        }
    }
}