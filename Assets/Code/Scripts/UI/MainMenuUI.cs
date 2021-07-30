using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenuUI : UI
{
    public Button easyButton;
    public Button mediumButton;
    public Button hardButton;

    public Toggle muteMusicToggle;
    public Toggle muteSFXToggle;

    private EventTrigger easyTrigger;
    private EventTrigger mediumTrigger;
    private EventTrigger hardTrigger;

    protected override void Awake()
    {
        easyTrigger = easyButton.GetComponent<EventTrigger>();
        mediumTrigger = mediumButton.GetComponent<EventTrigger>();
        hardTrigger = hardButton.GetComponent<EventTrigger>();
        base.Awake();
    }

    protected override void AddButtonEvents()
    {
        // Add Button onClick listeners
        easyButton.onClick.AddListener(GameManager.instance.SelectEasyMode);
        easyButton.onClick.AddListener(CloseUI);
        mediumButton.onClick.AddListener(GameManager.instance.SelectMediumMode);
        mediumButton.onClick.AddListener(CloseUI);
        hardButton.onClick.AddListener(GameManager.instance.SelectHardMode);
        hardButton.onClick.AddListener(CloseUI);

        // Add Button hover sfx
        EventTrigger.Entry pointerEnterEntry = new EventTrigger.Entry();
        pointerEnterEntry.eventID = EventTriggerType.PointerEnter;
        pointerEnterEntry.callback.AddListener((eventData) => { AudioManager.instance.PlayClip("Menu_Select"); });
        easyTrigger.triggers.Add(pointerEnterEntry);
        mediumTrigger.triggers.Add(pointerEnterEntry);
        hardTrigger.triggers.Add(pointerEnterEntry);

        // Add Button click sfx
        EventTrigger.Entry pointerClickEntry = new EventTrigger.Entry();
        pointerClickEntry.eventID = EventTriggerType.PointerClick;
        pointerClickEntry.callback.AddListener((eventData) => { AudioManager.instance.PlayClip("Menu_Press"); });
        easyTrigger.triggers.Add(pointerClickEntry);
        mediumTrigger.triggers.Add(pointerClickEntry);
        hardTrigger.triggers.Add(pointerClickEntry);

        // Add Toggle listeners
        muteMusicToggle.onValueChanged.AddListener(delegate { AudioManager.instance.ToggleMusic(muteMusicToggle.isOn); });
        muteSFXToggle.onValueChanged.AddListener(delegate { AudioManager.instance.ToggleSFX(muteSFXToggle.isOn); });

        if (AudioManager.instance.isMusicMuted)
            muteMusicToggle.isOn = true;

        if (AudioManager.instance.isSFXMuted)
            muteSFXToggle.isOn = true;
    }
}
