using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeUI : MonoBehaviour
{
    public TextMeshProUGUI DayText;
    public TextMeshProUGUI HourText;
    public Image FrameImage;

    public Sprite SpringSprite;
    public Sprite SummerSprite;
    public Sprite FallSprite;
    public Sprite WinterSprite;

    // Update is called once per frame
    void OnEnable()
    {
        TimeManager.OnMinuteChanged += UpdateUI;
        TimeManager.OnMonthChanged += UpdateFrame;
    }

    void OnDisable()
    {
        TimeManager.OnMinuteChanged -= UpdateUI;
        TimeManager.OnMonthChanged -= UpdateFrame;
    }

    void UpdateUI()
    {
        DayText.text = TimeManager.Instance.CurrentWeekDayName.ToString() + ", " + TimeManager.Instance.CurrentMonth.Name.ToString() + " " + ((int)(TimeManager.Instance.CurrentDay)).ToString();

        // Crappy way!
        int currentHour = TimeManager.Instance.CurrentHour;

        if (currentHour > 12)
        {
            currentHour -= 12;
        }
        if (currentHour == 0)
        {
            currentHour = 12;
        }

        HourText.text = System.String.Format("{0:D1}:{1:D2}", currentHour, TimeManager.Instance.CurrentMinute);
        HourText.text += " " + TimeManager.Instance.getHourAMPM();
    }

    void UpdateFrame(Month pCurrentMonth)
    {
        if (FrameImage != null)
        {
            switch (TimeManager.Instance.CurrentMonth.Name)
            {
                case TimeManager.MonthNames.Spring:
                    FrameImage.sprite = SpringSprite;
                    break;
                case TimeManager.MonthNames.Summer:
                    FrameImage.sprite = SummerSprite;
                    break;
                case TimeManager.MonthNames.Fall:
                    FrameImage.sprite = FallSprite;
                    break;
                case TimeManager.MonthNames.Winter:
                    FrameImage.sprite = WinterSprite;
                    break;
                default:
                    break;
            }
        }
    }
}
