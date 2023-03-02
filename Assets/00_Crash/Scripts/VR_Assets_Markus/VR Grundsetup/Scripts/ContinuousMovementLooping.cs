using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class ContinuousMovementLooping : MonoBehaviour
{
    public float speed = 1;
    public XRNode inputSource;
    public float gravity = -9.81f;
    public LayerMask groundLayer;
    public float additionalHeight = 0.2f;

    private float fallingSpeed;
    private XRRig rig;
    private Vector2 inputAxis;
    // private CharacterController character;
    private InputDevice device;

    // for looping
    [SerializeField] CapsuleCollider characterCapsule = null;

    // Start is called before the first frame update
    void Start()
    {
        characterCapsule = GetComponent<CapsuleCollider>();
        // character = GetComponent<CharacterController>();
        rig = GetComponent<XRRig>();
        device = InputDevices.GetDeviceAtXRNode(inputSource);

        
    }

    // Update is called once per frame
    void Update()
    {
        
        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis);

        
    }

    

    private void FixedUpdate()
    {
        FollowHeadset();

        //gravity

        bool isGrounded = CheckIfGrounded();
        if (isGrounded)
            fallingSpeed = 0;
        else
            fallingSpeed += gravity * Time.fixedDeltaTime;
        
    }

    private void FollowHeadset()
    {
        CapsuleFollowHeadset();

        //Quaternion headYaw = Quaternion.Euler(0, rig.cameraGameObject.transform.eulerAngles.y, 0);

        Quaternion headYaw = Quaternion.Euler(0, rig.cameraGameObject.transform.eulerAngles.y, 0);

        Vector3 headYawEulers = headYaw.eulerAngles;

        Vector3 headYawLocalEulers = headYaw.eulerAngles + transform.rotation.eulerAngles;

        Quaternion headYawLocalEulersQuaternion = Quaternion.Euler(headYawLocalEulers);

        Quaternion headYawLocal = headYaw * transform.rotation;

        


        Vector3 direction = headYawLocalEulersQuaternion *   new Vector3(inputAxis.x, 0, inputAxis.y);

        //new Vector3(inputAxis.x, 0, inputAxis.y)

        

        Move(direction * Time.fixedDeltaTime * speed);



        Move(transform.up * fallingSpeed * Time.fixedDeltaTime);

        Debug.DrawRay(rig.cameraGameObject.transform.position, -transform.up, Color.red);

    }

    void CapsuleFollowHeadset()
    {
        characterCapsule.height = rig.cameraInRigSpaceHeight + additionalHeight;
        Vector3 capsuleCenter = transform.InverseTransformPoint(rig.cameraGameObject.transform.position);
        characterCapsule.center = new Vector3(capsuleCenter.x, characterCapsule.height/2, capsuleCenter.z);
    }

    bool CheckIfGrounded()
    {
        //tells us if on ground
        Vector3 rayStart = transform.TransformPoint(characterCapsule.center);
        float rayLength = characterCapsule.center.y + 0.01f;
        bool hasHit = Physics.SphereCast(rayStart, characterCapsule.radius, - transform.up, out RaycastHit hitInfo, rayLength, groundLayer);
        return hasHit;
    }

    private void Move(Vector3 direction)
    {
        print("move");
        transform.position += direction;
    }
}
