using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretLookAt : MonoBehaviour
{

    [SerializeField] private GameObject _turret;


    [SerializeField] [Range(0,360)] 
    private float _fov = 90;

    [SerializeField] 
    private float _range = 10;

    [SerializeField] 
    private float _rotationSpeed = 90;

    private Vector3 _lastvalidtarget = Vector3.zero;
    
    // Update is called once per frame
    void Update()
    {
        Vector3 screenPosition = Input.mousePosition;
        screenPosition.z = Vector3.Distance(transform.position,Camera.main.transform.position);
        Vector3 target = Camera.main.ScreenToWorldPoint(screenPosition);

        Vector3 directionToTarget = (target - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, directionToTarget);

        if (angle <= _fov / 2)
        {

            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            _turret.transform.rotation = Quaternion.RotateTowards(_turret.transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

            _lastvalidtarget = directionToTarget;
        }
        else
        {
            Quaternion targetRotation = Quaternion.LookRotation(_lastvalidtarget);
            _turret.transform.rotation = Quaternion.RotateTowards(_turret.transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        float coneLength = _range;
        if (_fov < 360)
            DrawCone(coneLength);
        else
            Gizmos.DrawWireSphere(transform.position, coneLength);

    }

    private void DrawCone(float coneLength)
    {
        int segments = 60;

        

        Vector3 forward = transform.forward * coneLength;
        Quaternion rotation1 = Quaternion.AngleAxis(-_fov / 2, transform.up);
        Quaternion rotation2 = Quaternion.AngleAxis(_fov / 2, transform.up);
        Vector3 direction1 = rotation1 * forward;
        Vector3 direction2 = rotation2 * forward;
        Vector3 from = transform.position;
        Vector3 to1 = transform.position + direction1;
        Vector3 to2 = transform.position + direction2;

        Gizmos.DrawLine(from, to1);
        Gizmos.DrawLine(from, to2);


        float angleStep = _fov / 2f / segments;
        Vector3 start = from + direction1;
        Vector3 end = start;
        Quaternion rotation = Quaternion.AngleAxis(angleStep, transform.up);

        for (int i = 0; i < segments * 2; i++)
        {
            end = rotation * end;
            Gizmos.DrawLine(start, end);
            start = end;
        }
    }
}
