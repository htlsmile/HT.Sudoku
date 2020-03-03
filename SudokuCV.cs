﻿#define ShowMat1

using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HT.Sudoku
{
    public static class SudokuCV
    {
        private const string tempFilename = "tmp.png";

        public static void CaptureFullScreen(string fileName = tempFilename) => CaptureScreen(0, 0, System.Windows.SystemParameters.WorkArea.Width, System.Windows.SystemParameters.WorkArea.Height, fileName);

        public static void CaptureScreen(double x, double y, double width, double height, string fileName = tempFilename)
        {
            int ix = Convert.ToInt32(x);
            int iy = Convert.ToInt32(y);
            int iw = Convert.ToInt32(width);
            int ih = Convert.ToInt32(height);
            var bitmap = new System.Drawing.Bitmap(iw, ih);
            using var graphics = System.Drawing.Graphics.FromImage(bitmap);
            graphics.CopyFromScreen(ix, iy, 0, 0, new System.Drawing.Size(iw, ih));
            bitmap.Save(fileName);
        }

        public static async Task OCR(Action<int, int, int> setNumber, string fileName = tempFilename) => await Task.Run(() =>
        {
            var src = new Mat(fileName);
            var tmp = src.Clone();
            Cv2.CvtColor(src, tmp, ColorConversionCodes.RGB2GRAY);
            Cv2.GaussianBlur(tmp, tmp, new Size(3, 3), 0, 0);
            Cv2.Canny(tmp, tmp, 10, 255);
#if ShowMat
            Cv2.ImShow(nameof(Cv2.Canny), tmp);
            Cv2.WaitKey();
            Cv2.DestroyWindow(nameof(Cv2.Canny));
#endif
            Cv2.FindContours(tmp, out Point[][] contours, out HierarchyIndex[] hierarchy, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple);
            var rect = new Rect();
            var contourCount = new Dictionary<int, int>();
            for (int i = 0; i < contours.GetLength(0); i++)
            {
                var p = hierarchy[i].Parent;
                if (p >= 0)
                {
                    if (contourCount.ContainsKey(p))
                    {
                        contourCount[p]++;
                        if (contourCount[p] >= 9)
                        {
                            var r = Cv2.BoundingRect(contours[p]);
                            if (r.Height > rect.Height)
                            {
                                rect = r;
#if ShowMat
                                Cv2.Rectangle(src, r, Scalar.Red, 2);
                                Cv2.ImShow(nameof(Cv2.Rectangle), src);
                                Cv2.WaitKey();
                                Cv2.DestroyWindow(nameof(Cv2.Rectangle));
#endif
                            }
                        }
                    }
                    else
                    {
                        contourCount.Add(p, 1);
                    }
                }
            }
#if ShowMat
            Cv2.ImShow(nameof(Cv2.FindContours), src);
            Cv2.WaitKey();
            Cv2.DestroyWindow(nameof(Cv2.FindContours));
#endif
            tmp = new Mat(tmp, rect);
            src = new Mat(src, rect);
#if ShowMat
            Cv2.ImShow(nameof(Cv2.SelectROI), src);
            Cv2.WaitKey();
            Cv2.DestroyWindow(nameof(Cv2.SelectROI));
#endif
            Cv2.FindContours(tmp, out contours, out hierarchy, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple);
            for (int i = 0; i < contours.GetLength(0); i++)
            {
                var r = Cv2.BoundingRect(contours[i]);
                if (r.Height < (rect.Height / 12.0) && r.Height > (rect.Height / 18.0))
                {
#if ShowMat
                    Cv2.Rectangle(src, r, Scalar.Red, 2);
#endif
                    if (new Mat(src, r).ImWrite(fileName))
                    {
                        var m = (int)((r.Top + r.Height / 2.0) / (rect.Height / 9.0));
                        var n = (int)((r.Left + r.Width / 2.0) / (rect.Width / 9.0));
                        var text = OCRSingleNumber(fileName);
                        setNumber(m, n, text);
                    }
                }
            }
#if ShowMat
            Cv2.ImShow(nameof(Cv2.Rectangle), src);
            Cv2.WaitKey();
            Cv2.DestroyWindow(nameof(Cv2.Rectangle));
#endif
        });

        private static readonly Tesseract.TesseractEngine OCREngine = new Tesseract.TesseractEngine("TessData", "chi_sim");

        public static int OCRSingleNumber(string filename)
        {
            OCREngine.SetVariable("tessedit_char_whitelist", "0123456789");//设置识别变量，当前只能识别数字。
            using var pix = Tesseract.Pix.LoadFromFile(filename);
            using var page = OCREngine.Process(pix, Tesseract.PageSegMode.SingleBlock);
            return int.TryParse(page.GetText().Trim(), out var i) ? i : 0;
        }

    }
}
