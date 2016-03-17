using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RadialProgressBar : MonoBehaviour
{
    public Image loadingBar;
    public Text textIndicator;
    private float currentAmount;
    private float finalAmount;
    public float speed;

    private bool hasFinished = false;
    private ScoreScreenHandler scoreHandler;
    private string rank;

    private const float GOLD_RANK = 90f;
    private const float SILVER_RANK = 70f;
    private const float BRONZE_RANK = 50f;
    private bool hasWon;

    void Start()
    {
        finalAmount = (float)LEVEL_DATA.HIT_TARGETS / (float)LEVEL_DATA.TOTAL_TARGETS * 100f;
        textIndicator.text = currentAmount.ToString("F1") + "%";
        scoreHandler = FindObjectOfType<ScoreScreenHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasFinished)
        {

            if (currentAmount < finalAmount)
            {
                currentAmount += speed * Time.deltaTime;
                currentAmount = Mathf.Clamp(currentAmount, 0f, finalAmount);
                textIndicator.text = currentAmount.ToString("F1") + "%";

                if (currentAmount >= GOLD_RANK)
                    SetColor(COLORS.GOLD);
                else if (currentAmount >= SILVER_RANK)
                    SetColor(COLORS.SILVER);
                else if (currentAmount >= BRONZE_RANK)
                    SetColor(COLORS.BRONZE);
                else
                    SetColor(Color.red);
            }

            loadingBar.fillAmount = currentAmount / 100f;

            if (currentAmount == finalAmount)
            {
                hasFinished = true;

                if (currentAmount >= GOLD_RANK)
                {
                    rank = "RANK:\nGOLD!!!";
                    hasWon = true;
                }

                else if (currentAmount >= SILVER_RANK)
                {
                    rank = "RANK:\nSILVER!";
                    hasWon = true;
                }
                else if (currentAmount >= BRONZE_RANK)
                {
                    rank = "RANK:\nBRONZE!";
                    hasWon = true;
                }
                else
                {
                    rank = "RANK:\nDUMB FUCK!";
                    hasWon = false;
                }

                scoreHandler.SetWin(hasWon);
                StartCoroutine(scoreHandler.ShowRank(rank, textIndicator.color, 0.4f));
            }
        }
    }

    private void SetColor(Color c)
    {
        loadingBar.color = c;
        textIndicator.color = c;
    }
}
