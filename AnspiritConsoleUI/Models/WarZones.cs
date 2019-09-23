using System;
using System.Collections.Generic;

namespace AnspiritConsoleUI.Models
{
    public class WarZones
    { 
        public List<Deployment> this[int index]
        {
            get
                {
                switch (index)
                {
                    case 0:
                    case 1:
                        return FN;
                    case 2:
                    case 3:
                        return MFN;
                    case 4:
                    case 5:
                        return M;
                    case 6:
                    case 7:
                        return FS;
                    case 8:
                    case 9:
                        return MFS;
                    case 10:
                    case 11:
                        return MRS;
                    case 12:
                    case 13:
                        return RS;
                    case 14:
                    case 15:
                        return RM;
                    default:
                            throw new ArgumentOutOfRangeException();
                    }
            }
        }
        public List<Deployment> FN { get; set; } = new List<Deployment>();
        public List<Deployment> FS { get; set; } = new List<Deployment>();
        public List<Deployment> MFN { get; set; } = new List<Deployment>();
        public List<Deployment> MFS { get; set; } = new List<Deployment>();
        public List<Deployment> M { get; set; } = new List<Deployment>();
        public List<Deployment> MRS { get; set; } = new List<Deployment>();
        public List<Deployment> RS { get; set; } = new List<Deployment>();
        public List<Deployment> RM { get; set; } = new List<Deployment>();

    }
}
