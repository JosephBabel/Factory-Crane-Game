using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenuUI : UI
{
    public Button easyButton;
    public Button mediumButton;
    public Button hardButton;
    public Button quitButton;

    public Toggle muteMusicToggle;
    public Toggle muteSFXToggle;

    private EventTrigger easyTrigger;
    private EventTrigger mediumTrigger;
    private EventTrigger hardTrigger;
    private EventTrigger quitTrigger;

    protected override void Awake()
    {
        easyTrigger = easyButton.GetComponent<EventTrigger>();
        mediumTrigger = mediumButton.GetComponent<EventTrigger>();
        hardTrigger = hardButton.GetComponent<EventTrigger>();
        quitTrigger = quitButton.GetComponent<EventTrigger>();
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
        quitButton.onClick.AddListener(GameManager.instance.QuitGame);

        // Add Button sfx triggers
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((eventData) => { AudioManager.instance.PlayClip("Menu_Select"); });
        easyTrigger.triggers.Add(entry);
        mediumTrigger.triggers.Add(entry);
        hardTrigger.triggers.Add(entry);
        quitTrigger.triggers.Add(entry);

        // Add Toggle listeners
        muteMusicToggle.onValueChanged.AddListener(delegate { AudioManager.instance.ToggleMusic(muteMusicToggle.isOn); });
        muteSFXToggle.onValueChanged.AddListener(delegate { AudioManager.instance.ToggleSFX(muteSFXToggle.isOn); });

        if (AudioManager.instance.isMusicMuted)
            muteMusicToggle.isOn = true;

        if (AudioManager.instance.isSFXMuted)
            muteSFXToggle.isOn = true;
    }
}
