using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace AimingTrainingRoom
{
    public class Dartboard : ReactiveTarget
    {
        [SerializeField] float baseSpeed = 1f;
        [SerializeField] float difficulty = 1.0f;
    
        private void Start()
        {
            float speed = baseSpeed * difficulty;

            transform.DOMoveX(5, 2 / speed) // 2 is the duration
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
        }
        public void ReactToHit()
        {
            Debug.Log("Попал в мишень.");
            StartCoroutine(Die());
        }
        private IEnumerator Die()
        {
            this.transform.Rotate(-75, 0, 0);
            yield return new WaitForSeconds(1.5f);
            Destroy(this.gameObject);
        }
    }
}