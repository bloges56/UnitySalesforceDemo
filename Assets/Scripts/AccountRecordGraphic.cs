using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AccountRecordGraphic : MonoBehaviour
{
    public TMP_Text accountName;
    // Start is called before the first frame update
    public void Setup(Account account)
    {
        accountName.text =  account.name;
    }
}
