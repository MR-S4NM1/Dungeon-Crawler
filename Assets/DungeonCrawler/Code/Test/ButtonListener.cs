using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MrSanmi.DungeonCrawler
{
    public class ButtonListener : MonoBehaviour
    {
        protected Button _button;
        protected UnityAction _onClickAction;

        void Start()
        {
            _button = GetComponent<Button>();
            _onClickAction += OnClickActionNumber1;
            _onClickAction += OnClickActionNumber2;
            _button.onClick.AddListener(_onClickAction);
        }

        private void OnEnable()
        {
            _onClickAction += OnClickActionNumber1;
            _onClickAction += OnClickActionNumber2;
        }

        private void OnDisable()
        {
            _onClickAction -= OnClickActionNumber1;
            _onClickAction -= OnClickActionNumber2;
        }

        protected void OnClickActionNumber1()
        {
            Debug.Log("HOLA MUNDO :D");
        }

        protected void OnClickActionNumber2()
        {
            Debug.LogWarning("ADIOS MUNDO :(");
        }
    }

}