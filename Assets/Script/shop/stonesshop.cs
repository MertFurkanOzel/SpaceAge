using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class stonesshop : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI[] texts;
    [SerializeField] TMP_InputField[] ifs;
    [SerializeField] GameObject gameco;
    private playerdata pd;

    private void OnEnable()
    {
        pd = gameco.GetComponent<playerdata>();
        tas_miktari_guncelle();
    }

    private void tas_miktari_guncelle()
    {
        texts[0].text = pd.Stones_count[0].ToString();
        texts[1].text = pd.Stones_count[1].ToString();
        texts[2].text = pd.Stones_count[2].ToString();       
    }

    public void sell(int val)
    {
        if (ifs[val].text == "")
            return;
        int satilmak_istenen_tas_adet = int.Parse(ifs[val].text);
        int stone_shop_value= gameco.GetComponent<gamecontrol>()._stones[val].stone_shop_value;
        switch (val)
        {
            case 0:
                if(satilmak_istenen_tas_adet <= pd.Stones_count[0])
                {
                    pd.Stones_count[0] -= satilmak_istenen_tas_adet;
                    pd.Space_credits += satilmak_istenen_tas_adet * stone_shop_value;
                    gameco.GetComponent<gamecontrol>().stone_storage_filled_image();
                }
                break;
            case 1:
                if (satilmak_istenen_tas_adet <= pd.Stones_count[1])
                {
                    pd.Stones_count[1] -= satilmak_istenen_tas_adet;
                    pd.Space_credits += satilmak_istenen_tas_adet * stone_shop_value;
                    gameco.GetComponent<gamecontrol>().stone_storage_filled_image();
                }
                break;
            case 2:
                if (satilmak_istenen_tas_adet <= pd.Stones_count[2])
                {
                    pd.Stones_count[2] -= satilmak_istenen_tas_adet;
                    pd.Space_credits += satilmak_istenen_tas_adet * stone_shop_value;
                    gameco.GetComponent<gamecontrol>().stone_storage_filled_image();
                }
                break;
            default:
                break;
        }
        tas_miktari_guncelle();
        gameco.GetComponent<gamecontrol>().veri_guncelle_ve_kaydet();
        ifs[val].text = "";
    }
    
}
