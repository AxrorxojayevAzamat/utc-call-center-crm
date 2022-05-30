using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CallCenterCRM.Models
{
    public class Stats
    {
        public Regions Region { get; set; }
        public int DoneCount { get; set; }
        public int ProcessCount { get; set; }
        public int RejectedCount { get; set; }
    }

    public class ModeratorStats : Stats
    {
        public List<int>? BranchesCount { get; set; }
    }

    public class OrganizationStats : Stats
    {
        public int FizikCount { get; set; }
        public int YurikCount { get; set; }
        public int MaleCount { get; set; }
        public int FemaleCount { get; set; }
    }
}
