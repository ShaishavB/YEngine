using UnityEngine;
using System.IO;
using System.Runtime.CompilerServices;

namespace Yudiz.Gaming.Engine
{
    /// <summary>
    /// Base abstract class used to define a mission the player needs to complete to gain some premium currency.
    /// Subclassed for every mission.
    /// </summary>
    public abstract class MissionController
    {
        // Mission type
        public enum MissionType
        {
            Demo,
            MAX
        }

        public float progress;
        public float max;
        public bool isComplete { get { return (progress / max) >= 1.0f; } }

        public int reward;

        public void Serialize(BinaryWriter w)
        {
            w.Write(progress);
            w.Write(max);
            w.Write(reward);
        }

        public void Deserialize(BinaryReader r)
        {
            progress = r.ReadSingle();
            max = r.ReadSingle();
            reward = r.ReadInt32();
        }

        public virtual bool HaveProgressBar() { return true; }

        public abstract void Created();
        public abstract MissionType GetMissionType();
        public abstract string GetMissionDesc();
        public abstract void Start<T>(ref T param);
        public abstract void Update<T>(ref T param);

        static public MissionController GetNewMissionFromType(MissionType type)
        {
            switch (type)
            {
                case MissionType.Demo:
                    return new DemoMission();
            }

            return null;
        }


    }

    public class DemoMission : MissionController
    {

        public override void Created()
        {
            progress = 0f;
            max = 10f;
            Debug.Log("New Demo Mission Created");
        }

        public override bool HaveProgressBar()
        {
            return false;
        }

        public override string GetMissionDesc()
        {
            return "This is a Demo Mission With Some Task";
        }

        public override MissionType GetMissionType()
        {
            return MissionType.Demo;
        }

        public override void Start<T>(ref T param)
        {
            dynamic manager = (dynamic)param;
            Debug.Log("The Demo Mission Start :" + manager);
            progress = 0f;
        }

        public override void Update<T>(ref T param)
        {
            dynamic manager = (dynamic)param;
            Debug.Log("The Demo Mission Updated :" + manager);
            progress += Time.deltaTime;
        }
    }
}
