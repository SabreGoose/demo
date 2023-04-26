using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ExpBar : MonoBehaviour
{
    [SerializeField] private Sprite[] _updatingSprites;
    private Image _expImage;
    private float _experience;
    [SerializeField] private float _secondsBeforeSpriteChange;
    [SerializeField] private TextMeshProUGUI _expText;
    private int _nextLevel;

    private void Awake()
    {
        _expImage = GetComponent<Image>();
    }
    

    public void SetStartExp()
    {
        _nextLevel = GameManager.Instance.Level + 1;
        _experience = GameManager.Instance.Experience;
        int experience = (int) _experience;
        int experienceForNextLevel = (int) GameManager.Instance.ExperienceForLevel(_nextLevel);
        _expText.SetText(experience.ToString() + "/" + experienceForNextLevel.ToString());
        _expImage.fillAmount = _experience / experienceForNextLevel;
    }


    public IEnumerator UpdateBar()
    {
        //float expToSet = _experience + GameManager.Instance.CurrentScore;
        float expToSet = 0;
        int experienceForNextLevel = (int) GameManager.Instance.ExperienceForLevel(_nextLevel);
        float experienceForPrevLevel = GameManager.Instance.ExperienceForLevel(_nextLevel - 1);
        int experience;
        while(_experience < expToSet)
        {
            _experience += expToSet / 2 * Time.deltaTime;
            if (_experience > experienceForNextLevel)
            {
                _nextLevel++;
                experienceForNextLevel = (int) GameManager.Instance.ExperienceForLevel(_nextLevel);
                experienceForPrevLevel = GameManager.Instance.ExperienceForLevel(_nextLevel - 1);
            }
            experience = (int) _experience;
            _expImage.fillAmount = (_experience - experienceForPrevLevel) / (experienceForNextLevel - experienceForPrevLevel);
            _expText.SetText(experience.ToString() + "/" + experienceForNextLevel.ToString());
            yield return null;
        }
        GameManager.Instance.Experience = _experience;
    }


}
