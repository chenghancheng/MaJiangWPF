using Majiang.View;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

public static class ImageMethod
{
    // 方法来加载图像到列表中
    public static void LoadImages(List<BitmapImage> picBox, string pathName, bool isRound)
    {
        // 获取文件夹中的所有文件
        string[] fileList = Directory.GetFiles(pathName);

        foreach (var filePath in fileList)
        {
            // 检查文件扩展名是否为图像格式
            if (filePath.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                filePath.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase))
            {
                var filePath2 = "pack://application:,,," + filePath.Substring(1);
                // 加载图片文件
                BitmapImage bitmap = new BitmapImage(new Uri(filePath2));

                // 根据 isRound 参数决定是否需要圆角处理
                if (isRound)
                {
                    picBox.Add(RoundImage(bitmap));
                }
                else
                {
                    picBox.Add(bitmap);
                }
            }
        }
    }

    // 创建圆角图像的方法
    private static BitmapImage RoundImage(BitmapImage bitmap)
    {
        // 创建一个带圆角的矩形
        var width = bitmap.PixelWidth;
        var height = bitmap.PixelHeight;

        var rect = new Rectangle
        {
            Width = width,
            Height = height,
            RadiusX = 15,  // 设置圆角半径
            RadiusY = 15
        };

        // 创建 Image 控件并设置其 Source 属性为传入的 bitmap
        var image = new Image
        {
            Source = bitmap,
            Width = width,
            Height = height
        };

        // 使用 VisualBrush 将 Image 控件作为视觉对象
        var visualBrush = new VisualBrush(image);
        rect.Fill = visualBrush;

        // 创建一个 RenderTargetBitmap 来渲染带圆角的矩形
        var renderTargetBitmap = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Pbgra32);
        renderTargetBitmap.Render(rect);

        // 将渲染后的图像保存到内存流
        var stream = new System.IO.MemoryStream();
        PngBitmapEncoder encoder = new PngBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
        encoder.Save(stream);

        // 创建一个 BitmapImage 对象并将图像流设置为源
        var roundedBitmap = new BitmapImage();
        stream.Seek(0, SeekOrigin.Begin);
        roundedBitmap.BeginInit();
        roundedBitmap.StreamSource = stream;
        roundedBitmap.EndInit();

        return roundedBitmap;
    }

    public static BitmapSource AdaptImageSize(BitmapSource source, Size size, int rotate)
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source), "Source image cannot be null");
        }

        // 创建旋转变换
        RotateTransform rotateTransform = new RotateTransform(rotate);

        // 创建缩放变换，保持纵横比
        double scaleX = size.Width / source.PixelWidth;
        double scaleY = size.Height / source.PixelHeight;
        ScaleTransform scaleTransform = new ScaleTransform(scaleX, scaleY);

        // 将旋转和缩放变换组合成一个 TransformGroup
        TransformGroup transformGroup = new TransformGroup();
        transformGroup.Children.Add(rotateTransform);
        transformGroup.Children.Add(scaleTransform);

        // 使用 TransformedBitmap 创建最终的 BitmapSource
        TransformedBitmap transformedBitmap = new TransformedBitmap();
        transformedBitmap.BeginInit();
        transformedBitmap.Source = source;
        transformedBitmap.Transform = transformGroup;  // 应用组合的变换
        transformedBitmap.EndInit();

        return transformedBitmap;
    }

    //public static BitmapSource RotateBitmapImage(BitmapImage source, int angle)
    //{
    //    // 创建一个旋转变换
    //    RotateTransform rotateTransform = new RotateTransform(angle);

    //    // 使用 TransformedBitmap 来应用变换
    //    TransformedBitmap transformedBitmap = new TransformedBitmap();
    //    transformedBitmap.BeginInit();
    //    transformedBitmap.Source = source;
    //    transformedBitmap.Transform = rotateTransform; // 应用旋转
    //    transformedBitmap.EndInit();

    //    return transformedBitmap;
    //}

    public static BitmapSource RotateBitmapImage(BitmapImage source, int angle)
    {
        // 确保BitmapImage被正确初始化
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source), "Source image cannot be null");
        }

        // 创建一个旋转变换
        RotateTransform rotateTransform = new RotateTransform(angle);

        // 使用 TransformedBitmap 来应用变换
        TransformedBitmap transformedBitmap = new TransformedBitmap();

        // 先初始化BitmapImage，确保它已经初始化完毕
        //source.BeginInit();
        //source.EndInit();

        //transformedBitmap.BeginInit();
        transformedBitmap.Source = source;
        transformedBitmap.Transform = rotateTransform; // 应用旋转
                                                       //transformedBitmap.EndInit();

        return transformedBitmap;
    }
}
