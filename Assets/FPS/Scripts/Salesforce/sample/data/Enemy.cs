using Boomlagoon.JSON;
using Salesforce;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : SalesforceRecord
{
    public const string BASE_QUERY = "SELECT Id, Name FROM Enemy__c";

    public string name { get; set; }


    public Enemy() { }

    public Enemy(string id, string name) : base(id)
    {
        this.name = name;
    }

    public override string getSObjectName()
    {
        return "Enemy__c";
    }

    public override JSONObject toJson()
    {
        JSONObject record = base.toJson();
        record.Add("Name", name);
        return record;
    }

    public override void parseFromJson(JSONObject jsonObject)
    {
        base.parseFromJson(jsonObject);
        name = jsonObject.GetString("Name");
    }
}
