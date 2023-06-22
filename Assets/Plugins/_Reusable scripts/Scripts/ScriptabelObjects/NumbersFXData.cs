﻿using DG.Tweening;
using UnityEngine;

namespace Utils
{
    [CreateAssetMenu(fileName = "NumbersFXData", menuName = "ScriptableObjects/Utils/NumbersFXData")]
    public class NumbersFXData : ScriptableObject
    {
        [SerializeField][Tooltip("How long will number be showing")] private float _showingDuration;
        [SerializeField][Tooltip("How fast will number be showing up")] private float _inOutDuration;
        [SerializeField][Tooltip("Distance between start and end Y Value")] private float _deltaY;
        [SerializeField] private Ease _easeType;

        public float ShowingDuration { get { return _showingDuration; } }
        public float InOutDuration { get { return _inOutDuration; } }
        public float DeltaY { get { return _deltaY; } }
        public Ease EaseType { get { return _easeType; } }
    }
}