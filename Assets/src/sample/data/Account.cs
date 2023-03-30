using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Salesforce;
using Boomlagoon.JSON;

public class Account : SalesforceRecord
{
    public const string BASE_QUERY = "SELECT Id, Name FROM Account";

    public string name { get; set; }
  

    public Account() { }

    public Account(string id, string name) : base(id)
    {
        this.name = name;
    }

    public override string getSObjectName()
    {
        return "Account";
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
