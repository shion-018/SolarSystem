using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeBar : MonoBehaviour
{
   
    public int maxtime;
    int currenttime;
    public Slider slider;
    public Image image;

    int damage = 1;

    bool touch = false;

    float seconds;

    void Start()
    {
        maxtime = Random.Range(1, 5) * 5;


        //Slider�R���|�[�l���g�������I�Ɏ擾
        if (slider == null)
        {
            slider = GetComponentInChildren<Slider>();
        }

        //Slider�𖞃^���ɂ���
        slider.value = 1;
        //���݂̎��Ԃ��ő厞�ԂƓ����ɂ���
        currenttime = maxtime;

    }

    void Update()
    {
        image = image.GetComponentInChildren<Image>();
        if (touch)
        {
            seconds += Time.deltaTime;
            if (seconds >= 1)
            {
                seconds = 0;

                

                //���݂̎��Ԃ���_���[�W������
                currenttime -= damage;
                

                if (currenttime == maxtime/2 )
                {
                    image.color = Color.yellow;
                }

                //�ő厞�Ԃɂ����錻�݂̎��Ԃ�Slider�ɔ��f
                slider.value = (float)currenttime / (float)maxtime;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Finish"))
        {
            Debug.Log("�΂�");
            touch = true;
        }

        if(maxtime != currenttime)
        {
            currenttime = maxtime;
        }
    }
}
