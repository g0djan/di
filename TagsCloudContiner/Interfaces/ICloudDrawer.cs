using System.Drawing;
using TagsCloudVisualization;

namespace TagsCloudContainer.Interfaces
{
    public interface ICloudDrawer
    {
        void Draw(Cloud<TextRectangle> cloud, Graphics graphics);
        void Save(Bitmap bmp);
    }
}
