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
    using System.Text.RegularExpressions;

    public struct Position
    {
        public HorizontalAnchor LeftAnchor { get; private set; }

        public HorizontalAnchor RightAnchor { get; private set; }

        public VerticalAnchor TopAnchor { get; private set; }

        public VerticalAnchor BottomAnchor { get; private set; }

        public int LeftOffset { get; private set; }

        public int RightOffset { get; private set; }

        public int TopOffset { get; private set; }

        public int BottomOffset { get; private set; }

        public static bool TryParse(string text, out Position position)
        {
            var regex = new Regex(@"([lcr])?([+-]\d+)?,([tcb])?([+-]\d+)?(,([lcr])?([+-]\d+)?,([tcb])?([+-]\d+)?)?");

            var match = regex.Match(text);
            if (!match.Success)
            {
                position = new Position();
                return false;
            }

            position = new Position
            {
                LeftAnchor = match.Groups[1].Success
                    ? HorizontalAnchorFromText(match.Groups[1].Value)
                    : HorizontalAnchor.None,
                TopAnchor = match.Groups[3].Success
                    ? VerticalAnchorFromText(match.Groups[3].Value)
                    : VerticalAnchor.None,
                RightAnchor = match.Groups[6].Success
                    ? HorizontalAnchorFromText(match.Groups[6].Value)
                    : HorizontalAnchor.None,
                BottomAnchor = match.Groups[8].Success
                    ? VerticalAnchorFromText(match.Groups[8].Value)
                    : VerticalAnchor.None,
                LeftOffset = match.Groups[2].Success ? int.Parse(match.Groups[2].Value) : 0,
                TopOffset = match.Groups[4].Success ? int.Parse(match.Groups[4].Value) : 0,
                RightOffset = match.Groups[7].Success ? int.Parse(match.Groups[7].Value) : 0,
                BottomOffset = match.Groups[9].Success ? int.Parse(match.Groups[9].Value) : 0
            };

            return true;
        }

        private static HorizontalAnchor HorizontalAnchorFromText(string text)
        {
            switch (text)
            {
                case "l": return HorizontalAnchor.Left;
                case "c": return HorizontalAnchor.Center;
                case "r": return HorizontalAnchor.Right;
                default: return HorizontalAnchor.None;
            }
        }

        private static VerticalAnchor VerticalAnchorFromText(string text)
        {
            switch (text)
            {
                case "t": return VerticalAnchor.Top;
                case "c": return VerticalAnchor.Center;
                case "b": return VerticalAnchor.Bottom;
                default: return VerticalAnchor.None;
            }
        }
    }
}