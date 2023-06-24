using Salesforce;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.Profiling;

public class AccountUI : ObjectUI
{
    [SerializeField] AccountRepository accRepo;
    Dictionary<Account, AccountRecordGraphic> opportunityGraphics = new Dictionary<Account, AccountRecordGraphic>();
    public override IEnumerator SetupRecordList()
    {
        base.SetupRecordList();
        yield return accRepo.GetRecords();
        foreach (Account opportunity in accRepo.accounts)
        {
            var newRecord = Instantiate(recordUIPrefab, recordsParent).transform.GetComponent<AccountRecordGraphic>();
            newRecord.Setup(opportunity, this);
            opportunityGraphics.Add(opportunity, newRecord);
        }
    }

    public void SetName(string name)
    {
        accRepo.accountToInsert.name = name;
    }

    public void OnSelectAccount(Account opportunity)
    {
        OnSelectRecord();
        accRepo.accountToUpdate = opportunity;
        updateObjectPlaceholderText.text = opportunity.name;
    }

    public void UpdateName(string updateNameString)
    {
        accRepo.accountToUpdate.name = updateNameString;
    }

    protected override IEnumerator CreateRecordUI()
    {
        yield return accRepo.CreateRecord();
        var newRecord = Instantiate(recordUIPrefab, recordsParent).transform.GetComponent<AccountRecordGraphic>();
        newRecord.Setup(accRepo.accountToInsert, this);
        opportunityGraphics.Add(accRepo.accountToInsert, newRecord);
    }

    protected override IEnumerator UpdateRecordUI()
    {
        yield return accRepo.UpdateRecord();

        OnExitUpdateRecord();
    }

    public void InitiateDeleteAccount(Account opp)
    {
        InitiateDeleteRecord();
        accRepo.accountToDelete = opp;
        deleteObjectNameText.text = accRepo.accountToDelete.name;
    }
    protected override IEnumerator DeleteRecordUI()
    {
        yield return accRepo.DeleteRecord();
        Destroy(opportunityGraphics[accRepo.accountToDelete].gameObject);
        opportunityGraphics.Remove(accRepo.accountToDelete);

    }
}
