using FriendsGamesTools;
using System.Collections;
using UnityEngine;

namespace DrawSpell
{
    public class Boss : MonoBehaviourHasInstance<Boss>, IDamageable, ISpellTarget
    {
        public event OnDied Died;
        public event OnDamage SpellCasted;
        public int HP => Shapes.Shapes.Count;
        [SerializeField] private SpellShapes _shapes;
        public SpellShapes Shapes => _shapes;

        public Transform Transform => transform;
        private ISpellTarget _spellTarget;

        [SerializeField]
        private int strength = 5;

        [SerializeField] private Transform _spellOrigin;
        [SerializeField] private Animator animator;
        public void TakeDamage(Shape.ShapeType shapeType)
        {
            Shapes.DisposeShape(shapeType);
            TakeDamage(1);
        }

        public void SetSpellTarget(ISpellTarget target)
        {
            _spellTarget = target;
            (target as IDamageable).Died += SpellTarget_Died;
            _shapes.gameObject.SetActive(true);
        }

        private void SpellTarget_Died(IDamageable damageable)
        {
            _spellTarget = null;
        }

        private void Start()
        {
            InitShapes();
            StartCoroutine(ThinkingRoutine());
        }
        private void InitShapes()
        {
            for (int i = 0; i < strength; i++)
            {
                _shapes.CreateShape(Utils.RandomEnumElement<Shape.ShapeType>());

            }
            _shapes.gameObject.SetActive(false);
        }
        private void OnValidate()
        {
            if (strength < 1) strength = 1;
        }
        private IEnumerator ThinkingRoutine()
        {
            yield return new WaitUntil(() => _spellTarget != null);

            while (_spellTarget != null)
            {
                animator.SetTrigger("Prepare");
                yield return new WaitForSeconds(2f);
                animator.SetTrigger("Attack");
                yield return new WaitForSeconds(0.5f);
                if (_spellTarget == null) break;
                CastRandomSpell();
            }
        }
        public void Attack(Shape.ShapeType shapeType)
        {

            var spellInfo = GameSettings.instance.GetShapeInfo(shapeType);

            bool isTargetSpell = spellInfo.spell is TargetSpell;

            spellInfo.CastSpellInstance(isTargetSpell ? _spellOrigin.position : (_spellTarget as IDamageable).Transform.position, _spellTarget);

        }
        private void CastRandomSpell()
        {
            Attack(Utils.RandomEnumElement<Shape.ShapeType>());
        }

        public void TakeDamage(int damage)
        {
            if (HP <= 0)
            {
                Died?.Invoke(this);
                gameObject.SetActive(false);
                StopAllCoroutines();
            }
        }
    }
}