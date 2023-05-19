using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OpportunityRecordGraphic : MonoBehaviour, ObjectRecordGraphic
{
    public TMP_Text opportunityName;
    // Start is called before the first frame update
    Opportunity opportunity = new Opportunity();
    OpportunityUI opportunityUI;
    public void Setup(Opportunity opportunity, OpportunityUI opportunityUI)
    {
        opportunityName.text = opportunity.name;
        this.opportunity = opportunity;
        this.opportunityUI = opportunityUI;
    }

    public void OnSelect()
    {
        opportunityUI.OnSelectOpportunity(opportunity);
    }

    public void OnInitiateDelete()
    {
        opportunityUI.InitiateDeleteOpportunity(opportunity);
    }
}
