using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Button_Car : MonoBehaviour
{
    private Car car;
    private CarDataContainer carDataContainer;
    private LUILobby LUILobby;
    [SerializeField]
    public Button Button;
    public Image ImageCar;
    public TextMeshProUGUI TextLevel;
    public GameObject GOSelected;
    public GameObject GOLock;


    // Use this for initialization
    void Start()
    {
        LUILobby = LUILobby.Instance;
        Button.onClick.AddListener(() => LUILobby.SelectCar(carDataContainer));
    }

    public void SetCar(Car car)
    {
        //this.car = car;
        //TextLevel.text = car.Level.ToString();
        TextLevel.text = 2.ToString();

        // 차량 잠금 설정
        GOLock.SetActive(false);

        //// 차량 선택 설정
        //GOSelected.SetActive(false);
    }

    public void SetCar(CarDataContainer CarDataContainer)
    {
        this.carDataContainer = CarDataContainer;

        this.TextLevel.text = CarDataContainer.level.ToString();

        this.ImageCar.sprite = CarDataContainer.ImageCar;

        if (CarDataContainer.level == 0)
        {
            GOLock.SetActive(true);
        }
        else
        {
            GOLock.SetActive(false);
        }

        this.gameObject.SetActive(true);
    }


    public void SelectThisCar()
    {
        this.GOLock.SetActive(true);
    }

    public void UnSelectedThisCar()
    {
        this.GOLock.SetActive(false);
    }

}