using System.Collections.Generic;

public class Rules3
{
    public void AddRule(Rule3 rule)
    {
        GetRules.Add(rule);
    }

    public List<Rule3> GetRules { get; } = new List<Rule3>();
}

