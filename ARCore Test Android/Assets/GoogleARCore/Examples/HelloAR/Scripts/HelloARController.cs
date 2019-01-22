﻿//-----------------------------------------------------------------------
// <copyright file="HelloARController.cs" company="Google">
//
// Copyright 2017 Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------

#define CUSTOM

public enum ButtonType
{
	NULL = 0,
	Swim,
	Stand,
	Jump,
	Walk,
	Attack,

	Remove,
	Screenshot
}

namespace GoogleARCore.Examples.HelloAR
{
    using System.Collections.Generic;
	using System.Collections;
    using GoogleARCore;
    using GoogleARCore.Examples.Common;
    using UnityEngine;

#if UNITY_EDITOR
    // Set up touch input propagation while using Instant Preview in the editor.
    using Input = InstantPreviewInput;
#endif

    /// <summary>
    /// Controls the HelloAR example.
    /// </summary>
    public class HelloARController : MonoBehaviour
    {
		public enum ModelConst {
			M_NUMBER = 1
			,
		}

        /// <summary>
        /// The first-person camera being used to render the passthrough camera image (i.e. AR background).
        /// </summary>
        public Camera FirstPersonCamera;

        /// <summary>
        /// A prefab for tracking and visualizing detected planes.
        /// </summary>
        public GameObject DetectedPlanePrefab;

#if CUSTOM
		/// <summary>
		/// Custom Prefab. commented by junho
		/// </summary>
		public GameObject CustomPrefab;

		public GameObject EnvironPrefab;

		public GameObject ButtonContainer;

		public FadeController m_fader;

		public float perspectiveZoomSpeed = 0.5f;        // The rate of change of the field of view in perspective mode.
		public float orthoZoomSpeed = 0.5f;        // The rate of change of the orthographic size in orthographic mode.

		Anchor m_anchor;

#else
		/// <summary>
		/// A model to place when a raycast from a user touch hits a plane.
		/// </summary>
		public GameObject AndyPlanePrefab;


        /// <summary>
        /// A model to place when a raycast from a user touch hits a feature point.
        /// </summary>
        public GameObject AndyPointPrefab;
#endif
		/// <summary>
		/// A game object parenting UI for displaying the "searching for planes" snackbar.
		/// </summary>
		public GameObject SearchingForPlaneUI;

		/// <summary>
		/// The rotation in degrees need to apply to model when the Andy model is placed.
		/// </summary>
		private const float k_ModelRotation = 180.0f;

        /// <summary>
        /// A list to hold all planes ARCore is tracking in the current frame. This object is used across
        /// the application to avoid per-frame allocations.
        /// </summary>
        private List<DetectedPlane> m_AllPlanes = new List<DetectedPlane>();

        /// <summary>
        /// True if the app is in the process of quitting due to an ARCore connection error, otherwise false.
        /// </summary>
        private bool m_IsQuitting = false;

		public void Start()
		{
			StartCoroutine(Activate());
		}

