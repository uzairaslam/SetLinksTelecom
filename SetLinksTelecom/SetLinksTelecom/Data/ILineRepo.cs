using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SetLinksTelecom.Models;

namespace SetLinksTelecom.Data
{
    public interface ILineRepo
    {
        IList<Line> GetLines();
        Line GetLine(int id);
        void SaveLine(Line line);
        void UpdateLine(Line line);
    }
}
