using DG.DOTweenEditor;
using DG.Tweening;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GizmosHelper))]
public class GizmosHelperEditor : Editor
{
    GizmosHelper gizmosHelper;
    Sys sys;

    private void OnEnable() {
        gizmosHelper = target as GizmosHelper;
        sys = gizmosHelper.GetSys();
        UpdateEventNodePathPoints();
    }
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        if (GUILayout.Button("ÖØ»æÂ·¾¶")) {
            UpdateEventNodePathPoints();
        }
    }

    private void UpdateEventNodePathPoints() {
        List<EventNode> eventNodes = gizmosHelper.GetEventNodes();
        foreach (EventNode eventNode in eventNodes) {
            Tween tween = eventNode.UpdatePathPoints(sys);
            if (tween != null) {
                DOTweenEditorPreview.PrepareTweenForPreview(tween, false, false);
                DOTweenEditorPreview.Start();
            }
        }
    }
}
