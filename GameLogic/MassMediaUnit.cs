using System;
using System.Collections.Generic;

namespace _260Weeks.GameLogic
{
    public class MassMediaUnit : Member
    {
        public enum CampaignMode
        {
            Against = -1,
            Pro = 1
        }

        public enum CampaignRunResult
        {
            OK,
            RefusedServicePoints,
            RefusedOpinionSender,
            RefusedOpinionSubject,
            RefusedTooManyCampaigns
        }

        public class Campaign
        {
            public double Delta;

            public MassMediaUnit Media;

            public Member Subject;

            public uint TurnsLeft;

            public Campaign(MassMediaUnit media, Member subject, double delta, uint duration)
            {
                Media = media;
                Subject = subject;
                Delta = delta;
                TurnsLeft = duration;
            }

            public Campaign(MassMediaUnit media, Member subject, CampaignMode mode, uint duration) : this(media, subject, (double)mode, duration)
            {
            }

            public void Act()
            {
                foreach (Member target in Core.getInstance().Members)
                    target.AffectOpinion(Media, Subject, Delta);
                TurnsLeft--;
            }
        }

        public Businessman Owner;

        private List<Campaign> activeCampaigns;

        private Campaign presidentCampaign;

        public MassMediaUnit(string name) : base(name)
        {
            activeCampaigns = new List<Campaign>();
            presidentCampaign = new Campaign(this, Core.getInstance().Player, 0d, Params.MaxTurns);
        }

        public override void Turn()
        {
            foreach (Campaign campaign in activeCampaigns)
                campaign.Act();

            activeCampaigns.RemoveAll(campaign => campaign.TurnsLeft <= 0);

            presidentCampaign.Delta = Owner.Opinions[Core.getInstance().Player];
            presidentCampaign.Act();
        }

        public override void InitOpinions()
        {
            // Mass media do not have opinions because are owned by businessmen
        }

        public CampaignRunResult RunCampaign(Member sender, Member subject, CampaignMode mode, uint duration)
        {
            if (activeCampaigns.Count >= 2)
                return CampaignRunResult.RefusedTooManyCampaigns;

            if (Owner.Opinions[sender] < -Params.OpinionThreshold)
                return CampaignRunResult.RefusedOpinionSender;

            if (mode == CampaignMode.Pro && Owner.Opinions[subject] < -Params.OpinionThreshold)
                return CampaignRunResult.RefusedOpinionSubject;

            if (mode == CampaignMode.Against && Owner.Opinions[subject] > Params.OpinionThreshold)
                return CampaignRunResult.RefusedOpinionSubject;

            if (Owner.PresidentServicePoints + Utils.Unsigmoid(Owner.Opinions[sender]) < duration)
                return CampaignRunResult.RefusedServicePoints;

            activeCampaigns.Add(new Campaign(this, subject, mode, duration));
            return CampaignRunResult.OK;
        }

        public List<Campaign> GetCampaigns()
        {
            return new List<Campaign>(activeCampaigns);
        }

        public override void CheckValid()
        {
            if (Owner == null)
                throw (new NullReferenceException($"Mass media' owner is null ({Name})"));
        }

        public static MassMediaUnit GenerateRandom()
        {
            MassMediaUnit result = new MassMediaUnit(Utils.RandomFromList(StringManager.MassMediaTitles));
            result.Owner = Utils.RandomFromList(Core.getInstance().Businessmen);
            return result;
        }
    }
}