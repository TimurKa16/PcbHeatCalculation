using System.Collections.Generic;

namespace Диплом
{
    public class Project
    {
        public List<Element> elementList;
        public PCB pcb;

        public int ProjectId { set; get; }
        public string Name { set; get; }
        public string Date { set; get; }
        public int totalTime { set; get; }
        public int stepTime { set; get; }
        public int firstOverheat{ set; get; }
    }
}
