//using System.Collections;
//using System.Collections.Generic;
//using System.Diagnostics.Tracing;
//using TMPro;
//using UnityEngine;
//using UnityEngine.UI;

//public class LevelUIManager : MonoBehaviour
//{
//    public static LevelUIManager Instance;

//    const int ZERO_NUMBER = 0;
//    const int MAX_NUMBER = 999;

//    [SerializeField] private TextMeshProUGUI _scoreTxt;
    
//    [SerializeField] private List<TextMeshProUGUI> _targetCandyLeftTxtList = new List<TextMeshProUGUI>(5);

//    [SerializeField] private List<Image> _targetCandyImgList = new List<Image>(5);

//    [SerializeField] private Slider _starBar;

//    [SerializeField] private Image[] _starImageArr = new Image[3];

//    private string _spritePath = "Candy Item/Sprite/";
//    private string starOnName = "star_on";

//    private void Awake()
//    {
//        if (Instance == null)
//        {
//            Instance = this;
//        }
//        else
//        {
//            Destroy(gameObject);
//        }
//    }

//    private void Start()
//    {
//        _scoreTxt.text = ZERO_NUMBER.ToString();
//    }

//    private void OnEnable()
//    {
//        LevelManager.OnUpdateScore += UpdateUI;
//    }

//    public void IniCandy(int index, Candy.CandyType type, int candyLeft)
//    {
//        _targetCandyImgList[index].sprite = LoadCandySprite(type);
//        SetCandyLeft(_targetCandyLeftTxtList[index], candyLeft);
//    }

//    private void UpdateUI(int totalScore, List<int> candyLeftList, int[] starArr)
//    {
//        _scoreTxt.text = totalScore.ToString();
//        for (int i = 0; i < candyLeftList.Count; i++)
//        {
//            _targetCandyLeftTxtList[i].text = candyLeftList[i].ToString();
//        }

//        _starBar.value = totalScore * 1f;
//        for (int i = 0; i < starArr.Length; i++)
//        {
//            if (totalScore >= starArr[i])
//            {
//                starArr[i] = 999;
//                _starImageArr[i].sprite = LoadStarSprite();
//            }
//        } 
//    }

//    private void SetCandyLeft(TextMeshProUGUI candyText, int value)
//    {
//        candyText.text = value.ToString();
//    }

//    private Sprite LoadCandySprite(Candy.CandyType type)
//    {
//        return Resources.Load<Sprite>(_spritePath + type.ToString());
//    }

//    private Sprite LoadStarSprite()
//    {
//        return Resources.Load<Sprite>(_spritePath + starOnName);
//    }
//}