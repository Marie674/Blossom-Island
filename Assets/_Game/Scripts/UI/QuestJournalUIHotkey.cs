// Copyright (c) Pixel Crushers. All rights reserved.

using UnityEngine;
using PixelCrushers.QuestMachine;
using PixelCrushers.DialogueSystem;

public class QuestJournalUIHotkey : MonoBehaviour
{

    [Tooltip("Toggle the quest log window when this key is pressed.")]
    public KeyCode key = KeyCode.J;

    [Tooltip("Toggle the quest log window when this input button is presed.")]
    public string buttonName = string.Empty;

    [Tooltip("Use this quest log window. If unassigned, will automatically find quest log window in scene.")]
    public UnityUIQuestJournalUI questLogWindow;

    public UnityUIQuestJournalUI runtimeQuestLogWindow
    {
        get
        {
            if (questLogWindow == null) questLogWindow = FindObjectOfType<UnityUIQuestJournalUI>();
            return questLogWindow;
        }
    }

    void Awake()
    {
        if (questLogWindow == null) questLogWindow = FindObjectOfType<UnityUIQuestJournalUI>();
    }

    void Update()
    {
        if (runtimeQuestLogWindow == null) return;
        if (DialogueManager.IsDialogueSystemInputDisabled()) return;
        if (Input.GetKeyDown(key) || (!string.IsNullOrEmpty(buttonName) && DialogueManager.getInputButtonDown(buttonName)))
        {
            if (runtimeQuestLogWindow.isVisible) runtimeQuestLogWindow.Hide(); else runtimeQuestLogWindow.Show(GameManager.Instance.Player.GetComponent<QuestJournal>());
        }
    }

}
