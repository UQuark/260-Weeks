using System;
using System.Collections.Generic;

namespace _260Weeks.GameLogic
{
    public class Core
    {
        public uint TurnNumber = 0;

        public List<Member> Members;

        public List<Businessman> Businessmen
        {
            get
            {
                List<Businessman> result = new List<Businessman>();
                foreach (Member member in Members)
                    if (member is Businessman)
                        result.Add(member as Businessman);
                return result;
            }
        }

        public List<MassMediaUnit> MassMedia
        {
            get
            {
                List<MassMediaUnit> result = new List<MassMediaUnit>();
                foreach (Member member in Members)
                    if (member is MassMediaUnit)
                        result.Add(member as MassMediaUnit);
                return result;
            }
        }

        public List<SocialGroup> SocialGroups
        {
            get
            {
                List<SocialGroup> result = new List<SocialGroup>();
                foreach (Member member in Members)
                    if (member is SocialGroup)
                        result.Add(member as SocialGroup);
                return result;
            }
        }

        public static class IDManager
        {
            public static uint ID
            {
                get
                {
                    return counter++;
                }
            }
            private static uint counter = 0;
        }

        public President Player;

        public IPlayerInterface Interface;

        private bool initialized = false, gameOn = true;

        public Core(IPlayerInterface Interface)
        {
            instance = this;
            this.Interface = Interface;
            Members = new List<Member>();
        }

        public void Init(uint numberOfBusinessmen, uint numberOfMassMedia)
        {
            initialized = true;

            Player = new President();

            // Order is important: Businessmen, mass media, social groups, president

            for (uint i = 0; i < numberOfBusinessmen; i++)
                Members.Add(Businessman.GenerateRandom());

            for (uint i = 0; i < numberOfMassMedia; i++)
                Members.Add(MassMediaUnit.GenerateRandom());

            Members.AddRange(SocialGroup.LoadSocialGroups());

            Members.Add(Player);

            foreach (Member member in Members)
                member.CheckValid();

            foreach (Member member in Members)
                member.InitOpinions();
        }

        public bool Turn()
        {
            if (!gameOn)
                return false;
            if (!initialized)
                throw (new InvalidOperationException("Turn is not allowed before initializion"));
            if (TurnNumber >= Params.MaxTurns)
                return false;
            TurnNumber++;
            foreach (Member member in Members)
                member.Rollback();
            foreach (Member member in Members)
                member.Turn();
            foreach (Member member in Members)
                member.Commit();
            return true;
        }

        public Member GetMemberById(uint id)
        {
            foreach (Member member in Members)
                if (member.ID == id)
                    return member;
            return null;
        }

        public Member GetMemberByName(string name)
        {
            foreach (Member member in Members)
                if (member.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                    return member;
            return null;
        }

        public void GameOver()
        {
            gameOn = false;
        }

        private static Core instance;
        public static Core getInstance()
        {
            if (instance == null)
                throw (new NullReferenceException("getInstance is not allowed before initialization"));
            return instance;
        }
    }
}