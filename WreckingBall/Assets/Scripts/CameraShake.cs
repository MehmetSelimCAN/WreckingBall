using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    private IEnumerator IEShake() {
        Vector3 originalPosition = transform.localPosition;

        float elapsed = 0.0f;

        while (elapsed < 0.15f) {
            float x = Random.Range(-1f, 1f) * 0.4f;
            float y = Random.Range(-1f, 1f) * 0.4f;

            transform.localPosition = new Vector3(x, y, originalPosition.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPosition;
    }

    public void Shake() {
        StartCoroutine(IEShake());
    }

}
