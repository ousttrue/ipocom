using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace Ipocom
{
    public static class SonyMotionFormat
    {
        public static IReadOnlyDictionary<int, UnityEngine.HumanBodyBones> HumanBoneMap = new Dictionary<int, UnityEngine.HumanBodyBones>
        {
            { 0 , UnityEngine.HumanBodyBones.Hips           },
            { 3 , UnityEngine.HumanBodyBones.Spine          },
            { 5 , UnityEngine.HumanBodyBones.Chest          },
            { 6 , UnityEngine.HumanBodyBones.UpperChest     },
            { 8 , UnityEngine.HumanBodyBones.Neck           },
            { 10, UnityEngine.HumanBodyBones.Head            },
            { 11, UnityEngine.HumanBodyBones.LeftShoulder    },
            { 12, UnityEngine.HumanBodyBones.LeftUpperArm    },
            { 13, UnityEngine.HumanBodyBones.LeftLowerArm    },
            { 14, UnityEngine.HumanBodyBones.LeftHand        },
            { 15, UnityEngine.HumanBodyBones.RightShoulder   },
            { 16, UnityEngine.HumanBodyBones.RightUpperArm   },
            { 17, UnityEngine.HumanBodyBones.RightLowerArm   },
            { 18, UnityEngine.HumanBodyBones.RightHand       },
            { 19, UnityEngine.HumanBodyBones.LeftUpperLeg    },
            { 20, UnityEngine.HumanBodyBones.LeftLowerLeg    },
            { 21, UnityEngine.HumanBodyBones.LeftFoot        },
            { 22, UnityEngine.HumanBodyBones.LeftToes     },
            { 23, UnityEngine.HumanBodyBones.RightUpperLeg   },
            { 24, UnityEngine.HumanBodyBones.RightLowerLeg   },
            { 25, UnityEngine.HumanBodyBones.RightFoot       },
            { 26, UnityEngine.HumanBodyBones.RightToes    },
        };

        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct Transform
        {
            public float f0;
            public float f1;
            public float f2;
            public float f3;
            public float f4;
            public float f5;
            public float f6;

            public Quaternion Rotation()
            {
                return new Quaternion(
                    f0,
                    f1,
                    f2,
                    f3
                );
            }

            public Vector3 Translation()
            {
                return new Vector3(f4, f5, f6);
            }
        }

        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct BoneId
        {
            public UInt32 bnid0;
            public UInt16 bnid1;
        }

        // 80 byte
        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct MocopiHeader
        {
            public UInt32 magic;
            public UInt32 head_key;
            public UInt32 head;
            public UInt32 ftyp;
            // 22byte
            public UInt64 ftyp0;
            public UInt64 ftyp1;
            public UInt32 ftyp2;
            public UInt16 ftyp3;
            // 
            public UInt32 vrsn_key;
            public UInt32 vrsn0;
            public byte vrsn1;
            public UInt32 sndf_key;
            public UInt32 sndf;
            public UInt32 ipad_key; // 12
            public UInt64 ipad0;
            public UInt32 ipad1;
            public UInt32 rcvp_key; // 6
            public UInt32 rcvp0;
            public UInt16 rcvp1;
        }

        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct SkeletonHeader
        {
            public MocopiHeader header;
            public UInt32 skdf_key;
            public UInt32 skdf;
            public UInt32 bons;
        }

        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct SkeletonBone
        {
            public UInt32 magic; // 0x38 00 00 00
            public UInt32 bndt_key; // "bndt" => 4byte
            public UInt32 bndt;
            public UInt32 bnid_key; // "bndt" => 6byte
            public BoneId bnid; // BoNe ID ?
            public UInt32 pbid_key; // "pbid" => 6byte
            public BoneId pbid; // Parent Bone ID ?
            public UInt32 tran_key;
            public Transform tran;
        }

        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct FrameHeader
        {
            public MocopiHeader header;
            public UInt32 fram_key;
            public UInt32 fram;
            public UInt32 fnum_key;
            public UInt64 fnum;
            public UInt32 time_key;
            public UInt64 time;
            public UInt32 btrs_key;
        }

        // 54
        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct FrameBone
        {
            public UInt32 magic; // 0x2E 00 00 00
            public UInt32 btdt_key;
            public UInt32 btdt;
            public UInt32 bnid_key;
            public BoneId bnid;
            public UInt32 tran_key;
            public Transform tran;
        }

        const int BONE_COUNT = 27;
        static readonly int SKELETON_HEADER_SIZE = Marshal.SizeOf<SkeletonHeader>();
        static readonly int SKELETON_BONE_SIZE = Marshal.SizeOf<SkeletonBone>();
        static readonly int SKELETON_BYTES_SIZE = SKELETON_HEADER_SIZE + BONE_COUNT * SKELETON_BONE_SIZE;
        static readonly int FRAME_HEADER_SIZE = Marshal.SizeOf<FrameHeader>();
        static readonly int FRAME_BONE_SIZE = Marshal.SizeOf<FrameBone>();
        static readonly int FRAME_BYTES_SIZE = FRAME_HEADER_SIZE + BONE_COUNT * FRAME_BONE_SIZE;

        public enum FieldTypes
        {
            Head,
            Sndf,
            Skdf,
            Fram,
            Ftyp,
            Vrsn,
            Ipad,
            Rcvp,
            Fnum,
            Time,
            Btrs,
            Bons,
            Btdt,
            Bndt,
            Bnid,
            Tran,
            Pbid,
        }
        public static bool IsNested(this FieldTypes t)
        {
            switch (t)
            {
                case FieldTypes.Head:
                case FieldTypes.Sndf:
                case FieldTypes.Skdf:
                case FieldTypes.Fram:
                case FieldTypes.Bons:
                case FieldTypes.Btrs:
                case FieldTypes.Btdt:
                case FieldTypes.Bndt:
                    return true;
                default:
                    return false;
            }
        }

        static readonly IReadOnlyDictionary<UInt32, FieldTypes> s_keyMap = InitKeyMap();

        static Dictionary<UInt32, FieldTypes> InitKeyMap()
        {
            var map = new Dictionary<UInt32, FieldTypes>();
            foreach (FieldTypes type in Enum.GetValues(typeof(FieldTypes)))
            {
                var bytes = System.Text.Encoding.ASCII.GetBytes(type.ToString().ToLower());
                map.Add(BitConverter.ToUInt32(bytes, 0), type);
            }
            return map;
        }

        public static FieldTypes GetFieldType(ArraySegment<byte> key)
        {
            if (s_keyMap.TryGetValue(BitConverter.ToUInt32(key.Array, key.Offset), out FieldTypes type))
            {
                return type;
            }
            throw new ArgumentException($"unknown key: {System.Text.Encoding.ASCII.GetString(key.Array, key.Offset, key.Count)}");
        }

        public struct Field
        {
            public FieldTypes Type;
            public ArraySegment<byte> Value;

            public void Deconstruct(out FieldTypes type, out ArraySegment<byte> value)
            {
                type = Type;
                value = Value;
            }
        }

        public static Field ReadField(this BytesReader r)
        {
            var length = r.GetInt32();
            var type = r.Get(4);
            var value = r.Get(length);
            return new Field
            {
                Type = GetFieldType(type),
                Value = value,
            };
        }

        // https://github.com/seagetch/mcp-receiver/blob/main/doc/Protocol.md
        public static IEnumerable<Field> ParseFields(ArraySegment<byte> bytes)
        {
            var r = new BytesReader(bytes);
            while (!r.IsEnd)
            {
                yield return r.ReadField();
            }
        }

        [Serializable]
        public struct Head
        {
            public ArraySegment<byte> FileType;
            public byte Version;

            // head
            //   ftyp
            //   vrsn
            public static Head FromField(Field head)
            {
                if (head.Type != FieldTypes.Head)
                {
                    throw new ArgumentException("not head");
                }
                var it = ParseFields(head.Value).GetEnumerator();
                it.MoveNext();
                var ftyp = it.Current;
                it.MoveNext();
                var vrsn = it.Current;
                return new Head
                {
                    FileType = ftyp.Value,
                    Version = vrsn.Value.Array[vrsn.Value.Offset],
                };
            }
        }

        [Serializable]

        public struct Sndf
        {
            public ArraySegment<byte> IpAddress;
            public UInt16 ReceivePort;
            // sndf
            //   ipad
            //   rcvp
            public static Sndf FromField(Field sndf)
            {
                if (sndf.Type != FieldTypes.Sndf)
                {
                    throw new ArgumentException("not sndf");
                }
                var it = ParseFields(sndf.Value).GetEnumerator();
                it.MoveNext();
                var ipad = it.Current;
                it.MoveNext();
                var rcvp = it.Current;
                return new Sndf
                {
                    IpAddress = ipad.Value,
                    ReceivePort = BitConverter.ToUInt16(rcvp.Value.Array, rcvp.Value.Offset),
                };
            }
        }

        [Serializable]

        public struct Bnid
        {
            public UInt16 BoneId;
            public static Bnid FromField(Field bnid)
            {
                if (bnid.Type != FieldTypes.Bnid)
                {
                    throw new ArgumentException("not bnid");
                }
                return new Bnid
                {
                    BoneId = BitConverter.ToUInt16(bnid.Value.Array, bnid.Value.Offset),
                };
            }
        }

        [Serializable]

        public struct Pbid
        {
            public UInt16 ParentBoneId;
            public static Pbid FromField(Field pbid)
            {
                if (pbid.Type != FieldTypes.Pbid)
                {
                    throw new ArgumentException("not pbid");
                }
                return new Pbid
                {
                    ParentBoneId = BitConverter.ToUInt16(pbid.Value.Array, pbid.Value.Offset),
                };
            }
        }

        [Serializable]

        public struct Tran
        {
            public float rx;
            public float ry;
            public float rz;
            public float rw;
            public float tx;
            public float ty;
            public float tz;
            public static Tran FromField(Field tran)
            {
                if (tran.Type != FieldTypes.Tran)
                {
                    throw new ArgumentException("not tran");
                }
                return new Tran
                {
                    rx = BitConverter.ToSingle(tran.Value.Array, tran.Value.Offset),
                    ry = BitConverter.ToSingle(tran.Value.Array, tran.Value.Offset + 4),
                    rz = BitConverter.ToSingle(tran.Value.Array, tran.Value.Offset + 8),
                    rw = BitConverter.ToSingle(tran.Value.Array, tran.Value.Offset + 12),
                    tx = BitConverter.ToSingle(tran.Value.Array, tran.Value.Offset + 16),
                    ty = BitConverter.ToSingle(tran.Value.Array, tran.Value.Offset + 20),
                    tz = BitConverter.ToSingle(tran.Value.Array, tran.Value.Offset + 24),
                };
            }

            public Quaternion Rotation()
            {
                return new Quaternion(
                    rx,
                    ry,
                    rz,
                    rw
                );
            }

            public Vector3 Translation()
            {
                return new Vector3(tx, ty, tz);
            }
        }

        [Serializable]

        public struct Bndt
        {
            public Bnid BoneId;
            public Pbid ParentBoneId;
            public Tran Transformation;

            public static Bndt FromField(Field bndt)
            {
                if (bndt.Type != FieldTypes.Bndt)
                {
                    throw new ArgumentException("not bndt");
                }
                var it = ParseFields(bndt.Value).GetEnumerator();
                it.MoveNext();
                var bnid = it.Current;
                it.MoveNext();
                var pbid = it.Current;
                it.MoveNext();
                var tran = it.Current;
                return new Bndt
                {
                    BoneId = Bnid.FromField(bnid),
                    ParentBoneId = Pbid.FromField(pbid),
                    Transformation = Tran.FromField(tran),
                };
            }
        }

        [Serializable]

        public class Skdf
        {
            public Bndt[] Bones = new Bndt[BONE_COUNT];

            // skdf
            //   bons
            //     bndt x 27
            public static Skdf FromField(Field skdf)
            {
                if (skdf.Type != FieldTypes.Skdf)
                {
                    throw new ArgumentException("not skdf");
                }
                var bons = ReadField(new BytesReader(skdf.Value));
                if (bons.Type != FieldTypes.Bons)
                {
                    throw new ArgumentException("not bons");
                }
                var skeleton = new Skdf();
                int i = 0;
                foreach (var bndt in ParseFields(bons.Value))
                {
                    skeleton.Bones[i] = Bndt.FromField(bndt);
                    ++i;
                }
                if (i != BONE_COUNT)
                {
                    throw new ArgumentException($"{i}!={BONE_COUNT}");
                }
                return skeleton;
            }
        }

        [Serializable]

        public struct Btdt
        {
            public Bnid BoneId;
            public Tran Transformation;

            public static Btdt FromField(Field btdt)
            {
                if (btdt.Type != FieldTypes.Btdt)
                {
                    throw new ArgumentException("not btdt");
                }
                var it = ParseFields(btdt.Value).GetEnumerator();
                it.MoveNext();
                var bnid = it.Current;
                it.MoveNext();
                var tran = it.Current;
                return new Btdt
                {
                    BoneId = Bnid.FromField(bnid),
                    Transformation = Tran.FromField(tran),
                };
            }
        }

        [Serializable]

        public class Fram
        {
            public UInt32 FrameNumber;
            public UInt32 Time;
            public Btdt[] BoneTransformations = new Btdt[27];

            // fram
            //   fnum
            //   time
            //   btrs
            //     btdt x 27
            public static Fram FromField(Field fram)
            {
                if (fram.Type != FieldTypes.Fram)
                {
                    throw new ArgumentException("not fram");
                }
                var it = ParseFields(fram.Value).GetEnumerator();
                it.MoveNext();
                var fnum = it.Current;
                it.MoveNext();
                var time = it.Current;
                it.MoveNext();
                var btrs = it.Current;
                var frames = new Fram
                {
                    FrameNumber = BitConverter.ToUInt32(fnum.Value.Array, fnum.Value.Offset),
                    Time = BitConverter.ToUInt32(time.Value.Array, time.Value.Offset),
                };
                var i = 0;
                foreach (var btdt in ParseFields(btrs.Value))
                {
                    frames.BoneTransformations[i] = Btdt.FromField(btdt);
                    ++i;
                }
                if (i != BONE_COUNT)
                {
                    throw new ArgumentException($"{i}!={BONE_COUNT}");
                }
                return frames;
            }
        }

        public static readonly byte[] FileType = Encoding.ASCII.GetBytes("sony motion format");
        public static readonly byte Version = 1;

        public static object Parse(byte[] bytes)
        {
            var r = new BytesReader(bytes);

            // header
            var head = Head.FromField(r.ReadField());
            if (!head.FileType.SequenceEqual(FileType))
            {
                throw new ArgumentException($"invalid file type: {Encoding.ASCII.GetString(head.FileType.Array, head.FileType.Offset, head.FileType.Count)}");
            }
            if (head.Version != Version)
            {
                throw new ArgumentException($"invalid version: {head.Version}");
            }
            var sndf = Sndf.FromField(r.ReadField());

            var f = r.ReadField();
            switch (f.Type)
            {
                case FieldTypes.Skdf:
                    return new SkeletonMessage
                    {
                        head = head,
                        sndf = sndf,
                        skdf = Skdf.FromField(f),
                    };

                case FieldTypes.Fram:
                    return new FrameMessage
                    {
                        head = head,
                        sndf = sndf,
                        fram = Fram.FromField(f),
                    };

                default:
                    throw new ArgumentException();
            }
        }
    }
}