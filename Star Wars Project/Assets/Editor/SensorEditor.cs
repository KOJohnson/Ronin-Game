using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SensorComponent))]
public class SensorEditor : Editor
{
    private void OnSceneGUI()
    {
        SensorComponent sensorComponent = (SensorComponent)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(sensorComponent.transform.position, Vector3.up, Vector3.forward, 360, sensorComponent.Radius);
        
        Handles.color = Color.red;
        Handles.DrawWireArc(sensorComponent.transform.position, Vector3.up, Vector3.forward, 360, sensorComponent.InstantDetectionRange);

        Vector3 viewAngleLeft = DirectionFromAngle(sensorComponent.transform.eulerAngles.y, -sensorComponent.Angle / 2);
        Vector3 viewAngleRight = DirectionFromAngle(sensorComponent.transform.eulerAngles.y, sensorComponent.Angle / 2);
        
        Handles.color = Color.white;
        Handles.DrawLine(sensorComponent.transform.position, sensorComponent.transform.position + viewAngleLeft * sensorComponent.Radius);
        Handles.DrawLine(sensorComponent.transform.position, sensorComponent.transform.position + viewAngleRight * sensorComponent.Radius);

        if (sensorComponent.CanSeePlayer)
        {
            Handles.color = Color.green;
            Handles.DrawLine(sensorComponent.transform.position, sensorComponent.PlayerRef.transform.position + new Vector3(0,1.8f,0));
        }

        if (sensorComponent.PlayerPosition != Vector3.zero)
        {
            Handles.color = Color.yellow;
            Handles.DrawWireCube(sensorComponent.PlayerPosition, new Vector3(0.5f, 0.5f, 0.5f));
            Handles.Label(sensorComponent.PlayerPosition + new Vector3(0, 0.5f), $"Player Position is: {sensorComponent.PlayerPosition}");
        }
        
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}