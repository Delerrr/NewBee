using DG.DOTweenEditor;
using DG.Tweening;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GizmosHelper))]
public class GizmosHelperEditor : Editor {
    GizmosHelper gizmosHelper;
    Sys sys;
    private bool loopEnabled = false;

    private void OnEnable() {
        gizmosHelper = target as GizmosHelper;
        sys = gizmosHelper.GetSys();
        List<EventNode> eventNodes = gizmosHelper.GetEventNodes();
        if (!loopEnabled) {
            gizmosHelper.updatePathPointsLoop += () => {
                foreach (EventNode eventNode in eventNodes) {
                    Tween tween = eventNode.UpdatePathPoints(sys, gizmosHelper);
                    if (tween != null) {
                        DOTweenEditorPreview.PrepareTweenForPreview(tween, false, false);
                        DOTweenEditorPreview.Start();
                    }
                }
            };
            gizmosHelper.StartUpdatePathPointsLoopMethod();
            loopEnabled = true;
        }
    }
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        if (GUILayout.Button("ÖØ»æÂ·¾¶")) {
            UpdateEventNodePathPoints();
        }
    }

    private void UpdateEventNodePathPoints() {
    }
}
