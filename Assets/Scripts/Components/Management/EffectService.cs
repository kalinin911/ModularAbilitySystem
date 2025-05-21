using Assets.Scripts.AbilitySystem.Core;
using UnityEngine;

namespace Assets.Scripts.Components.Management
{
    public class EffectService : MonoBehaviour, IEffectService
    {
        [SerializeField] private GameObject circleSlashVFXPrefab;
        [SerializeField] private GameObject quickSlashVFXPrefab;
        [SerializeField] private GameObject stompVFXPrefab;
        [SerializeField] private GameObject damageTextPrefab;

        public void PlayCircleSlashEffect(Vector3 position)
        {
            if (circleSlashVFXPrefab != null)
            {
                GameObject vfx = Instantiate(circleSlashVFXPrefab, position, Quaternion.identity);
                Destroy(vfx, 2f);
            }
            else
            {
                // Simple fallback if no prefab is assigned
                StartCoroutine(SimpleCircleEffect(position, 3f, Color.blue, 1f));
            }
        }

        public void PlayQuickSlashEffect(Vector3 position, Vector3 direction)
        {
            if (quickSlashVFXPrefab != null)
            {
                GameObject vfx = Instantiate(quickSlashVFXPrefab, position, Quaternion.LookRotation(direction));
                Destroy(vfx, 2f);
            }
            else
            {
                // Simple fallback if no prefab is assigned
                StartCoroutine(SimpleRectEffect(position, direction, new Vector3(2f, 1f, 2f), Color.cyan, 0.5f));
            }
        }

        public void PlayStompEffect(Vector3 position)
        {
            if (stompVFXPrefab != null)
            {
                GameObject vfx = Instantiate(stompVFXPrefab, position, Quaternion.identity);
                Destroy(vfx, 2f);
            }
            else
            {
                // Simple fallback if no prefab is assigned
                StartCoroutine(SimpleCircleEffect(position, 2f, Color.yellow, 1f));
            }
        }

        public void ShowDamageText(Vector3 position, float amount)
        {
            if (damageTextPrefab != null)
            {
                GameObject text = Instantiate(damageTextPrefab, position + Vector3.up, Quaternion.identity);
                text.GetComponent<TextMesh>().text = amount.ToString("0");
                Destroy(text, 1f);
            }
        }

        // Simple circle effect with lines
        private System.Collections.IEnumerator SimpleCircleEffect(Vector3 position, float radius, Color color, float duration)
        {
            float timer = 0f;
            int segments = 20;

            while (timer < duration)
            {
                float progress = timer / duration;
                float currentRadius = radius * progress;

                for (int i = 0; i < segments; i++)
                {
                    float angle1 = i * Mathf.PI * 2f / segments;
                    float angle2 = (i + 1) * Mathf.PI * 2f / segments;

                    Vector3 point1 = position + new Vector3(Mathf.Cos(angle1), 0.05f, Mathf.Sin(angle1)) * currentRadius;
                    Vector3 point2 = position + new Vector3(Mathf.Cos(angle2), 0.05f, Mathf.Sin(angle2)) * currentRadius;

                    Debug.DrawLine(point1, point2, color, 0.1f);
                }

                timer += Time.deltaTime;
                yield return null;
            }
        }

        // Simple rectangle effect with lines
        private System.Collections.IEnumerator SimpleRectEffect(Vector3 position, Vector3 direction, Vector3 size, Color color, float duration)
        {
            float timer = 0f;
            Quaternion rotation = Quaternion.LookRotation(direction);

            while (timer < duration)
            {
                float progress = timer / duration;

                // Calculate rectangle points
                Vector3 forward = rotation * Vector3.forward * size.z * progress;
                Vector3 right = rotation * Vector3.right * size.x * 0.5f * progress;
                Vector3 up = Vector3.up * size.y * 0.5f * progress;

                Vector3 bottomFrontLeft = position + forward - right - up;
                Vector3 bottomFrontRight = position + forward + right - up;
                Vector3 bottomBackLeft = position - right - up;
                Vector3 bottomBackRight = position + right - up;

                Vector3 topFrontLeft = position + forward - right + up;
                Vector3 topFrontRight = position + forward + right + up;
                Vector3 topBackLeft = position - right + up;
                Vector3 topBackRight = position + right + up;

                // Draw bottom rectangle
                Debug.DrawLine(bottomBackLeft, bottomBackRight, color, 0.1f);
                Debug.DrawLine(bottomBackRight, bottomFrontRight, color, 0.1f);
                Debug.DrawLine(bottomFrontRight, bottomFrontLeft, color, 0.1f);
                Debug.DrawLine(bottomFrontLeft, bottomBackLeft, color, 0.1f);

                // Draw top rectangle
                Debug.DrawLine(topBackLeft, topBackRight, color, 0.1f);
                Debug.DrawLine(topBackRight, topFrontRight, color, 0.1f);
                Debug.DrawLine(topFrontRight, topFrontLeft, color, 0.1f);
                Debug.DrawLine(topFrontLeft, topBackLeft, color, 0.1f);

                // Draw connecting lines
                Debug.DrawLine(bottomBackLeft, topBackLeft, color, 0.1f);
                Debug.DrawLine(bottomBackRight, topBackRight, color, 0.1f);
                Debug.DrawLine(bottomFrontRight, topFrontRight, color, 0.1f);
                Debug.DrawLine(bottomFrontLeft, topFrontLeft, color, 0.1f);

                timer += Time.deltaTime;
                yield return null;
            }
        }
    }
}
