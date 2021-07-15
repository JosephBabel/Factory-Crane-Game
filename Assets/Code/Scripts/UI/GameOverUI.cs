using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class GameOverUI : UI
{
    public Button mainMenuButton;
    public TextMeshProUGUI totalProfitText;
    public TextMeshProUGUI bombsExplodedText;
    public TextMeshProUGUI itemsDroppedText;
    public TextMeshProUGUI itemsLaunchedText;

    private EventTrigger mainMenuTrigger;

    protected override void Awake()
    {
        mainMenuTrigger = mainMenuButton.GetComponent<EventTrigger>();
        base.Awake();
    }

    protected override void AddButtonEvents()
    {
        mainMenuButton.onClick.AddListener(GameManager.instance.OpenMainMenu);
        mainMenuButton.onClick.AddListener(CloseUI);

        // Add Button sfx trigger
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((eventData) => { AudioManager.instance.PlayClip("Menu_Select"); });
        mainMenuTrigger.triggers.Add(entry);
    }
}
