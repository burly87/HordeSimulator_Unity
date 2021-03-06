﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_SeekHero : MonoBehaviour
{
    public string charType = "Hero";

    [SerializeField] private float attackRange = 2.0f;
    [SerializeField] private float attackSpeed = 2.23f;
    private float attackCooldown;
    [SerializeField] private float damage = 10.0f;

    public float weight = 1.0f;

    public float MyWeight
    {
        get { return weight; }
    }

    Character MyCharacter;

    void Start()
    {
        MyCharacter = GetComponent<Character>();
        attackCooldown = attackSpeed;
    }

    void DoEnemyBehavior()
    {
        if (Character.characterByType.ContainsKey(charType) == false)
        {
            //nothing to do
            return;
        }

        // calculate nearest
        Character closestChar = null;
        float dist = Mathf.Infinity;

        foreach (Character c in Character.characterByType[charType])
        {
            float d = Vector3.Distance(this.transform.position, c.transform.position);
            if (closestChar == null || d < dist)
            {
                closestChar = c;
                dist = d;
            }

        }
        // no Hero existing
        if (closestChar == null) { return; }

        //MyCharacter.LookAt(closestChar.transform);

        if (dist <= attackRange)
        {
            MyCharacter.animator.SetTrigger("isAttacking");
            attackCooldown -= Time.deltaTime;
            if (attackCooldown <= 0.0f)
            {
                
                attackCooldown = attackSpeed;
                closestChar.Hit(closestChar, damage);
            }
        }
        else
        {

            Vector3 dir = closestChar.transform.position - this.transform.position;
			WeightedDirection wd = new WeightedDirection( dir, weight);
			MyCharacter.enemyAIList.Add( wd );

        }
    }
}