		/// <summary>
		/// The Unity Update() method.
		/// </summary>
		public void Update()
		{
			_UpdateApplicationLifecycle();

			// Hide snackbar when currently tracking at least one plane.
			Session.GetTrackables<DetectedPlane>(m_AllPlanes);
			bool showSearchingUI = true;
			for (int i = 0; i < m_AllPlanes.Count; i++)
			{
				if (m_AllPlanes[i].TrackingState == TrackingState.Tracking)
				{
					showSearchingUI = false;
					break;
				}
			}

			SearchingForPlaneUI.SetActive(showSearchingUI);

			// If there are two touches on the device...
			if (PenguinBehaviour.Pengnum == (int)ModelConst.M_NUMBER && Input.touchCount == 2)
			{
				// Store both touches.
				Touch touchZero = Input.GetTouch(0);
				Touch touchOne = Input.GetTouch(1);

				// Find the position in the previous frame of each touch.
				Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
				Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

				// Find the magnitude of the vector (the distance) between the touches in each frame.
				float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
				float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

				// Find the difference in the distances between each frame.
				float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

				Vector3 m_anchorvec = m_anchor.transform.localScale;
				m_anchorvec += new Vector3(0.01f, 0.01f, 0.01f) * deltaMagnitudeDiff;

				m_anchorvec.x = Mathf.Clamp(m_anchorvec.x, 0.5f, 2.5f);
				m_anchorvec.y = Mathf.Clamp(m_anchorvec.y, 0.5f, 2.5f);
				m_anchorvec.z = Mathf.Clamp(m_anchorvec.z, 0.5f, 2.5f);
				m_anchor.transform.localScale = m_anchorvec;
			}

			// If the player has not touched the screen, we are done with this update.
			Touch touch;
			if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
			{
				return;
			}

			// Raycast against the location the player touched to search for planes.
			TrackableHit hit;
			TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon |
				TrackableHitFlags.FeaturePointWithSurfaceNormal;

#if CUSTOM
			// button control part using raycast
			#region
			/*
			switch (touch.phase)
			{
				case TouchPhase.Began:
					Vector3 touchPosToVector3 = new Vector3(touch.position.x, touch.position.y, 100);
					//touchPos = FirstPersonCamera.ScreenToWorldPoint(touchPosToVector3);
					ray = FirstPersonCamera.ScreenPointToRay(touchPosToVector3);

					// thrid param : distance
					if (Physics.Raycast(ray, out rayhit, 100))
					{
						if (rayhit.collider.tag == "Cube")
						{
							// maybe can approach animator of parent
							// rayhit.collider.GetComponentsInParent<>;
							animator.SetTrigger(atkhash);
							return;
						}
					}
					break;
				case TouchPhase.Moved:
					// 터치 이동 시.
					// Debug.Log("터치 이동");
					break;

				case TouchPhase.Stationary:
					// 터치 고정 시.
					// Debug.Log("터치 고정");
					break;

				case TouchPhase.Ended:
					// 터치 종료 시. ( 손을 뗐을 시 )
					// Debug.Log("터치 종료");
					break;

				case TouchPhase.Canceled:
					// 터치 취소 시. ( 시스템에 의해서 터치가 취소된 경우 (ex: 전화가 왔을 경우 등) )
					// Debug.Log("터치 취소");
					break;
			}
			*/
			#endregion

			// put model on the polygon floor
			if (PenguinBehaviour.Pengnum < (int)ModelConst.M_NUMBER && Frame.Raycast(touch.position.x, touch.position.y, raycastFilter, out hit) )
            {
				// Use hit pose and camera pose to check if hittest is from the
				// back of the plane, if it is, no need to create the anchor.
				if ((hit.Trackable is DetectedPlane) &&
                    Vector3.Dot(FirstPersonCamera.transform.position - hit.Pose.position,
                        hit.Pose.rotation * Vector3.up) < 0)
                {
                    Debug.Log("Hit at back of the current DetectedPlane");
                }
                else
                {
					// Choose the Andy model for the Trackable that got hit.

					// Animator needed for each instantiated GameObject
					GameObject prefab;
					GameObject eprefab;
					// Animator animator;
					
					if (hit.Trackable is FeaturePoint)
                    {
						//prefab = CustomPrefab;
						return;
					}
                    else
                    {
						prefab = CustomPrefab;
						eprefab = EnvironPrefab;
					}

					// Instantiate Andy model at the hit pose.
					//hit.Pose.position + new Vector3(0, 0.1f, 0);
					var customObject = Instantiate(prefab, hit.Pose.position + new Vector3(0, 0.005f, 0), hit.Pose.rotation);

					var customEnvironObject = Instantiate(eprefab, hit.Pose.position + new Vector3(0, 0.005f, 0), hit.Pose.rotation);

					// Compensate for the hitPose rotation facing away from the raycast (i.e. camera).
					customObject.transform.Rotate(0, k_ModelRotation, 0, Space.Self);

					// Create an anchor to allow ARCore to track the hitpoint as understanding of the physical
					// world evolves.
					var anchor = hit.Trackable.CreateAnchor(hit.Pose);

					// Make Andy model a child of the anchor.
					customObject.transform.parent = anchor.transform;
					customEnvironObject.transform.parent = anchor.transform;

					m_anchor = anchor;

					//GameObject.Find("Plane Generator").transform.chi
					Transform trPlaneGenObj = GameObject.Find("Plane Generator").transform;
					for (int i = 0; i < trPlaneGenObj.childCount; i++)
					{
						trPlaneGenObj.GetChild(i).gameObject.SetActive(false);
					}
					ButtonContainer.SetActive(true);
					GameObject.Find("peng_roundimg").SetActive(false);
#else
            if (Frame.Raycast(touch.position.x, touch.position.y, raycastFilter, out hit))
            {
				// Use hit pose and camera pose to check if hittest is from the
				// back of the plane, if it is, no need to create the anchor.
				if ((hit.Trackable is DetectedPlane) &&
                    Vector3.Dot(FirstPersonCamera.transform.position - hit.Pose.position,
                        hit.Pose.rotation * Vector3.up) < 0)
                {
                    Debug.Log("Hit at back of the current DetectedPlane");
                }
                else
                {
					// Choose the Andy model for the Trackable that got hit.
					GameObject prefab;
                    if (hit.Trackable is FeaturePoint)
                    {
                        prefab = AndyPointPrefab;
                    }
                    else
                    {
                        prefab = AndyPlanePrefab;
                    }

					// Instantiate Andy model at the hit pose.
					var andyObject = Instantiate(prefab, hit.Pose.position, hit.Pose.rotation);

                    // Compensate for the hitPose rotation facing away from the raycast (i.e. camera).
                    andyObject.transform.Rotate(0, k_ModelRotation, 0, Space.Self);

                    // Create an anchor to allow ARCore to track the hitpoint as understanding of the physical
                    // world evolves.
                    var anchor = hit.Trackable.CreateAnchor(hit.Pose);

                    // Make Andy model a child of the anchor.
                    andyObject.transform.parent = anchor.transform;
#endif
				}
			}
        }

        /// <summary>
        /// Check and update the application lifecycle.
        /// </summary>
        private void _UpdateApplicationLifecycle()
        {
            // Exit the app when the 'back' button is pressed.
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }

            // Only allow the screen to sleep when not tracking.
            if (Session.Status != SessionStatus.Tracking)
            {
                const int lostTrackingSleepTimeout = 15;
                Screen.sleepTimeout = lostTrackingSleepTimeout;
            }
            else
            {
                Screen.sleepTimeout = SleepTimeout.NeverSleep;
            }

            if (m_IsQuitting)
            {
                return;
            }

            // Quit if ARCore was unable to connect and give Unity some time for the toast to appear.
            if (Session.Status == SessionStatus.ErrorPermissionNotGranted)
            {
                _ShowAndroidToastMessage("Camera permission is needed to run this application.");
                m_IsQuitting = true;
                Invoke("_DoQuit", 0.5f);
            }
            else if (Session.Status.IsError())
            {
                _ShowAndroidToastMessage("ARCore encountered a problem connecting.  Please start the app again.");
                m_IsQuitting = true;
                Invoke("_DoQuit", 0.5f);
            }
        }

        /// <summary>
        /// Actually quit the application.
        /// </summary>
        private void _DoQuit()
        {
            Application.Quit();
        }

        /// <summary>
        /// Show an Android toast message.
        /// </summary>
        /// <param name="message">Message string to show in the toast.</param>
        private void _ShowAndroidToastMessage(string message)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            if (unityActivity != null)
            {
                AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
                unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity,
                        message, 0);
                    toastObject.Call("show");
                }));
            }
        }

		IEnumerator Activate()
		{
			m_fader.FadeOut(3.0f, () => {
				Debug.Log("Fade out");
			});
			yield return null;
		}
	}
}
