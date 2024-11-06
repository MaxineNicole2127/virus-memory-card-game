using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Virus
{
    public int id;
    public string name;
    public string description;
    public int minFatality;
    public int maxFatality;
    public string originCountry;
    public Sprite frontCard;

    public Virus(int id, string name, string description, string originCountry, Sprite frontCard, int minFatality = 0, int maxFatality = 0)
    {
        this.SetVirus(id, name, description, originCountry, frontCard, minFatality, maxFatality);
    }

    public void SetVirus(int id, string name, string description, string originCountry, Sprite frontCard, int minFatality = 0, int maxFatality = 0)
    {
        this.id = id;
        this.name = name;
        this.description = description;
        this.minFatality = minFatality;
        this.maxFatality = maxFatality;
        this.originCountry = originCountry;
        this.frontCard = frontCard;
    }


}


public class InstantiateCardScript : MonoBehaviour
{
    [SerializeField] Virus[] viruses = new Virus[10]; // virus details
    [SerializeField] Sprite[] virus_card = new Sprite[10]; // front card images of the virus
    [SerializeField] private int noOfSelectedCards;
    [SerializeField] private int rows;
    [SerializeField] private int cols;
    [SerializeField] private FlipCardScript mainCard;

    // positioning spaces
    [SerializeField] private float XSpace = 4f;
    [SerializeField] private float YSpace = -5f;

    private Virus[] selectedVirus;

    

    void Start()
    {
        AddViruses();
        selectedVirus = SelectViruses(noOfSelectedCards);
        int[,] cardIndices = CardIndices();
        PositionCards(cardIndices, selectedVirus);
    }

    /* Functions for selecting viruses */
    private void AddViruses() // supplies array with all the viruses in the database
    {
        viruses[0].SetVirus(
            0,
            "Marburg Virus",
            "a hemorrhagic fever virus transmitted to people from fruit bats and spreads among humans through human-to-human transmission",
            "Germany",
            virus_card[0],
            24, 88
        );
        viruses[1].SetVirus(
            1,
            "Ebola Virus",
            "a rare but severe illness in humans that enters the body through cuts in the skin or when touching one’s eyes, nose or mouth",
            "South Sudan",
            virus_card[1],
            25, 90
        );
        viruses[2].SetVirus(
            2,
            "Hantavirus",
            "a severe and potentially deadly disease that affects the lungs. Symptoms of HPS usually start to show 1 to 8 weeks after contact with an infected rodent.",
            "Korea",
            virus_card[2],
            5, 15
        );
        viruses[3].SetVirus(
            3,
            "Bird flu Virus",
            "an infectious disease that spreads between both wild and domesticated birds and very rarely spreads from birds to humans.",
            "Scotland",
            virus_card[3],
            45, 50
        );
        viruses[4].SetVirus(
            4,
            "Lassa Virus",
            "an acute viral haemorrhagic illness that spreads through exposure to food or household items contaminated with urine or faeces of infected Mastomys rats.",
            "Nigeria",
            virus_card[4],
            1, 15
        );
        viruses[5].SetVirus(
            5,
            "Junin Virus",
            "a causative agent of Argentine Hemorrhagic Fever (AHF) that is transmitted to humans through inhalation of infected rodent secretions and excretions.",
            "Argentia",
            virus_card[5],
            15, 30
        );
        viruses[6].SetVirus(
            6,
            "Crimea-Congo fever",
            "a widespread disease caused by a tick-borne virus (Nairovirus) .",
            "Congo",
            virus_card[6],
            10, 40
        );
        viruses[7].SetVirus(
            7,
            "Machupo Virus",
            "An Arenavirus endemic to Bolivia that causes the disease commonly called Bolivian hemorrhagic fever.",
            "Bolivia",
            virus_card[7],
            25, 35
        );
        viruses[8].SetVirus(
            8,
            "Kyasanur Forest Virus",
            "is a viral disease that spreads to people primarily through tick bites or contact with infected animals. ",
            "India",
            virus_card[8],
            3, 15
        );
        viruses[9].SetVirus(
            9,
            "Dengue Fever",
            "a viral infection that spreads from mosquitoes to people, more common in tropical and subtropical climates",
            "Japan",
            virus_card[9],
            10, 20
        );
    }
    private int[] Shuffle(int[] arr, int cutLength) // returns a shuffled integer array cut into a certain size
    {
        int[] newArr = arr.Clone() as int[];
        int[] cutArray = new int[cutLength]; // the array to be returned

        for (int i = 0; i < newArr.Length; i++) // shuffles array
        {
            int temp = newArr[i];
            int j = Random.Range(0, newArr.Length);
            newArr[i] = newArr[j];
            newArr[j] = temp;
        }

        if (newArr.Length > cutLength)
        {
            for (int i = 0; i < cutLength; i++) // cuts array
            {
                cutArray[i] = newArr[i];
            }
        }
        else
        {
            cutArray = newArr.Clone() as int[];
        }

        return cutArray;
    }

    private Virus[] SelectViruses(int cutLength) // returns the Virus array of the selected cards for the game
    {
        int[] virusIndices = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        int[] selectedVirusIndices = Shuffle(virusIndices, cutLength);

        // create an array of selected viruses
        Virus[] selectedViruses = new Virus[cutLength];
        for (int i = 0; i < selectedVirusIndices.Length; i++)
        {
            selectedViruses[i] = viruses[selectedVirusIndices[i]];
        }

        return selectedViruses;
    }


    /* Functions for instantiating cards on canvas */
    private int[,] CardIndices() // returns shuffled indices of the virus, which will determine the card displayed
    {
        //int[] indices = new int[noOfSelectedCards * 2
        int[] temp1 = Enumerable.Range(0, noOfSelectedCards).ToArray();
        int[] temp2 = Enumerable.Range(0, noOfSelectedCards).ToArray();

        int[] indices = temp1.Concat(temp2).ToArray();
        indices = Shuffle(indices, indices.Length);
        int[,] indices2D = new int[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                indices2D[i, j] = indices[i * cols + j];
            }
        }

        return indices2D;
    }

    private void PositionCards(int[,] cardIndices, Virus[] selectedVirus)
    {
        Vector3 startPosition = mainCard.transform.position;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                FlipCardScript gameCard;
                if (i == 0 && j == 0)
                {
                    gameCard = mainCard;
                }
                else
                {
                    gameCard = Instantiate(mainCard) as FlipCardScript;
                }

                int pos = cardIndices[i, j];
                Virus virus = selectedVirus[pos];
                gameCard.ChangeImage(virus.id, virus.frontCard);

                float positionX = (XSpace * j) + startPosition.x;
                float positionY = (YSpace * i) + startPosition.y;

                gameCard.transform.position = new Vector3(positionX, positionY, startPosition.z);
            }
        }
    }


    
}
