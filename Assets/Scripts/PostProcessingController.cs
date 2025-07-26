using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public class PostProcessingController : MonoBehaviour
{
    MotionBlur blur;
    LensDistortion distortion;
    [SerializeField] float speedUp, speedDown;
    [SerializeField] float intensityOfBlur;
    [SerializeField] float intensityOfDistortion;
    bool startEffect;
    private void Awake()
    {
        var volume = GetComponent<Volume>();
        if (volume.profile.TryGet(out MotionBlur tmp) && volume.profile.TryGet(out LensDistortion tmp2))
        {
            blur = tmp;
            blur.active = true;

            distortion = tmp2;
            distortion.active = true;

            //Default values
            blur.intensity.value = 0;
            distortion.intensity.value = 0;
        }
        else
        {
            Debug.LogError("Couldnt assign Post Processing effect");
        }
    }
    private void Update()
    {
        if(startEffect)
        {
            //Debug.Log($"Blur: {blur.intensity.value}, Distortion: {distortion.intensity.value}");
            // Move blur intensity toward 0
            blur.intensity.value = Mathf.MoveTowards(blur.intensity.value, intensityOfBlur, speedUp * Time.deltaTime);

            // Move distortion intensity toward 0
            distortion.intensity.value = Mathf.Lerp(distortion.intensity.value, intensityOfDistortion, speedUp * Time.deltaTime);

            // If both are "close enough", stop the effect
            if (Mathf.Abs(blur.intensity.value - intensityOfBlur) < 0.01f &&
                Mathf.Abs(distortion.intensity.value - intensityOfDistortion) < 0.01f)
            {
                startEffect = false;
            }

        }
        else
        {
            //Debug.Log($"Blur: {blur.intensity.value}, Distortion: {distortion.intensity.value}");
            // Move blur intensity toward 0
            blur.intensity.value = Mathf.MoveTowards(blur.intensity.value, 0f, speedDown * Time.deltaTime);

            // Move distortion intensity toward 0
            distortion.intensity.value = Mathf.Lerp(distortion.intensity.value, 0f, speedDown * Time.deltaTime);
        }

    }

    private void Dashed(bool isDashing)
    {
        if (!isDashing) return;
        startEffect = true;
    }
    private void OnEnable()
    {
        EventBus.Dashing += Dashed;
    }
    private void OnDisable()
    {
        EventBus.Dashing -= Dashed;
    }
}
