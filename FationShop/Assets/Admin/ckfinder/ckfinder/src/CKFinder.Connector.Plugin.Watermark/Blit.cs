/*
 * CKFinder
 * ========
 * http://cksource.com/ckfinder
 * Copyright (C) 2007-2016, CKSource - Frederico Knabben. All rights reserved.
 *
 * The software, this file and its contents are subject to the MIT License.
 * Please read the LICENSE.md file before using, installing, copying,
 * modifying or distribute this file or part of its contents.
 */

namespace CKSource.CKFinder.Connector.Plugin.Watermark
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Drawing2D;

    using ImageProcessor;
    using ImageProcessor.Common.Exceptions;
    using ImageProcessor.Imaging;
    using ImageProcessor.Imaging.Helpers;
    using ImageProcessor.Processors;

    public class Blit : IGraphicsProcessor
    {
        public dynamic DynamicParameter { get; set; }

        public Dictionary<string, string> Settings { get; set; }

        public Blit()
        {
            Settings = new Dictionary<string, string>();
        }

        public Image ProcessImage(ImageFactory factory)
        {
            var image = factory.Image;

            try
            {
                ImageLayer imageLayer = DynamicParameter;
                var overlay = new Bitmap(imageLayer.Image);

                // Set the resolution of the overlay and the image to match.
                overlay.SetResolution(image.HorizontalResolution, image.VerticalResolution);

                var size = imageLayer.Size;
                var width = image.Width;
                var height = image.Height;
                var overlayWidth = size != Size.Empty ? size.Width : Math.Min(width, overlay.Width);
                var overlayHeight = size != Size.Empty ? size.Height : Math.Min(height, overlay.Height);

                var position = imageLayer.Position;
                var opacity = imageLayer.Opacity;

                if (image.Size != overlay.Size || image.Size != new Size(overlayWidth, overlayHeight))
                {
                    // Find the maximum possible dimensions and resize the image.
                    var layer = new ResizeLayer(new Size(overlayWidth, overlayHeight), ResizeMode.Stretch);
                    var resizer = new Resizer(layer);
                    overlay = resizer.ResizeImage(overlay, factory.FixGamma);
                    overlayWidth = overlay.Width;
                    overlayHeight = overlay.Height;
                }

                // Figure out bounds.
                var parent = new Rectangle(0, 0, width, height);
                var child = new Rectangle(0, 0, overlayWidth, overlayHeight);

                // Apply opacity.
                if (opacity < 100)
                {
                    overlay = Adjustments.Alpha(overlay, opacity);
                }

                using (var graphics = Graphics.FromImage(image))
                {
                    graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    graphics.CompositingQuality = CompositingQuality.HighQuality;

                    if (position != null)
                    {
                        // Draw the image in position catering for overflow.
                        graphics.DrawImage(overlay, position.Value);
                    }
                    else
                    {
                        var centered = ImageMaths.CenteredRectangle(parent, child);
                        graphics.DrawImage(overlay, new PointF(centered.X, centered.Y));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ImageProcessingException("Error processing image with " + GetType().Name, ex);
            }

            return image;
        }
    }
}