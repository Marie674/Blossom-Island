using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;
using System.Linq;
using PixelCrushers;
public class CharacterCreationWindow : MonoBehaviour
{

    PlayerCharacter.CharacterDirection CurrentDirection = PlayerCharacter.CharacterDirection.Down;

    public Image BodyImage;
    public TextMeshProUGUI BodyText;
    int BodyIndex = 0;
    Texture2D[] BodySprites;

    public Image EyesImage;
    public TextMeshProUGUI EyesText;
    int EyesIndex = 0;
    Texture2D[] EyesSprites;

    public Image HairImage;
    public TextMeshProUGUI HairText;
    int HairIndex = 0;
    Texture2D[] HairSprites;

    public Image TopImage;
    public TextMeshProUGUI TopText;
    int TopIndex = 0;
    Texture2D[] TopSprites;

    public Image BottomImage;
    public TextMeshProUGUI BottomText;
    int BottomIndex = 0;
    Texture2D[] BottomSprites;

    public Image ShoesImage;
    public TextMeshProUGUI ShoesText;
    int ShoesIndex = 0;
    Texture2D[] ShoesSprites;

    void Start()
    {
        BodySprites = Resources.LoadAll<Texture2D>("Character Art/Bodies");

        EyesSprites = Resources.LoadAll<Texture2D>("Character Art/Eyes");

        HairSprites = Resources.LoadAll<Texture2D>("Character Art/Hairstyles");

        TopSprites = Resources.LoadAll<Texture2D>("Character Art/Tops");

        BottomSprites = Resources.LoadAll<Texture2D>("Character Art/Bottoms");

        ShoesSprites = Resources.LoadAll<Texture2D>("Character Art/Shoes");

        SetAll();
    }


    public void Create()
    {
        FindObjectOfType<PlayerCharacter>().GetComponent<CharacterAppearance>().ChangeAll(BodyIndex, EyesIndex, HairIndex, TopIndex, BottomIndex, ShoesIndex);
        GameManager.Instance.Player.gameObject.SetActive(true);
    }
    void SetAll()
    {
        SetBody(BodyIndex);
        SetEyes(EyesIndex);
        SetHair(HairIndex);
        SetTop(TopIndex);
        SetBottom(BottomIndex);
        SetShoes(ShoesIndex);
    }
    public void CycleBody(int i)
    {
        BodyIndex = Mathf.Clamp(BodyIndex + i, 0, BodySprites.Length - 1);
        SetBody(BodyIndex);

    }
    public void SetBody(int pIndex)
    {

        BodyText.text = "Body " + (pIndex + 1).ToString();

        Texture2D currentTexture = BodySprites[pIndex];
        Sprite[] sprites = Resources.LoadAll<Sprite>("Character Art/Bodies/Body" + (pIndex + 1) + "/" + currentTexture.name);
        Sprite sprite;

        switch (CurrentDirection)
        {
            case PlayerCharacter.CharacterDirection.Down:
                sprite = sprites.Single(s => s.name == currentTexture.name + "Idle Down_0");
                BodyImage.sprite = sprite;
                break;
            case PlayerCharacter.CharacterDirection.Left:
                sprite = sprites.Single(s => s.name == currentTexture.name + "Idle Left_0");

                BodyImage.sprite = sprite;
                break;
            case PlayerCharacter.CharacterDirection.Right:
                sprite = sprites.Single(s => s.name == currentTexture.name + "Idle Right_0");
                BodyImage.sprite = sprite;
                break;
            case PlayerCharacter.CharacterDirection.Up:
                sprite = sprites.Single(s => s.name == currentTexture.name + "Idle Up_0");
                BodyImage.sprite = sprite;
                break;
            default:
                break;

        }
    }

