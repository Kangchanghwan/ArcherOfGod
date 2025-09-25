using System;
using System.Threading; // CancellationTokenSource 사용을 위해 필요
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace UI
{
    public class UI_Timer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI tmp; // 시간을 표시할 TextMeshPro 컴포넌트
    }
}