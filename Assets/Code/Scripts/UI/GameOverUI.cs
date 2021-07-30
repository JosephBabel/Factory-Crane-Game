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

        // Add Button hover sfx
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((eventData) => { AudioManager.instance.PlayClip("Menu_Select"); });
        mainMenuTrigger.triggers.Add(entry);

        // Add Button click sfx
        EventTrigger.Entry pointerClickEntry = new EventTrigger.Entry();
        pointerClickEntry.eventID = EventTriggerType.PointerClick;
        pointerClickEntry.callback.AddListener((eventData) => { AudioManager.instance.PlayClip("Menu_Press"); });
        mainMenuTrigger.triggers.Add(pointerClickEntry);
    }
}
