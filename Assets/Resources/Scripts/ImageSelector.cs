using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class ImageSelector : MonoBehaviour
{

    public TMP_Dropdown Dropdown;
    public Image Image;



    //An array of images names that match their respected file names in Resources/Images
    private string[] Imagenames = { "Arsenal", "AstonVilla", "Bournemouth", "Brentford", "Brighton", "Burnley", "Chelsea", "CrystalPalace", "Everton", "Fulham", "Leeds", "Liverpool", "ManCity", "ManUtd", "Newcastle",
    "NottForest", "Sunderland", "Tottenham", "WestHam", "Wolves"};

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        //removes any existing dropdown options
        Dropdown.ClearOptions();

        // adds the team names from the previous array and puts it in the dropdown list as a selectable option
        Dropdown.AddOptions(new System.Collections.Generic.List<string>(Imagenames));


        // when the user selects an option from the dropdown menu it calls the function DropDownValueChanged
        Dropdown.onValueChanged.AddListener(DropDownValueChanged);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // this function is called whenever the dropdown value changes
    void DropDownValueChanged(int index)
    {
        string SelectedImage = Imagenames[index];

        //loads the image from the image folder like Newcastle.png
        Sprite NewSprite = Resources.Load<Sprite>("Images/" + SelectedImage);


        // if the file exists display it otherwise warn the console so the user knows
        if (NewSprite != null)
        {
            Image.sprite = NewSprite;
        }
        else
        {
            Debug.LogWarning("Image not found: " + SelectedImage);
        }
    }
}
