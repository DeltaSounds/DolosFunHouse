﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    [SerializeField] private GameObject[] _characters;
    [SerializeField] private int _selectedCharacter;
    [SerializeField] private Vector3 _spawnPos;

    //private void Start()
    //{
    //    _characters[_selectedCharacter].SetActive(true);
    //}

    private void Start()
    {
        for (int i = 0; i < _characters.Length; i++)
        {
            Instantiate(_characters[i], _spawnPos, Quaternion.identity, gameObject.transform);
        }

        _characters[_selectedCharacter].SetActive(true);
    }

    public void NextCharacter()
    {
        _characters[_selectedCharacter].SetActive(false);
        _selectedCharacter = (_selectedCharacter + 1) % _characters.Length;
        _characters[_selectedCharacter].SetActive(true);
    }

    public void PreviousCharacter()
    {
        _characters[_selectedCharacter].SetActive(false);
        _selectedCharacter--;

        if (_selectedCharacter < 0)
        {
            _selectedCharacter += _characters.Length;
        }

        _characters[_selectedCharacter].SetActive(true);
    }

    public void StartGame()
    {
        PlayerPrefs.SetInt("selectedCharacter", _selectedCharacter);
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
}