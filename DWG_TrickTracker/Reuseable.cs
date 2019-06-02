using UnityEngine;
using System;

namespace DWG_TT
{

    class FNCS
    {
        static public float Get2DDist(Vector3 p_basePnt, Vector3 p_toPnt)
        {
            float deltaX = (p_basePnt.x - p_toPnt.x);
            float deltaZ = (p_basePnt.z - p_toPnt.z);
            return (float)Math.Sqrt(deltaX * deltaX + deltaZ * deltaZ);
        }

        static public float Get3DDist(Vector3 p_basePnt, Vector3 p_toPnt)
        {
            float deltaX = (p_basePnt.x - p_toPnt.x);
            float deltaY = (p_basePnt.y - p_toPnt.y);
            float deltaZ = (p_basePnt.z - p_toPnt.z);
            return (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ);
        }

        static public double GetHghtDiff(Vector3 p_fromPos, Vector3 p_toPos)
        {
            return p_fromPos.y - p_toPos.y;
        }

        static public Vector3 GetPlanePos(Vector3 p_fromPos, Vector3 p_toPlane, Vector3 p_nrmlDir)
        {
            Vector3 checkV = p_fromPos.normalized;
            checkV = p_fromPos - p_toPlane;
            checkV = Vector3.Project(checkV, p_nrmlDir);
            return p_toPlane + checkV;
        }

        static public Tuple<Vector3, Quaternion, Vector3> GetWorldTransform()
        {
            // Needs updated to not require PlayerController

            // 2M to the right and 2m forward from the camera pos
            //Vector3 placePos = (PlayerController.Instance.cameraController.transform.position + PlayerController.Instance.cameraController.transform.right * 2);
            //placePos = (placePos + PlayerController.Instance.cameraController.transform.forward * 2);
            //PlayerController.Instance.cameraController._actualCam;

            // Board estimated contact pos
            //Vector3 placePos = (PlayerController.Instance.boardController.triggerManager.playerOffset.position);//PlayerController.Instance.boardController.triggerManager.grindOffset.position PlayerController.Instance.boardController.triggerManager.grindContactSplinePosition.position);
            Vector3 placePos = (PlayerController.Instance.boardController.boardTargetPosition.position);
            Quaternion placeRot = PlayerController.Instance.cameraController.transform.rotation;

            //dist = Vector3.Distance(cameraPos, textPos);
            Vector3 placeScale = new Vector3(1f, 1f, 1f);

            return Tuple.Create(placePos, placeRot, placeScale);
        }

    }
}
