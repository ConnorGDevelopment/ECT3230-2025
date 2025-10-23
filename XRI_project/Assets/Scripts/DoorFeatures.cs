using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class DoorFeatures : CoreFeatures {
    [Header("Door Configurations")]
    [SerializeField]
    private Transform _transform;

    [SerializeField]
    private float _maximumAngle = 90.0f;

    [SerializeField]
    private bool _reverseDirection = false;

    [SerializeField]
    private bool _reverseAngleDirection = false;

    [SerializeField]
    private float _doorSpeed = 2.0f;

    [SerializeField]
    private bool _open = false;

    [SerializeField]
    private bool _makeKinematicOnOpen = false;

    [Header("Interactions Configuration")]
    [SerializeField]
    private XRSocketInteractor _socketInteractor;

    [SerializeField]
    private XRSimpleInteractable _simpleInteractable;

    [SerializeField]
    private Transform _doorPivot;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start() {
        // When key gets close to the socket, add a listener
        _socketInteractor?.selectEntered.AddListener(
            selectEnteredEventArgs => {
                OpenDoor();
                //PlayOnStart();

            }
        );

        _socketInteractor?.selectExited.AddListener(selectExitEventArgs => {
            CloseDoor();
            PlayOnEnd();
            _socketInteractor.socketActive = featureUsage == FeatureUsage.Once ? false : true;
        });

    }

    // Update is called once per frame
    void Update() {

    }

    public void OpenDoor() {
        if (!_open) {
            PlayOnStart();
            _open = true;
            StartCoroutine(_processMotion());
        }
    }

    private IEnumerator _processMotion() {
        while(_open) {
            var angle = _doorPivot.localEulerAngles.y < 180 ? _doorPivot.localEulerAngles.y : _doorPivot.localEulerAngles.y - 360;

            angle = _reverseAngleDirection ? Mathf.Abs(angle) : angle;

            if (angle < _maximumAngle) {
                _doorPivot.Rotate(Vector3.up, _doorSpeed * Time.deltaTime * (_reverseAngleDirection ? -1 : 1));
            } else {
                _open = false;
                var featureRigidBody = GetComponent<Rigidbody>();
                if (featureRigidBody != null && _makeKinematicOnOpen) {
                    featureRigidBody.isKinematic = true;
                }
            }

                yield return null;
        }
    }

    public void CloseDoor() {
        return; 
    }
}