    public void CycleEyes(int i)
    {
        EyesIndex = Mathf.Clamp(EyesIndex + i, 0, EyesSprites.Length - 1);
        SetEyes(EyesIndex);

    }
    public void SetEyes(int pIndex)
    {

        Texture2D currentTexture = EyesSprites[pIndex];
        Sprite[] sprites = Resources.LoadAll<Sprite>("Character Art/Eyes/Eyes" + (pIndex + 1) + "/" + currentTexture.name);
        Sprite sprite;

        EyesText.text = "Eyes " + (pIndex + 1).ToString();
        switch (CurrentDirection)
        {
            case PlayerCharacter.CharacterDirection.Down:
                sprite = sprites.Single(s => s.name == currentTexture.name + "Idle Down_0");
                EyesImage.sprite = sprite;
                break;
            case PlayerCharacter.CharacterDirection.Left:
                sprite = sprites.Single(s => s.name == currentTexture.name + "Idle Left_0");
                EyesImage.sprite = sprite;
                break;
            case PlayerCharacter.CharacterDirection.Right:
                sprite = sprites.Single(s => s.name == currentTexture.name + "Idle Right_0");

                EyesImage.sprite = sprite;
                break;
            case PlayerCharacter.CharacterDirection.Up:
                sprite = sprites.Single(s => s.name == currentTexture.name + "Idle Up_0");

                EyesImage.sprite = sprite;
                break;
            default:
                break;

        }
    }

    public void CycleHair(int i)
    {
        HairIndex = Mathf.Clamp(HairIndex + i, 0, HairSprites.Length - 1);
        SetHair(HairIndex);

    }
    public void SetHair(int pIndex)
    {

        Texture2D currentTexture = HairSprites[pIndex];
        Sprite[] sprites = Resources.LoadAll<Sprite>("Character Art/Hairstyles/Hair" + (pIndex + 1) + "/" + currentTexture.name);
        Sprite sprite;

        HairText.text = "Hair " + (pIndex + 1).ToString();
        switch (CurrentDirection)
        {
            case PlayerCharacter.CharacterDirection.Down:
                sprite = sprites.Single(s => s.name == currentTexture.name + "Idle Down_0");

                HairImage.sprite = sprite;
                break;
            case PlayerCharacter.CharacterDirection.Left:
                sprite = sprites.Single(s => s.name == currentTexture.name + "Idle Left_0");
                HairImage.sprite = sprite;
                break;
            case PlayerCharacter.CharacterDirection.Right:
                sprite = sprites.Single(s => s.name == currentTexture.name + "Idle Right_0");

                HairImage.sprite = sprite;
                break;
            case PlayerCharacter.CharacterDirection.Up:
                sprite = sprites.Single(s => s.name == currentTexture.name + "Idle Up_0");

                HairImage.sprite = sprite;
                break;
            default:
                break;

        }
    }

    public void CycleTop(int i)
    {
        TopIndex = Mathf.Clamp(TopIndex + i, 0, TopSprites.Length - 1);
        SetTop(TopIndex);

    }
    public void SetTop(int pIndex)
    {
        Texture2D currentTexture = TopSprites[pIndex];
        Sprite[] sprites = Resources.LoadAll<Sprite>("Character Art/Tops/Top" + (pIndex + 1) + "/" + currentTexture.name);
        Sprite sprite;

        TopText.text = "Top " + (pIndex + 1).ToString();
        switch (CurrentDirection)
        {
            case PlayerCharacter.CharacterDirection.Down:
                sprite = sprites.Single(s => s.name == currentTexture.name + "Idle Down_0");
                TopImage.sprite = sprite;
                break;
            case PlayerCharacter.CharacterDirection.Left:
                sprite = sprites.Single(s => s.name == currentTexture.name + "Idle Left_0");
                TopImage.sprite = sprite;
                break;
            case PlayerCharacter.CharacterDirection.Right:
                sprite = sprites.Single(s => s.name == currentTexture.name + "Idle Right_0");
                TopImage.sprite = sprite;
                break;
            case PlayerCharacter.CharacterDirection.Up:
                sprite = sprites.Single(s => s.name == currentTexture.name + "Idle Up_0");
                TopImage.sprite = sprite;
                break;
            default:
                break;

        }
    }

