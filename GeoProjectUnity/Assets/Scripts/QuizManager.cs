using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuizManager : MonoBehaviour
{
    [SerializeField] GameObject[] AnswerButtons;
    [SerializeField] CountrySO[] Countries;
    [SerializeField] Image QuestionImage;
    [SerializeField] TMP_Text QuestionText;
    [SerializeField] TMP_Text CountryName;
    [SerializeField] TextMeshProUGUI Timer;
    [SerializeField] float WrongAnswerPenalty = 5f;
    [SerializeField] bool IsTimerRunning;

    private CountrySO currentCountry;
    private CountrySO nextCountry;
    private int currentRightAnswerIndex;
    private List<CountrySO> previousAnswers = new List<CountrySO>();
    private float timerValue;

    private void Awake()
    {
        DEBUG_EnableCountryNamesOnButtons(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        Timer.text = $"Timer: 00:00";
        previousAnswers.Clear();
        currentCountry = GetRandomCountrySO();
        DisplayCountryInfo(currentCountry);
    }

    private void Update()
    {
        DisplayTimer();
    }

    #region Quiz Tools

    private void DisplayCountryInfo(CountrySO _country)
    {
        previousAnswers.Add(_country);
        DisplayQuestionFlag(_country);
        DisplayQuestionText(_country);
        DisplayCountryName(_country);
        SetAnswerButtons(_country);
        SetAllButtonsInteractable(true);
    }

    /// <summary>
    /// Gets a random country scriptable object from Countries
    /// </summary>
    /// <returns></returns>
    private CountrySO GetRandomCountrySO()
    {
        int countryIndex = Random.Range(0, Countries.Length);
        return Countries[countryIndex];
    }

    /// <summary>
    /// Sets the current question to _country
    /// </summary>
    /// <param name="_country"></param>
    private void DisplayQuestionFlag(CountrySO _country)
    {
        QuestionImage.sprite = _country.GetFlag();
    }
    
    private void DisplayQuestionText(CountrySO _country)
    {
        QuestionText.text = _country.GetQuestion();
    }

    private void DisplayCountryName(CountrySO _country)
    {
        CountryName.text = _country.GetCountry();
    }

    public void OnAnswerSelected(int _index)
    {
        if (_index == currentRightAnswerIndex)
        {
            currentRightAnswerIndex = -1;
        
            DisplayCountryInfo(nextCountry);
        }
        else
        {
            AnswerButtons[_index].GetComponent<Button>().interactable = false;
            timerValue += WrongAnswerPenalty;
        }
    }

    private void SetAnswerButtons(CountrySO _country)
    {
        List<CountrySO> possibleAnswers = GetAnswerPool(_country);

        if (possibleAnswers.Count == 0)
        {
            QuestionText.text = "You win! Game Over";
            SetAllButtonsVisible(false);
            IsTimerRunning = false;
            return;
        }
        List<CountrySO> wrongAnswers = GetWrongAnswerPool(_country);
        int rightAnswer = Random.Range(0, possibleAnswers.Count);
        currentRightAnswerIndex = Random.Range(0, AnswerButtons.Count());
        nextCountry = possibleAnswers[rightAnswer];

        for (int i = 0; i < AnswerButtons.Count(); i++)
        {
            if (i == currentRightAnswerIndex)
            {
                SetAnswerButton(currentRightAnswerIndex, possibleAnswers[rightAnswer]);
            }
            else
            {
                SetAnswerButton(i, wrongAnswers[Random.Range(0, wrongAnswers.Count())]);
            }
        }
    }

    private void SetAllButtonsVisible(bool _value)
    {
        for (int i = 0; i < AnswerButtons.Count(); i++)
        {
            AnswerButtons[i].SetActive(_value);
        }
    }

    private List<CountrySO> GetAnswerPool(CountrySO _country)
    {
        List<CountrySO> answerPool = new List<CountrySO>();
        string countryString = _country.GetCountry();
        char endingCountryChar = countryString.ToUpper()[countryString.Length - 1];

        answerPool.AddRange(Countries.Where(cso => cso.GetCountry().StartsWith(endingCountryChar)));
        //remove countries already answered from the answer pool
        answerPool = answerPool.Where(cso => !previousAnswers.Contains(cso)).ToList();
        return answerPool;
    }

    private List<CountrySO> GetWrongAnswerPool(CountrySO _country)
    {
        List<CountrySO> answerPool = new List<CountrySO>();
        string countryString = _country.GetCountry();
        char endingCountryChar = countryString.ToUpper()[countryString.Length - 1];

        answerPool.AddRange(Countries.Where(cso => !cso.GetCountry().StartsWith(endingCountryChar)));

        //remove countries already answered from the answer pool
        answerPool = answerPool.Where(cso => !previousAnswers.Contains(cso)).ToList();
        return answerPool;
    }

    private void SetAnswerButton(int _index, CountrySO _country)
    {
        if (_index < 0 || _index >= Countries.Length) return;

        GameObject answerButton = AnswerButtons[_index];
        answerButton.GetComponent<Image>().sprite = _country.GetFlag();
        //Debug
        TextMeshProUGUI text = answerButton.GetComponentInChildren<TextMeshProUGUI>();
        //If the text is enabled
        if (text != null)
        {
            text.text = _country.GetCountry();
        }
    }

    private void SetAllButtonsInteractable(bool _isInteractive)
    {
        for (int i = 0; i < AnswerButtons.Length; i++)
        {
            AnswerButtons[i].GetComponent<Button>().interactable = _isInteractive;
        }
    }

    private void DisplayTimer()
    {
        if (IsTimerRunning)
        {
            timerValue += Time.deltaTime;
            float minutes = Mathf.FloorToInt(timerValue / 60);
            float seconds = Mathf.FloorToInt(timerValue % 60);
            string timeDisplatStr = string.Format("{0:00}:{1:00}", minutes, seconds);
            Timer.text = $"Timer: {timeDisplatStr}";
        }
    }

    private void DEBUG_EnableCountryNamesOnButtons(bool _isEnabled)
    {
        for (int i = 0; i < AnswerButtons.Length; i++)
        {
            TextMeshProUGUI text = AnswerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            text.gameObject.SetActive(_isEnabled);
        }
    }


    #endregion
}
