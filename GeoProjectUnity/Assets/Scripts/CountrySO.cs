using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Country Name", fileName = "New Country")]
public class CountrySO : ScriptableObject
{
    [SerializeField] string countryName = "Empty";
    [TextArea(2, 6)] 
    [SerializeField] string question = "Pick a country that starts with";
    //Some Countries have their official flag the same as another, eg Guadeloupe has France's flag as its official flag
    [SerializeField] CountrySO[] altCountries;
    [SerializeField] string[] answers = new string[4];
    [SerializeField] int correctAnswerIndex;
    [SerializeField] Texture2D flag;

    public string GetCountry()
    {
        return countryName;
    }

    public string GetQuestion()
    {
        return question;
    }

    public CountrySO AltCountries(int _index)
    {
        return altCountries[_index];
    }

    public string GetAnswer(int _index)
    {
        return answers[_index];
    }

    public void SetAnswer(int _index, string _answer)
    {
        answers[_index] = _answer;
    }
}
