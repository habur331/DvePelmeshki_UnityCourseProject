using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoilEnumerator: IEnumerator<Vector2>
{
    private readonly Vector2[] _recoils;
    private int _currentIndex = -1;

    public Vector2 Current => _currentIndex < _recoils.Length ? _recoils[_currentIndex] : _recoils[^1];

    public bool Started => _currentIndex != -1;

    object IEnumerator.Current => Current;
    
    public RecoilEnumerator(Vector2[] inputRecoils)
    {
        if (inputRecoils is null || inputRecoils.Length == 0)
            throw new InvalidOperationException();
        
        _recoils = inputRecoils;
    }

    public bool MoveNext()
    {
        _currentIndex++;
        return true;
    }

    public void Reset()
    {
        _currentIndex = -1;
    }
    
    public void Dispose()
    {
     
    }
}