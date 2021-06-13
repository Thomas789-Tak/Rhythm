using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Basic_Selector : MonoBehaviour
{
    public GameObject Container;
    public Animator Car_Name_Animator;
    public TextMeshProUGUI Car_Name_Text;
    bool need_to_rotate_right, need_to_rotate_left, available_for_action;
    float target_container_rotation_y;
    int Car_1_Color, Car_2_Color, Car_3_Color, Car_4_Color, Car_5_Color, Car_6_Color, Car_7_Color,
    Car_8_Color, Car_9_Color, Car_10_Color, Car_11_Color, Car_12_Color, Car_13_Color,
    Car_14_Color, Car_15_Color;
    // Start is called before the first frame update
    void Start()
    {
      need_to_rotate_right = false;
      need_to_rotate_left = false;
      available_for_action = true;
    }

    // Update is called once per frame
    void Update()
    {
      if(need_to_rotate_right){
        Container.transform.rotation = Quaternion.Slerp(Container.transform.rotation, Quaternion.Euler(0f, target_container_rotation_y + 1f, 0f), 0.035f);
        if(Container.transform.rotation.eulerAngles.y >= target_container_rotation_y || Container.transform.rotation.eulerAngles.y >= 359.95f){
          Container.transform.rotation = Quaternion.Euler(0f, target_container_rotation_y, 0f);
          need_to_rotate_right = false;
          available_for_action = true;
        }
      }else if(need_to_rotate_left){
        Container.transform.rotation = Quaternion.Slerp(Container.transform.rotation, Quaternion.Euler(0f, target_container_rotation_y - 1f, 0f), 0.035f);
        if(Mathf.Abs(Container.transform.rotation.eulerAngles.y) <= Mathf.Abs(target_container_rotation_y) && target_container_rotation_y != 0f){
          Container.transform.rotation = Quaternion.Euler(0f, target_container_rotation_y, 0f);
          need_to_rotate_left = false;
          available_for_action = true;
        }else if(Mathf.Abs(Container.transform.rotation.eulerAngles.y) - Mathf.Abs(target_container_rotation_y) <= 0.05f && target_container_rotation_y == 0f){
          Container.transform.rotation = Quaternion.Euler(0f, target_container_rotation_y, 0f);
          need_to_rotate_left = false;
          available_for_action = true;
        }
      }
    }

    public void Rotate_Right(){
      if(available_for_action){
        target_container_rotation_y = Mathf.RoundToInt(Container.transform.rotation.eulerAngles.y + 24f);
        Car_Name_Animator.Play("Car_Name_Change_Animation", -1, 0f);
        StartCoroutine(Change_Car_Name(24));
        need_to_rotate_right = true;
        available_for_action = false;
      }
    }

    public void Rotate_Left(){
      if(available_for_action){
        target_container_rotation_y = Mathf.RoundToInt(Container.transform.rotation.eulerAngles.y - 24f);
        Car_Name_Animator.Play("Car_Name_Change_Animation", -1, 0f);
        if(target_container_rotation_y == -24f){
          target_container_rotation_y = 336f;
        }
        StartCoroutine(Change_Car_Name(-24));
        need_to_rotate_left = true;
        available_for_action = false;
      }
    }

    public void Change_Car_Color(){
      if(available_for_action){
        float container_Rotation = Mathf.RoundToInt(Container.transform.rotation.eulerAngles.y);
        if(container_Rotation == 24f){
          // THE BEE
          switch(Car_2_Color){
            case 0:
              Container.transform.GetChild(1).GetChild(0).GetChild(0).gameObject.SetActive(false);
              Container.transform.GetChild(1).GetChild(0).GetChild(1).gameObject.SetActive(true);
              Car_2_Color++;
            break;

            case 1:
              Container.transform.GetChild(1).GetChild(0).GetChild(1).gameObject.SetActive(false);
              Container.transform.GetChild(1).GetChild(0).GetChild(2).gameObject.SetActive(true);
              Car_2_Color++;
            break;

            case 2:
              Container.transform.GetChild(1).GetChild(0).GetChild(2).gameObject.SetActive(false);
              Container.transform.GetChild(1).GetChild(0).GetChild(3).gameObject.SetActive(true);
              Car_2_Color++;
            break;

            case 3:
              Container.transform.GetChild(1).GetChild(0).GetChild(3).gameObject.SetActive(false);
              Container.transform.GetChild(1).GetChild(0).GetChild(0).gameObject.SetActive(true);
              Car_2_Color = 0;
            break;
          }

        }else if(container_Rotation == 48f){

          // THE ROGELIA
          switch(Car_3_Color){
            case 0:
              Container.transform.GetChild(2).GetChild(0).GetChild(0).gameObject.SetActive(false);
              Container.transform.GetChild(2).GetChild(0).GetChild(1).gameObject.SetActive(true);
              Car_3_Color++;
            break;

            case 1:
              Container.transform.GetChild(2).GetChild(0).GetChild(1).gameObject.SetActive(false);
              Container.transform.GetChild(2).GetChild(0).GetChild(2).gameObject.SetActive(true);
              Car_3_Color++;
            break;

            case 2:
              Container.transform.GetChild(2).GetChild(0).GetChild(2).gameObject.SetActive(false);
              Container.transform.GetChild(2).GetChild(0).GetChild(3).gameObject.SetActive(true);
              Car_3_Color++;
            break;

            case 3:
              Container.transform.GetChild(2).GetChild(0).GetChild(3).gameObject.SetActive(false);
              Container.transform.GetChild(2).GetChild(0).GetChild(0).gameObject.SetActive(true);
              Car_3_Color = 0;
            break;
          }

        }else if(container_Rotation == 72f){

          //THE GENTLEMAN
          switch(Car_4_Color){
            case 0:
              Container.transform.GetChild(3).GetChild(0).GetChild(0).gameObject.SetActive(false);
              Container.transform.GetChild(3).GetChild(0).GetChild(1).gameObject.SetActive(true);
              Car_4_Color++;
            break;

            case 1:
              Container.transform.GetChild(3).GetChild(0).GetChild(1).gameObject.SetActive(false);
              Container.transform.GetChild(3).GetChild(0).GetChild(2).gameObject.SetActive(true);
              Car_4_Color++;
            break;

            case 2:
              Container.transform.GetChild(3).GetChild(0).GetChild(2).gameObject.SetActive(false);
              Container.transform.GetChild(3).GetChild(0).GetChild(3).gameObject.SetActive(true);
              Car_4_Color++;
            break;

            case 3:
              Container.transform.GetChild(3).GetChild(0).GetChild(3).gameObject.SetActive(false);
              Container.transform.GetChild(3).GetChild(0).GetChild(0).gameObject.SetActive(true);
              Car_4_Color = 0;
            break;
          }

        }else if(container_Rotation == 96f){

          //THE SUNFLOWER
          switch(Car_5_Color){
            case 0:
              Container.transform.GetChild(4).GetChild(0).GetChild(0).gameObject.SetActive(false);
              Container.transform.GetChild(4).GetChild(0).GetChild(1).gameObject.SetActive(true);
              Car_5_Color++;
            break;

            case 1:
              Container.transform.GetChild(4).GetChild(0).GetChild(1).gameObject.SetActive(false);
              Container.transform.GetChild(4).GetChild(0).GetChild(2).gameObject.SetActive(true);
              Car_5_Color++;
            break;

            case 2:
              Container.transform.GetChild(4).GetChild(0).GetChild(2).gameObject.SetActive(false);
              Container.transform.GetChild(4).GetChild(0).GetChild(3).gameObject.SetActive(true);
              Car_5_Color++;
            break;

            case 3:
              Container.transform.GetChild(4).GetChild(0).GetChild(3).gameObject.SetActive(false);
              Container.transform.GetChild(4).GetChild(0).GetChild(0).gameObject.SetActive(true);
              Car_5_Color = 0;
            break;
          }

        }else if(container_Rotation == 120f){

          //THE SKYSCRAPER
          switch(Car_6_Color){
            case 0:
              Container.transform.GetChild(5).GetChild(0).GetChild(0).gameObject.SetActive(false);
              Container.transform.GetChild(5).GetChild(0).GetChild(1).gameObject.SetActive(true);
              Car_6_Color++;
            break;

            case 1:
              Container.transform.GetChild(5).GetChild(0).GetChild(1).gameObject.SetActive(false);
              Container.transform.GetChild(5).GetChild(0).GetChild(2).gameObject.SetActive(true);
              Car_6_Color++;
            break;

            case 2:
              Container.transform.GetChild(5).GetChild(0).GetChild(2).gameObject.SetActive(false);
              Container.transform.GetChild(5).GetChild(0).GetChild(3).gameObject.SetActive(true);
              Car_6_Color++;
            break;

            case 3:
              Container.transform.GetChild(5).GetChild(0).GetChild(3).gameObject.SetActive(false);
              Container.transform.GetChild(5).GetChild(0).GetChild(0).gameObject.SetActive(true);
              Car_6_Color = 0;
            break;
          }

        }else if(container_Rotation == 144f){

          //THE GOOD BESSY
          switch(Car_7_Color){
            case 0:
              Container.transform.GetChild(6).GetChild(0).GetChild(0).gameObject.SetActive(false);
              Container.transform.GetChild(6).GetChild(0).GetChild(1).gameObject.SetActive(true);
              Car_7_Color++;
            break;

            case 1:
              Container.transform.GetChild(6).GetChild(0).GetChild(1).gameObject.SetActive(false);
              Container.transform.GetChild(6).GetChild(0).GetChild(2).gameObject.SetActive(true);
              Car_7_Color++;
            break;

            case 2:
              Container.transform.GetChild(6).GetChild(0).GetChild(2).gameObject.SetActive(false);
              Container.transform.GetChild(6).GetChild(0).GetChild(3).gameObject.SetActive(true);
              Car_7_Color++;
            break;

            case 3:
              Container.transform.GetChild(6).GetChild(0).GetChild(3).gameObject.SetActive(false);
              Container.transform.GetChild(6).GetChild(0).GetChild(0).gameObject.SetActive(true);
              Car_7_Color = 0;
            break;
          }

        }else if(container_Rotation == 168f){

          //Dai-ichi Nyoko
          switch(Car_8_Color){
            case 0:
              Container.transform.GetChild(7).GetChild(0).GetChild(0).gameObject.SetActive(false);
              Container.transform.GetChild(7).GetChild(0).GetChild(1).gameObject.SetActive(true);
              Car_8_Color++;
            break;

            case 1:
              Container.transform.GetChild(7).GetChild(0).GetChild(1).gameObject.SetActive(false);
              Container.transform.GetChild(7).GetChild(0).GetChild(2).gameObject.SetActive(true);
              Car_8_Color++;
            break;

            case 2:
              Container.transform.GetChild(7).GetChild(0).GetChild(2).gameObject.SetActive(false);
              Container.transform.GetChild(7).GetChild(0).GetChild(3).gameObject.SetActive(true);
              Car_8_Color++;
            break;

            case 3:
              Container.transform.GetChild(7).GetChild(0).GetChild(3).gameObject.SetActive(false);
              Container.transform.GetChild(7).GetChild(0).GetChild(0).gameObject.SetActive(true);
              Car_8_Color = 0;
            break;
          }

        }else if(container_Rotation == 192f){

          //THE MIDNIGHT HUNTER
          switch(Car_9_Color){
            case 0:
              Container.transform.GetChild(8).GetChild(0).GetChild(0).gameObject.SetActive(false);
              Container.transform.GetChild(8).GetChild(0).GetChild(1).gameObject.SetActive(true);
              Car_9_Color++;
            break;

            case 1:
              Container.transform.GetChild(8).GetChild(0).GetChild(1).gameObject.SetActive(false);
              Container.transform.GetChild(8).GetChild(0).GetChild(2).gameObject.SetActive(true);
              Car_9_Color++;
            break;

            case 2:
              Container.transform.GetChild(8).GetChild(0).GetChild(2).gameObject.SetActive(false);
              Container.transform.GetChild(8).GetChild(0).GetChild(3).gameObject.SetActive(true);
              Car_9_Color++;
            break;

            case 3:
              Container.transform.GetChild(8).GetChild(0).GetChild(3).gameObject.SetActive(false);
              Container.transform.GetChild(8).GetChild(0).GetChild(0).gameObject.SetActive(true);
              Car_9_Color = 0;
            break;
          }

        }else if(container_Rotation == 216f){

          //LITTLE BIG FREEDOM
          switch(Car_10_Color){
            case 0:
              Container.transform.GetChild(9).GetChild(0).GetChild(0).gameObject.SetActive(false);
              Container.transform.GetChild(9).GetChild(0).GetChild(1).gameObject.SetActive(true);
              Car_10_Color++;
            break;

            case 1:
              Container.transform.GetChild(9).GetChild(0).GetChild(1).gameObject.SetActive(false);
              Container.transform.GetChild(9).GetChild(0).GetChild(2).gameObject.SetActive(true);
              Car_10_Color++;
            break;

            case 2:
              Container.transform.GetChild(9).GetChild(0).GetChild(2).gameObject.SetActive(false);
              Container.transform.GetChild(9).GetChild(0).GetChild(3).gameObject.SetActive(true);
              Car_10_Color++;
            break;

            case 3:
              Container.transform.GetChild(9).GetChild(0).GetChild(3).gameObject.SetActive(false);
              Container.transform.GetChild(9).GetChild(0).GetChild(0).gameObject.SetActive(true);
              Car_10_Color = 0;
            break;
          }

        }else if(container_Rotation == 240f){

          //THE ENDLESS SAFARI
          switch(Car_11_Color){
            case 0:
              Container.transform.GetChild(10).GetChild(0).GetChild(0).gameObject.SetActive(false);
              Container.transform.GetChild(10).GetChild(0).GetChild(1).gameObject.SetActive(true);
              Car_11_Color++;
            break;

            case 1:
              Container.transform.GetChild(10).GetChild(0).GetChild(1).gameObject.SetActive(false);
              Container.transform.GetChild(10).GetChild(0).GetChild(2).gameObject.SetActive(true);
              Car_11_Color++;
            break;

            case 2:
              Container.transform.GetChild(10).GetChild(0).GetChild(2).gameObject.SetActive(false);
              Container.transform.GetChild(10).GetChild(0).GetChild(3).gameObject.SetActive(true);
              Car_11_Color++;
            break;

            case 3:
              Container.transform.GetChild(10).GetChild(0).GetChild(3).gameObject.SetActive(false);
              Container.transform.GetChild(10).GetChild(0).GetChild(0).gameObject.SetActive(true);
              Car_11_Color = 0;
            break;
          }

        }else if(container_Rotation == 264f){

          //THE ASPHALT KING
          switch(Car_12_Color){
            case 0:
              Container.transform.GetChild(11).GetChild(0).GetChild(0).gameObject.SetActive(false);
              Container.transform.GetChild(11).GetChild(0).GetChild(1).gameObject.SetActive(true);
              Car_12_Color++;
            break;

            case 1:
              Container.transform.GetChild(11).GetChild(0).GetChild(1).gameObject.SetActive(false);
              Container.transform.GetChild(11).GetChild(0).GetChild(2).gameObject.SetActive(true);
              Car_12_Color++;
            break;

            case 2:
              Container.transform.GetChild(11).GetChild(0).GetChild(2).gameObject.SetActive(false);
              Container.transform.GetChild(11).GetChild(0).GetChild(3).gameObject.SetActive(true);
              Car_12_Color++;
            break;

            case 3:
              Container.transform.GetChild(11).GetChild(0).GetChild(3).gameObject.SetActive(false);
              Container.transform.GetChild(11).GetChild(0).GetChild(0).gameObject.SetActive(true);
              Car_12_Color = 0;
            break;
          }

        }else if(container_Rotation == 288f){

          //THE DRAGON-FLY
          switch(Car_13_Color){
            case 0:
              Container.transform.GetChild(12).GetChild(0).GetChild(0).gameObject.SetActive(false);
              Container.transform.GetChild(12).GetChild(0).GetChild(1).gameObject.SetActive(true);
              Car_13_Color++;
            break;

            case 1:
              Container.transform.GetChild(12).GetChild(0).GetChild(1).gameObject.SetActive(false);
              Container.transform.GetChild(12).GetChild(0).GetChild(2).gameObject.SetActive(true);
              Car_13_Color++;
            break;

            case 2:
              Container.transform.GetChild(12).GetChild(0).GetChild(2).gameObject.SetActive(false);
              Container.transform.GetChild(12).GetChild(0).GetChild(3).gameObject.SetActive(true);
              Car_13_Color++;
            break;

            case 3:
              Container.transform.GetChild(12).GetChild(0).GetChild(3).gameObject.SetActive(false);
              Container.transform.GetChild(12).GetChild(0).GetChild(0).gameObject.SetActive(true);
              Car_13_Color = 0;
            break;
          }

        }else if(container_Rotation == 312f){

          //JUSTE UNE VICTOIRE
          switch(Car_14_Color){
            case 0:
              Container.transform.GetChild(13).GetChild(0).GetChild(0).gameObject.SetActive(false);
              Container.transform.GetChild(13).GetChild(0).GetChild(1).gameObject.SetActive(true);
              Car_14_Color++;
            break;

            case 1:
              Container.transform.GetChild(13).GetChild(0).GetChild(1).gameObject.SetActive(false);
              Container.transform.GetChild(13).GetChild(0).GetChild(2).gameObject.SetActive(true);
              Car_14_Color++;
            break;

            case 2:
              Container.transform.GetChild(13).GetChild(0).GetChild(2).gameObject.SetActive(false);
              Container.transform.GetChild(13).GetChild(0).GetChild(3).gameObject.SetActive(true);
              Car_14_Color++;
            break;

            case 3:
              Container.transform.GetChild(13).GetChild(0).GetChild(3).gameObject.SetActive(false);
              Container.transform.GetChild(13).GetChild(0).GetChild(0).gameObject.SetActive(true);
              Car_14_Color = 0;
            break;
          }

        }else if(container_Rotation == 336f || container_Rotation == -24f){

          //THE DIRTY ROCKET
          switch(Car_15_Color){
            case 0:
              Container.transform.GetChild(14).GetChild(0).GetChild(0).gameObject.SetActive(false);
              Container.transform.GetChild(14).GetChild(0).GetChild(1).gameObject.SetActive(true);
              Car_15_Color++;
            break;

            case 1:
              Container.transform.GetChild(14).GetChild(0).GetChild(1).gameObject.SetActive(false);
              Container.transform.GetChild(14).GetChild(0).GetChild(2).gameObject.SetActive(true);
              Car_15_Color++;
            break;

            case 2:
              Container.transform.GetChild(14).GetChild(0).GetChild(2).gameObject.SetActive(false);
              Container.transform.GetChild(14).GetChild(0).GetChild(3).gameObject.SetActive(true);
              Car_15_Color++;
            break;

            case 3:
              Container.transform.GetChild(14).GetChild(0).GetChild(3).gameObject.SetActive(false);
              Container.transform.GetChild(14).GetChild(0).GetChild(0).gameObject.SetActive(true);
              Car_15_Color = 0;
            break;
          }

        }else if(container_Rotation == 0f || container_Rotation == 360f){

          //LEENA
          switch(Car_1_Color){
            case 0:
              Container.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(false);
              Container.transform.GetChild(0).GetChild(0).GetChild(1).gameObject.SetActive(true);
              Car_1_Color++;
            break;

            case 1:
              Container.transform.GetChild(0).GetChild(0).GetChild(1).gameObject.SetActive(false);
              Container.transform.GetChild(0).GetChild(0).GetChild(2).gameObject.SetActive(true);
              Car_1_Color++;
            break;

            case 2:
              Container.transform.GetChild(0).GetChild(0).GetChild(2).gameObject.SetActive(false);
              Container.transform.GetChild(0).GetChild(0).GetChild(3).gameObject.SetActive(true);
              Car_1_Color++;
            break;

            case 3:
              Container.transform.GetChild(0).GetChild(0).GetChild(3).gameObject.SetActive(false);
              Container.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(true);
              Car_1_Color = 0;
            break;
          }

        }

      }
    }

    IEnumerator Change_Car_Name(float degrees_to_add){
      float final_rotation = Mathf.RoundToInt(Container.transform.rotation.eulerAngles.y + degrees_to_add);
      yield return new WaitForSeconds(0.5f);
      if(final_rotation == 24f){
        Car_Name_Text.text = "The Bee";
      }else if(final_rotation == 48f){
        Car_Name_Text.text = "The \nRogelia";
      }else if(final_rotation == 72f){
        Car_Name_Text.text = "The \nGentleman";
      }else if(final_rotation == 96f){
        Car_Name_Text.text = "The \nSunflower";
      }else if(final_rotation == 120f){
        Car_Name_Text.text = "The \nSkyscraper";
      }else if(final_rotation == 144f){
        Car_Name_Text.text = "The Good \nBessy";
      }else if(final_rotation == 168f){
        Car_Name_Text.text = "Dai-ichi \nNyoko";
      }else if(final_rotation == 192f){
        Car_Name_Text.text = "The Midnight \nHunter";
      }else if(final_rotation == 216f){
        Car_Name_Text.text = "Little \nBig Freedom";
      }else if(final_rotation == 240f){
        Car_Name_Text.text = "The Endless \nSafari";
      }else if(final_rotation == 264f){
        Car_Name_Text.text = "The \nAsphalt King";
      }else if(final_rotation == 288f){
        Car_Name_Text.text = "The \nDragon-fly";
      }else if(final_rotation == 312f){
        Car_Name_Text.text = "Juste Une \nVictoire";
      }else if(final_rotation == 336f || final_rotation == -24f){
        Car_Name_Text.text = "The \nDirty Rocket";
      }else if(final_rotation == 0f || final_rotation == 360f){
        Car_Name_Text.text = "Leena";
      }
    }
}
