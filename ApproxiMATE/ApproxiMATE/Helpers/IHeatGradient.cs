using System.Collections.Generic;
using Xamarin.Forms;

namespace ApproxiMATE.Helpers
{
    public interface IHeatGradient
    {
        List<Color> Colors { get; set; }
        int Max { get; set; }
        int Min { get; set; }

        Color SteppedColor(int step);
    }
}