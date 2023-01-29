using System.Collections.Generic;
using UnityEngine;

namespace Ipocom.SonyMotionFormat
{
    public static class Definition
    {
        public const int BONE_COUNT = 27;

        public static IReadOnlyDictionary<Bones, UnityEngine.HumanBodyBones> HumanBoneMap = new Dictionary<Bones, UnityEngine.HumanBodyBones>
        {
            { Bones.root , UnityEngine.HumanBodyBones.Hips},
            { Bones.torso_3 , UnityEngine.HumanBodyBones.Spine},
            { Bones.torso_5, UnityEngine.HumanBodyBones.Chest},
            { Bones.torso_6 , UnityEngine.HumanBodyBones.UpperChest},
            { Bones.neck_1 , UnityEngine.HumanBodyBones.Neck},
            { Bones.head, UnityEngine.HumanBodyBones.Head},
            { Bones.l_shoulder, UnityEngine.HumanBodyBones.LeftShoulder},
            { Bones.l_up_arm, UnityEngine.HumanBodyBones.LeftUpperArm},
            { Bones.l_low_arm, UnityEngine.HumanBodyBones.LeftLowerArm},
            { Bones.l_hand, UnityEngine.HumanBodyBones.LeftHand},
            { Bones.r_shoulder, UnityEngine.HumanBodyBones.RightShoulder},
            { Bones.r_up_arm, UnityEngine.HumanBodyBones.RightUpperArm},
            { Bones.r_low_arm, UnityEngine.HumanBodyBones.RightLowerArm},
            { Bones.r_hand, UnityEngine.HumanBodyBones.RightHand},
            { Bones.l_up_leg, UnityEngine.HumanBodyBones.LeftUpperLeg},
            { Bones.l_low_leg, UnityEngine.HumanBodyBones.LeftLowerLeg},
            { Bones.l_foot, UnityEngine.HumanBodyBones.LeftFoot},
            { Bones.l_toes, UnityEngine.HumanBodyBones.LeftToes},
            { Bones.r_up_leg, UnityEngine.HumanBodyBones.RightUpperLeg},
            { Bones.r_low_leg, UnityEngine.HumanBodyBones.RightLowerLeg},
            { Bones.r_foot, UnityEngine.HumanBodyBones.RightFoot},
            { Bones.r_toes, UnityEngine.HumanBodyBones.RightToes},
        };

        public static (Bones, Bones)[] HeadTailPairs = new (Bones, Bones)[]{
            (Bones.root, Bones.torso_1),
            (Bones.torso_1, Bones.torso_2),
            (Bones.torso_2, Bones.torso_3),
            (Bones.torso_3, Bones.torso_4),
            (Bones.torso_4, Bones.torso_5),
            (Bones.torso_5, Bones.torso_6),
            (Bones.torso_6, Bones.torso_7),
            (Bones.torso_7, Bones.neck_1),
            (Bones.neck_1, Bones.neck_2),
            (Bones.neck_2, Bones.head),
            // (Bones.head),
            (Bones.l_shoulder, Bones.l_up_arm),
            (Bones.l_up_arm, Bones.l_low_arm),
            (Bones.l_low_arm, Bones.l_hand),
            // (Bones.l_hand),
            (Bones.r_shoulder, Bones.r_up_arm),
            (Bones.r_up_arm, Bones.r_low_arm),
            (Bones.r_low_arm, Bones.r_hand),
            // (Bones.r_hand),
            (Bones.l_up_leg, Bones.l_low_leg),
            (Bones.l_low_leg, Bones.l_foot),
            (Bones.l_foot, Bones.l_toes),
            // (Bones.l_toes),
            (Bones.r_up_leg, Bones.r_low_leg),
            (Bones.r_low_leg, Bones.r_foot),
            (Bones.r_foot, Bones.r_toes),
            // (Bones.r_toes),
        };

        static readonly Color32 COLOR_WHITE = new Color32(255, 255, 255, 255);
        static readonly Color32 COLOR_GREEN = new Color32(210, 255, 64, 255);
        static readonly Color32 COLOR_HIP = new Color32(206, 234, 240, 255);
        static readonly Color32 COLOR_HEAD = new Color32(255, 132, 0, 255);
        static readonly Color32 COLOR_ANKLE = new Color32(220, 228, 67, 255);
        static readonly Color32 COLOR_WRIST = new Color32(193, 20, 162, 255);

        public static IReadOnlyDictionary<Bones, UnityEngine.Color32> ColorMap = new Dictionary<Bones, UnityEngine.Color32>
        {
            {Bones.root, COLOR_HIP},
            {Bones.torso_1, COLOR_GREEN},
            {Bones.torso_2, COLOR_GREEN},
            {Bones.torso_3, COLOR_GREEN},
            {Bones.torso_4, COLOR_GREEN},
            {Bones.torso_5, COLOR_GREEN},
            {Bones.torso_6, COLOR_GREEN},
            {Bones.torso_7, COLOR_GREEN},
            {Bones.neck_1, COLOR_WHITE},
            {Bones.neck_2, COLOR_WHITE},
            {Bones.head, COLOR_HEAD},
            {Bones.l_shoulder, COLOR_WHITE},
            {Bones.l_up_arm, COLOR_WHITE},
            {Bones.l_low_arm, COLOR_WHITE},
            {Bones.l_hand, COLOR_WRIST},
            {Bones.r_shoulder, COLOR_WHITE},
            {Bones.r_up_arm, COLOR_WHITE},
            {Bones.r_low_arm, COLOR_WHITE},
            {Bones.r_hand, COLOR_WRIST},
            {Bones.l_up_leg, COLOR_WHITE},
            {Bones.l_low_leg, COLOR_WHITE},
            {Bones.l_foot, COLOR_ANKLE},
            {Bones.l_toes, COLOR_WHITE},
            {Bones.r_up_leg, COLOR_WHITE},
            {Bones.r_low_leg, COLOR_WHITE},
            {Bones.r_foot, COLOR_ANKLE},
            {Bones.r_toes, COLOR_WHITE},
        };
    }
}
