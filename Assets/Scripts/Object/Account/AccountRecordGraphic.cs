using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AccountRecordGraphic : MonoBehaviour, ObjectRecordGraphic 
{
    public TMP_Text accountName;
    // Start is called before the first frame update
    Account account = new Account();
    AccountUI accountUI;
    public void Setup(Account account, AccountUI accountUI)
    {
        accountName.text =  account.name;
        this.account= account;
        this.accountUI = accountUI;
    }

    public void OnSelect()
    {
        accountUI.OnSelectAccount(account);
    }

    public void OnInitiateDelete()
    {
        accountUI.InitiateDeleteAccount(account);
    }
}
