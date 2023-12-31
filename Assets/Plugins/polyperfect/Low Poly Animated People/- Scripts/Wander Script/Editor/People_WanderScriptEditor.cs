﻿using Polyperfect.Common;
using UnityEditor;

#if UNITY_EDITOR
namespace Polyperfect.People
{
    [CustomEditor(typeof(People_WanderScript))]
    [CanEditMultipleObjects]
    public class People_WanderScriptEditor : Common_WanderScriptEditor { }
}
#endif