using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatesManager
{
    private static StatesManager _instance = null;

    public static StatesManager Instance()
    {
        if (_instance == null)
            _instance = new StatesManager();
        return _instance;
    }

    private Dictionary<IAIState, int> _allStates = new Dictionary<IAIState, int>();

    public int RegisterState(IAIState newState)
    {
        if (_allStates.ContainsKey(newState)) return _allStates[newState];

        _allStates.Add(newState, _allStates.Count);
        return _allStates.Count - 1;
    }

    public int GetID(IAIState fromState)
    {
        if (_allStates.ContainsKey(fromState)) return _allStates[fromState];
        return -1;
    }

    public IAIState GetState(int Id)
    {
        foreach (var pair in _allStates)
            if (pair.Value == Id)
                return pair.Key;

        return null;
    }

}