    public void CycleBottom(int i)
    {
        BottomIndex = Mathf.Clamp(BottomIndex + i, 0, BottomSprites.Length - 1);
        SetBottom(BottomIndex);

    }
    public void SetBottom(int pIndex)
    {
        Texture2D currentTexture = BottomSprites[pIndex];
        Sprite[] sprites = Resources.LoadAll<Sprite>("Character Art/Bottoms/Bottom" + (pIndex + 1) + "/" + currentTexture.name);
        Sprite sprite;

        BottomText.text = "Bottom " + (pIndex + 1).ToString();
        switch (CurrentDirection)
        {
            case PlayerCharacter.CharacterDirection.Down:
                sprite = sprites.Single(s => s.name == currentTexture.name + "Idle Down_0");

                BottomImage.sprite = sprite;
                break;
            case PlayerCharacter.CharacterDirection.Left:
                sprite = sprites.Single(s => s.name == currentTexture.name + "Idle Left_0");


                BottomImage.sprite = sprite;
                break;
            case PlayerCharacter.CharacterDirection.Right:
                sprite = sprites.Single(s => s.name == currentTexture.name + "Idle Right_0");


                BottomImage.sprite = sprite;
                break;
            case PlayerCharacter.CharacterDirection.Up:
                sprite = sprites.Single(s => s.name == currentTexture.name + "Idle Up_0");

                BottomImage.sprite = sprite;
                break;
            default:
                break;

        }
    }

    public void CycleShoes(int i)
    {
        ShoesIndex = Mathf.Clamp(ShoesIndex + i, 0, ShoesSprites.Length - 1);
        SetShoes(ShoesIndex);

    }
    public void SetShoes(int pIndex)
    {
        Texture2D currentTexture = ShoesSprites[pIndex];
        Sprite[] sprites = Resources.LoadAll<Sprite>("Character Art/Shoes/Shoes" + (pIndex + 1) + "/" + currentTexture.name);
        Sprite sprite;

        ShoesText.text = "Shoes " + (pIndex + 1).ToString();
        switch (CurrentDirection)
        {
            case PlayerCharacter.CharacterDirection.Down:
                sprite = sprites.Single(s => s.name == currentTexture.name + "Idle Down_0");
                ShoesImage.sprite = sprite;
                break;
            case PlayerCharacter.CharacterDirection.Left:
                sprite = sprites.Single(s => s.name == currentTexture.name + "Idle Left_0");
                ShoesImage.sprite = sprite;
                break;
            case PlayerCharacter.CharacterDirection.Right:
                sprite = sprites.Single(s => s.name == currentTexture.name + "Idle Right_0");
                ShoesImage.sprite = sprite;
                break;
            case PlayerCharacter.CharacterDirection.Up:
                sprite = sprites.Single(s => s.name == currentTexture.name + "Idle Up_0");
                ShoesImage.sprite = sprite;
                break;
            default:
                break;

        }
    }
    public void RotateLeft()
    {
        if (CurrentDirection == PlayerCharacter.CharacterDirection.Down)
        {
            CurrentDirection = PlayerCharacter.CharacterDirection.Left;
        }
        else if (CurrentDirection == PlayerCharacter.CharacterDirection.Left)
        {
            CurrentDirection = PlayerCharacter.CharacterDirection.Up;
        }
        else if (CurrentDirection == PlayerCharacter.CharacterDirection.Up)
        {
            CurrentDirection = PlayerCharacter.CharacterDirection.Right;
        }
        else if (CurrentDirection == PlayerCharacter.CharacterDirection.Right)
        {
            CurrentDirection = PlayerCharacter.CharacterDirection.Down;
        }
        SetAll();
    }
    public void RotateRight()
    {
        if (CurrentDirection == PlayerCharacter.CharacterDirection.Down)
        {
            CurrentDirection = PlayerCharacter.CharacterDirection.Right;
        }
        else if (CurrentDirection == PlayerCharacter.CharacterDirection.Right)
        {
            CurrentDirection = PlayerCharacter.CharacterDirection.Up;
        }
        else if (CurrentDirection == PlayerCharacter.CharacterDirection.Up)
        {
            CurrentDirection = PlayerCharacter.CharacterDirection.Left;
        }
        else if (CurrentDirection == PlayerCharacter.CharacterDirection.Left)
        {
            CurrentDirection = PlayerCharacter.CharacterDirection.Down;
        }
        SetAll();
    }

}
