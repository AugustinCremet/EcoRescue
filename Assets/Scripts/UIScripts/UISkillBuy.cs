using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISkillBuy : MonoBehaviour
{
    private DisplayTrainer _displayTrainer;

    [SerializeField] private SkillObject _skill;
    private void Start()
    {
        _displayTrainer = FindObjectOfType(typeof(DisplayTrainer)) as DisplayTrainer;
    }

    public void OnClick()
    {
        _displayTrainer.SaveBuyRef(_skill, gameObject);
        
        string skill = _skill.ToString().ToLower();
        if (skill.IndexOf(" (") != -1)
            skill = skill.Remove(skill.IndexOf(" ("));
        FindObjectOfType<SpeechManager>().StopAllSkills();
        FindObjectOfType<SpeechManager>().PlaySpeech(skill + "description");
    }
}
