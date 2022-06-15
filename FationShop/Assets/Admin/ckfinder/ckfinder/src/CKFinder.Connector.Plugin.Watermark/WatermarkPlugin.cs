namespace CKSource.CKFinder.Connector.Plugin.Watermark
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using CKSource.CKFinder.Connector.Core;
    using CKSource.CKFinder.Connector.Core.Events;
    using CKSource.CKFinder.Connector.Core.Events.Messages;
    using CKSource.CKFinder.Connector.Core.Exceptions;
    using CKSource.CKFinder.Connector.Core.Nodes;
    using CKSource.CKFinder.Connector.Core.Plugins;
    using CKSource.CKFinder.Connector.Core.ResizedImages;

    using ImageProcessor;
    using ImageProcessor.Common.Exceptions;
    using ImageProcessor.Imaging;
    using ImageProcessor.Imaging.Formats;

    [Export(typeof(IPlugin))]
    public class WatermarkPlugin : IPlugin, IDisposable
    {
        private Image _watermarkImage;

        private object _fileUploadSubscription;

        private IEventAggregator _eventAggregator;

        private int _opacity;

        private Position _position;

        public void Initialize(IComponentResolver componentResolver, IReadOnlyDictionary<string, IReadOnlyCollection<string>> options)
        {
            var watermarkImagePath = options["watermarkPath"].FirstOrDefault();
            if (string.IsNullOrEmpty(watermarkImagePath))
            {
                throw new CustomErrorException("Parameter watermarkPath is required");
            }

            Position.TryParse(options["position"].FirstOrDefault() ?? "l,t", out _position);
            if ((_position.LeftAnchor == HorizontalAnchor.None && _position.RightAnchor == HorizontalAnchor.None) ||
                (_position.TopAnchor == VerticalAnchor.None && _position.BottomAnchor == VerticalAnchor.None))
            {
                throw new CustomErrorException("Position is invalid");
            }

            _opacity = int.Parse(options["opacity"].FirstOrDefault() ?? "50");

            _watermarkImage = Image.FromFile(watermarkImagePath);

            _eventAggregator = componentResolver.Resolve<IEventAggregator>();
            _fileUploadSubscription = _eventAggregator.Subscribe<FileUploadEvent>(next => async messageContext => await OnFileUpload(messageContext, next));
        }

        public void Dispose()
        {
            _eventAggregator?.Unsubscribe(_fileUploadSubscription);
            _watermarkImage?.Dispose();
        }

        private async Task OnFileUpload(MessageContext<FileUploadEvent> messageContext, EventHandlerFunc<FileUploadEvent> next)
        {
            var stream = messageContext.Message.Stream;
            var imageSettings = messageContext.ComponentResolver.Resolve<IImageSettings>();

            using (var imageFactory = new ImageFactory())
            {
                try
                {
                    imageFactory.Load(stream);
                    stream.Dispose();
                    stream = new MemoryStream();
                    messageContext.Message.Stream = stream;
                }
                catch (ImageFormatException)
                {
                    // This is not an image or the format is not supported.
                    await next(messageContext);
                    return;
                }

                var rectangle = CalculateRectangle(imageFactory.Image, _watermarkImage, _position);

                new PngFormat().ApplyProcessor(new Blit
                {
                    DynamicParameter = new ImageLayer
                    {
                        Image = _watermarkImage,
                        Opacity = _opacity,
                        Size = rectangle.Size,
                        Position = rectangle.Location
                    }
                }.ProcessImage, imageFactory);

                var format = ImageFormats.GetFormatForFile(messageContext.Message.File);
                format.Quality = imageSettings.Quality;
                imageFactory.Format(format);

                imageFactory.Save(stream);
                stream.Position = 0;
            }

            await next(messageContext);
        }

        private static Rectangle CalculateRectangle(Image image, Image watermarkImage, Position position)
        {
            var x1 = CalculateX1(image, watermarkImage, position);
            var x2 = CalculateX2(image, watermarkImage, position);

            var y1 = CalculateY1(image, watermarkImage, position);
            var y2 = CalculateY2(image, watermarkImage, position);

            return new Rectangle(x1, y1, x2 - x1, y2 - y1);
        }

        private static int CalculateX1(Image image, Image watermarkImage, Position position)
        {
            var centerX = image.Size.Width / 2;

            switch (position.LeftAnchor)
            {
                case HorizontalAnchor.None:
                    return position.LeftOffset + CalculateX2(image, watermarkImage, position) - watermarkImage.Width;
                case HorizontalAnchor.Left:
                    return position.LeftOffset;
                case HorizontalAnchor.Center:
                    return position.LeftOffset + centerX - watermarkImage.Size.Width / 2;
                case HorizontalAnchor.Right:
                    return position.LeftOffset + image.Size.Width;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static int CalculateX2(Image image, Image watermarkImage, Position position)
        {
            var centerX = image.Size.Width / 2;

            switch (position.RightAnchor)
            {
                case HorizontalAnchor.None:
                    return position.RightOffset + CalculateX1(image, watermarkImage, position) + watermarkImage.Width;
                case HorizontalAnchor.Left:
                    return position.RightOffset;
                case HorizontalAnchor.Center:
                    return position.RightOffset + centerX + watermarkImage.Size.Width / 2;
                case HorizontalAnchor.Right:
                    return position.RightOffset + image.Size.Width;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static int CalculateY1(Image image, Image watermarkImage, Position position)
        {
            var centerY = image.Size.Height / 2;

            switch (position.TopAnchor)
            {
                case VerticalAnchor.None:
                    return position.TopOffset + CalculateY2(image, watermarkImage, position) - watermarkImage.Height;
                case VerticalAnchor.Top:
                    return position.TopOffset;
                case VerticalAnchor.Center:
                    return position.TopOffset + centerY - watermarkImage.Size.Height / 2;
                case VerticalAnchor.Bottom:
                    return position.TopOffset + image.Size.Height;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static int CalculateY2(Image image, Image watermarkImage, Position position)
        {
            var centerY = image.Size.Height / 2;

            switch (position.BottomAnchor)
            {
                case VerticalAnchor.None:
                    return position.BottomOffset + CalculateY1(image, watermarkImage, position) + watermarkImage.Height;
                case VerticalAnchor.Top:
                    return position.BottomOffset;
                case VerticalAnchor.Center:
                    return position.BottomOffset + centerY + watermarkImage.Size.Height / 2;
                case VerticalAnchor.Bottom:
                    return position.BottomOffset + image.Size.Height;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
