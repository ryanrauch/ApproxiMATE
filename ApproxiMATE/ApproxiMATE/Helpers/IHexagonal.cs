using Xamarin.Forms.GoogleMaps;

namespace ApproxiMATE.Helpers
{
    public interface IHexagonal
    {
        Position CenterLocation { get; }
        Position ExactLocation { get; }
        Polygon HexagonalPolygon(Position center);
        Polygon HexagonalPolygon(Position center, int column, int row);
        void SetCenter(Position center);
        void SetLayer(int layer);
    }
}