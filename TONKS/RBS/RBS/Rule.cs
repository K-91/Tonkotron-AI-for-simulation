using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rule
{
    public string atecedentA;
    public string atecedentB;
    public Type consequentState;
    public Predicate compare;
    public enum Predicate
    { And, Or, nAnd,aNotB }

    public Rule(string atecedentA, string atecedentB, Type consequentState, Predicate compare)
    {
        this.atecedentA = atecedentA;
        this.atecedentB = atecedentB;
        this.consequentState = consequentState;
        this.compare = compare;
    }


    public Type CheckRule(Dictionary<string, bool> stats)
    {
        bool atecedentABool = stats[atecedentA];
        bool atecedentBBool = stats[atecedentB];

        switch (compare)
        {
            case Predicate.And:
                if (atecedentABool && atecedentBBool)
                {
                    return consequentState;
                }
                else
                {
                    return null;
                }

            case Predicate.Or:

                if (atecedentABool || atecedentBBool)
                {
                    return consequentState;
                }
                else
                {
                    return null;
                }


            case Predicate.nAnd:

                if (!atecedentABool && !atecedentBBool)
                {
                    return consequentState;
                }
                else
                {
                    return null;
                }
            case Predicate.aNotB:
                if(atecedentABool && !atecedentBBool)
                {
                    return consequentState;
                }
                else
                {
                    return null;
                }
            default:
                return null;
        }
    }
}

