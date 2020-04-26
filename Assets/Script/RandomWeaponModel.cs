using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomWeaponModel : MonoBehaviour
{
    private string weaponModelPath;

    // Start is called before the first frame update
    void Start()
    {
        weaponModelPath = "Prefabs/Weapons";

        int[] wHArr = GetRandomSequence(55);
        int j = 0;

        //wHArr[0] = 48;

        if (0<=wHArr[0] && wHArr[0] <= 8)
        {
            //手枪
            j = wHArr[0] - 0;
            weaponModelPath += "/Pistol/Pistol_" + j;
        }
        else if (9 <= wHArr[0] && wHArr[0] <= 15)
        {
            //冲锋枪
            j = wHArr[0] - 9;
            weaponModelPath += "/SMG/SMG_" + j;
        }
        else if (16 <= wHArr[0] && wHArr[0] <= 24)
        {
            //霰弹枪
            j = wHArr[0] - 16;
            weaponModelPath += "/ShotGun/ShotGun_" + j;
        }
        else if (25 <= wHArr[0] && wHArr[0] <= 33)
        {
            //狙击枪
            j = wHArr[0] - 25;
            weaponModelPath += "/Sniper/Sniper_" + j;
        }
        else if (34 <= wHArr[0] && wHArr[0] <= 48)
        {
            //武士刀
            j = wHArr[0] - 34;
            weaponModelPath += "/Katana/Katana_" + j;
        }
        else if (49 <= wHArr[0] && wHArr[0] <= 54)
        {
            //大锤
            j = wHArr[0] - 49;
            weaponModelPath += "/Hammer/Hammer_" + j;
        }

        Debug.Log(weaponModelPath);

        GameObject a = Instantiate(Resources.Load(weaponModelPath), gameObject.transform) as GameObject;
        
        a.transform.localRotation = Quaternion.identity;

        if (34 <= wHArr[0] && wHArr[0] <= 48)
        {
            //武士刀
            a.transform.localPosition = new Vector3(0, -40f, 0);
            a.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        }
        else if (49 <= wHArr[0] && wHArr[0] <= 54)
        {
            //锤子
            a.transform.localPosition = new Vector3(0, -25f, 0);
            a.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
        }
        else
        {
            a.transform.localPosition = new Vector3(0, -10f, 0);
            a.transform.localScale = new Vector3(1, 1, 1);
        }
        

    }

    // Update is called once per frame
    void Update()
    {

    }

    public static int[] GetRandomSequence(int total)
    {
        int[] hashtable = new int[total];
        int[] output = new int[total];

        for (int i = 0; i < total; i++)
        {
            int num = Random.Range(0, total);
            while (hashtable[num] > 0)
            {
                num = Random.Range(0, total);
            }

            output[i] = num;
            hashtable[num] = 1;
        }

        return output;
    }
}