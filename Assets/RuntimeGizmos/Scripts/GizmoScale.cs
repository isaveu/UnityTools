﻿using UnityEngine;

namespace RuntimeGizmos
{
    /// Handle Scale Gizmo at Runtime
    public class GizmoScale : MonoBehaviour
    {
        /// Scale speed scalar
        public float scaleSpeed = 7.5f;

        /// X handle of gizmo
        public GameObject xHandle;

        /// Y handle of gizmo
        public GameObject yHandle;

        /// Z handle of gizmo
        public GameObject zHandle;

        /// Components of each scaling handle
        public GameObject xCube, xCylinder, yCube, yCylinder, zCube, zCylinder;

        /// Center handle of gizmo
        public GameObject centerHandle;

        /// Target for scaling
        public GameObject scaleTarget;

        /// Array of detector scripts stored as [x, y, z, center]
        private GizmoClickDetection[] detectors;

        /// Initial local scales of the scaleTarget
        private float initialScaleX, initialScaleY, initialScaleZ;

        /// Previous local scale of gizmo before uniform scale
        private Vector3? previousGizmoScale;
    
        public void Awake()
        {
            // Get the initial scales
            initialScaleX = gameObject.transform.localScale.x;
            initialScaleY = gameObject.transform.localScale.y;
            initialScaleZ = gameObject.transform.localScale.z;

            // Get the click detection scripts
            detectors = new GizmoClickDetection[4];
            detectors[0] = xHandle.GetComponent<GizmoClickDetection>();
            detectors[1] = yHandle.GetComponent<GizmoClickDetection>();
            detectors[2] = zHandle.GetComponent<GizmoClickDetection>();
            detectors[3] = centerHandle.GetComponent<GizmoClickDetection>();

            // Set the same position for the target and the gizmo
            transform.position = scaleTarget.transform.position;
        }

        public void Update()
        {
            // Store the previous local scale of the gizmo
            if(Input.GetMouseButtonDown(0) && detectors[3].pressing)
            {
                previousGizmoScale = gameObject.transform.localScale;
            }
            else if (Input.GetMouseButtonUp(0) && previousGizmoScale != null)
            {
                gameObject.transform.localScale = ((Vector3) previousGizmoScale);
            }

            for (int i = 0; i < 4; i++)
            {
                if (Input.GetMouseButton(0) && detectors[i].pressing)
                {
                    switch (i)
                    {
                        // X Axis
                        case 0:
                        {
                            // Scale along the X axis
                            float delta = Input.GetAxis("Mouse X") * (Time.deltaTime);
                            delta *= scaleSpeed;

                            if ((scaleTarget.transform.localScale.x - delta) <= 0.01f) return;
                            scaleTarget.transform.localScale += new Vector3(-delta, 0.0f, 0.0f);

                            // Scale the handle's cylinder then move the cube to its new end
                            float lengthBefore = xCylinder.GetComponent<Renderer>().bounds.size.x;

                            xCylinder.transform.localScale += new Vector3(0.0f, 0.0f, -delta);
                            xCylinder.GetComponent<MeshFilter>().mesh.RecalculateBounds();
                            float lengthAfter = xCylinder.GetComponent<Renderer>().bounds.size.x;

                            xCube.transform.position += new Vector3(lengthAfter - lengthBefore, 0.0f, 0.0f);

                            xCylinder.transform.position = new Vector3(
                                lengthAfter / 2.0f,
                                xCylinder.transform.position.y,
                                xCylinder.transform.position.z
                            );

                            previousGizmoScale = null;
                        }
                            break;

                        // Y Axis
                        case 1:
                        {
                            // Scale along the Y axis
                            float delta = Input.GetAxis("Mouse Y") * (Time.deltaTime);
                            delta *= scaleSpeed;

                            if ((scaleTarget.transform.localScale.y + delta) <= 0.01f) return;
                            scaleTarget.transform.localScale += new Vector3(0.0f, delta, 0.0f);

                            // Scale the handle's cylinder then move the cube to its new end
                            float lengthBefore =yCylinder.GetComponent<Renderer>().bounds.size.y;

                            yCylinder.transform.localScale += new Vector3(0.0f, 0.0f, delta);
                            yCylinder.GetComponent<MeshFilter>().mesh.RecalculateBounds();
                            float lengthAfter = yCylinder.GetComponent<Renderer>().bounds.size.y;

                            yCube.transform.position += new Vector3(0.0f, lengthAfter - lengthBefore, 0.0f);

                            yCylinder.transform.position = new Vector3(
                                yCylinder.transform.position.x,
                                lengthAfter / 2.0f,
                                yCylinder.transform.position.z
                            );

                            previousGizmoScale = null;
                        }
                            break;

                        // Z Axis
                        case 2:
                        {
                            // Scale along the Z axis
                            float delta = Input.GetAxis("Mouse X") * (Time.deltaTime);
                            delta *= scaleSpeed;

                            if ((scaleTarget.transform.localScale.z + delta) <= 0.01f) return;
                            scaleTarget.transform.localScale += new Vector3(0.0f, 0.0f, delta);

                            // Scale the handle's cylinder then move the cube to its new end
                            float lengthBefore = zCylinder.GetComponent<Renderer>().bounds.size.z;

                            zCylinder.transform.localScale += new Vector3(0.0f, 0.0f, delta);
                            zCylinder.GetComponent<MeshFilter>().mesh.RecalculateBounds();
                            float lengthAfter = zCylinder.GetComponent<Renderer>().bounds.size.z;

                            zCube.transform.position += new Vector3(0.0f, 0.0f, lengthAfter - lengthBefore);

                            zCylinder.transform.position = new Vector3(
                                zCylinder.transform.position.x,
                                zCylinder.transform.position.y,
                                lengthAfter / 2.0f
                            );

                            previousGizmoScale = null;
                        }
                            break;

                        // Center (uniform scale)
                        case 3:
                        {
                            float delta = (Input.GetAxis("Mouse X") + Input.GetAxis("Mouse Y")) * (Time.deltaTime);
                            delta *= scaleSpeed;

                            if ((gameObject.transform.localScale.x + delta) <= (initialScaleX / 25.0f)) return;
                            if ((gameObject.transform.localScale.y + delta) <= (initialScaleY / 25.0f)) return;
                            if ((gameObject.transform.localScale.z + delta) <= (initialScaleZ / 25.0f)) return;

                            scaleTarget.transform.localScale += new Vector3(delta, delta, delta);
                            gameObject.transform.localScale += new Vector3(delta, delta, delta);
                        }
                            break;
                    }

                    break;
                }
            }
        }

    }
}
