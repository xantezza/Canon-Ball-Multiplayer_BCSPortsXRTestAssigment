using Infrastructure;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utils.MonoBehaviours
{
    [DefaultExecutionOrder(-17000)]
    public class EntryPointLoader : MonoBehaviour
    {
        private void Awake()
        {
            if (EntryPoint.IsAwakened)
                Destroy(gameObject);
            else
                SceneManager.LoadScene(0);
        }
    }
}