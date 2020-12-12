using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class House : MonoBehaviour
{
    [Tooltip("该房子应该采集什么资源？")]
    public CollectType collectType;
    [Tooltip("是否指挥NPC出去采集")]
    public bool isCollect;
    [Tooltip("NPC数量")]
    public int NPCNumber;
    [Tooltip("是否是采集小屋")]
    public bool isCollectHouse;



    public Queue<NPC> NPCQueue = new Queue<NPC>();
    private EnvironmentCreate environmentCreate;        // 记录离房子最近的资源区域
    private Transform birthPoint;                       // NPC出生点
    private int NPCDebt;                                // 记录需要取出多少NPC
    private ItemData warehouse;                         // 仓库，存放资源
    private bool isUpdateUI;                            // 是否刷新UI
    private PlayerInteraction playerInteraction;
    #region 菜单UI
    [HideInInspector]
    public Transform houseMenu;                        // 房子的操作菜单UI
    private Text headline;                              // 标题

    private Text NPCGetText;                            // NPC获取提示文本
    private InputField NPCGetInputField;                // NPC获取输入框
    private Button NPCGetButton;                        // NPC获取确定按钮

    private Text NPCAddText;                            // NPC添加提示文本
    private InputField NPCAddInputField;                // NPC添加输入框
    private Button NPCAddButton;                        // NPC添加确定按钮

    private Text collectText;                           // 资源采集数量
    private InputField collectInputField;               // 资源获取输入框
    private Button collectButton;                       // 资源获取确定按钮

    private Text debtorText;                            // 未入账人数
    private Button closeButton;                         // 关闭按钮
    private Toggle isCollectToggle;                     // 是否采集
    #endregion

    private void Start()
    {
        #region 房屋菜单组件获取
        houseMenu = GameObject.Find("Canvas").transform.Find("房屋菜单");
        headline = houseMenu.Find("标题").GetComponent<Text>();

        NPCGetText = houseMenu.Find("NPC人数获取").Find("人数").GetComponent<Text>();
        NPCGetInputField = houseMenu.Find("NPC人数获取").Find("InputField").GetComponent<InputField>();
        NPCGetButton = houseMenu.Find("NPC人数获取").Find("Button").GetComponent<Button>();

        NPCAddText = houseMenu.Find("NPC人数添加").Find("人数").GetComponent<Text>();
        NPCAddInputField = houseMenu.Find("NPC人数添加").Find("InputField").GetComponent<InputField>();
        NPCAddButton = houseMenu.Find("NPC人数添加").Find("Button").GetComponent<Button>();

        collectText = houseMenu.Find("资源获取").Find("资源数量").GetComponent<Text>();
        collectInputField = houseMenu.Find("资源获取").Find("InputField").GetComponent<InputField>();
        collectButton = houseMenu.Find("资源获取").Find("Button").GetComponent<Button>();

        debtorText = houseMenu.Find("未入账").GetComponent<Text>();
        closeButton = houseMenu.Find("关闭按钮").GetComponent<Button>();
        isCollectToggle = houseMenu.Find("是否采集").GetComponent<Toggle>();
        #endregion

        playerInteraction = PlayerInputManage.single.GetComponent<PlayerInteraction>();
        isCollect = true;
        birthPoint = transform.Find("NPC生成点");
        warehouse = new ItemData() { name = collectType.ToString(), number = 0 };

        StartCoroutine(Dispatch());
        // 生成NPC
        for (int i = 0; i < NPCNumber; i++)
        {
            NPC temp = Instantiate(ResourcePath.Single.NPCList[
                                Random.Range(0, ResourcePath.Single.NPCList.Count)]).GetComponent<NPC>();
            temp.gameObject.SetActive(false);
            NPCQueue.Enqueue(temp);
        }
    }
    private void Update()
    {
        if (isUpdateUI)
        {
            UpdateUI();
        }
    }
    /// <summary>
    /// 将资源添加到仓库里，如果资源类型不对会直接丢弃
    /// </summary>
    /// <param name="itemData">资源信息</param>
    /// <returns></returns>
    public bool AddCollect(ItemData itemData)
    {
        if (warehouse.name != itemData.name)
            return false;

        warehouse.number += itemData.number;
        return true;
    }
    private IEnumerator Dispatch()
    {
        yield return new WaitForSeconds(3f);
        float minDistance = float.MaxValue;
        // 寻找离房子最近的资源区域
        foreach (var x in EnvironmentCreate.environmentCreates)
        {
            if (minDistance > Vector3.Distance(transform.position, x.transform.position))
            {
                minDistance = Vector3.Distance(transform.position, x.transform.position);
                environmentCreate = x;
            }
        }
        while (true)
        {
            if (NPCQueue.Count > 0)
            {
                // 如果需要取出NPC就将NPC打入玩家的账户
                if (NPCDebt > 0)
                {
                    NPC npc = NPCQueue.Dequeue();
                    playerInteraction.NPCQueue.Enqueue(npc);
                    NPCDebt--;
                }
                // 否则就指挥NPC进行采集
                else if (isCollect && isCollectHouse)
                {
                    bool isTemp = true;         // 检查是否还有资源可以采集
                    switch (collectType)
                    {
                        case CollectType.石头:
                            isTemp = environmentCreate.stoneListPosition.Count > 0 ? true : false;
                            break;
                        case CollectType.木头:
                            isTemp = environmentCreate.woodListPosition.Count > 0 ? true : false;
                            break;
                        case CollectType.食物:
                            isTemp = environmentCreate.foodListPosition.Count > 0 ? true : false;
                            break;
                        case CollectType.回复药:
                            isTemp = environmentCreate.medicineListPosition.Count > 0 ? true : false;
                            break;
                        default:
                            break;
                    }
                    if(isTemp)
                    {
                        NPC npc = NPCQueue.Dequeue();
                        npc.transform.position = birthPoint.position;
                        npc.gameObject.SetActive(true);
                        npc.SetTask(collectType, environmentCreate, transform);
                    }
                }

                yield return new WaitForSeconds(3f);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
    public void OpenMenu()
    {
        PlayerInputManage.single.ForbidMove();
        PlayerInputManage.single.ForbidVMCamera();
        Cursor.lockState = CursorLockMode.None;
        isUpdateUI = true;

        houseMenu.gameObject.SetActive(true);
        headline.text = gameObject.name;

        NPCGetInputField.text = "";
        NPCGetButton.onClick.RemoveAllListeners();
        NPCGetButton.onClick.AddListener(NPCGet);

        NPCAddInputField.text = "";
        NPCAddButton.onClick.RemoveAllListeners();
        NPCAddButton.onClick.AddListener(NPCAdd);

        collectInputField.text = "";
        collectButton.onClick.RemoveAllListeners();
        collectButton.onClick.AddListener(GetCollect);

        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(CloseMenu);
        isCollectToggle.isOn = isCollect;
        isCollectToggle.onValueChanged.RemoveAllListeners();
        isCollectToggle.onValueChanged.AddListener((bool isOn) => isCollect = isOn);
    }
    /// <summary>
    /// 关闭菜单
    /// </summary>
    public void CloseMenu()
    {
        PlayerInputManage.single.MayMove();
        PlayerInputManage.single.MayVMCamera();
        Cursor.lockState = CursorLockMode.Locked;
        isUpdateUI = false;
        houseMenu.gameObject.SetActive(false);
    }

    private void UpdateUI()
    {
        NPCGetText.text = $"NPC空闲人数:{NPCQueue.Count}";
        NPCAddText.text = $"手中NPC人数:{playerInteraction.NPCQueue.Count}";
        collectText.text = $"{collectType}数量:{warehouse.number}";
        debtorText.text = $"为入账人数：{NPCDebt}";


    }
    /// <summary>
    /// 当有空闲NPC的时候就会往玩家账号里添加NPC
    /// </summary>
    private void NPCGet()
    {
        if (int.TryParse(NPCGetInputField.text, out int result))
        {
            NPCDebt += result;
        }
    }
    /// <summary>
    /// 将玩家手中的NPC添加进来
    /// </summary>
    private void NPCAdd()
    {
        if (int.TryParse(NPCAddInputField.text, out int result))
        {
            // 如果数量比玩家手中的NPC大就全部添加进来
            int count = result > playerInteraction.NPCQueue.Count ? playerInteraction.NPCQueue.Count : result;
            for (int i = 0; i < count; i++)
            {
                NPCQueue.Enqueue(playerInteraction.NPCQueue.Dequeue());
            }
        }
    }
    /// <summary>
    /// 从房子里拿资源，如果超过背包容量会丢弃一部分
    /// </summary>
    private void GetCollect()
    {
        if (int.TryParse(collectInputField.text, out int result))
        {

            if (warehouse.number < result)
            {
                playerInteraction.box.AddItem(new ItemData(warehouse.name, warehouse.number));
                warehouse.number = 0;
            }
            else
            {
                playerInteraction.box.AddItem(new ItemData(warehouse.name, result));
                warehouse.number -= result;
            }
        }
    }
}
