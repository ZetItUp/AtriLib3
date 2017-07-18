using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace AtriLib3.UI
{
    public class ControlGraphicsData
    {
        public static int DEFAULT_DIMENSION = 8;

        // Textbox Normal State
        public Rectangle TextBoxTopLeft { get; private set; }
        public Rectangle TextBoxTopRight { get; private set; }
        public Rectangle TextBoxTopFill { get; private set; }
        public Rectangle TextBoxBottomLeft { get; private set; }
        public Rectangle TextBoxBottomRight { get; private set; }
        public Rectangle TextBoxBottomFill { get; private set; }
        public Rectangle TextBoxLeftFill { get; private set; }
        public Rectangle TextBoxRightFill { get; private set; }
        public Rectangle TextBoxBase { get; private set; }

        public Rectangle GetTextBoxCursorRectangle(int fontSize = 8)
        {
            if(fontSize < 5)
            {
                fontSize = 5;
            }

            return new Rectangle(WindowDimension, WindowDimension * 8, 3, fontSize);
        }

        // Textbox Hover State
        public Rectangle TextBoxHoverTopLeft { get; private set; }
        public Rectangle TextBoxHoverTopRight { get; private set; }
        public Rectangle TextBoxHoverTopFill { get; private set; }
        public Rectangle TextBoxHoverBottomLeft { get; private set; }
        public Rectangle TextBoxHoverBottomRight { get; private set; }
        public Rectangle TextBoxHoverBottomFill { get; private set; }
        public Rectangle TextBoxHoverLeftFill { get; private set; }
        public Rectangle TextBoxHoverRightFill { get; private set; }
        public Rectangle TextBoxHoverBase { get; private set; }

        // Button MouseDown State
        public Rectangle ButtonMouseDownTopLeft { get; private set; }
        public Rectangle ButtonMouseDownTopRight { get; private set; }
        public Rectangle ButtonMouseDownTopFill { get; private set; }
        public Rectangle ButtonMouseDownBottomLeft { get; private set; }
        public Rectangle ButtonMouseDownBottomRight { get; private set; }
        public Rectangle ButtonMouseDownBottomFill { get; private set; }
        public Rectangle ButtonMouseDownLeftFill { get; private set; }
        public Rectangle ButtonMouseDownRightFill { get; private set; }
        public Rectangle ButtonMouseDownBase { get; private set; }

        // Button Hover State
        public Rectangle ButtonHoverTopLeft { get; private set; }
        public Rectangle ButtonHoverTopRight { get; private set; }
        public Rectangle ButtonHoverTopFill { get; private set; }
        public Rectangle ButtonHoverBottomLeft { get; private set; }
        public Rectangle ButtonHoverBottomRight { get; private set; }
        public Rectangle ButtonHoverBottomFill { get; private set; }
        public Rectangle ButtonHoverLeftFill { get; private set; }
        public Rectangle ButtonHoverRightFill { get; private set; }
        public Rectangle ButtonHoverBase { get; private set; }

        // Button Normal State
        public Rectangle ButtonTopLeft { get; private set; }
        public Rectangle ButtonTopRight { get; private set; }
        public Rectangle ButtonTopFill { get; private set; }
        public Rectangle ButtonBottomLeft { get; private set; }
        public Rectangle ButtonBottomRight { get; private set; }
        public Rectangle ButtonBottomFill { get; private set; }
        public Rectangle ButtonLeftFill { get; private set; }
        public Rectangle ButtonRightFill { get; private set; }
        public Rectangle ButtonBase { get; private set; }

        // CheckBox
        public Rectangle CheckBoxCheckedRectangle { get; private set; }
        public Rectangle CheckBoxUncheckedRectangle { get; private set; }
        public Rectangle CheckBoxCheckedHoverRectangle { get; private set; }
        public Rectangle CheckBoxUncheckedHoverRectangle { get; private set; }

        // Title Bar
        public Rectangle TitleBarTopLeft { get; private set; }
        public Rectangle TitleBarTopRight { get; private set; }
        public Rectangle TitleBarBottomLeft { get; private set; }
        public Rectangle TitleBarBottomRight { get; private set; }
        public Rectangle TitleBarFill { get; private set; }

        // Window Border
        public Rectangle WindowBorderTopLeft { get; private set; }
        public Rectangle WindowBorderTopRight { get; private set; }
        public Rectangle WindowBorderBottomLeft { get; private set; }
        public Rectangle WindowBorderBottomRight { get; private set; }

        public Rectangle WindowBorderTopFill { get; private set; }
        public Rectangle WindowBorderBottomFill { get; private set; }
        public Rectangle WindowBorderLeftFill { get; private set; }
        public Rectangle WindowBorderRightFill { get; private set; }

        // Window Base
        public Rectangle WindowBase { get; private set; }

        public int WindowDimension { get; private set; } = DEFAULT_DIMENSION;

        public ControlGraphicsData()
        {

        }

        public ControlGraphicsData(int windowDimension)
        {
            WindowDimension = windowDimension;
        }

        /// <summary>
        /// Set the entire Window Data depending on the Window Dimensions.
        /// Read help for more information.
        /// </summary>
        public void SetDataByDimension()
        {
            TitleBarTopLeft = new Rectangle(0, 0, WindowDimension, WindowDimension);
            TitleBarTopRight = new Rectangle(WindowDimension, 0, WindowDimension, WindowDimension);
            TitleBarBottomLeft = new Rectangle(0, WindowDimension, WindowDimension, WindowDimension);
            TitleBarBottomRight = new Rectangle(WindowDimension, WindowDimension, WindowDimension, WindowDimension);
            TitleBarFill = new Rectangle(WindowDimension * 2, 0, WindowDimension / 2, WindowDimension * 2);

            WindowBorderTopLeft = new Rectangle(0, WindowDimension * 2, WindowDimension, WindowDimension);
            WindowBorderTopRight = new Rectangle(WindowDimension, WindowDimension * 2, WindowDimension, WindowDimension);
            WindowBorderBottomLeft = new Rectangle(0, WindowDimension * 3, WindowDimension, WindowDimension);
            WindowBorderBottomRight = new Rectangle(WindowDimension, WindowDimension * 3, WindowDimension, WindowDimension);

            WindowBorderTopFill = new Rectangle(WindowDimension * 2, WindowDimension * 2, WindowDimension / 2, WindowDimension);
            WindowBorderBottomFill = new Rectangle(WindowDimension * 2, WindowDimension * 3, WindowDimension / 2, WindowDimension);
            WindowBorderLeftFill = new Rectangle(0, WindowDimension * 4, WindowDimension, WindowDimension / 2);
            WindowBorderRightFill = new Rectangle(WindowDimension, WindowDimension * 4, WindowDimension, WindowDimension / 2);

            WindowBase = new Rectangle(0, WindowDimension * 5, 1, 1);

            CheckBoxCheckedRectangle = new Rectangle(WindowDimension * 3, 0, WindowDimension * 2, WindowDimension * 2);
            CheckBoxUncheckedRectangle = new Rectangle(WindowDimension * 3, WindowDimension * 2, WindowDimension * 2, WindowDimension * 2);
            CheckBoxCheckedHoverRectangle = new Rectangle(WindowDimension * 5, 0, WindowDimension * 2, WindowDimension * 2);
            CheckBoxUncheckedHoverRectangle = new Rectangle(WindowDimension * 5, WindowDimension * 2, WindowDimension * 2, WindowDimension * 2);

            ButtonTopLeft = new Rectangle(0, WindowDimension * 6, WindowDimension / 2, WindowDimension / 2);
            ButtonTopRight = new Rectangle(WindowDimension / 2, WindowDimension * 6, WindowDimension / 2, WindowDimension / 2);
            ButtonBottomLeft = new Rectangle(0, WindowDimension * 6 + WindowDimension / 2, WindowDimension / 2, WindowDimension / 2);
            ButtonBottomRight = new Rectangle(WindowDimension / 2, WindowDimension * 6 + WindowDimension / 2, WindowDimension / 2, WindowDimension / 2);
            ButtonTopFill = new Rectangle(WindowDimension / 2, WindowDimension * 6, 1, WindowDimension / 2);
            ButtonBottomFill = new Rectangle(WindowDimension / 2, WindowDimension * 6 + WindowDimension / 2, 1, WindowDimension / 2);
            ButtonBase = new Rectangle(WindowDimension / 2, WindowDimension * 6 + WindowDimension / 2, 1, 1);
            ButtonLeftFill = new Rectangle(0, WindowDimension * 6 + WindowDimension / 2, WindowDimension / 2, 1);
            ButtonRightFill = new Rectangle(WindowDimension / 2, WindowDimension * 6 + WindowDimension / 2, WindowDimension / 2, 1);

            ButtonHoverTopLeft = new Rectangle(0, WindowDimension * 7, WindowDimension / 2, WindowDimension / 2);
            ButtonHoverTopRight = new Rectangle(WindowDimension / 2, WindowDimension * 7, WindowDimension / 2, WindowDimension / 2);
            ButtonHoverBottomLeft = new Rectangle(0, WindowDimension * 7 + WindowDimension / 2, WindowDimension / 2, WindowDimension / 2);
            ButtonHoverBottomRight = new Rectangle(WindowDimension / 2, WindowDimension * 7 + WindowDimension / 2, WindowDimension / 2, WindowDimension / 2);
            ButtonHoverTopFill = new Rectangle(WindowDimension / 2, WindowDimension * 7, 1, WindowDimension / 2);
            ButtonHoverBottomFill = new Rectangle(WindowDimension / 2, WindowDimension * 7 + WindowDimension / 2, 1, WindowDimension / 2);
            ButtonHoverBase = new Rectangle(WindowDimension / 2, WindowDimension * 7 + WindowDimension / 2, 1, 1);
            ButtonHoverLeftFill = new Rectangle(0, WindowDimension * 7 + WindowDimension / 2, WindowDimension / 2, 1);
            ButtonHoverRightFill = new Rectangle(WindowDimension / 2, WindowDimension * 7 + WindowDimension / 2, WindowDimension / 2, 1);

            ButtonMouseDownTopLeft = new Rectangle(0, WindowDimension * 8, WindowDimension / 2, WindowDimension / 2);
            ButtonMouseDownTopRight = new Rectangle(WindowDimension / 2, WindowDimension * 8, WindowDimension / 2, WindowDimension / 2);
            ButtonMouseDownBottomLeft = new Rectangle(0, WindowDimension * 8 + WindowDimension / 2, WindowDimension / 2, WindowDimension / 2);
            ButtonMouseDownBottomRight = new Rectangle(WindowDimension / 2, WindowDimension * 8 + WindowDimension / 2, WindowDimension / 2, WindowDimension / 2);
            ButtonMouseDownTopFill = new Rectangle(WindowDimension / 2, WindowDimension * 8, 1, WindowDimension / 2);
            ButtonMouseDownBottomFill = new Rectangle(WindowDimension / 2, WindowDimension * 8 + WindowDimension / 2, 1, WindowDimension / 2);
            ButtonMouseDownBase = new Rectangle(WindowDimension / 2, WindowDimension * 8 + WindowDimension / 2, 1, 1);
            ButtonMouseDownLeftFill = new Rectangle(0, WindowDimension * 8 + WindowDimension / 2, WindowDimension / 2, 1);
            ButtonMouseDownRightFill = new Rectangle(WindowDimension / 2, WindowDimension * 8 + WindowDimension / 2, WindowDimension / 2, 1);

            TextBoxTopLeft = new Rectangle(WindowDimension, WindowDimension * 6, WindowDimension / 2, WindowDimension / 2);
            TextBoxTopRight = new Rectangle(WindowDimension + WindowDimension / 2, WindowDimension * 6, WindowDimension / 2, WindowDimension / 2);
            TextBoxBottomLeft = new Rectangle(WindowDimension, WindowDimension * 6 + WindowDimension / 2, WindowDimension / 2, WindowDimension / 2);
            TextBoxBottomRight = new Rectangle(WindowDimension + WindowDimension / 2, WindowDimension * 6 + WindowDimension / 2, WindowDimension / 2, WindowDimension / 2);
            TextBoxTopFill = new Rectangle(WindowDimension + WindowDimension / 2, WindowDimension * 6, 1, WindowDimension / 2);
            TextBoxBottomFill = new Rectangle(WindowDimension + WindowDimension / 2, WindowDimension * 6 + WindowDimension / 2, 1, WindowDimension / 2);
            TextBoxLeftFill = new Rectangle(WindowDimension, WindowDimension * 6 + WindowDimension / 2, WindowDimension / 2, 1);
            TextBoxRightFill = new Rectangle(WindowDimension + WindowDimension / 2, WindowDimension * 6 + WindowDimension / 2, WindowDimension / 2, 1);
            TextBoxBase = new Rectangle(WindowDimension + WindowDimension / 2, WindowDimension * 6 + WindowDimension / 2, 1, 1);

            TextBoxHoverTopLeft = new Rectangle(WindowDimension, WindowDimension * 7, WindowDimension / 2, WindowDimension / 2);
            TextBoxHoverTopRight = new Rectangle(WindowDimension + WindowDimension / 2, WindowDimension * 7, WindowDimension / 2, WindowDimension / 2);
            TextBoxHoverBottomLeft = new Rectangle(WindowDimension, WindowDimension * 7 + WindowDimension / 2, WindowDimension / 2, WindowDimension / 2);
            TextBoxHoverBottomRight = new Rectangle(WindowDimension + WindowDimension / 2, WindowDimension * 7 + WindowDimension / 2, WindowDimension / 2, WindowDimension / 2);
            TextBoxHoverTopFill = new Rectangle(WindowDimension + WindowDimension / 2, WindowDimension * 7, 1, WindowDimension / 2);
            TextBoxHoverBottomFill = new Rectangle(WindowDimension + WindowDimension / 2, WindowDimension * 7 + WindowDimension / 2, 1, WindowDimension / 2);
            TextBoxHoverLeftFill = new Rectangle(WindowDimension, WindowDimension * 7 + WindowDimension / 2, WindowDimension / 2, 1);
            TextBoxHoverRightFill = new Rectangle(WindowDimension + WindowDimension / 2, WindowDimension * 7 + WindowDimension / 2, WindowDimension / 2, 1);
            TextBoxHoverBase = new Rectangle(WindowDimension + WindowDimension / 2, WindowDimension * 7 + WindowDimension / 2, 1, 1);
        }
    }
}
