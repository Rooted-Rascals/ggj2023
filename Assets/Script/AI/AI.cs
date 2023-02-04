using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class AI : MonoBehaviour
{
    public float movementSpeed = 2.0f;
    public float attackRange = 1f;

    public void OnDrawGizmos()
    {
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(transform.position, Vector3.up, Mathf.Sqrt(attackRange), 0.25f);
    }

    public void MoveTo(Vector3 target)
    {
        float deltaX = target.x - transform.position.x;
        float deltaZ = target.z - transform.position.z;
        transform.position += new Vector3(deltaX, 0f, deltaZ).normalized * (Time.deltaTime * movementSpeed);
    }

    public Tile GetTileUnderneath()
    {
        RaycastHit hit;
        if (Physics.Raycast(
                transform.position,
                Vector3.down,
                out hit,
                500f,
                LayerMask.GetMask("Tiles")
            ))
        {
            return hit.collider.GetComponent<Tile>();
        }

        return null;
    }

    public bool IsInAttackRange(Vector3 target)
    {
        return Mathf.Pow(target.x - transform.position.x, 2) + Mathf.Pow(target.z - transform.position.z, 2) <= attackRange;
    }

    public void Attack(GameObject target)
    {
        // Trigger attack animation
        // Reduce target life point
    }

}
