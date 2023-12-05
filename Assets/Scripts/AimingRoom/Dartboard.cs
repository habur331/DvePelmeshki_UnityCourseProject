using System.Collections;
using AimingRoom;
using UnityEngine;
using DG.Tweening;

namespace AimingTrainingRoom
{
    public class Dartboard : MonoBehaviour,  IReactiveTarget
    {
        [SerializeField] float difficulty = 1.5f;
        [SerializeField] float speed = 0f;
        [SerializeField] float distanceToMove = 5.0f;
        
        private Tween _moveTween;
        
        
        public IEnumerator StartMoving(float level)
        {
            float raiseDuration = 0.3f;
         
            transform.DORotate(new Vector3(-90, 0, 0), raiseDuration, RotateMode.WorldAxisAdd);
            
            yield return new WaitForSeconds(raiseDuration);
            
            float levelFactor = 1.2f;

            if (level > 1)
                speed = difficulty * Mathf.Pow(levelFactor, (level));

            _moveTween = transform.DOMoveX(transform.position.x + distanceToMove, 2 / speed) // 2 is the duration
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
        }
        
        public void ReactToHit(int damage = 0)
        {
            Debug.Log("Попал в мишень.");
            
            FindObjectOfType<AimingRoom.AimingRoom>().MarkHitTarget(this);
            
            StartCoroutine(Die());
        }
        
        public IEnumerator Die()
        {
            _moveTween.Kill();
            this.transform.Rotate(75, 0, 0);
            yield return new WaitForSeconds(1.5f);
            
            FindObjectOfType<AimingRoom.AimingRoom>().RemoveDartboard(this);
            Destroy(this.gameObject);
        }
    }
}