using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Perception.Randomization.Randomizers;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using FloatParameter = UnityEngine.Perception.Randomization.Parameters.FloatParameter;
using UnityEngine.Perception.Randomization.Samplers;

[Serializable]

[AddRandomizerMenu("Perception/Custom Camera Randomizer")]
public class CustomCameraRandomizer : Randomizer
{

    public bool useVariableFieldOfView = true;
    public FloatParameter cameraFieldOfViewParameter = new FloatParameter { value = new UniformSampler(5.0f, 50.0f) };

    public bool usePhysicalCamera = true;
    public FloatParameter cameraFocalLengthParameter = new FloatParameter { value = new UniformSampler(1.0f, 23.0f) };

    public bool useMovingCamera = true;
    public Vector3 initialCameraPosition;

    public bool useRotatingCamera = true;
    public Vector3 initialCameraRotation;

    private FloatParameter randomFloat = new FloatParameter { value = new UniformSampler(0, 1) };

    public FloatParameter randomFloatx = new FloatParameter { value = new UniformSampler(0, 1) };
    public FloatParameter randomFloaty = new FloatParameter { value = new UniformSampler(0, 1) };
    public FloatParameter randomFloatz = new FloatParameter { value = new UniformSampler(0, 1) };

    public FloatParameter randomFloatrx = new FloatParameter { value = new UniformSampler(0, 1) };
    public FloatParameter randomFloatry = new FloatParameter { value = new UniformSampler(0, 1) };
    public FloatParameter randomFloatrz = new FloatParameter { value = new UniformSampler(0, 1) };

    protected override void OnIterationStart()
    {
        var taggedForegroundObjs = tagManager.Query<CustomForegroundScaleRandomizerTag>().ToList(); //assuming there is one foreground object already placed
        var taggedObjects = tagManager.Query<CustomCameraRandomizerTag>();

        foreach (var taggedObject in taggedObjects)
        {
            var volume = taggedObject.GetComponent<Camera>();

            // change Field of View
            if (useVariableFieldOfView)
            {                
                float newFoV = cameraFieldOfViewParameter.Sample();
                volume.fieldOfView = newFoV;
            }

            // change Focal Length
            if (usePhysicalCamera)
            {
                float newFL = cameraFocalLengthParameter.Sample();
                volume.focalLength = newFL;
            }

            // transform the camera
            if (useMovingCamera)
            {
                float x_value, y_value, z_value;

                x_value = initialCameraPosition[0] + randomFloatx.Sample();
                y_value = initialCameraPosition[1] + randomFloaty.Sample();
                z_value = initialCameraPosition[2] + randomFloatz.Sample();

                volume.transform.position = new Vector3(x_value, y_value, z_value);
                
                if (Vector3.Distance(volume.transform.position, taggedForegroundObjs[0].GetComponent<Transform>().position) < 2.4)
                {
                    volume.transform.position = new Vector3(x_value, y_value, z_value+1.9f);
                }

                float disttochar = Vector3.Distance(volume.transform.position, taggedForegroundObjs[0].GetComponent<Transform>().position);

                if (disttochar > 4.5f)
                {
                    volume.transform.position = new Vector3(x_value, y_value, z_value - (disttochar - 4.5f));
                }
            }

            // rotate the camera
            if (useRotatingCamera)
            {
                volume.transform.LookAt(taggedForegroundObjs[0].GetComponent<Transform>());

                volume.transform.rotation = volume.transform.rotation * Quaternion.Euler(new Vector3(-6.2f, 0.0f, 0.0f));
            }
        }
    }
}