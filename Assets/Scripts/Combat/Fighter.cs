using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] float weaponDamage = 5f;
        Health target;
        Mover mover;
        ActionScheduler actionScheduler;
        Animator animator;

        float timeSinceLastAttack = Mathf.Infinity;

        // Hash variables for the animator
        int attackHash = Animator.StringToHash("attack");
        int stopAttackHash = Animator.StringToHash("stopAttack");

        void Start()
        {
            mover = GetComponent<Mover>();
            actionScheduler = GetComponent<ActionScheduler>();
            animator = GetComponent<Animator>();
        }
        void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if(target == null) return;
            if(target.IsDead) return;

            if (!GetIsInRange())
            {
                mover.MoveTo(target.transform.position, 1f);
            }
            else
            {
                mover.Cancel();
                AttackBehaviour();
            }
        }

        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack < timeBetweenAttacks) return;
            // This will trigger the Hit() event
            TriggerAttack();
            timeSinceLastAttack = 0;

        }

        private void TriggerAttack()
        {
            animator.ResetTrigger(stopAttackHash);
            animator.SetTrigger(attackHash);
        }

        void Hit()
        {
            if(target == null) return;
            target.TakeDamage(weaponDamage);
        }

        bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < weaponRange;
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if(combatTarget == null) return false;
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead;
        }

        public void Attack(GameObject combatTarget)
        {
            actionScheduler.StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            StopAttack();
            target = null;
            mover.Cancel();
        }

        private void StopAttack()
        {
            animator.ResetTrigger(attackHash);
            animator.SetTrigger(stopAttackHash);
        }

        // Animation Event

    }

}