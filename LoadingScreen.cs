using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadingScreen : MonoBehaviour
{
    private AsyncOperation loadOperation;

    [SerializeField] private Image progressBar;
    [SerializeField] private TextMeshProUGUI percentText;

    private float currentValue;
    private float targetValue;

    [SerializeField][Range(0, 1)]
    private float progressAnimationMultiplier = 0.15f;

    private void Start()
    {
        progressBar.fillAmount = currentValue = targetValue = 0;

        var currentScene = SceneManager.GetActiveScene();
        loadOperation = SceneManager.LoadSceneAsync(currentScene.buildIndex + 1);

        loadOperation.allowSceneActivation = false;
    }

    private void Update()
    {
        targetValue = loadOperation.progress / 0.9f;

        currentValue = Mathf.MoveTowards(currentValue, targetValue, progressAnimationMultiplier * Time.deltaTime);
        progressBar.fillAmount = currentValue;
        float InPercent = currentValue * 100;
        percentText.text = string.Format("{0}%", InPercent.ToString("F0"));

        if (Mathf.Approximately(currentValue, 1))
        {
            loadOperation.allowSceneActivation = true;
        }
    }
}
