using Assets.Scripts.Components.Camera;
using Assets.Scripts.Components.Character;
using UnityEngine;

namespace Assets.Scripts.Components.Management
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameObject _heroPrefab;
        [SerializeField] private GameObject _enemyPrefab;
        [SerializeField] private GameObject _groundPrefab;
        [SerializeField] private Vector3[] _enemyPositions = new Vector3[] { new Vector3(-3, 0.5f, 3), new Vector3(3, 0.5f, 3) };

        private void Awake()
        {
            // Ensure we have an AbilitySystemManager
            if (FindObjectOfType<AbilitySystemManager>() == null)
            {
                GameObject abilitySystemObj = new GameObject("AbilitySystem");
                abilitySystemObj.AddComponent<AbilitySystemManager>();
            }
        }

        private void Start()
        {
            SetupScene();
        }

        private void SetupScene()
        {
            // Create ground
            if (_groundPrefab != null)
                Instantiate(_groundPrefab, Vector3.zero, Quaternion.identity);

            // Create hero
            GameObject hero = null;
            if (_heroPrefab != null)
            {
                hero = Instantiate(_heroPrefab, new Vector3(0, 0.5f, 0), Quaternion.identity);
            }
            else
            {
                // Create a default hero if prefab is missing
                hero = GameObject.CreatePrimitive(PrimitiveType.Cube);
                hero.name = "Hero";
                hero.transform.position = new Vector3(0, 0.5f, 0);
                hero.GetComponent<Renderer>().material.color = Color.blue;

                // Add required components
                hero.AddComponent<HeroController>();
                hero.AddComponent<Health>();
                hero.AddComponent<Rigidbody>().constraints =
                    RigidbodyConstraints.FreezePositionY |
                    RigidbodyConstraints.FreezeRotationX |
                    RigidbodyConstraints.FreezeRotationZ;
            }

            // Create enemies
            if (_enemyPrefab != null)
            {
                foreach (var position in _enemyPositions)
                {
                    Instantiate(_enemyPrefab, position, Quaternion.identity);
                }
            }
            else
            {
                // Create default enemies if prefab is missing
                foreach (var position in _enemyPositions)
                {
                    GameObject enemy = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    enemy.name = "Enemy";
                    enemy.transform.position = position;
                    enemy.GetComponent<Renderer>().material.color = Color.red;

                    // Add required components
                    enemy.AddComponent<Health>();
                    enemy.AddComponent<Rigidbody>().constraints =
                        RigidbodyConstraints.FreezePositionY |
                        RigidbodyConstraints.FreezeRotationX |
                        RigidbodyConstraints.FreezeRotationZ;

                    // Set layer
                    enemy.layer = LayerMask.NameToLayer("Enemy");
                }
            }

            // Set up camera
            var camera = UnityEngine.Camera.main;
            if (camera != null)
            {
                IsometricCameraFollow cameraFollow = camera.GetComponent<IsometricCameraFollow>();
                if (cameraFollow == null)
                {
                    cameraFollow = camera.gameObject.AddComponent<IsometricCameraFollow>();
                }
                cameraFollow.SetTarget(hero.transform);
            }
        }
    }
}
