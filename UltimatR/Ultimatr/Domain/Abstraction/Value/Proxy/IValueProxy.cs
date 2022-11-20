using System.Instant;

namespace UltimatR
{
    public interface IValueProxy : IFigure
    {
        IRubrics Rubrics { get; }

        ISleeve Valuator { get; set;  }
    }
}