using UnityEngine;

namespace Mkey
{
    public class FishBehavior : MonoBehaviour
    {
        [SerializeField]
        private SceneCurve [] pathes;
        [SerializeField]
        private float speed = 5f;

        #region temp vars
        private TweenSeq animSeq;
        private Transform rel;
        #endregion temp vars

        #region regular
        private void Start()
        {
            // temp transform for move path
            rel = new GameObject().transform;
            rel.position = transform.position;
            rel.parent = null;

            transform.localPosition = Vector3.zero;
            animSeq = new TweenSeq();
            float locScale = transform.localScale.x;

            animSeq.Add((callBack) => // scale out
            {
                SimpleTween.Value(gameObject, locScale, locScale * 1.2f, 0.20f).SetOnUpdate((float val) =>
                {
                    if(transform) transform.localScale = new Vector3(val, val, val);
                }).AddCompleteCallBack(callBack);
            });

            animSeq.Add((callBack) =>  //scale in
            {
                SimpleTween.Value(gameObject, locScale * 1.2f, locScale, 0.20f).SetOnUpdate((float val) =>
                      {
                    if(transform) transform.localScale = new Vector3(val, val, val);
                }).AddCompleteCallBack(callBack);
            });

            animSeq.Add((callBack) =>
            {
                SceneCurve path = pathes[Random.Range(0, pathes.Length)];
                if (path) path.MoveAlongPath(gameObject, rel, path.Length / speed, 0, EaseAnim.EaseInOutSine, callBack);
                else callBack();
            });

            animSeq.Add((callBack) =>
            {
                Destroy(gameObject);
                callBack();
            });

            animSeq.Start();
        }

        private void OnDestroy()
        {
            SimpleTween.Cancel(gameObject, false);
            if(animSeq!=null) animSeq.Break();
            if (rel) Destroy(rel.gameObject);
        }
        #endregion regular
    }
}